using System.Numerics;
using FFXIVClientStructs.FFXIV.Component.GUI;
using KamiToolKit;
using KamiToolKit.Nodes;
using KamiToolKit.Nodes.TabBar;

namespace Calculator.Demos;

public static class TabBars {
	private static NativeController NativeController => Services.NativeController;
	
	public static void TabBarDemo(TreeListCategoryNode tabBarCategory) {
		tabBarCategory.AddHeader("TabBar");
		TabBar(tabBarCategory);
	}

	private static void TabBar(TreeListCategoryNode tabBarCategory) {
		var container = new VerticalListNode {
			Size = new Vector2(tabBarCategory.Width, 0.0f),
			IsVisible = true,
			FitContents = true,
		};

		var tabContainer = new ResNode {
			Size = new Vector2(tabBarCategory.Width, 28.0f), IsVisible = true,
		};

		var textNode1 = new TextNode {
			TextFlags = TextFlags.AutoAdjustNodeSize, Text = "First Tab Element", IsVisible = true,
		};
		
		var textNode2 = new TextNode {
			TextFlags = TextFlags.AutoAdjustNodeSize, Text = "Second Tab Element", IsVisible = false, 
		};
		
		var textNode3 = new TextNode {
			TextFlags = TextFlags.AutoAdjustNodeSize, Text = "Third Tab Element", IsVisible = false, 
		};
		
		// The tab bar itself is a very simple node, it only provides the ability to .AddTab with a label
		// and to set up onclick events when a tab is clicked
		var tabBar = new TabBarNode {
			Size = new Vector2(tabBarCategory.Width, 24.0f), 
			IsVisible = true,
		};

		tabBar.AddTab("First", () => {
			textNode1.IsVisible = true;
			textNode2.IsVisible = false;
			textNode3.IsVisible = false;
		});
		
		tabBar.AddTab("Second", () => {
			textNode1.IsVisible = false;
			textNode2.IsVisible = true;
			textNode3.IsVisible = false;
		});
		
		tabBar.AddTab("Third", () => {
			textNode1.IsVisible = false;
			textNode2.IsVisible = false;
			textNode3.IsVisible = true;
		});
		
		NativeController.AttachNode(textNode1, tabContainer);
		NativeController.AttachNode(textNode2, tabContainer);
		NativeController.AttachNode(textNode3, tabContainer);
		
		container.AddNode(tabBar);
		container.AddNode(tabContainer);
		tabBarCategory.AddNode(container);
	}
}