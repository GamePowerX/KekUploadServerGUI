using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using KekUploadServerApi.Console;

namespace KekUploadServerGUI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Closing += MainWindow_Closing;
        LogsTextBox.Clear();
        Main.Instance.Server.ConsoleLineWritten += Server_ConsoleLineWritten;
        if (Main.Instance.FileLogging == null) return;
        FileLoggingPluginNotice.IsVisible = false;
        LogPathTextBox.IsEnabled = true;
    }

    private void Server_ConsoleLineWritten(object? sender, ConsoleLineWrittenEventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(() => { LogsTextBox.Text += e.Line + "\n"; });
    }

    private void ShowSettingsButton_Click(object? sender, RoutedEventArgs e)
    {
        MenuPanel.IsVisible = false;
        SettingsPanel.IsVisible = true;
    }

    private void ShowLogsButton_Click(object? sender, RoutedEventArgs e)
    {
        MenuPanel.IsVisible = false;
        LogsPanel.IsVisible = true;
    }

    private void ShowUploadsButton_Click(object? sender, RoutedEventArgs e)
    {
        MenuPanel.IsVisible = false;
        UploadsPanel.IsVisible = true;
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

    private void BackToMenuButton_Click(object? sender, RoutedEventArgs e)
    {
        LogsPanel.IsVisible = false;
        SettingsPanel.IsVisible = false;
        UploadsPanel.IsVisible = false;
        MenuPanel.IsVisible = true;
    }
}