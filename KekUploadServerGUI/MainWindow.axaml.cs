using Avalonia.Controls;
using Avalonia.Interactivity;

namespace KekUploadServerGUI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.Closing += MainWindow_Closing;
    }

    private void ShowSettingsButton_Click(object? sender, RoutedEventArgs e)
    {
        
    }

    private void ShowLogsButton_Click(object? sender, RoutedEventArgs e)
    {
        
    }

    private void ShowUploadsButton_Click(object? sender, RoutedEventArgs e)
    {
        
    }

    private void SaveSettingsButton_Click(object? sender, RoutedEventArgs e)
    {
        
    }

    private void StopServerButton_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
    
    private void MainWindow_Closing(object? sender, WindowClosingEventArgs e)
    {
        if (e.CloseReason != WindowCloseReason.WindowClosing) return;
        e.Cancel = true;
        Hide();
        Main.Instance.Shutdown();
    }
}