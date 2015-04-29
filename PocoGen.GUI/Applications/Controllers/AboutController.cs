using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
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
            this.StartUpdateCheck(viewModel);

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

        private void StartUpdateCheck(AboutViewModel viewModel)
        {
            Version myVersion = this.GetType().Assembly.GetName().Version;
            myVersion = new Version("1.0.0.0");
            Task<Version> newestVersionTask = this.GetLatestVersionTask();
            viewModel.UpdateStatusText = "Checking for updates...";

            newestVersionTask.ContinueWith(t =>
            {
                if (t.IsFaulted || t.Result == null)
                {
                    viewModel.UpdateStatusText = "Update-check failed.";
                    return;
                }

                if (t.Result > myVersion)
                {
                    viewModel.IsUpdateAvailable = true;
                    viewModel.UpdateStatusText = "An update is available.";
                }
                else
                {
                    viewModel.UpdateStatusText = "You have the latest version.";
                }
            });
        }

        private Task<Version> GetLatestVersionTask()
        {
            return Task.Run(() =>
                {
                    Rss20FeedFormatter feedFormatter = new Rss20FeedFormatter();
                    using (XmlReader rssReader = XmlReader.Create("http://pocogen.codeplex.com/project/feeds/rss?ProjectRSSFeed=codeplex%3a%2f%2frelease%2fpocogen"))
                    {
                        if (!feedFormatter.CanRead(rssReader))
                        {
                            return null;
                        }

                        feedFormatter.ReadFrom(rssReader);
                        SyndicationItem newestItem = feedFormatter.Feed.Items.FirstOrDefault();
                        if (newestItem == null)
                        {
                            return null;
                        }

                        // The title contains the version in the format " v1.2.3.4 (Date)", extract the version
                        Regex regex = new Regex("v(?<Version>([0-9]+\\.){3}[0-9]) ", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                        Match match = regex.Match(newestItem.Title.Text);
                        if (match.Success)
                        {
                            return new Version(match.Groups["Version"].Value);
                        }
                        else
                        {
                            return null;
                        }
                    }
                });
        }
    }
}