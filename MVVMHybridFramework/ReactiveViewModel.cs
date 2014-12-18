using System;
using System.Threading;
using System.Windows.Threading;
using ReactiveUI;

namespace MvvmHybridFramework
{
    public class ReactiveViewModel<TView> : ReactiveObject
        where TView : class, IView
    {
        private readonly TView view;

        protected ReactiveViewModel(TView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            this.view = view;

            // Check if the code is running within the WPF application model
            if (SynchronizationContext.Current is DispatcherSynchronizationContext)
            {
                // Set DataContext of the view has to be delayed so that the ViewModel can initialize the internal data (e.g. Commands)
                // before the view starts with DataBinding.
                Dispatcher.CurrentDispatcher.BeginInvoke((Action)delegate
                {
                    this.view.DataContext = this;
                });
            }
            else
            {
                // When the code runs outside of the WPF application model then we set the DataContext immediately.
                view.DataContext = this;
            }
        }

        public object View
        {
            get { return this.view; }
        }

        protected TView ViewCore
        {
            get { return this.view; }
        }
    }
}