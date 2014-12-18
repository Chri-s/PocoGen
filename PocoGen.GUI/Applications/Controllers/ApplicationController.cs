using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Dialogs.Controls;
using PocoGen.Common;
using PocoGen.Common.FileFormat;
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
            this.tableNameGeneratorController = tableNameGeneratorController;
            this.columnNameGeneratorController = columnNameGeneratorController;
            this.outputController = outputController;
            this.aboutController = aboutController;
            this.engine = engine;
            this.messageBus = messageBus;
        }

        public void Initialize(string[] args)
        {
            this.connectionController.Initialize();
            this.tableController.Initialize();
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

        private void SaveAs()
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "POCO file definition (*.poco)|*.poco"
            };

            if ((dialog.ShowDialog() ?? false) != true)
            {
                return;
            }

            this.engine.GetDefinition().Save(dialog.FileName);
            this.shellViewModel.DefinitionFilePath = dialog.FileName;
        }

        private void New()
        {
            this.engine.Reset();
            this.shellViewModel.DefinitionFilePath = null;
            this.messageBus.SendMessage(new DefinitionLoaded());
        }

        private void Save()
        {
            if (!string.IsNullOrWhiteSpace(this.shellViewModel.DefinitionFilePath))
            {
                this.engine.GetDefinition().Save(this.shellViewModel.DefinitionFilePath);
            }
            else
            {
                this.SaveAs();
            }
        }

        private void Open()
        {
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
            Definition definition;

            try
            {
                definition = Definition.Load(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while loading definition: " + ex.GetExceptionMessages());
                return;
            }

            List<UnrecognizedPlugIn> unrecognizedPlugIns;

            this.engine.SetFromDefinition(definition, out unrecognizedPlugIns);
            this.shellViewModel.DefinitionFilePath = path;

            if (unrecognizedPlugIns.Count > 0)
            {
                StringBuilder message = new StringBuilder("The following plug ins are not found in your installation. If you save the current poco definition, you will remove these plug ins from the definition.\r\n\r\n");

                foreach (UnrecognizedPlugIn unrecognizedPlugIn in unrecognizedPlugIns)
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

                    message.Append(unrecognizedPlugIn.PlugIn.Name);
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
                    dialog.Controls.Add(new CommonFileDialogLabel("Since your POCO definition is not saved, you must select the path where the output files will be generated:"));
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
                MessageBox.Show(this.shellViewModel.Window, "An error occured while generating the files: " + ex.Message, "Poco Generator");
            }

            this.shellViewModel.IsBusy = false;
        }
    }
}