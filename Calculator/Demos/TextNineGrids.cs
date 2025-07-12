using System.Drawing;
using System.Numerics;
using Dalamud.Interface;
using FFXIVClientStructs.FFXIV.Component.GUI;
using KamiToolKit.Classes;
using KamiToolKit.Nodes;

namespace Calculator.Demos;

public static class TextNineGrids {
	private static HorizontalFlexNode GetContainer(TreeListCategoryNode treeListCategoryNode) => new() {
		Width = treeListCategoryNode.Width,
		AlignmentFlags = FlexFlags.FitContentHeight | FlexFlags.CenterVertically | FlexFlags.CenterHorizontally,
		IsVisible = true,
	};
	
	public static void ImageDemo(TreeListCategoryNode textNineGridCategory) {
		textNineGridCategory.AddHeader("TextNineGrid");
		TextNineGrid(textNineGridCategory);
	}

	private static void TextNineGrid(TreeListCategoryNode textNineGridCategory) {
		var flexGrid = GetContainer(textNineGridCategory);

		// Represents a TextNode with a background texture that makes it easier to read the text
		var textNineGrid = new TextNineGridNode {
			IsVisible = true, 
			
			// Width value of size is ignored as the actual size is computed
			Size = new Vector2(150.0f, 26.0f),
			
			// Value to show
			Label = "Some Text Label, 69.420",

			// Font type to use
			FontType = FontType.TrumpGothic,
			
			// Color to display
			TextColor = KnownColor.OrangeRed.Vector().Lighten(0.80f),
		};
		
		flexGrid.AddNode(textNineGrid);
		textNineGridCategory.AddNode(flexGrid);
	}
}