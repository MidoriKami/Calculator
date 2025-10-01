using System.Numerics;
using FFXIVClientStructs.FFXIV.Component.GUI;
using KamiToolKit.Classes;
using KamiToolKit.Nodes;

namespace KamiToolKitDemo.Demos;

public static class KeyboardInput {
	private static HorizontalFlexNode GetContainer(TreeListCategoryNode treeListCategoryNode) => new() {
		Width = treeListCategoryNode.Width,
		AlignmentFlags = FlexFlags.FitHeight | FlexFlags.CenterHorizontally | FlexFlags.FitContentHeight,
		IsVisible = true,
	};

	public static void InputDemo(TreeListCategoryNode treeListCategoryNode) {
		treeListCategoryNode.AddHeader("TextInput");
		TextInput(treeListCategoryNode);
		
		treeListCategoryNode.AddHeader("NumericInput");
		NumericInput(treeListCategoryNode);
	}

	private static void TextInput(TreeListCategoryNode treeListCategoryNode) {
		var flexGrid = GetContainer(treeListCategoryNode);

		var inputString = string.Empty;
		var demoTextNode = new TextNode {
			Position = new Vector2(250.0f, 0.0f),
			Size = new Vector2(300.0f, 28.0f),
			String = $"InputText: {inputString}",
			AlignmentType = AlignmentType.Left,
		};
		
		// Text input nodes are for getting strings from the user
		var textInputNode = new TextInputNode {
			Size = new Vector2(200.0f, 28.0f),
			IsVisible = true,

			// Set maximum string length, can be 0 for no limit
			MaxCharacters = 20,

			// You can show or hide the character limit node
			ShowLimitText = true,

			// Event that is called with each key pressed
			OnInputReceived = newString => {
				inputString = newString.ToString();
				demoTextNode.String = $"InputText: {inputString}";
			},

			// Event that is called when return or escape is pressed
			OnInputComplete = newString => {
				inputString = newString.ToString();
				demoTextNode.String = $"InputText: {inputString}, Input is complete!";
			},
		};
		
		flexGrid.AddNode(textInputNode);
		flexGrid.AddNode(demoTextNode);
		treeListCategoryNode.AddNode(flexGrid);
	}

	private static void NumericInput(TreeListCategoryNode treeListCategoryNode) {
		var flexGrid = GetContainer(treeListCategoryNode);

		var demoTextNode = new TextNode {
			Position = new Vector2(250.0f, 0.0f),
			Size = new Vector2(300.0f, 28.0f),
			String = "InputNumber: Not Set",
			AlignmentType = AlignmentType.Left,
		};
		
		// Numeric Inputs are for getting numbers from the user
		var numericInputNode = new NumericInputNode {
			Size = new Vector2(200.0f, 28.0f),
			IsVisible = true,

			// Min Value
			Min = -100,
			
			// Max value
			Max = 100,
			
			// How much is added or subtracted when pressing + or -
			Step = 2,
			
			// Event that is called when number is changed
			OnValueUpdate = newValue => {
				demoTextNode.String = $"InputNumber: {newValue}";
			},
		};
		
		flexGrid.AddNode(numericInputNode);
		flexGrid.AddNode(demoTextNode);
		treeListCategoryNode.AddNode(flexGrid);
	}
}