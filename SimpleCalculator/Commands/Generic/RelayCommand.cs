using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator.Commands.Generic
{
    class RelayCommand<T> : System.Windows.Input.ICommand
    {
        #region Fields
        /// <summary>
        /// Encapsulated the execute action
        /// </summary>
        Action<T> execute;

        /// <summary>
        /// Encapsulated the representation for the validation of the execute method
        /// </summary>
        Predicate<T> canExecute;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the RelayCommand class
        /// Create a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action<T> execute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            this.execute = execute;
            this.canExecute = DefaultCanExecute;
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            this.execute = execute;
            this.canExecute = canExecute != null ? canExecute : DefaultCanExecute;
        }
        #endregion Constructors

        #region ICommand Members
        /// <summary>
        /// An event to raise when the CanExecute value is change
        /// </summary>
        /// <remarks>
        /// Any subscription to this event will automatically subscribe to both 
        /// the local OnCanExecuteChanged method AND
        /// the CommandManager RequerySuggested event
        /// </remarks>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                System.Windows.Input.CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                System.Windows.Input.CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        /// <summary>
        /// An event to allow the CanExecuteChanged event to be raised manually
        /// </summary>
        private event EventHandler CanExecuteChangedInternal;

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null ? DefaultCanExecute(TranslateParameter(parameter)) : canExecute(TranslateParameter(parameter));
        }

        /// <summary>
        /// Execute the encapsulted command
        /// </summary>
        /// <param name="parameter">the parameter that represents the execute method</param>
        public void Execute(object parameter)
        {
            this.execute(TranslateParameter(parameter));
        }
        #endregion ICommand Members

        // commentting out the OnCanExecute test how the codes affect program
        /// <summary>
        /// Raises the can execute changed.
        /// </summary>
        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        public void Destroy()
        {
            this.canExecute = _ => false;
            this.execute = _ => { return; };
        }

        /// <summary>
        /// Defines if command can be executed (default behaviour)
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Always true</returns>
        public bool DefaultCanExecute(T parameter)
        {
            return true;
        }

        private T TranslateParameter(object parameter)
        {
            T value = default(T);
            if (parameter != null && typeof(T).IsEnum)
                value = (T)Enum.Parse(typeof(T), (string)parameter);
            else
                value = (T)parameter;
            return value;
        }
    }
}
