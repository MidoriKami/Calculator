using System.Numerics;
using FFXIVClientStructs.FFXIV.Component.GUI;
using KamiToolKit.Nodes;

namespace KamiToolKitDemo.Demos;

public static class Texts {
	public static void TextDemo(TreeListCategoryNode treeListCategoryNode) {
		treeListCategoryNode.AddHeader("Text");
		Text(treeListCategoryNode);
	}

	private static void Text(TreeListCategoryNode treeListCategoryNode) {

		// Node for displaying text
		var textNode = new TextNode {
			Size = new Vector2(treeListCategoryNode.Width, 200.0f),
			IsVisible = true,
			
			// Various font types are available, some fonts only have glyphs for numbers
			FontType = FontType.Jupiter,
			
			// For this example, we have a big textblock that we want to multiline and wordwrap
			TextFlags = TextFlags.WordWrap | TextFlags.MultiLine,
			
			// Default fontsize is 12, we want this to be a little bigger
			FontSize = 16,
			
			// Vertical spacing between lines not including character height, only needed because we are multiline
			LineSpacing = 18,
			
			// It's important to set the string last, as it will use the above parameters to set the style
			String = "Hey GCBTW, just wanted to say CONGRATULATIONS (ucob will be harder in 2020) on the THE UNENDING COIL OF BAHAMUT 2017 " +
			         "(it will be harder in 2020) CLEAR. I know you've been working really hard (ucob will be harder) at it, and I'm happy " +
			         "that you've (used divebomb markers that wont be available in 2020 and) achieved your goal of completion. " +
			         "I know your journey through (ucob will be even harder) the content was filled with ups and downs, " +
			         "but you and (divebomb markers wont be available in 2020) your group stuck t(ucob will be harder)ogether through them all," +
			         " and you were rewarded for your patience and (divebomb markers wont be available in 2020) tenacity. Congratulations " +
			         "(divebomb markers wont be available in 2020) once again, and here's to more success in your future (ucob will be harder) endeavors!!!",
		};
		
		treeListCategoryNode.AddNode(textNode);
	}
}