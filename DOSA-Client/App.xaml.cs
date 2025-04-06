using System.Configuration;
using System.Data;
using System.Windows;

namespace DOSA_Client;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
{
    AppDomain.CurrentDomain.UnhandledException += (s, ex) =>
    {
        MessageBox.Show($"Unhandled exception: {((Exception)ex.ExceptionObject).Message}");
    };

    DispatcherUnhandledException += (s, ex) =>
    {
        MessageBox.Show($"Dispatcher exception: {ex.Exception.Message}");
        ex.Handled = true;
    };

    TaskScheduler.UnobservedTaskException += (s, ex) =>
    {
        MessageBox.Show($"Unobserved task exception: {ex.Exception.Message}");
    };

    try
    {
        base.OnStartup(e);
        var mainWindow = new MainWindow(); // If you're injecting ViewModels here, it's a common crash point
        mainWindow.Show();
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Startup exception: {ex.Message}");
    }
}

}



