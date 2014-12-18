using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using PocoGen.Common;
using PocoGen.Gui.Applications.Messages;
using PocoGen.Gui.Applications.ViewModels;
using ReactiveUI;

namespace PocoGen.Gui.Applications.Controllers
{
    [Export]
    internal class ConnectionController
    {
        private readonly ConnectionViewModel connectionViewModel;
        private readonly IMessageBus messageBus;
        private readonly ShellViewModel shellViewModel;
        private readonly CompositionContainer container;

        [Import]
        public Engine Engine { get; set; }

        [ImportingConstructor]
        public ConnectionController(ShellViewModel shellViewModel, ConnectionViewModel connectionViewModel, IMessageBus messageBus, CompositionContainer container)
        {
            this.connectionViewModel = connectionViewModel;
            this.shellViewModel = shellViewModel;
            this.messageBus = messageBus;
            this.container = container;
            shellViewModel.ConnectionViewModel = connectionViewModel;
        }

        public void Initialize()
        {
            foreach (Lazy<ISchemaReader, ISchemaReaderMetadata> schemaReader in this.Engine.AvailableSchemaReaders.OrderBy(sr => sr.Metadata.Name))
            {
                this.connectionViewModel.SchemaReaders.Add(new Domain.SchemaReaderModel(schemaReader));
            }

            this.connectionViewModel.WhenAnyValue(x => x.ConnectionString).Subscribe(cs => this.Engine.ConnectionString = cs);
            this.connectionViewModel.WhenAny(x => x.SelectedSchemaReader, x => (x.GetValue() == null) ? null : x.GetValue().SchemaReader).Subscribe(sr => this.Engine.SchemaReader = sr);
            this.connectionViewModel.WhenAnyValue(x => x.UseAnsiQuoting).Subscribe(qu => this.Engine.UseAnsiQuoting = qu);

            this.connectionViewModel.TestConnection.Subscribe(_ => this.TestConnection());
            this.connectionViewModel.LoadTables.Subscribe(_ => this.messageBus.SendMessage(new LoadTables()));
            this.connectionViewModel.ConfigureSelectedSchemaReader.Subscribe(_ => this.ShowSettings());

            this.messageBus.Listen<DefinitionLoaded>().ObserveOnDispatcher().Subscribe(_ => this.DefinitionLoaded());
        }

        private void DefinitionLoaded()
        {
            this.connectionViewModel.ConnectionString = this.Engine.ConnectionString;
            this.connectionViewModel.UseAnsiQuoting = this.Engine.UseAnsiQuoting;

            if (this.Engine.SchemaReader == null)
            {
                this.connectionViewModel.SelectedSchemaReader = null;
            }
            else
            {
                Domain.SchemaReaderModel schemaReader = this.connectionViewModel.SchemaReaders.FirstOrDefault(sr => sr.Guid == this.Engine.SchemaReader.Guid);
                this.connectionViewModel.SelectedSchemaReader = schemaReader;
            }
        }

        private async void TestConnection()
        {
            this.shellViewModel.BusyMessage = "Testing connection";
            this.shellViewModel.IsBusy = true;

            string errorMessages = await this.Engine.TestConnection();

            if (string.IsNullOrEmpty(errorMessages))
            {
                MessageBox.Show("Connection succeeded.");
            }
            else
            {
                MessageBox.Show(errorMessages);
            }

            this.shellViewModel.IsBusy = false;
        }

        private void ShowSettings()
        {
            SettingsViewModel vm = this.container.GetExportedValue<SettingsViewModel>();
            vm.Title = this.connectionViewModel.SelectedSchemaReader.DatabaseType + " - Settings";
            vm.SettingsObject = this.connectionViewModel.SelectedSchemaReader.SchemaReader.Settings;
            vm.ShowDialog(this.shellViewModel.Window);
        }
    }
}