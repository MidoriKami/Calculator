using System.Numerics;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Component.GUI;
using KamiToolKit.Classes;
using KamiToolKit.Nodes;
using KamiToolKit.System;
using Lumina.Excel.Sheets;

namespace Calculator.Demos;

public static class DropDowns {
	private static HorizontalFlexNode<NodeBase> GetContainer(TreeListCategoryNode treeListCategoryNode) => new() {
		Width = treeListCategoryNode.Width,
		Height = 250.0f,
		AlignmentFlags = FlexFlags.FitWidth,
		IsVisible = true,
	};

	private static TextNode GetTextNode() => new() {
		TextFlags = TextFlags.AutoAdjustNodeSize,
		AlignmentType = AlignmentType.Left,
		Height = 28.0f,
		Text = "No option selected",
	};
	
	public static void DropDownDemo(TreeListCategoryNode treeListCategoryNode) {
		treeListCategoryNode.AddHeader("TextDropDown");
		TextDropDown(treeListCategoryNode);
		
		treeListCategoryNode.AddHeader("LuminaDropDown");
		LuminaDropDown(treeListCategoryNode);
	}

	private static void TextDropDown(TreeListCategoryNode treeListCategoryNode) {
		var flexGrid = GetContainer(treeListCategoryNode);
		var textNode = GetTextNode();

		// Basic text dropdown node, you can set various text options that the user can select
		// Note: the number of buttons generated is dependent on the higher of the dropdown element
		var textDropDown = new TextDropDownNode {
			Size = new Vector2(300.0f, 28.0f),
			IsVisible = true,

			// How many buttons the list will have at most, if there are fewer options, fewer buttons will be shown
			MaxListOptions = 2,

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

			// DropDown's provide an OnOptionSelected callback to trigger doing something when the user selects an option
			OnOptionSelected = option => {
				textNode.Text = $"Option Selected: {option}";
			},
		};
		
		textNode.Height = textDropDown.Height;
		
		flexGrid.AddNode(textDropDown);
		flexGrid.AddNode(textNode);
		
		treeListCategoryNode.AddNode(flexGrid);
	}

	private static void LuminaDropDown(TreeListCategoryNode treeListCategoryNode) {
		var flexGrid = GetContainer(treeListCategoryNode);
		var textNode = GetTextNode();
		
		// LuminaDropDowns extract data from the games internal datasheets
		// This allows you to let the user choose from a filtered list of data
		var luminaDropDown = new LuminaDropDownNode<Aetheryte> {
			Size = new Vector2(300.0f, 28.0f),
			IsVisible = true,
			
			// How many buttons the list will have at most, if there are fewer options, fewer buttons will be shown
			MaxListOptions = 9,
			
			// Here we define a filter function that will limit the range of options
			FilterFunction = option => option.IsAetheryte && !option.AethernetName.Value.Name.ExtractText().IsNullOrEmpty(),
			
			// As each lumina object could have several different strings, we have to define what we want to show to the user
			LabelFunction = option => option.AethernetName.Value.Name.ExtractText(),
			
			// Finally, our callback will give us a reference to the full lumina object that represents the option the user chose
			OnOptionSelected = option => {
				textNode.Text = $"Option Selected: {option.AethernetName.Value.Name.ExtractText()}";
			},
		};
		
		textNode.Height = luminaDropDown.Height;
		
		flexGrid.AddNode(luminaDropDown);
		flexGrid.AddNode(textNode);
		
		treeListCategoryNode.AddNode(flexGrid);
	}
}