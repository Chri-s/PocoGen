using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using MvvmHybridFramework;
using PocoGen.Common;
using PocoGen.Gui.Applications.Messages;
using PocoGen.Gui.Applications.ViewModels;
using PocoGen.Gui.Domain;
using ReactiveUI;

namespace PocoGen.Gui.Applications.Controllers
{
    [Export]
    internal class OutputController
    {
        private readonly OutputWriterConfigurationViewModel outputWriterConfigurationViewModel;
        private readonly ShellViewModel shellViewModell;
        private readonly IMessageBus messageBus;
        private readonly CompositionContainer container;
        private readonly UnsubscriptionList<OutputWriterViewModel> unsubscribeList = new UnsubscriptionList<OutputWriterViewModel>();

        [Import]
        public Engine Engine { get; set; }

        [ImportingConstructor]
        public OutputController(OutputWriterConfigurationViewModel outputWriterConfigurationViewModel, ShellViewModel shellViewModell, IMessageBus messageBus, CompositionContainer container)
        {
            this.outputWriterConfigurationViewModel = outputWriterConfigurationViewModel;
            this.shellViewModell = shellViewModell;
            this.messageBus = messageBus;
            this.container = container;
        }

        public void Initialize()
        {
            var outputWriters = from writer in this.Engine.AvailableOutputWriters
                                let writerModel = new OutputWriterModel(new OutputWriterPlugIn(writer))
                                orderby writerModel.Name
                                select writerModel;

            this.outputWriterConfigurationViewModel.AvailableOutputWriters.AddRange(outputWriters);

            this.shellViewModell.OutputWriterConfigurationViewModel = this.outputWriterConfigurationViewModel;

            this.outputWriterConfigurationViewModel.Add.ObserveOnDispatcher().Subscribe(_ => this.AddOutputWriter());

            this.messageBus.Listen<DefinitionLoaded>().ObserveOnDispatcher().Subscribe(_ => this.DefinitionLoaded());
        }

        private void DefinitionLoaded()
        {
            this.outputWriterConfigurationViewModel.OutputWriters.Clear();
            this.unsubscribeList.UnsubscribeAll();

            foreach (OutputWriterPlugIn outputWriter in this.Engine.OutputWriters)
            {
                this.outputWriterConfigurationViewModel.OutputWriters.Add(this.GetOutputWriterViewModel(outputWriter));
            }
        }

        private OutputWriterViewModel GetOutputWriterViewModel(OutputWriterPlugIn plugIn)
        {
            OutputWriterViewModel vm = this.container.GetExportedValue<OutputWriterViewModel>();
            vm.OutputWriter = plugIn;
            vm.FileName = plugIn.FileName;

            IDisposable subscription = vm.WhenAny(x => x.FileName, x => x.Sender).Subscribe(x => x.OutputWriter.FileName = x.FileName);
            this.unsubscribeList.AddSubscriber(vm, subscription);

            subscription = vm.Configure.Subscribe(_ => this.ShowSettings(vm));
            this.unsubscribeList.AddSubscriber(vm, subscription);

            subscription = vm.Delete.Subscribe(_ => this.Delete(vm));
            this.unsubscribeList.AddSubscriber(vm, subscription);

            subscription = vm.GeneratePreview.Subscribe(_ => this.GeneratePreview(vm));
            this.unsubscribeList.AddSubscriber(vm, subscription);

            return vm;
        }

        private void AddOutputWriter()
        {
            OutputWriterPlugIn newOutputWriter = this.outputWriterConfigurationViewModel.SelectedOutputWriterToAdd.OutputWriter.Clone();
            this.outputWriterConfigurationViewModel.OutputWriters.Add(this.GetOutputWriterViewModel(newOutputWriter));
            this.Engine.OutputWriters.Add(newOutputWriter);
        }

        private void ShowSettings(OutputWriterViewModel outputWriterVm)
        {
            SettingsViewModel vm = this.container.GetExportedValue<SettingsViewModel>();
            vm.Title = outputWriterVm.OutputWriter.Name + " - Settings";
            vm.SettingsObject = outputWriterVm.OutputWriter.Settings;

            vm.ShowDialog(this.shellViewModell.Window);
        }

        private void Delete(OutputWriterViewModel outputWriterVm)
        {
            this.outputWriterConfigurationViewModel.OutputWriters.Remove(outputWriterVm);
            this.unsubscribeList.Unsubscribe(outputWriterVm);
            this.Engine.OutputWriters.Remove(outputWriterVm.OutputWriter);
        }

        private async void GeneratePreview(OutputWriterViewModel outputWriterVm)
        {
            this.shellViewModell.BusyMessage = "Generating preview...";
            this.shellViewModell.IsBusy = true;

            PreviewViewModel vm = this.container.GetExportedValue<PreviewViewModel>();

            try
            {
                using (StringWriter writer = new StringWriter())
                {
                    OutputInformation information = new OutputInformation();
                    await this.Engine.GenerateCode(writer, outputWriterVm.OutputWriter, information);

                    vm.Text = writer.ToString();
                    vm.SyntaxHighlightingLanguage = information.SyntaxHighlightingLanguage;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while generating the code: " + ex.Message);
            }

            vm.ShowDialog();

            this.shellViewModell.IsBusy = false;
        }
    }
}