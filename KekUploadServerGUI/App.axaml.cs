using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace KekUploadServerGUI;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var dataContext = new MainWindowViewModel();
            var mainWindow = new MainWindow
            {
                DataContext = dataContext
            };
            desktop.MainWindow = mainWindow;
            Main.Window = mainWindow;
            Main.DataContext = dataContext;
            Main.Lifetime = desktop;
            Main.Instance.InvokeWindowInitialized(this);
        }

        base.OnFrameworkInitializationCompleted();
    }
}