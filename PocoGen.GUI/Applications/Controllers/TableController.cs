using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using PocoGen.Common;
using PocoGen.Gui.Applications.ViewModels;
using ReactiveUI;

namespace PocoGen.Gui.Applications.Controllers
{
    [Export]
    internal class TableController
    {
        private readonly CompositionContainer container;
        private readonly IMessageBus messageBus;
        private readonly TableListViewModel tableListViewModel;
        private readonly ShellViewModel shellViewModel;
        private readonly Engine engine;

        [ImportingConstructor]
        public TableController(TableListViewModel tableListViewModel, ShellViewModel shellViewModel, CompositionContainer container, Engine engine, IMessageBus messageBus)
        {
            this.tableListViewModel = tableListViewModel;
            this.shellViewModel = shellViewModel;
            this.container = container;
            this.engine = engine;
            this.messageBus = messageBus;

            this.messageBus.Listen<Messages.LoadTables>().ObserveOnDispatcher().Subscribe(_ => this.LoadTables());
            this.messageBus.Listen<Messages.DefinitionLoaded>().ObserveOnDispatcher().Subscribe(_ => this.DefinitionLoaded());
        }

        public void Initialize()
        {
            this.tableListViewModel.SelectAll.Subscribe(_ => this.SelectAll());
            this.tableListViewModel.UnselectAll.Subscribe(_ => this.UnselectAll());
            this.shellViewModel.TableListViewModel = this.tableListViewModel;
            this.engine.TableRenamed += this.TableRenamed;
        }

        private void TableRenamed(object sender, TableEventArgs e)
        {
            TableViewModel tableVm = this.tableListViewModel.Tables.FirstOrDefault(table => table.TableName == e.Table.Name);

            // This happens when the tables are loaded but not added to the list
            if (tableVm == null)
            {
                return;
            }

            TableChange tableChange = this.engine.TableChanges[e.Table.Name];
            tableVm.ClassName = (tableChange == null || string.IsNullOrEmpty(tableChange.ClassName)) ? e.Table.GeneratedClassName : tableChange.ClassName;

            foreach (ColumnViewModel columnVm in tableVm.Columns)
            {
                Column column = e.Table.Columns[columnVm.ColumnName];
                PocoGen.Common.ColumnChange columnChange = tableChange == null ? null : tableChange.Columns[columnVm.ColumnName];

                columnVm.PropertyName = (columnChange == null || string.IsNullOrEmpty(columnChange.PropertyName)) ? column.GeneratedPropertyName : columnChange.PropertyName;
            }
        }

        private void DefinitionLoaded()
        {
            this.tableListViewModel.Tables.Clear();
        }

        private async void LoadTables()
        {
            this.shellViewModel.BusyMessage = "Reading schema...";
            this.shellViewModel.IsBusy = true;

            try
            {
                await this.engine.LoadTables();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while reading schema: " + ex.Message);
            }

            using (var x = this.tableListViewModel.Tables.SuppressChangeNotifications())
            {
                await Task.Run(() =>
                {
                    this.tableListViewModel.Tables.Clear();

                    object lockObject = new object();

                    Parallel.ForEach(
                        this.engine.Tables,
                        () => new List<TableViewModel>(),
                        (table, loopState, list) =>
                        {
                            TableChange tableChange = this.engine.TableChanges[table.Name];
                            TableViewModel tableVm = this.container.GetExportedValue<TableViewModel>();
                            tableVm.TableName = table.Name;
                            tableVm.ClassName = (tableChange == null || string.IsNullOrEmpty(tableChange.ClassName)) ? table.GeneratedClassName : tableChange.ClassName;
                            tableVm.Include = (tableChange == null) ? true : !tableChange.Ignore;
                            tableVm.IsView = table.IsView;
                            tableVm.WhenAny(
                                vm => vm.ClassName,
                                vm => vm.Include,
                                vm => vm.TableName,
                                (className, included, tableName) => new { ClassName = className.GetValue(), Included = included.GetValue(), TableName = tableName.GetValue() })
                                .Subscribe(data => this.SyncTableChange(data.TableName, data.ClassName, !data.Included));

                            foreach (Column column in table.Columns)
                            {
                                ColumnChange columnChange = (tableChange == null) ? null : tableChange.Columns[column.Name];
                                ColumnViewModel columnVm = this.container.GetExportedValue<ColumnViewModel>();
                                columnVm.ColumnName = column.Name;
                                columnVm.Included = (columnChange == null) ? true : !columnChange.Ignore;
                                columnVm.IsPrimaryKey = column.IsPK;
                                columnVm.PropertyName = (columnChange == null || string.IsNullOrEmpty(columnChange.PropertyName)) ? column.GeneratedPropertyName : columnChange.PropertyName;
                                columnVm.TableName = table.Name;
                                columnVm.WhenAny(vm => vm.TableName, vm => vm.ColumnName, vm => vm.PropertyName, vm => vm.Included, (tableName, columnName, propertyName, included) => new { TableName = tableName.GetValue(), ColumnName = columnName.GetValue(), PropertyName = propertyName.GetValue(), Included = included.GetValue() }).Subscribe(data => this.SyncColumnChange(data.TableName, data.ColumnName, data.PropertyName, !data.Included));
                                tableVm.Columns.Add(columnVm);
                            }

                            list.Add(tableVm);

                            return list;
                        },
                        list =>
                        {
                            lock (lockObject)
                            {
                                this.tableListViewModel.Tables.AddRange(list);
                            }
                        });
                });
            }

            this.shellViewModel.IsBusy = false;
        }

        private void SyncTableChange(string tableName, string className, bool ignore)
        {
            TableChange tableChange = this.engine.TableChanges[tableName];
            Table table = this.engine.Tables[tableName];

            if (tableChange == null)
            {
                if (TableChange.NeedsTableChange(table, ignore, className))
                {
                    tableChange = new TableChange(tableName, className, ignore);
                    this.engine.TableChanges.Add(tableChange);
                }
            }
            else
            {
                tableChange.ClassName = (table.GeneratedClassName == className) ? string.Empty : className;
                tableChange.Ignore = ignore;

                if (!tableChange.HasChangesTo(table))
                {
                    this.engine.TableChanges.Remove(tableChange);
                }
            }
        }

        private void SyncColumnChange(string tableName, string columnName, string propertyName, bool ignore)
        {
            Table table = this.engine.Tables[tableName];
            Column column = table.Columns[columnName];

            TableChange tableChange = this.engine.TableChanges[tableName];
            PocoGen.Common.ColumnChange columnChange = (tableChange == null) ? null : tableChange.Columns[columnName];

            if (columnChange == null)
            {
                if (PocoGen.Common.ColumnChange.NeedsColumnChange(column, ignore, propertyName))
                {
                    if (tableChange == null)
                    {
                        tableChange = new TableChange(tableName, string.Empty, false);
                        this.engine.TableChanges.Add(tableChange);
                    }

                    tableChange.Columns.Add(new PocoGen.Common.ColumnChange(columnName, propertyName, ignore));
                }
            }
            else
            {
                columnChange.PropertyName = (column.GeneratedPropertyName == propertyName) ? string.Empty : propertyName;
                columnChange.Ignore = ignore;

                if (!columnChange.HasChangesTo(column))
                {
                    tableChange.Columns.Remove(columnChange);

                    // Remove the table change if it was only needed for the column change
                    if (!tableChange.HasChangesTo(table))
                    {
                        this.engine.TableChanges.Remove(tableChange);
                    }
                }
            }
        }

        private void SelectAll()
        {
            foreach (TableViewModel table in this.tableListViewModel.TableView)
            {
                table.Include = true;
            }
        }

        private void UnselectAll()
        {
            foreach (TableViewModel table in this.tableListViewModel.TableView)
            {
                table.Include = false;
            }
        }
    }
}