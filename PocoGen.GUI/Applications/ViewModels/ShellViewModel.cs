using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using MvvmHybridFramework;
using PocoGen.Gui.Applications.Views;
using ReactiveUI;

namespace PocoGen.Gui.Applications.ViewModels
{
    [Export]
    internal class ShellViewModel : ReactiveViewModel<IShellView>
    {
        public event CancelEventHandler Closing;

        private readonly ReactiveCommand<object> exitCommand;

        [ImportingConstructor]
        public ShellViewModel(IShellView view)
            : base(view)
        {
            this.exitCommand = ReactiveCommand.Create();
            this.exitCommand.Subscribe(_ => this.Close());

            this.Tabs = new ReactiveList<object>() { null, null, null, null };

            this.New = ReactiveCommand.Create();
            this.SaveDefinition = ReactiveCommand.Create();
            this.SaveDefinitionAs = ReactiveCommand.Create();
            this.OpenDefinition = ReactiveCommand.Create();
            this.Generate = ReactiveCommand.Create();
            this.About = ReactiveCommand.Create();

            this.WhenAnyValue(x => x.ConnectionViewModel).Subscribe(vm =>
            {
                this.Tabs[0] = vm;
                this.SelectedTab = vm;
            });
            this.WhenAnyValue(x => x.NameGeneratorConfigurationsViewModel).Subscribe(vm => this.Tabs[1] = vm);
            this.WhenAnyValue(x => x.TableListViewModel).Subscribe(vm => this.Tabs[2] = vm);
            this.WhenAnyValue(x => x.OutputWriterConfigurationViewModel).Subscribe(vm => this.Tabs[3] = vm);

            this.ViewCore.Closing += (sender, e) => this.OnClosing(e);
        }

        public string Title
        {
            get { return "Poco Generator"; }
        }

        public ICommand ExitCommand
        {
            get { return this.exitCommand; }
        }

        public ReactiveCommand<object> SaveDefinition { get; private set; }

        public ReactiveCommand<object> SaveDefinitionAs { get; private set; }

        public ReactiveCommand<object> OpenDefinition { get; private set; }

        public ReactiveCommand<object> Generate { get; private set; }

        public ReactiveCommand<object> New { get; private set; }

        public ReactiveCommand<object> About { get; private set; }

        public ReactiveList<object> Tabs { get; private set; }

        private ConnectionViewModel connectionViewModel;
        public ConnectionViewModel ConnectionViewModel
        {
            get { return this.connectionViewModel; }
            set { this.RaiseAndSetIfChanged(ref this.connectionViewModel, value); }
        }

        private NameGeneratorConfigurationsViewModel nameGeneratorConfigurationsViewModel;
        public NameGeneratorConfigurationsViewModel NameGeneratorConfigurationsViewModel
        {
            get { return this.nameGeneratorConfigurationsViewModel; }
            set { this.RaiseAndSetIfChanged(ref this.nameGeneratorConfigurationsViewModel, value); }
        }

        private TableListViewModel tableListViewModel;
        public TableListViewModel TableListViewModel
        {
            get { return this.tableListViewModel; }
            set { this.RaiseAndSetIfChanged(ref this.tableListViewModel, value); }
        }

        private OutputWriterConfigurationViewModel outputWriterConfigurationViewModel;
        public OutputWriterConfigurationViewModel OutputWriterConfigurationViewModel
        {
            get { return this.outputWriterConfigurationViewModel; }
            set { this.RaiseAndSetIfChanged(ref this.outputWriterConfigurationViewModel, value); }
        }

        private object selectedTab;
        public object SelectedTab
        {
            get { return this.selectedTab; }
            set { this.RaiseAndSetIfChanged(ref this.selectedTab, value); }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return this.isBusy; }
            set { this.RaiseAndSetIfChanged(ref this.isBusy, value); }
        }

        private string busyMessage;
        public string BusyMessage
        {
            get { return this.busyMessage; }
            set { this.RaiseAndSetIfChanged(ref this.busyMessage, value); }
        }

        private string definitionFilePath;
        public string DefinitionFilePath
        {
            get { return this.definitionFilePath; }
            set { this.RaiseAndSetIfChanged(ref this.definitionFilePath, value); }
        }

        public void Show()
        {
            this.ViewCore.Show();
        }

        private void Close()
        {
            this.ViewCore.Close();
        }

        public Window Window
        {
            get
            {
                return this.ViewCore as Window;
            }
        }

        private void OnClosing(CancelEventArgs e)
        {
            CancelEventHandler handler = this.Closing;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}