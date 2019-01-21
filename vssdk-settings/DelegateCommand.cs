using System;
using System.Windows.Input;

namespace vssdk_settings
{
    /// <summary>
    /// Define a command whose implementation is a delegate.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _action;

        /// <summary>
        /// Create a new command using a delegate as the command's implementation.
        /// </summary>
        /// <param name="action">The delegate to run when the command is executed.</param>
        public DelegateCommand(Action<object> action)
        {
            _action = action;
        }

        /// <summary>
        /// Invoke the command.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this
        /// object can be set to null.</param>
        public virtual void Execute(object parameter) => _action(parameter);

        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this
        /// object can be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public virtual bool CanExecute(object parameter) => true;

        /// <summary>
        /// Fire the event indicating that the command's ability to execute has changed.
        /// </summary>
        protected virtual void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public virtual event EventHandler CanExecuteChanged;
    }
}