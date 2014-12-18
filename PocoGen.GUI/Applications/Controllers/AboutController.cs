using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using PocoGen.Common;
using PocoGen.Gui.Applications.ViewModels;

namespace PocoGen.Gui.Applications.Controllers
{
    [Export]
    internal class AboutController
    {
        private readonly CompositionContainer container;
        private readonly ShellViewModel shellViewModel;
        private readonly Engine engine;

        [ImportingConstructor]
        public AboutController(CompositionContainer container, ShellViewModel shellViewModel, Engine engine)
        {
            this.container = container;
            this.shellViewModel = shellViewModel;
            this.engine = engine;
        }

        public void Initialize()
        {
            this.shellViewModel.About.Subscribe(_ => this.Show());
        }

        public void Show()
        {
            AboutViewModel viewModel = this.container.GetExportedValue<AboutViewModel>();

            viewModel.Version = this.GetType().Assembly.GetName().Version.ToString(4);

            List<AssemblyName> assemblyNames = new List<AssemblyName>();

            foreach (Lazy<ISchemaReader, ISchemaReaderMetadata> availableSchemaReader in this.engine.AvailableSchemaReaders)
            {
                AssemblyName name = availableSchemaReader.Value.GetType().Assembly.GetName();
                if (!assemblyNames.Any(n => n.Name == name.Name))
                {
                    assemblyNames.Add(name);
                }
            }

            foreach (Lazy<ITableNameGenerator, ITableNameGeneratorMetadata> availableTableNameGenerator in this.engine.AvailableTableNameGenerators)
            {
                AssemblyName name = availableTableNameGenerator.Value.GetType().Assembly.GetName();
                if (!assemblyNames.Any(n => n.Name == name.Name))
                {
                    assemblyNames.Add(name);
                }
            }

            foreach (Lazy<IColumnNameGenerator, IColumnNameGeneratorMetadata> availableColumnNameGenerator in this.engine.AvailableColumnNameGenerators)
            {
                AssemblyName name = availableColumnNameGenerator.Value.GetType().Assembly.GetName();
                if (!assemblyNames.Any(n => n.Name == name.Name))
                {
                    assemblyNames.Add(name);
                }
            }

            foreach (Lazy<IOutputWriter, IOutputWriterMetadata> availableOutputWriter in this.engine.AvailableOutputWriters)
            {
                AssemblyName name = availableOutputWriter.Value.GetType().Assembly.GetName();
                if (!assemblyNames.Any(n => n.Name == name.Name))
                {
                    assemblyNames.Add(name);
                }
            }

            var sortedNames = from an in assemblyNames
                              orderby an.Name
                              select an;

            foreach (AssemblyName name in sortedNames)
            {
                viewModel.Modules.Add(name);
            }

            viewModel.Show(this.shellViewModel.Window);
        }
    }
}