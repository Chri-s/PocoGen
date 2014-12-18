using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using MvvmHybridFramework;
using PocoGen.Common;
using PocoGen.Gui.Applications.Messages;
using PocoGen.Gui.Applications.ViewModels;
using ReactiveUI;

namespace PocoGen.Gui.Applications.Controllers
{
    [Export]
    internal class TableNameGeneratorController
    {
        private readonly TableNameGeneratorConfiguationViewModel configurationViewModel;
        private readonly CompositionContainer container;
        private readonly ShellViewModel shellViewModel;
        private readonly IMessageBus messageBus;
        private readonly UnsubscriptionList<TableNameGeneratorViewModel> unsubscribeList = new UnsubscriptionList<TableNameGeneratorViewModel>();

        [ImportingConstructor]
        public TableNameGeneratorController(TableNameGeneratorConfiguationViewModel configurationViewModel, CompositionContainer container, ShellViewModel shellViewModel, IMessageBus messageBus)
        {
            this.configurationViewModel = configurationViewModel;
            this.container = container;
            this.shellViewModel = shellViewModel;
            this.messageBus = messageBus;
        }

        [Import]
        public Engine Engine { get; set; }

        public void Initialize()
        {
            this.shellViewModel.TableNameGeneratorConfiguationViewModel = this.configurationViewModel;

            IEnumerable<TableNameGeneratorPlugIn> availableTableNameGenerators = from generator in this.Engine.AvailableTableNameGenerators
                                                                                 select new TableNameGeneratorPlugIn(generator);

            this.configurationViewModel.TableNameGenerators.Changed.Subscribe(_ => this.UpdateIsFirstAndLastItem());
            this.configurationViewModel.TableNameGenerators.ItemsRemoved.Subscribe(vm =>
            {
                this.Engine.TableNameGenerators.Remove(vm.TableNameGenerator);
                this.unsubscribeList.Unsubscribe(vm);
            });

            this.configurationViewModel.AvailableTableNameGenerators.AddRange(availableTableNameGenerators);

            this.configurationViewModel.Add.Subscribe(_ => this.AddSelectedTableNameGenerator());

            this.messageBus.Listen<DefinitionLoaded>().Subscribe(_ => this.DefinitionLoaded());
        }

        private void DefinitionLoaded()
        {
            this.configurationViewModel.TableNameGenerators.Clear();

            foreach (TableNameGeneratorPlugIn tableNameGenerator in this.Engine.TableNameGenerators)
            {
                this.configurationViewModel.TableNameGenerators.Add(this.GetTableNameGeneratorViewModel(tableNameGenerator));
            }
        }

        private void AddSelectedTableNameGenerator()
        {
            TableNameGeneratorViewModel vm = this.GetTableNameGeneratorViewModel(this.configurationViewModel.SelectedTableNameGeneratorToAdd.Clone());
            this.Engine.TableNameGenerators.Add(vm.TableNameGenerator);
            this.configurationViewModel.TableNameGenerators.Add(vm);
            this.Engine.ApplyNamingGenerators();
        }

        private TableNameGeneratorViewModel GetTableNameGeneratorViewModel(TableNameGeneratorPlugIn plugIn)
        {
            TableNameGeneratorViewModel vm = this.container.GetExportedValue<TableNameGeneratorViewModel>();
            vm.TableNameGenerator = plugIn;

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
            int numberOfGenerators = this.configurationViewModel.TableNameGenerators.Count;

            for (int index = 0; index < numberOfGenerators; index++)
            {
                this.configurationViewModel.TableNameGenerators[index].IsFirstItem = (index == 0);
                this.configurationViewModel.TableNameGenerators[index].IsLastItem = (index == numberOfGenerators - 1);
            }
        }

        private void Configure(TableNameGeneratorPlugIn plugIn)
        {
            SettingsViewModel vm = this.container.GetExportedValue<SettingsViewModel>();
            vm.Title = plugIn.Name + " - Settings";
            vm.SettingsObject = plugIn.Settings;
            vm.ShowDialog(this.shellViewModel.Window);
            this.Engine.ApplyNamingGenerators();
        }

        private void MoveUp(TableNameGeneratorViewModel viewModel)
        {
            this.Move(viewModel, -1);
            this.Engine.ApplyNamingGenerators();
        }

        private void MoveDown(TableNameGeneratorViewModel viewModel)
        {
            this.Move(viewModel, +1);
            this.Engine.ApplyNamingGenerators();
        }

        private void Move(TableNameGeneratorViewModel viewModel, int indexChange)
        {
            int oldIndex = this.configurationViewModel.TableNameGenerators.IndexOf(viewModel);
            int newIndex = oldIndex + indexChange;

            this.configurationViewModel.TableNameGenerators.Move(oldIndex, newIndex);
            this.Engine.TableNameGenerators.RemoveAt(oldIndex);
            this.Engine.TableNameGenerators.Insert(newIndex, viewModel.TableNameGenerator);
        }

        private void Remove(TableNameGeneratorViewModel viewModel)
        {
            this.Engine.TableNameGenerators.Remove(viewModel.TableNameGenerator);
            this.configurationViewModel.TableNameGenerators.Remove(viewModel);
            this.Engine.ApplyNamingGenerators();
        }
    }
}