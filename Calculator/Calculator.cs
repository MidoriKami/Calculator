using System.Numerics;
using Dalamud.Game.Addon.Events;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Component.GUI;
using KamiToolKit;
using KamiToolKit.Addon;
using KamiToolKit.Nodes;
using KamiToolKit.Nodes.ComponentNodes;

namespace Calculator;

public sealed class CalculatorPlugin : IDalamudPlugin {
    
    public CalculatorPlugin(IDalamudPluginInterface pluginInterface) {
        pluginInterface.Create<Services>();

        // Native Controller is required for injecting KamiToolKit elements into the native UI
        // It provides tracking and safety features that ensure the stability of the game when manipulating native elements
        Services.NativeController = new NativeController(pluginInterface);

        // Construct instance of AddonController, with desired parameters
        Services.AddonCalculator = new AddonCalculator {
            InternalName = "Calculator",
            Title = "Calculator",
            Size = new Vector2(305.0f, 415.0f),
        };
        
        // For this demo, we will open the calculator window as soon as the plugin loads
        OpenCalculator();

        Services.CommandManager.AddHandler("/calc", new CommandInfo(OnCommand) {
            HelpMessage = "Open Calculator Window",
        });
    }

    private void OnCommand(string command, string arguments) {
        if (command is "/calc") {
            OpenCalculator();
        }
    }

    public void Dispose() {
        // Disposing our Addon will close it and remove it from the game
        Services.AddonCalculator.Dispose();
        
        // Disposing native controller will attempt to clean up any nodes or addons that we might have missed
        Services.NativeController.Dispose();
    }

    // Opens the calculator window, it is not safe to call this from any thread except the main thread
    private void OpenCalculator()
        => Services.AddonCalculator.Open(Services.NativeController);
}

// A simple static class for storing any dalamud services that we might want, and for storing any objects that we'll need
public class Services {
    [PluginService] public static IFramework Framework { get; set; } = null!;
    [PluginService] public static ICommandManager CommandManager { get; set; } = null!;
    
    public static NativeController NativeController { get; set; } = null!;
    public static AddonCalculator AddonCalculator { get; set; } = null!;
}


// Here we define our window, also known as an "Addon"
// Most core functionality is handled for you by NativeAddon, there are several functions available for overriding
// We do not need to call "base.function" for any overriden function other than dispose, they are all empty stubs.
public class AddonCalculator : NativeAddon {
    
    private TextButtonNode? number0;
    private TextButtonNode? number1;
    private TextButtonNode? number2;
    private TextButtonNode? number3;
    private TextButtonNode? number4;
    private TextButtonNode? number5;
    private TextButtonNode? number6;
    private TextButtonNode? number7;
    private TextButtonNode? number8;
    private TextButtonNode? number9;

    private TextButtonNode? add;
    private TextButtonNode? subtract;
    private TextButtonNode? multiply;
    private TextButtonNode? divide;

    private TextButtonNode? enter;

    private BorderNineGridNode? box;
    private TextNode? result;

    private const float UnitSize = 65.0f;
    private const float FramePadding = 8.0f;
    private const float UnitPadding = 10.0f;
    private const float VerticalPadding = UnitPadding / 4.0f;

    private int currentValue;
    private int lastValue;
    private CurrentOperation currentOperation = CurrentOperation.None;
    
    // OnSetup is your entry-point to adding native elements to the window
    // Here you should allocate and attach your nodes to the UI
    protected override unsafe void OnSetup(AtkUnitBase* addon) {

        var xPos = FramePadding;
        var yPos = Size.Y - UnitSize - FramePadding;

        // Create custom node
        number0 = new TextButtonNode {
            Position = new Vector2(xPos, yPos), 
            Size = new Vector2(UnitSize * 2.0f + UnitPadding, UnitSize), 
            IsVisible = true, 
            Label = "0",
        };
        
        number0.AddEvent(AddonEventType.ButtonClick, () => EditNumber(0));

        // Attach custom node to addon
        //
        // IMPORTANT: Once attached, >> do not detach or dispose these nodes <<
        // When attaching the game will take ownership of the nodes and all associated data,
        // and will properly clean up when the addon is closed
        Services.NativeController.AttachToAddon(number0, this);

        xPos += number0.Width + UnitPadding;
        
        enter = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "=",
        };
        
        enter.AddEvent(AddonEventType.ButtonClick, () => {
            currentValue = currentOperation switch {
                CurrentOperation.Add => currentValue + lastValue,
                CurrentOperation.Subtract => currentValue - lastValue,
                CurrentOperation.Multiply => currentValue * lastValue,
                CurrentOperation.Divide => currentValue / lastValue,
                _ => currentValue,
            };

            currentOperation = CurrentOperation.None;
        });
        
        Services.NativeController.AttachToAddon(enter, this);
        
        xPos += enter.Width + UnitPadding;
        
        add = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "+",
        };
        
        add.AddEvent(AddonEventType.ButtonClick, () => {
            if (currentOperation is not CurrentOperation.Add) {
                lastValue = currentValue;
                currentValue = 0;
                currentOperation = CurrentOperation.Add;
            }
        });
        
        Services.NativeController.AttachToAddon(add, this);
        
        xPos = FramePadding;
        yPos -= VerticalPadding + UnitSize;
        
        number1 = new TextButtonNode {
            Position = new Vector2(xPos, yPos), 
            Size = new Vector2(UnitSize, UnitSize), 
            IsVisible = true, 
            Label = "1",
        };
        
        number1.AddEvent(AddonEventType.ButtonClick, () => EditNumber(1));
        
        Services.NativeController.AttachToAddon(number1, this);
       
        xPos += number1.Width + UnitPadding;

        number2 = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "2",
        };
        
        number2.AddEvent(AddonEventType.ButtonClick, () => EditNumber(2));
        
        Services.NativeController.AttachToAddon(number2, this);

        xPos += number2.Width + UnitPadding;
        
        number3 = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "3",
        };
        
        number3.AddEvent(AddonEventType.ButtonClick, () => EditNumber(3));
        
        Services.NativeController.AttachToAddon(number3, this);
        
        xPos += number3.Width + UnitPadding;

        subtract = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "-",
        };
        
        subtract.AddEvent(AddonEventType.ButtonClick, () => {
            if (currentOperation is not CurrentOperation.Subtract) {
                lastValue = currentValue;
                currentValue = 0;
                currentOperation = CurrentOperation.Subtract;
            }
        });
        
        Services.NativeController.AttachToAddon(subtract, this);
        
        xPos = FramePadding;
        yPos -= VerticalPadding + UnitSize;

        number4 = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "4",
        };
        
        number4.AddEvent(AddonEventType.ButtonClick, () => EditNumber(4));
        
        Services.NativeController.AttachToAddon(number4, this);
        
        xPos += number4.Width + UnitPadding;

        number5 = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "5",
        };
        
        number5.AddEvent(AddonEventType.ButtonClick, () => EditNumber(5));
        
        Services.NativeController.AttachToAddon(number5, this);
        
        xPos += number5.Width + UnitPadding;

        number6 = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "6",
        };
        
        number6.AddEvent(AddonEventType.ButtonClick, () => EditNumber(6));
        
        Services.NativeController.AttachToAddon(number6, this);
        
        xPos += number6.Width + UnitPadding;

        multiply = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "X",
        };
        
        multiply.AddEvent(AddonEventType.ButtonClick, () => {
            if (currentOperation is not CurrentOperation.Multiply) {
                lastValue = currentValue;
                currentValue = 0;
                currentOperation = CurrentOperation.Multiply;
            }
        });
        
        Services.NativeController.AttachToAddon(multiply, this);
        
        xPos = FramePadding;
        yPos -= VerticalPadding + UnitSize;

        number7 = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "7",
        };
        
        number7.AddEvent(AddonEventType.ButtonClick, () => EditNumber(7));
        
        Services.NativeController.AttachToAddon(number7, this);
        
        xPos += number7.Width + UnitPadding;

        number8 = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "8",
        };
        
        number8.AddEvent(AddonEventType.ButtonClick, () => EditNumber(8));
        
        Services.NativeController.AttachToAddon(number8, this);
        
        xPos += number8.Width + UnitPadding;

        number9 = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "9",
        };
        
        number9.AddEvent(AddonEventType.ButtonClick, () => EditNumber(9));
        
        Services.NativeController.AttachToAddon(number9, this);
        
        xPos += number9.Width + UnitPadding;

        divide = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "/",
        };
        
        divide.AddEvent(AddonEventType.ButtonClick, () => {
            if (currentOperation is not CurrentOperation.Divide) {
                lastValue = currentValue;
                currentValue = 0;
                currentOperation = CurrentOperation.Divide;
            }
        });
        
        Services.NativeController.AttachToAddon(divide, this);

        box = new BorderNineGridNode {
            Position = new Vector2(FramePadding, FramePadding + addon->WindowHeaderCollisionNode->Height),
            Size = new Vector2(Size.X - FramePadding * 2.0f, yPos - addon->WindowHeaderCollisionNode->Y - FramePadding - UnitPadding * 2.0f),
            IsVisible = true,
        };
        
        Services.NativeController.AttachToAddon(box, this);

        result = new TextNode {
            Position = box.Position + new Vector2(FramePadding, FramePadding) * 2.5f, 
            Size = box.Size - new Vector2(FramePadding * 1.25f, FramePadding) * 5.0f, 
            Text = currentValue.ToString(),
            IsVisible = true, 
            AlignmentType = AlignmentType.BottomRight,
            FontSize = 40,
        };

        Services.NativeController.AttachToAddon(result, this);
    }

    protected override unsafe void OnHide(AtkUnitBase* addon) {
        if (result is null) return;
        
        currentValue = 0;
        result.Text = currentValue.ToString();
    }

    protected override unsafe void OnUpdate(AtkUnitBase* addon) {
        if (result is null) return;
        
        result.Text = currentValue.ToString();
    }

    private void EditNumber(int value) {
        if (result is null) return;
        if (currentValue is 0 && value is 0) return;
    
        currentValue *= 10;
        currentValue += value;
        result.Text = currentValue.ToString();
    }
}

public enum CurrentOperation {
    None,
    Add,
    Subtract,
    Multiply,
    Divide,
}
