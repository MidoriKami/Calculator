using System.Numerics;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Component.GUI;
using KamiToolKit.Classes;
using KamiToolKit.Nodes;
using Lumina.Excel.Sheets;

namespace Calculator.Demos;

public static class Lists {
	private static HorizontalFlexNode GetContainer(TreeListCategoryNode treeListCategoryNode) => new() {
		Width = treeListCategoryNode.Width,
		Height = 200.0f,
		AlignmentFlags = FlexFlags.FitWidth,
		IsVisible = true,
	};
	
	private static TextNode GetTextNode() => new() {
		TextFlags = TextFlags.AutoAdjustNodeSize,
		AlignmentType = AlignmentType.Left,
		Text = "No option selected",
		Height = 200.0f,
	};
	
	public static void ListDemo(TreeListCategoryNode treeListCategoryNode) {
		treeListCategoryNode.AddHeader("TextList");
		TextList(treeListCategoryNode);
		
		treeListCategoryNode.AddHeader("LuminaList");
		LuminaList(treeListCategoryNode);
	}

	private static void TextList(TreeListCategoryNode treeListCategoryNode) {
		var flexGrid = GetContainer(treeListCategoryNode);
		var textNode = GetTextNode();
		
		// Basic text list node, you can set various text options that the user can select
		// Note: the number of buttons generated is dependent on the higher of the dropdown element
		var textListNode = new TextListNode {
			Size = new Vector2(300.0f, 150.0f),
			IsVisible = true,

			// Set how many options at most we will show at a time
			MaxButtons = 3,
			
			// Here we can select which texture we want to use for the button
			Options = [
				"Pick Me!",
				"No me!",
				"I only tell truth",
				"I only lie",
				"I'm not associate with those two",
				"This is the cool one",
				"Get in the car Morty! NOW!",
				"Sir, this is a Wendy's",
				"No! This is Patrick!",
			],

			// Lists provide an OnOptionSelected callback to trigger doing something when the user selects an option
			OnOptionSelected = option => {
				textNode.Text = $"Option Selected: {option}";
			},
		};
		
		flexGrid.AddNode(textListNode);
		flexGrid.AddNode(textNode);
		treeListCategoryNode.AddNode(flexGrid);
	}

	private static void LuminaList(TreeListCategoryNode treeListCategoryNode) {
		var flexGrid = GetContainer(treeListCategoryNode);
		var textNode = GetTextNode();
		
		// LuminaLists extract data from the games internal datasheets
		// This allows you to let the user choose from a filtered list of data
		var luminaListNode = new LuminaListNode<Aetheryte> {
			Size = new Vector2(300.0f, 150.0f),
			IsVisible = true,
						
			// Set how many options at most we will show at a time
			MaxButtons = 8,
			
			// Here we define a filter function that will limit the range of options
			FilterFunction = option => option.IsAetheryte && !option.AethernetName.Value.Name.ExtractText().IsNullOrEmpty(),

			// As each lumina object could have several different strings, we have to define what we want to show to the user
			LabelFunction = option => option.AethernetName.Value.Name.ExtractText(),

			// Finally, our callback will give us a reference to the full lumina object that represents the option the user chose
			OnOptionSelected = option => {
				textNode.Text = $"Option Selected: {option.AethernetName.Value.Name.ExtractText()}";
			},
		};
		
		flexGrid.AddNode(luminaListNode);
		flexGrid.AddNode(textNode);
		treeListCategoryNode.AddNode(flexGrid);
	}
}