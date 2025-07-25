﻿using System.Globalization;
using System.Numerics;
using Calculator.Demos;
using Dalamud.Game.Addon.Events;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Component.GUI;
using KamiToolKit;
using KamiToolKit.Addon;
using KamiToolKit.Nodes;

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
            Position = new Vector2(400.0f, 600.0f),
            NativeController = Services.NativeController,
        };

        Services.AddonWidgetDemo = new AddonWidgetDemo {
            InternalName = "AddonWidgetDemo", 
            Title = "Widget Demo", 
            Size = new Vector2(750.0f, 750.0f), 
            Position = new Vector2(1525.0f, 200.0f),
            NativeController = Services.NativeController,
        };
        
        // For this demo, we will open the calculator window as soon as the plugin loads
        OpenCalculator();
        OpenWidgetDemo();

        // We will also add a chat command to open this window
        Services.CommandManager.AddHandler("/calc", new CommandInfo(OnCommand) {
            HelpMessage = "Open Calculator Window",
        });
        
        // We will also add a chat command to open this window
        Services.CommandManager.AddHandler("/widget", new CommandInfo(OnCommand) {
            HelpMessage = "Open Widget Window",
        });
    }

    private void OnCommand(string command, string arguments) {
        switch (command) {
            case "/calc":
                OpenCalculator();
                break;
            
            case "/widget":
                OpenWidgetDemo();
                break;
        }
    }

    public void Dispose() {
        // Disposing our Addon will close it and remove it from the game
        Services.AddonCalculator.Dispose();
        Services.AddonWidgetDemo.Dispose();
        
        // Disposing native controller will attempt to clean up any nodes or addons that we might have missed
        Services.NativeController.Dispose();
    }

    // Opens the calculator window, it is not safe to call this from any thread except the main thread
    private void OpenCalculator()
        => Services.AddonCalculator.Open();
    
    private void OpenWidgetDemo()
        => Services.AddonWidgetDemo.Open();
}

// A simple static class for storing any dalamud services that we might want, and for storing any objects that we'll need
public class Services {
    [PluginService] public static ICommandManager CommandManager { get; set; } = null!;
	[PluginService] public static IDalamudPluginInterface PluginInterface { get; set; } = null!;
	[PluginService] public static IDataManager DataManager { get; set; } = null!;

	public static NativeController NativeController { get; set; } = null!;
    public static AddonCalculator AddonCalculator { get; set; } = null!;
    public static AddonWidgetDemo AddonWidgetDemo { get; set; } = null!;
}

// Example of creating a custom node that we can use together instead of always having to manage each part
public class TextBox : ResNode {

    private readonly BorderNineGridNode boxOutline;
    private readonly TextNode resultText;

    public TextBox() {
        boxOutline = new BorderNineGridNode {
            IsVisible = true,
        };
        
        Services.NativeController.AttachNode(boxOutline, this);

        resultText = new TextNode {
            IsVisible = true, 
            AlignmentType = AlignmentType.BottomRight,
            FontSize = 40,
        };
        
        Services.NativeController.AttachNode(resultText, this);
    }

    public string Value {
        get => resultText.Text.ToString();
        set => resultText.Text = value;
    }
    
    // Override Width property to have it set the width of our individual parts
    public override float Width {
        get => base.Width;
        set {
            base.Width = value;
            boxOutline.Width = value;
            resultText.Width = value - 48.0f;
            resultText.X = 24.0f;
        }
    }

    // Override Height property to have it set the height of our individual parts
    public override float Height {
        get => base.Height;
        set {
            base.Height = value;
            boxOutline.Height = value;
            resultText.Height = value - 48.0f;
            resultText.Y = 24.0f;
        }
    }
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

    private TextBox? textBox;

    private const float UnitSize = 65.0f;
    private const float FramePadding = 8.0f;
    private const float UnitPadding = 10.0f;
    private const float VerticalPadding = UnitPadding / 4.0f;

    private float currentValue;
    private float lastValue;
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
        
        number0.AddEvent(AddonEventType.ButtonClick, _ => EditNumber(0));

        // Attach custom node to addon
        //
        // IMPORTANT: Once attached, >> do not detach or dispose these nodes <<
        // When attaching the game will take ownership of the nodes and all associated data,
        // and will properly clean up when the addon is closed
        AttachNode(number0);

        xPos += number0.Width + UnitPadding;
        
        enter = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "=",
        };
        
        enter.AddEvent(AddonEventType.ButtonClick, _ => {
            currentValue = currentOperation switch {
                CurrentOperation.Add => currentValue + lastValue,
                CurrentOperation.Subtract => lastValue - currentValue,
                CurrentOperation.Multiply => currentValue * lastValue,
                CurrentOperation.Divide => lastValue / currentValue,
                _ => currentValue,
            };

            if (textBox is not null) {
                textBox.Value = currentValue.ToString(CultureInfo.InvariantCulture);
            }

            currentOperation = CurrentOperation.None;
        });
        AttachNode(enter);
        
        xPos += enter.Width + UnitPadding;
        
        add = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "+",
        };
        
        add.AddEvent(AddonEventType.ButtonClick, _ => {
            if (currentOperation is not CurrentOperation.Add) {
                lastValue = currentValue;
                currentValue = 0;
                currentOperation = CurrentOperation.Add;
            }
        });
        AttachNode(add);
        
        xPos = FramePadding;
        yPos -= VerticalPadding + UnitSize;
        
        number1 = new TextButtonNode {
            Position = new Vector2(xPos, yPos), 
            Size = new Vector2(UnitSize, UnitSize), 
            IsVisible = true, 
            Label = "1",
        };
        
        number1.AddEvent(AddonEventType.ButtonClick, _ => EditNumber(1));
        AttachNode(number1);
       
        xPos += number1.Width + UnitPadding;

        number2 = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "2",
        };
        
        number2.AddEvent(AddonEventType.ButtonClick, _ => EditNumber(2));
        AttachNode(number2);

        xPos += number2.Width + UnitPadding;
        
        number3 = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "3",
        };
        
        number3.AddEvent(AddonEventType.ButtonClick, _ => EditNumber(3));
        AttachNode(number3);
        
        xPos += number3.Width + UnitPadding;

        subtract = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "-",
        };
        
        subtract.AddEvent(AddonEventType.ButtonClick, _ => {
            if (currentOperation is not CurrentOperation.Subtract) {
                lastValue = currentValue;
                currentValue = 0;
                currentOperation = CurrentOperation.Subtract;
            }
        });
        AttachNode(subtract);
        
        xPos = FramePadding;
        yPos -= VerticalPadding + UnitSize;

        number4 = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "4",
        };
        
        number4.AddEvent(AddonEventType.ButtonClick, _ => EditNumber(4));
        AttachNode(number4);
        
        xPos += number4.Width + UnitPadding;

        number5 = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "5",
        };
        
        number5.AddEvent(AddonEventType.ButtonClick, _ => EditNumber(5));
        AttachNode(number5);
        
        xPos += number5.Width + UnitPadding;

        number6 = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "6",
        };
        
        number6.AddEvent(AddonEventType.ButtonClick, _ => EditNumber(6));
        AttachNode(number6);
        
        xPos += number6.Width + UnitPadding;

        multiply = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "X",
        };
        
        multiply.AddEvent(AddonEventType.ButtonClick, _ => {
            if (currentOperation is not CurrentOperation.Multiply) {
                lastValue = currentValue;
                currentValue = 0;
                currentOperation = CurrentOperation.Multiply;
            }
        });
        AttachNode(multiply);
        
        xPos = FramePadding;
        yPos -= VerticalPadding + UnitSize;

        number7 = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "7",
        };
        
        number7.AddEvent(AddonEventType.ButtonClick, _ => EditNumber(7));
        AttachNode(number7);
        
        xPos += number7.Width + UnitPadding;

        number8 = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "8",
        };
        
        number8.AddEvent(AddonEventType.ButtonClick, _ => EditNumber(8));
        AttachNode(number8);
        
        xPos += number8.Width + UnitPadding;

        number9 = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "9",
        };
        
        number9.AddEvent(AddonEventType.ButtonClick, _ => EditNumber(9));
        AttachNode(number9);
        
        xPos += number9.Width + UnitPadding;

        divide = new TextButtonNode {
            Position = new Vector2(xPos, yPos),
            Size = new Vector2(UnitSize, UnitSize),
            IsVisible = true,
            Label = "/",
        };
        
        divide.AddEvent(AddonEventType.ButtonClick, _ => {
            if (currentOperation is not CurrentOperation.Divide) {
                lastValue = currentValue;
                currentValue = 0;
                currentOperation = CurrentOperation.Divide;
            }
        });
        AttachNode(divide);

        textBox = new TextBox {
            Position = new Vector2(FramePadding, FramePadding + addon->WindowHeaderCollisionNode->Height),
            Size = new Vector2(Size.X - FramePadding * 2.0f, yPos - addon->WindowHeaderCollisionNode->Y - FramePadding - UnitPadding * 2.0f),
            IsVisible = true,
            Value = "0",
        };
        
        AttachNode(textBox);
    }

    // OnHide is called when our window is about to close, but hasn't closed yet.
    // If you need an event to trigger immediately before the window actually closes, use OnFinalize
    protected override unsafe void OnHide(AtkUnitBase* addon) {
        if (textBox is null) return;
        
        currentValue = 0;
        textBox.Value = currentValue.ToString(CultureInfo.InvariantCulture);
    }

    private void EditNumber(float value) {
        if (textBox is null) return;
        if (currentValue is 0 && value is 0) return;
    
        currentValue *= 10;
        currentValue += value;
        textBox.Value = currentValue.ToString(CultureInfo.InvariantCulture);
    }
}

public enum CurrentOperation {
    None,
    Add,
    Subtract,
    Multiply,
    Divide,
}

public class CustomWindowNode : WindowNode {

    public CustomWindowNode() {
        HeaderContainerNode.Y += 6.0f;
        TitleNode.X += 32.0f;
        TitleNode.Y -= 4.0f;
    }
}
