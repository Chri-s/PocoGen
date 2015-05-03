using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PocoGen.Gui.Applications.ViewModels;
using ReactiveUI;

namespace PocoGen.Gui.Applications.Controllers
{
    [Export]
    internal class NameGeneratorConfigurationsController : ReactiveObject
    {
        private readonly NameGeneratorConfigurationsViewModel configurationViewModel;
        private readonly CompositionContainer container;
        private readonly ShellViewModel shellViewModel;

        [ImportingConstructor]
        public NameGeneratorConfigurationsController(NameGeneratorConfigurationsViewModel configurationViewModel, CompositionContainer container, ShellViewModel shellViewModel)
        {
            this.configurationViewModel = configurationViewModel;
            this.container = container;
            this.shellViewModel = shellViewModel;
        }

        public void Initialize()
        {
            this.shellViewModel.NameGeneratorConfigurationsViewModel = this.configurationViewModel;
        }
    }
}