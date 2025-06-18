using System.Numerics;
using Dalamud.Game.Command;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Component.GUI;
using KamiToolKit;
using KamiToolKit.Addon;

namespace Calculator;

public sealed class CalculatorPlugin : IDalamudPlugin {
    private static NativeController NativeController { get; set; } = null!;
    private static AddonCalculator AddonCalculator { get; set; } = null!;

    public CalculatorPlugin(IDalamudPluginInterface pluginInterface, ICommandManager commandManager) {
        NativeController = new NativeController(pluginInterface);

        AddonCalculator = new AddonCalculator {
            InternalName = "DevSandbox", 
            Title = "Development Sandbox Addon Window", 
            Size = new Vector2(650.0f, 500.0f), 
            NativeController = NativeController,
        };

        pluginInterface.Inject(AddonCalculator);

        AddonCalculator.Open();

        commandManager.AddHandler("/dev", new CommandInfo( (command, _) => {
            if (command is "/dev") {
                AddonCalculator.Open();
            }
        }));
    }

    public void Dispose() {
        AddonCalculator.Dispose();
        NativeController.Dispose();
    }
}

public class AddonCalculator : NativeAddon {
    
    protected override unsafe void OnSetup(AtkUnitBase* addon) {
        

    }
}
