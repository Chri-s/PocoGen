using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using ReactiveUI;

namespace PocoGen.Gui.Applications.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class TableViewModel : ReactiveObject
    {
        public TableViewModel()
            : base()
        {
            this.Columns = new ReactiveList<ColumnViewModel>();
        }

        private bool include;
        public bool Include
        {
            get { return this.include; }
            set { this.RaiseAndSetIfChanged(ref this.include, value); }
        }

        private string tableName;
        public string TableName
        {
            get { return this.tableName; }
            set { this.RaiseAndSetIfChanged(ref this.tableName, value); }
        }

        private string className;
        [Required(AllowEmptyStrings = false)]
        public string ClassName
        {
            get { return this.className; }
            set { this.RaiseAndSetIfChanged(ref this.className, value); }
        }

        private bool isView;
        public bool IsView
        {
            get { return this.isView; }
            set { this.RaiseAndSetIfChanged(ref this.isView, value); }
        }

        private bool isExpanded;
        public bool IsExpanded
        {
            get { return this.isExpanded; }
            set { this.RaiseAndSetIfChanged(ref this.isExpanded, value); }
        }

        public ReactiveList<ColumnViewModel> Columns { get; private set; }
    }
}