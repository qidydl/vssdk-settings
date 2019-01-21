using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace vssdk_settings
{
    /// <summary>
    /// Define a command whose implementation is a delegate, which can only be executed under certain conditions.
    /// </summary>
    public class ConditionalDelegateCommand : DelegateCommand
    {
        private readonly Func<bool> _condition;

        private bool _canExecute = true;

        /// <summary>
        /// Create a new command using one delegate as the command's implementation and another delegate to determine
        /// whether the command is available to be executed.
        /// </summary>
        /// <param name="context">The ViewModel the command is linked to. Changes in this ViewModel will trigger
        /// reevaluation of the command's availability to be executed. May be null if the condition never changes.</param>
        /// <param name="action">The delegate to run when the command is executed.</param>
        /// <param name="condition">The delegate to run to check whether the command may be executed.</param>
        public ConditionalDelegateCommand(INotifyPropertyChanged context, Action<object> action, Func<bool> condition)
            : this(action, condition, context)
        { }

        /// <summary>
        /// Create a new command using one delegate as the command's implementation and another delegate to determine
        /// whether the command is available to be executed.
        /// </summary>
        /// <param name="action">The delegate to run when the command is executed.</param>
        /// <param name="condition">The delegate to run to check whether the command may be executed.</param>
        /// <param name="contexts">ViewModels the command is linked to. Changes in the ViewModel's properties or
        /// collection contents will trigger reevaluation of the command's availability to be executed. May be null if
        /// the condition never changes.</param>
        public ConditionalDelegateCommand(Action<object> action, Func<bool> condition, params object[] contexts)
            : base(action)
        {
            _condition = condition;
            _canExecute = _condition();

            foreach (var context in contexts)
            {
                if (context is INotifyPropertyChanged propContext)
                {
                    propContext.PropertyChanged += ContextPropertyChanged;
                }

                if (context is INotifyCollectionChanged collContext)
                {
                    collContext.CollectionChanged += ContextCollectionChanged;
                }
            }
        }

        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this
        /// object can be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public override bool CanExecute(object parameter) => _canExecute;

        /// <summary>
        /// Check whether this command's ability to execute has changed.
        /// </summary>
        private void CheckCanExecuteChanged()
        {
            bool newCanExecute = _condition();
            if (newCanExecute != _canExecute)
            {
                _canExecute = newCanExecute;
                OnCanExecuteChanged();
            }
        }

        /// <summary>
        /// Take action when the command's context's properties have changed. The state of the context can affect
        /// whether the command is able to be executed.
        /// </summary>
        /// <param name="sender">The context whose properties have changed.</param>
        /// <param name="e">The event arguments indicating which property has changed.</param>
        private void ContextPropertyChanged(object sender, PropertyChangedEventArgs e) => CheckCanExecuteChanged();

        /// <summary>
        /// Take action when the command's context's collection has changed. The contents of the context can affect
        /// whether the command is able to be executed.
        /// </summary>
        /// <param name="sender">The context whose contents have changed.</param>
        /// <param name="e">The event arguments indicating how the collection has changed.</param>
        private void ContextCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => CheckCanExecuteChanged();
    }
}