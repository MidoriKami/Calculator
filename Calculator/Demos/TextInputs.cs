using System.Numerics;
using FFXIVClientStructs.FFXIV.Component.GUI;
using KamiToolKit;
using KamiToolKit.Nodes;

namespace Calculator.Demos;

public static class KeyboardInput {
	private static NativeController NativeController => Services.NativeController;

	public static void InputDemo(TreeListCategoryNode treeListCategoryNode) {
		treeListCategoryNode.AddHeader("TextInput");
		TextInput(treeListCategoryNode);
		
		treeListCategoryNode.AddHeader("NumericInput");
		NumericInput(treeListCategoryNode);
	}

	private static void TextInput(TreeListCategoryNode treeListCategoryNode) {
		ResNode containerNode;
		treeListCategoryNode.AddNode(containerNode = new ResNode {
			Width = treeListCategoryNode.Width,
			Height = 28.0f,
			IsVisible = true,
		});

		var inputString = string.Empty;
		var demoTextNode = new TextNode {
			Position = new Vector2(250.0f, 0.0f),
			Size = new Vector2(300.0f, 28.0f),
			Text = $"InputText: {inputString}",
			AlignmentType = AlignmentType.Left,
		};
			
		NativeController.AttachNode(demoTextNode, containerNode);
		
		// Text input nodes are for getting strings from the user
		NativeController.AttachNode(new TextInputNode {
			X = 20.0f,
			Size = new Vector2(200.0f, 28.0f),
			IsVisible = true,
			
			// Set maximum string length, can be 0 for no limit
			MaxCharacters = 20,
			
			// You can show or hide the character limit node
			ShowLimitText = true,
			
			// Event that is called with each key pressed
			OnInputReceived = newString => {
				inputString = newString.ToString();
				demoTextNode.Text = $"InputText: {inputString}";
			}, 
			
			// Event that is called when return or escape is pressed
			OnInputComplete = newString => {
				inputString = newString.ToString();
				demoTextNode.Text = $"InputText: {inputString}, Input is complete!";
			}, 
		}, containerNode);
	}

	private static void NumericInput(TreeListCategoryNode treeListCategoryNode) {
		ResNode containerNode;
		treeListCategoryNode.AddNode(containerNode = new ResNode {
			Width = treeListCategoryNode.Width,
			Height = 28.0f,
			IsVisible = true,
		});

		var demoTextNode = new TextNode {
			Position = new Vector2(250.0f, 0.0f),
			Size = new Vector2(300.0f, 28.0f),
			Text = $"InputNumber: Not Set",
			AlignmentType = AlignmentType.Left,
		};
			
		NativeController.AttachNode(demoTextNode, containerNode);
		
		// Numeric Inputs are for getting numbers from the user
		NativeController.AttachNode(new NumericInputNode {
			X = 20.0f,
			Size = new Vector2(200.0f, 28.0f),
			IsVisible = true,
			
			// Event that is called when number is changed
			OnValueUpdate = newValue => {
				demoTextNode.Text = $"InputNumber: {newValue}";
			},
		}, containerNode);
	}
}