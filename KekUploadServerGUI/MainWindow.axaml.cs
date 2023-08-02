using System.IO;
using System.Text.Json;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using KekUploadServerApi.Console;
using Newtonsoft.Json.Linq;

namespace KekUploadServerGUI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Closing += MainWindow_Closing;
        LogsTextBox.Clear();
        Main.Instance.Server.ConsoleLineWritten += Server_ConsoleLineWritten;
        LoadSettings();
        if (Main.Instance.FileLogging == null) return;
        LoadFileLoggingSettings();
    }

    private void LoadSettings()
    {
        var server = Main.Instance.Server;
        UrlTextBox.Text = server.GetConfigValueString("Kestrel:EndPoints:Http:Url") ?? "http://0.0.0.0:5254";
        DefaultLogLevelTextBox.Text = server.GetConfigValueString("Logging:LogLevel:Default") ?? "Information";
        AspNetCoreLogLevelTextBox.Text =
            server.GetConfigValueString("Logging:LogLevel:Microsoft.AspNetCore") ?? "Warning";
        AllowedHostsTextBox.Text = server.GetConfigValueString("AllowedHosts") ?? "*";
        DatabaseConnectionStringTextBox.Text = server.GetConfigValueString("ConnectionStrings:KekUploadDb") ??
                                               "Server=localhost;Database=upload;Port=5432;Username=postgres;Password=password";
        MaxExtensionLengthTextBox.Text = (server.GetConfigValue<int?>("MaxExtensionLength") ?? 10).ToString();
        IdLengthTextBox.Text = (server.GetConfigValue<int?>("IdLength") ?? 12).ToString();
        BaseUrlTextBox.Text = server.GetConfigValueString("BaseUrl") ?? "http://localhost:5254";
        EmbedDescriptionTextBox.Text = server.GetConfigValueString("Description") ?? "File Uploaded to KekUploadServer";
        EmbedColorTextBox.Text = server.GetConfigValueString("EmbedColor") ?? "#2BFF00";
        WebRootPathTextBox.Text = server.GetConfigValueString("WebRootPath") ?? "web";
        ThumbnailDirectoryTextBox.Text = server.GetConfigValueString("ThumbnailDirectory") ?? "thumbs";
        PluginDirectoryTextBox.Text = server.GetConfigValueString("PluginDirectory") ?? "plugins";
    }

    private void LoadFileLoggingSettings()
    {
        var server = Main.Instance.Server;
        var dir = server.GetPluginDataPath(Main.Instance.FileLogging!);
        var path = Path.Combine(dir, "config.json");
        if (!File.Exists(path)) return;
        var config = File.ReadAllText(path);
        var json = JsonDocument.Parse(config);
        var root = json.RootElement;
        LogPathTextBox.Text = root.GetProperty("LogPath").GetString() ?? "logs";
        var logLevel = root.GetProperty("LogLevel"); // as int
        var logLevelInt = logLevel.GetInt32();
        if (logLevelInt is > 6 or < 0) logLevelInt = 2;
        LogLevelComboBox.SelectedIndex = logLevelInt;
        IncludeDateTimeCheckBox.IsChecked = root.GetProperty("IncludeDateTime").GetBoolean();
        IncludeLevelCheckBox.IsChecked = root.GetProperty("IncludeLevel").GetBoolean();
        IncludeEventIdCheckBox.IsChecked = root.GetProperty("IncludeEventId").GetBoolean();
        IncludeStateCheckBox.IsChecked = root.GetProperty("IncludeState").GetBoolean();
        IncludeExceptionCheckBox.IsChecked = root.GetProperty("IncludeException").GetBoolean();
        IncludeScopeCheckBox.IsChecked = root.GetProperty("IncludeScope").GetBoolean();
        FormatScopeAsJsonCheckBox.IsChecked = root.GetProperty("FormatScopeAsJson").GetBoolean();
        IncludeCategoryCheckBox.IsChecked = root.GetProperty("IncludeCategory").GetBoolean();
        DateTimeFormatTextBox.Text = root.GetProperty("DateTimeFormat").GetString() ?? "yyyy-MM-dd HH:mm:ss";
        FileFormatTextBox.Text = root.GetProperty("FileFormat").GetString() ??
                                 "{DateTime} [{Level}] {Category}: {Message}{NewLine}{Exception}";
        FileExtensionTextBox.Text = root.GetProperty("FileExtension").GetString() ?? "log";
        FilePrefixTextBox.Text = root.GetProperty("FilePrefix").GetString() ?? "KekUploadServerLog";
        FileSuffixTextBox.Text = root.GetProperty("FileSuffix").GetString() ?? "";
        UseUtcCheckBox.IsChecked = root.GetProperty("UseUtc").GetBoolean();

        FileLoggingPluginNotice.IsVisible = false;

        LogPathTextBox.IsEnabled = true;
        LogLevelComboBox.IsEnabled = true;
        IncludeDateTimeCheckBox.IsEnabled = true;
        IncludeLevelCheckBox.IsEnabled = true;
        IncludeEventIdCheckBox.IsEnabled = true;
        IncludeStateCheckBox.IsEnabled = true;
        IncludeExceptionCheckBox.IsEnabled = true;
        IncludeScopeCheckBox.IsEnabled = true;
        FormatScopeAsJsonCheckBox.IsEnabled = true;
        IncludeCategoryCheckBox.IsEnabled = true;
        DateTimeFormatTextBox.IsEnabled = true;
        FileFormatTextBox.IsEnabled = true;
        FileExtensionTextBox.IsEnabled = true;
        FilePrefixTextBox.IsEnabled = true;
        FileSuffixTextBox.IsEnabled = true;
        UseUtcCheckBox.IsEnabled = true;
    }

    private void SaveConfig()
    {
        SetIfNotNull("Kestrel:EndPoints:Http:Url", UrlTextBox.Text);
        SetIfNotNull("Logging:LogLevel:Default", DefaultLogLevelTextBox.Text);
        SetIfNotNull("Logging:LogLevel:Microsoft.AspNetCore", AspNetCoreLogLevelTextBox.Text);
        SetIfNotNull("AllowedHosts", AllowedHostsTextBox.Text);
        SetIfNotNull("ConnectionStrings:KekUploadDb", DatabaseConnectionStringTextBox.Text);
        SetIntIfNotNull("MaxExtensionLength", MaxExtensionLengthTextBox);
        SetIntIfNotNull("IdLength", IdLengthTextBox);
        SetIfNotNull("BaseUrl", BaseUrlTextBox.Text);
        SetIfNotNull("Description", EmbedDescriptionTextBox.Text);
        SetIfNotNull("EmbedColor", EmbedColorTextBox.Text);
        SetIfNotNull("WebRootPath", WebRootPathTextBox.Text);
        SetIfNotNull("ThumbnailDirectory", ThumbnailDirectoryTextBox.Text);
        SetIfNotNull("PluginDirectory", PluginDirectoryTextBox.Text);
        if (Main.Instance.FileLogging != null) SaveFileLoggingConfig();
    }

    private void SaveFileLoggingConfig()
    {
        if (string.IsNullOrWhiteSpace(LogPathTextBox.Text)) return;
        var server = Main.Instance.Server;
        var dir = server.GetPluginDataPath(Main.Instance.FileLogging!);
        var path = Path.Combine(dir, "config.json");
        var config = File.ReadAllText(path);
        var json = JObject.Parse(config);
        SetIfNotNullJObject(json, "LogPath", LogPathTextBox.Text);
        SetIfNotNullJObject(json, "LogLevel", LogLevelComboBox.SelectedIndex);
        SetIfNotNullJObject(json, "IncludeDateTime", IncludeDateTimeCheckBox.IsChecked);
        SetIfNotNullJObject(json, "IncludeLevel", IncludeLevelCheckBox.IsChecked);
        SetIfNotNullJObject(json, "IncludeEventId", IncludeEventIdCheckBox.IsChecked);
        SetIfNotNullJObject(json, "IncludeState", IncludeStateCheckBox.IsChecked);
        SetIfNotNullJObject(json, "IncludeException", IncludeExceptionCheckBox.IsChecked);
        SetIfNotNullJObject(json, "IncludeScope", IncludeScopeCheckBox.IsChecked);
        SetIfNotNullJObject(json, "FormatScopeAsJson", FormatScopeAsJsonCheckBox.IsChecked);
        SetIfNotNullJObject(json, "IncludeCategory", IncludeCategoryCheckBox.IsChecked);
        SetIfNotNullJObject(json, "DateTimeFormat", DateTimeFormatTextBox.Text);
        SetIfNotNullJObject(json, "FileFormat", FileFormatTextBox.Text);
        SetIfNotNullJObject(json, "FileExtension", FileExtensionTextBox.Text);
        SetIfNotNullJObject(json, "FilePrefix", FilePrefixTextBox.Text);
        SetIfNotNullJObject(json, "FileSuffix", FileSuffixTextBox.Text);
        SetIfNotNullJObject(json, "UseUtc", UseUtcCheckBox.IsChecked);
        File.WriteAllText(path, json.ToString());
    }

    private static void SetIfNotNullJObject(JObject json, string key, object? value)
    {
        switch (value)
        {
            case null:
            case string s when string.IsNullOrWhiteSpace(s):
                return;
            default:
                json[key] = JToken.FromObject(value);
                break;
        }
    }

    private static void SetIntIfNotNull(string key, TextBox textBox)
    {
        if (int.TryParse(textBox.Text, out var value)) SetIfNotNull(key, value);
    }

    private static void SetIfNotNull<T>(string key, T value)
    {
        var server = Main.Instance.Server;
        if (value == null) return;
        if (value is string s && string.IsNullOrWhiteSpace(s)) return;
        server.SetConfigValue(key, value);
    }

    private void Server_ConsoleLineWritten(object? sender, ConsoleLineWrittenEventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            LogsTextBox.Text += e.Line + "\n";
            if (AutoScrollCheckBox.IsChecked == true) LogsTextBox.CaretIndex = LogsTextBox.Text.Length;
        });
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
        SaveConfig();
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

    private void ClearLogsButton_Click(object? sender, RoutedEventArgs e)
    {
        LogsTextBox.Clear();
    }
}