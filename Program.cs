using Avalonia;
using connect;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace scoreboard_client;

class Program
{
    public static UdpTcpClient client = new UdpTcpClient();

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        new Thread(async () => await connectAsync()).Start();
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();

    public static async Task connectAsync()
    {
        await client.StartAsync();
    }
}
