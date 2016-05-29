using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using ReactiveUI;

namespace MvvmHybridFramework
{
    public class ReactiveValidatingViewModel<TView> : ReactiveViewModel<TView>, INotifyDataErrorInfo
        where TView : class, IView
    {
        private static readonly ValidationResult[] NoErrors = new ValidationResult[0];

        private readonly Dictionary<string, List<ValidationResult>> errors;

        private bool hasErrors;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveValidatingViewModel"/> class.
        /// </summary>
        protected ReactiveValidatingViewModel(TView view)
            : base(view)
        {
            this.errors = new Dictionary<string, List<ValidationResult>>();

            this.Initialize();

            this.Changed.Subscribe(this.ValidateProperty);
        }

        /// <summary>
        /// Gets a value that indicates whether the entity has validation errors.
        /// </summary>
        public bool HasErrors
        {
            get { return this.hasErrors; }
            private set { this.RaiseAndSetIfChanged(ref this.hasErrors, value); }
        }

        /// <summary>
        /// Occurs when the validation errors have changed for a property or for the entire entity.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Gets the validation errors for the entire entity.
        /// </summary>
        /// <returns>The validation errors for the entity.</returns>
        public IEnumerable<ValidationResult> GetErrors()
        {
            return this.GetErrors(null);
        }

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve validation errors for; 
        /// or null or String.Empty, to retrieve entity-level errors.</param>
        /// <returns>The validation errors for the property or entity.</returns>
        public IEnumerable<ValidationResult> GetErrors(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                List<ValidationResult> result;
                if (this.errors.TryGetValue(propertyName, out result))
                {
                    return result;
                }

                return NoErrors;
            }
            else
            {
                return this.errors.Values.SelectMany(x => x).Distinct().ToArray();
            }
        }

        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            return this.GetErrors(propertyName);
        }

        /// <summary>
        /// Validates the object and all its properties. The validation results are stored and can be retrieved by the 
        /// GetErrors method. If the validation results are changing then the ErrorsChanged event will be raised.
        /// </summary>
        /// <returns>True if the object is valid, otherwise false.</returns>
        public bool Validate()
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(this, new ValidationContext(this), validationResults, true);
            if (validationResults.Any())
            {
                this.errors.Clear();
                foreach (var validationResult in validationResults)
                {
                    var propertyNames = validationResult.MemberNames.Any() ? validationResult.MemberNames : new string[] { string.Empty };
                    foreach (string propertyName in propertyNames)
                    {
                        if (!this.errors.ContainsKey(propertyName))
                        {
                            this.errors.Add(propertyName, new List<ValidationResult>() { validationResult });
                        }
                        else
                        {
                            this.errors[propertyName].Add(validationResult);
                        }
                    }
                }

                this.RaiseErrorsChanged();
                return false;
            }
            else
            {
                if (this.errors.Any())
                {
                    this.errors.Clear();
                    this.RaiseErrorsChanged();
                }
            }

            return true;
        }

        /// <summary>
        /// Put your initialization logic in this class. After this class the validation will be called
        /// for each property change.
        /// </summary>
        protected virtual void Initialize()
        {
        }

        /// <summary>
        /// Validates the property with the specified value. The validation results are stored and can be retrieved by the 
        /// GetErrors method. If the validation results are changing then the ErrorsChanged event will be raised.
        /// </summary>
        /// <param name="value">The value of the property.</param>
        /// <param name="propertyName">The property name. This optional parameter can be skipped
        /// because the compiler is able to create it automatically.</param>
        /// <returns>True if the property value is valid, otherwise false.</returns>
        /// <exception cref="ArgumentException">The argument propertyName must not be null or empty.</exception>
        protected bool ValidateProperty(object value, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("The argument propertyName must not be null or empty.");
            }

            List<ValidationResult> validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(value, new ValidationContext(this) { MemberName = propertyName }, validationResults);
            if (validationResults.Any())
            {
                this.errors[propertyName] = validationResults;
                this.RaiseErrorsChanged(propertyName);
                return false;
            }
            else
            {
                if (this.errors.Remove(propertyName))
                {
                    this.RaiseErrorsChanged(propertyName);
                }
            }

            return true;
        }

        /// <summary>
        /// Raises the <see cref="E:ErrorsChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.DataErrorsChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            this.ErrorsChanged?.Invoke(this, e);
        }

        private void ValidateProperty(IReactivePropertyChangedEventArgs<IReactiveObject> e)
        {
            PropertyDescriptor property = TypeDescriptor.GetProperties(this)[e.PropertyName];

            object value = property.GetValue(this);

            this.ValidateProperty(value, e.PropertyName);
        }

        private void RaiseErrorsChanged(string propertyName = "")
        {
            this.HasErrors = this.errors.Any();
            this.OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
        }
    }
}