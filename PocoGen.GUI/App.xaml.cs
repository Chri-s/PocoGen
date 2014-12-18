using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using System.Windows;
using PocoGen.Gui.Applications.Controllers;
using ReactiveUI;

namespace PocoGen.Gui
{
    public partial class App : Application
    {
        private AggregateCatalog catalog;
        private CompositionContainer container;
        private ApplicationController controller;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.catalog = new AggregateCatalog();

            // Add the PocoGen.GUI assembly to the catalog
            this.catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            // Load schema readers and engine
            this.catalog.Catalogs.Add(new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));

            this.container = new CompositionContainer(this.catalog, CompositionOptions.DisableSilentRejection | CompositionOptions.IsThreadSafe);
            CompositionBatch batch = new CompositionBatch();
            batch.AddExportedValue(this.container);
            batch.AddExportedValue(typeof(IMessageBus).FullName, MessageBus.Current);
            this.container.Compose(batch);

            this.controller = this.container.GetExportedValue<ApplicationController>();
            this.controller.Initialize(e.Args);
            this.controller.Run();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            this.controller.Shutdown();
            this.container.Dispose();
            this.catalog.Dispose();

            base.OnExit(e);
        }
    }
}