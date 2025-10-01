using System.Numerics;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using KamiToolKit;
using KamiToolKitDemo.Demos;

namespace KamiToolKitDemo;

public sealed class KamiToolKitDemo : IDalamudPlugin {
    public KamiToolKitDemo(IDalamudPluginInterface pluginInterface) {
        pluginInterface.Create<Services>();

        // Native Controller is required for injecting KamiToolKit elements into the native UI
        // It provides tracking and safety features that ensure the stability of the game when manipulating native elements
        Services.NativeController = new NativeController(pluginInterface);

        Services.AddonWidgetDemo = new AddonWidgetDemo {
            InternalName = "AddonWidgetDemo", 
            Title = "Widget Demo", 
            Size = new Vector2(750.0f, 750.0f), 
            Position = new Vector2(1525.0f, 200.0f),
            NativeController = Services.NativeController,
        };
        
        // For this demo, we will open the widget window as soon as the plugin loads
        OpenWidgetDemo();

        // We will also add a chat command to open this window
        Services.CommandManager.AddHandler("/widget", new CommandInfo(OnCommand) {
            HelpMessage = "Open Widget Window",
        });
    }

    private void OnCommand(string command, string arguments) {
        switch (command) {
            case "/widget":
                OpenWidgetDemo();
                break;
        }
    }

    public void Dispose() {
        // Disposing our Addon will close it and remove it from the game
        Services.AddonWidgetDemo.Dispose();
        
        // Disposing native controller will attempt to clean up any nodes or addons that we might have missed
        Services.NativeController.Dispose();
    }
    
    private void OpenWidgetDemo()
        => Services.AddonWidgetDemo.Open();
}

// A simple static class for storing any dalamud services that we might want, and for storing any objects that we'll need
public class Services {
    [PluginService] public static ICommandManager CommandManager { get; set; } = null!;
	[PluginService] public static IDalamudPluginInterface PluginInterface { get; set; } = null!;
	[PluginService] public static IDataManager DataManager { get; set; } = null!;

	public static NativeController NativeController { get; set; } = null!;
    public static AddonWidgetDemo AddonWidgetDemo { get; set; } = null!;
}
