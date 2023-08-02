using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using KekUploadServerApi;
using Microsoft.Extensions.Logging;

namespace KekUploadServerGUI;

public class Main : IPlugin
{
    public static MainWindow Window = null!;
    public static IClassicDesktopStyleApplicationLifetime Lifetime = null!;
    private ILogger<Main> _logger = null!;
    private Thread _windowThread = null!;

    public Main()
    {
        Instance = this;
    }

    public IKekUploadServer Server { get; private set; } = null!;
    public IPlugin? FileLogging { get; private set; }
    public static Main Instance { get; private set; } = null!;

    public Task Load(IKekUploadServer server)
    {
        Server = server;
        _logger = Server.GetPluginLogger<Main>();
        _logger.LogInformation("KekUploadServerGUI loaded!");
        _windowThread = new Thread(() => { Program.MainPlugin(Array.Empty<string>()); });
        FileLogging = Server.GetPlugin("FileLogging");
        return Task.CompletedTask;
    }

    public Task Start()
    {
        _windowThread.Start();
        _logger.LogInformation("KekUploadServerGUI started!");
        return Task.CompletedTask;
    }

    public async Task Unload()
    {
        await Dispatcher.UIThread.InvokeAsync(() => { Lifetime.Shutdown(); });
        _logger.LogInformation("KekUploadServerGUI unloaded!");
    }

    public PluginInfo Info => new()
    {
        Name = "KekUploadServerGUI",
        Author = "GamePowerX",
        Version = "1.0.0-alpha1",
        Description = "A GUI for KekUploadServer",
        OptionalDependencies = new[] { "FileLogging" }
    };

    public void Shutdown()
    {
        Server.Shutdown(this, "User requested shutdown from GUI", TimeSpan.FromSeconds(5));
    }
}