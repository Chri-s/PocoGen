using System;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Linq;
using MvvmHybridFramework;
using PocoGen.Gui.Applications.Views;
using PocoGen.Gui.Domain;
using ReactiveUI;

namespace PocoGen.Gui.Applications.ViewModels
{
    [Export]
    internal class ConnectionViewModel : ReactiveValidatingViewModel<IConnectionView>
    {
        [ImportingConstructor]
        public ConnectionViewModel(IConnectionView view)
            : base(view)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.WhenAnyValue(x => x.SelectedSchemaReader)
                .Select(x => (x == null) ? false : !string.IsNullOrEmpty(x.ConnectionStringHelpUrl))
                .ToProperty(this, x => x.IsConnectionStringHelpEnabled, out this.isConnectionStringHelpEnabled);

            this.WhenAnyValue(x => x.SelectedSchemaReader)
                .Select(x => (x == null) ? string.Empty : x.ConnectionStringHelpUrl)
                .ToProperty(this, x => x.ConnectionStringHelpUrl, out this.connectionStringHelpUrl);

            IObservable<bool> arePropertiesSet = this.WhenAny(x => x.SelectedSchemaReader, x => x.ConnectionString, (schemaReader, connString) => schemaReader.GetValue() != null && !string.IsNullOrWhiteSpace(this.ConnectionString));

            this.TestConnection = ReactiveCommand.Create(arePropertiesSet);
            this.LoadTables = ReactiveCommand.Create(arePropertiesSet);
            this.ConfigureSelectedSchemaReader = ReactiveCommand.Create(this.WhenAnyValue(x => x.SelectedSchemaReader).Select(x => x != null && x.HasSettings));
        }

        public string Title
        {
            get { return "Connection"; }
        }

        private string connectionString;
        [Required]
        public string ConnectionString
        {
            get { return this.connectionString; }
            set { this.RaiseAndSetIfChanged(ref this.connectionString, value); }
        }

        private bool useAnsiQuoting;
        public bool UseAnsiQuoting
        {
            get { return this.useAnsiQuoting; }
            set { this.RaiseAndSetIfChanged(ref this.useAnsiQuoting, value); }
        }

        private readonly ReactiveList<SchemaReaderModel> schemaReaders = new ReactiveList<SchemaReaderModel>();
        public ReactiveList<SchemaReaderModel> SchemaReaders
        {
            get { return this.schemaReaders; }
        }

        private SchemaReaderModel selectedSchemaReader;
        [Required]
        public SchemaReaderModel SelectedSchemaReader
        {
            get { return this.selectedSchemaReader; }
            set { this.RaiseAndSetIfChanged(ref this.selectedSchemaReader, value); }
        }

        private ObservableAsPropertyHelper<string> connectionStringHelpUrl;
        public string ConnectionStringHelpUrl
        {
            get { return this.connectionStringHelpUrl.Value; }
        }

        private ObservableAsPropertyHelper<bool> isConnectionStringHelpEnabled;
        public bool IsConnectionStringHelpEnabled
        {
            get { return this.isConnectionStringHelpEnabled.Value; }
        }

        public ReactiveCommand<object> TestConnection { get; private set; }

        public ReactiveCommand<object> LoadTables { get; private set; }

        public ReactiveCommand<object> ConfigureSelectedSchemaReader { get; private set; }
    }
}