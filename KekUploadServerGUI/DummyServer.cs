using System;
using System.Globalization;
using System.Threading;
using KekUploadServerApi;
using KekUploadServerApi.Console;

namespace KekUploadServerGUI;

public class DummyServer : KekUploadDummyServer
{
    private bool _running = true;

    public DummyServer()
    {
        new Thread(() =>
        {
            while (_running)
            {
                ConsoleLineWritten?.Invoke(this,
                    new ConsoleLineWrittenEventArgs(DateTime.Now.ToString(CultureInfo.CurrentCulture) +
                                                    " [DummyServer] Test log " + Guid.NewGuid()));
                Thread.Sleep(TimeSpan.FromSeconds(3));
            }
        }).Start();
    }

    public override void Shutdown(IPlugin plugin, string reason = "Plugin initiated shutdown", TimeSpan delay = new())
    {
        Log($"Plugin {plugin.Info.Name} initiated shutdown: {reason} (Delay: {delay})");
        _running = false;
        new Thread(() =>
        {
            Thread.Sleep(delay);
            Environment.Exit(0);
        }).Start();
    }

    public override event EventHandler<ConsoleLineWrittenEventArgs>? ConsoleLineWritten;

    private new void Log(string line)
    {
        ConsoleLineWritten?.Invoke(this, new ConsoleLineWrittenEventArgs(line));
        KekUploadDummyServer.Log(line);
    }
}