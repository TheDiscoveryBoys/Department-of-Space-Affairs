// DelegateCommand with support for parameters
using System.Windows.Input;

public class DelegateCommand<T> : ICommand
{
    private readonly Action<T> _execute;

    public DelegateCommand(Action<T> execute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter)
    {
        return true; // Always executable in this case
    }

    public void Execute(object parameter)
    {
        _execute((T)parameter);  // Cast the parameter to the expected type
    }
}