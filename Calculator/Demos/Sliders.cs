using System.Numerics;
using FFXIVClientStructs.FFXIV.Component.GUI;
using KamiToolKit.Classes;
using KamiToolKit.Nodes;
using KamiToolKit.Nodes.Slider;

namespace Calculator.Demos;

public static class Sliders {
	private static HorizontalFlexNode GetContainer(TreeListCategoryNode treeListCategoryNode) => new() {
		Width = treeListCategoryNode.Width,
		AlignmentFlags = FlexFlags.CenterHorizontally | FlexFlags.FitContentHeight,
		IsVisible = true,
	};
	
	private static TextNode GetTextNode() => new() {
		TextFlags = TextFlags.AutoAdjustNodeSize,
		AlignmentType = AlignmentType.Left,
		String = "No option selected",
		Height = 32.0f,
	};

	public static void SliderDemo(TreeListCategoryNode treeListCategoryNode) {
		treeListCategoryNode.AddHeader("Slider");
		Slider(treeListCategoryNode);
	}

	private static void Slider(TreeListCategoryNode treeListCategoryNode) {
		var flexGrid = GetContainer(treeListCategoryNode);
		var textNode = GetTextNode();
		
		// Sliders let the user choose values between a set range
		var sliderNode = new SliderNode {
			Size = new Vector2(300.0f, 32.0f),
			IsVisible = true,

			// Minimum value to allow
			Min = 5,

			// Maximum value to allow
			Max = 30,

			// Event that is called when the value changes
			OnValueChanged = newValue => textNode.String = $"Value: {newValue}",
		};
		
		flexGrid.AddNode(sliderNode);
		flexGrid.AddNode(textNode);
		treeListCategoryNode.AddNode(flexGrid);
	}
}