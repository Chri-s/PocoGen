using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Dialogs.Controls;
using PocoGen.Common;
using PocoGen.Gui.Applications.Messages;
using PocoGen.Gui.Applications.ViewModels;
using ReactiveUI;

namespace PocoGen.Gui.Applications.Controllers
{
    [Export]
    internal class ApplicationController
    {
        private readonly ColumnNameGeneratorController columnNameGeneratorController;
        private readonly ShellViewModel shellViewModel;
        private readonly ConnectionController connectionController;
        private readonly TableController tableController;
        private readonly NameGeneratorConfigurationsController nameGeneratorController;
        private readonly TableNameGeneratorController tableNameGeneratorController;
        private readonly OutputController outputController;
        private readonly AboutController aboutController;
        private readonly Engine engine;
        private readonly IMessageBus messageBus;

        [ImportingConstructor]
        public ApplicationController(
            ShellViewModel shellViewModel,
            ConnectionController connectionController,
            TableController tableController,
            NameGeneratorConfigurationsController nameGeneratorController,
            TableNameGeneratorController tableNameGeneratorController,
            ColumnNameGeneratorController columnNameGeneratorController,
            OutputController outputController,
            AboutController aboutController,
            Engine engine,
            IMessageBus messageBus)
        {
            this.shellViewModel = shellViewModel;
            this.connectionController = connectionController;
            this.tableController = tableController;
            this.nameGeneratorController = nameGeneratorController;
            this.tableNameGeneratorController = tableNameGeneratorController;
            this.columnNameGeneratorController = columnNameGeneratorController;
            this.outputController = outputController;
            this.aboutController = aboutController;
            this.engine = engine;
            this.messageBus = messageBus;

            this.shellViewModel.Closing += this.ShellViewModel_Closing;
        }

        public void Initialize(string[] args)
        {
            this.connectionController.Initialize();
            this.tableController.Initialize();
            this.nameGeneratorController.Initialize();
            this.tableNameGeneratorController.Initialize();
            this.columnNameGeneratorController.Initialize();
            this.outputController.Initialize();
            this.aboutController.Initialize();

            this.shellViewModel.New.Subscribe(_ => this.New());
            this.shellViewModel.SaveDefinition.Subscribe(_ => this.Save());
            this.shellViewModel.SaveDefinitionAs.Subscribe(_ => this.SaveAs());
            this.shellViewModel.OpenDefinition.Subscribe(_ => this.Open());
            this.shellViewModel.Generate.Subscribe(_ => this.Generate());

            if (args.Length == 1)
            {
                this.Open(args[0]);
            }
        }

        public void Run()
        {
            this.shellViewModel.Show();
        }

        public void Shutdown()
        {
        }

        private void ShellViewModel_Closing(object sender, CancelEventArgs e)
        {
            if (!this.engine.IsChanged)
            {
                return;
            }

            MessageBoxResult result = MessageBox.Show(this.shellViewModel.Window, "Your project has unsaved changed. Do you want to save them?", "PocoGen", MessageBoxButton.YesNoCancel, MessageBoxImage.None, MessageBoxResult.Cancel);

            if (result == MessageBoxResult.Yes)
            {
                if (!this.Save())
                {
                    e.Cancel = true;
                }
            }
            else if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private bool SaveAs()
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "POCO file definition (*.poco)|*.poco"
            };

            if ((dialog.ShowDialog() ?? false) != true)
            {
                return false;
            }

            this.engine.Save(dialog.FileName);
            this.shellViewModel.DefinitionFilePath = dialog.FileName;

            return true;
        }

        private void New()
        {
            if (this.engine.IsChanged)
            {
                if (MessageBox.Show(this.shellViewModel.Window, "Your project has unsaved changes. Do you really want to create a new project and lose those changes?", "PocoGen", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No) == MessageBoxResult.No)
                {
                    return;
                }
            }

            this.engine.Reset();
            this.shellViewModel.DefinitionFilePath = null;
            this.messageBus.SendMessage(new DefinitionLoaded());
        }

        private bool Save()
        {
            if (!string.IsNullOrWhiteSpace(this.shellViewModel.DefinitionFilePath))
            {
                this.engine.Save(this.shellViewModel.DefinitionFilePath);
                return true;
            }
            else
            {
                return this.SaveAs();
            }
        }

        private void Open()
        {
            if (this.engine.IsChanged)
            {
                if (MessageBox.Show(this.shellViewModel.Window, "Your project has unsaved changes. Do you really want to open a project and lose those changes?", "PocoGen", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No) == MessageBoxResult.No)
                {
                    return;
                }
            }

            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "POCO file definition (*.poco)|*.poco"
            };

            if ((dialog.ShowDialog(this.shellViewModel.Window) ?? false) != true)
            {
                return;
            }

            this.Open(dialog.FileName);
        }

        private void Open(string path)
        {
            List<UnknownPlugIn> unrecognizedPlugIns;

            try
            {
                this.engine.Load(path, out unrecognizedPlugIns);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while loading definition: " + ex.GetExceptionMessages());
                return;
            }

            this.shellViewModel.DefinitionFilePath = path;

            if (unrecognizedPlugIns.Count > 0)
            {
                StringBuilder message = new StringBuilder("The following plug ins are not found in your installation. If you save the current project, you will remove these plug ins from the project.\r\n\r\n");

                foreach (UnknownPlugIn unrecognizedPlugIn in unrecognizedPlugIns)
                {
                    switch (unrecognizedPlugIn.PlugInType)
                    {
                        case PlugInType.SchemaReader:
                            message.Append("Schema Reader: ");
                            break;

                        case PlugInType.TableNameGenerator:
                            message.Append("Table Name Generator: ");
                            break;

                        case PlugInType.ColumnNameGenerator:
                            message.Append("Column Name Generator: ");
                            break;

                        case PlugInType.OutputWriter:
                            message.Append("Output Writer: ");
                            break;

                        default:
                            message.Append(Enum.GetName(typeof(PlugInType), unrecognizedPlugIn.PlugInType) + ": ");
                            break;
                    }

                    message.Append(unrecognizedPlugIn.Name);
                }

                MessageBox.Show(this.shellViewModel.Window, message.ToString(), "Poco Generator");
            }

            this.messageBus.SendMessage(new DefinitionLoaded());
        }

        private async void Generate()
        {
            string basePath;

            if (string.IsNullOrEmpty(this.shellViewModel.DefinitionFilePath))
            {
                using (CommonOpenFileDialog dialog = new CommonOpenFileDialog("Select base path"))
                {
                    dialog.Controls.Add(new CommonFileDialogLabel("Since your project is not saved, you must select the path where the output files will be generated:"));
                    dialog.IsFolderPicker = true;
                    dialog.Multiselect = false;
                    if (dialog.ShowDialog(this.shellViewModel.Window) != CommonFileDialogResult.Ok)
                    {
                        return;
                    }

                    basePath = dialog.FileName;
                }
            }
            else
            {
                basePath = System.IO.Path.GetDirectoryName(this.shellViewModel.DefinitionFilePath);
            }

            this.shellViewModel.BusyMessage = "Generating output...";
            this.shellViewModel.IsBusy = true;
            try
            {
                await this.engine.Generate(basePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.shellViewModel.Window, "An error occured while generating the files: " + ex.Message, "PocoGen");
            }

            this.shellViewModel.IsBusy = false;
        }
    }
}