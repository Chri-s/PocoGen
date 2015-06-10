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
                            TableViewModel tableVm = this.container.GetExportedValue<TableViewModel>();
                            tableVm.TableName = table.Name;
                            tableVm.ClassName = table.EffectiveClassName;
                            tableVm.Include = !table.Ignore;
                            tableVm.IsView = table.IsView;
                            tableVm.WhenAnyValue(vm => vm.ClassName).Subscribe(cn => table.EffectiveClassName = cn);
                            tableVm.WhenAnyValue(vm => vm.Include).Subscribe(i => table.Ignore = !i);
                            table.WhenAnyValue(t => t.EffectiveClassName).Subscribe(ecf => tableVm.ClassName = ecf);
                            table.WhenAnyValue(t => t.Ignore).Subscribe(i => tableVm.Include = !i);

                            foreach (Column column in table.Columns)
                            {
                                ColumnViewModel columnVm = this.container.GetExportedValue<ColumnViewModel>();
                                columnVm.ColumnName = column.Name;
                                columnVm.Included = !column.Ignore;
                                columnVm.IsPrimaryKey = column.IsPK;
                                columnVm.PropertyName = column.EffectivePropertyName;
                                columnVm.TableName = table.Name;
                                columnVm.WhenAnyValue(vm => vm.PropertyName).Subscribe(pn => column.EffectivePropertyName = pn);
                                columnVm.WhenAnyValue(vm => vm.Included).Subscribe(i => column.Ignore = !i);
                                column.WhenAnyValue(c => c.EffectivePropertyName).Subscribe(epn => columnVm.PropertyName = epn);
                                column.WhenAnyValue(c => c.Ignore).Subscribe(i => columnVm.Included = !i);
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