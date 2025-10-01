using System.Numerics;
using FFXIVClientStructs.FFXIV.Component.GUI;
using KamiToolKit.Classes;
using KamiToolKit.Nodes;

namespace KamiToolKitDemo.Demos;

public static class CheckBoxes {
	private static HorizontalFlexNode GetContainer(TreeListCategoryNode treeListCategoryNode) => new() {
		Width = treeListCategoryNode.Width,
		AlignmentFlags = FlexFlags.FitContentHeight | FlexFlags.CenterVertically,
		IsVisible = true,
	};
	
	public static void CheckBoxDemo(TreeListCategoryNode treeListCategoryNode) {
		treeListCategoryNode.AddHeader("Checkbox");
		CheckBox(treeListCategoryNode);
	}

	private static void CheckBox(TreeListCategoryNode treeListCategoryNode) {
		var flexGrid = GetContainer(treeListCategoryNode);

		var statusText = new TextNode {
			TextFlags = TextFlags.AutoAdjustNodeSize, 
			Height = 28.0f, 
			IsVisible = true,
			String = "Checked",
		};
		
		var checkBox = new CheckboxNode {
			Size = new Vector2(150.0f, 20.0f),
			IsVisible = true,
			
			// You can set the initial checked/unchecked state
			IsChecked = true,
			
			// Enabled allows interaction, disabled doesn't alloy interaction
			IsEnabled = true,
			
			// Text to show next to the button
			String = "Some awesome checkbox",
			
			// Event that is called when the checkbox state is changed
			OnClick = newState => statusText.String = newState ? "Checked" : "Unchecked",
		};

		var enableDisableButton = new TextButtonNode {
			Size = new Vector2(150.0f, 28.0f),
			IsVisible = true,
			String = "Toggle Enabled",
			OnClick = () => checkBox.IsEnabled = !checkBox.IsEnabled,
		};
		
		flexGrid.AddNode(checkBox);
		flexGrid.AddNode(statusText);
		flexGrid.AddNode(enableDisableButton);

		treeListCategoryNode.AddNode(flexGrid);
	}
}