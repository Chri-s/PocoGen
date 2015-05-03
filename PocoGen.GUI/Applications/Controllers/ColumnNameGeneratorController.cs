using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHybridFramework;
using PocoGen.Common;
using PocoGen.Gui.Applications.Messages;
using PocoGen.Gui.Applications.ViewModels;
using ReactiveUI;

namespace PocoGen.Gui.Applications.Controllers
{
    [Export]
    internal class ColumnNameGeneratorController
    {
        private readonly ColumnNameGeneratorConfigurationViewModel columnNameConfigurationViewModel;
        private readonly CompositionContainer container;
        private readonly Engine engine;
        private readonly IMessageBus messageBus;
        private readonly ShellViewModel shellViewModel;
        private readonly UnsubscriptionList<ColumnNameGeneratorViewModel> unsubscribeList = new UnsubscriptionList<ColumnNameGeneratorViewModel>();

        [ImportingConstructor]
        public ColumnNameGeneratorController(
            ColumnNameGeneratorConfigurationViewModel columnNameConfigurationViewModel,
            Engine engine,
            ShellViewModel shellViewModel,
            CompositionContainer container,
            IMessageBus messageBus)
        {
            this.messageBus = messageBus;
            this.container = container;
            this.shellViewModel = shellViewModel;
            this.engine = engine;
            this.columnNameConfigurationViewModel = columnNameConfigurationViewModel;
        }

        public void Initialize()
        {
            this.shellViewModel.NameGeneratorConfigurationsViewModel.ColumnNameGenerator = this.columnNameConfigurationViewModel;

            IEnumerable<ColumnNameGeneratorPlugIn> availableColumnNameGenerators = from generator in this.engine.AvailableColumnNameGenerators
                                                                                   select new ColumnNameGeneratorPlugIn(generator);

            this.columnNameConfigurationViewModel.ColumnNameGenerators.Changed.Subscribe(_ => this.UpdateIsFirstAndLastItem());
            this.columnNameConfigurationViewModel.ColumnNameGenerators.ItemsRemoved.Subscribe(vm =>
            {
                this.engine.ColumnNameGenerators.Remove(vm.ColumnNameGenerator);
                this.unsubscribeList.Unsubscribe(vm);
            });

            this.columnNameConfigurationViewModel.AvailableColumnNameGenerators.AddRange(availableColumnNameGenerators);

            this.columnNameConfigurationViewModel.Add.Subscribe(_ => this.AddSelectedColumnNameGenerator());

            this.messageBus.Listen<DefinitionLoaded>().Subscribe(_ => this.DefinitionLoaded());
        }

        private void DefinitionLoaded()
        {
            this.columnNameConfigurationViewModel.ColumnNameGenerators.Clear();

            foreach (ColumnNameGeneratorPlugIn columnNameGenerator in this.engine.ColumnNameGenerators)
            {
                this.columnNameConfigurationViewModel.ColumnNameGenerators.Add(this.GetColumnNameGeneratorViewModel(columnNameGenerator));
            }
        }

        private void AddSelectedColumnNameGenerator()
        {
            ColumnNameGeneratorViewModel vm = this.GetColumnNameGeneratorViewModel(this.columnNameConfigurationViewModel.SelectedColumnNameGeneratorToAdd.Clone());
            this.engine.ColumnNameGenerators.Add(vm.ColumnNameGenerator);
            this.columnNameConfigurationViewModel.ColumnNameGenerators.Add(vm);
            this.engine.ApplyNamingGenerators();
        }

        private ColumnNameGeneratorViewModel GetColumnNameGeneratorViewModel(ColumnNameGeneratorPlugIn plugIn)
        {
            ColumnNameGeneratorViewModel vm = this.container.GetExportedValue<ColumnNameGeneratorViewModel>();
            vm.ColumnNameGenerator = plugIn;

            IDisposable subscription = vm.Configure.Subscribe(_ => this.Configure(plugIn));
            this.unsubscribeList.AddSubscriber(vm, subscription);

            subscription = vm.Delete.Subscribe(_ => this.Remove(vm));
            this.unsubscribeList.AddSubscriber(vm, subscription);

            subscription = vm.MoveDown.Subscribe(_ => this.MoveDown(vm));
            this.unsubscribeList.AddSubscriber(vm, subscription);

            subscription = vm.MoveUp.Subscribe(_ => this.MoveUp(vm));
            this.unsubscribeList.AddSubscriber(vm, subscription);

            return vm;
        }

        private void UpdateIsFirstAndLastItem()
        {
            int numberOfGenerators = this.columnNameConfigurationViewModel.ColumnNameGenerators.Count;

            for (int index = 0; index < numberOfGenerators; index++)
            {
                this.columnNameConfigurationViewModel.ColumnNameGenerators[index].IsFirstItem = (index == 0);
                this.columnNameConfigurationViewModel.ColumnNameGenerators[index].IsLastItem = (index == numberOfGenerators - 1);
            }
        }

        private void Configure(ColumnNameGeneratorPlugIn plugIn)
        {
            SettingsViewModel vm = this.container.GetExportedValue<SettingsViewModel>();
            vm.Title = plugIn.Name + " - Settings";
            vm.SettingsObject = plugIn.Settings;
            vm.ShowDialog(this.shellViewModel.Window);
            this.engine.ApplyNamingGenerators();
        }

        private void MoveUp(ColumnNameGeneratorViewModel viewModel)
        {
            this.Move(viewModel, -1);
            this.engine.ApplyNamingGenerators();
        }

        private void MoveDown(ColumnNameGeneratorViewModel viewModel)
        {
            this.Move(viewModel, +1);
            this.engine.ApplyNamingGenerators();
        }

        private void Move(ColumnNameGeneratorViewModel viewModel, int indexChange)
        {
            int oldIndex = this.columnNameConfigurationViewModel.ColumnNameGenerators.IndexOf(viewModel);
            int newIndex = oldIndex + indexChange;

            this.columnNameConfigurationViewModel.ColumnNameGenerators.Move(oldIndex, newIndex);
            this.engine.ColumnNameGenerators.RemoveAt(oldIndex);
            this.engine.ColumnNameGenerators.Insert(newIndex, viewModel.ColumnNameGenerator);
        }

        private void Remove(ColumnNameGeneratorViewModel viewModel)
        {
            this.engine.ColumnNameGenerators.Remove(viewModel.ColumnNameGenerator);
            this.columnNameConfigurationViewModel.ColumnNameGenerators.Remove(viewModel);
            this.engine.ApplyNamingGenerators();
        }
    }
}