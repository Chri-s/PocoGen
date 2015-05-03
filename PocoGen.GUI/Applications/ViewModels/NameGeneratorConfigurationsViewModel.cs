using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace PocoGen.Gui.Applications.ViewModels
{
    [Export]
    internal class NameGeneratorConfigurationsViewModel : ReactiveObject
    {
        public NameGeneratorConfigurationsViewModel()
        {
            this.Tabs = new ReactiveList<object>() { null, null };

            this.WhenAnyValue(x => x.TableNameGenerator).Subscribe(vm =>
            {
                this.Tabs[0] = vm;
                this.SelectedTab = vm;
            });
            this.WhenAnyValue(x => x.ColumnNameGenerator).Subscribe(vm => this.Tabs[1] = vm);
        }

        public string Title { get { return "Naming"; } }

        private TableNameGeneratorConfiguationViewModel tableNameGenerator;
        public TableNameGeneratorConfiguationViewModel TableNameGenerator
        {
            get { return this.tableNameGenerator; }
            set { this.RaiseAndSetIfChanged(ref this.tableNameGenerator, value); }
        }

        private ColumnNameGeneratorConfigurationViewModel columnNameGenerator;
        public ColumnNameGeneratorConfigurationViewModel ColumnNameGenerator
        {
            get { return this.columnNameGenerator; }
            set { this.RaiseAndSetIfChanged(ref this.columnNameGenerator, value); }
        }

        public ReactiveList<object> Tabs { get; private set; }

        private object selectedTab;
        public object SelectedTab
        {
            get { return this.selectedTab; }
            set { this.RaiseAndSetIfChanged(ref this.selectedTab, value); }
        }
    }
}