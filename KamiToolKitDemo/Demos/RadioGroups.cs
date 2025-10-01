using KamiToolKit.Nodes;

namespace KamiToolKitDemo.Demos;

public static class RadioGroups {

	public static void RadioGroupDemo(TreeListCategoryNode radioGroupCategory) {
		radioGroupCategory.AddHeader("RadioGroup");
		RadioGroup(radioGroupCategory);
	}

	private static void RadioGroup(TreeListCategoryNode radioGroupCategory) {
		// Vertical node that fits items vertically, and can space elements horizontally via tab-spacing
		var tabList = new TabbedVerticalListNode {
			Width = radioGroupCategory.Width,
			IsVisible = true,
		};

		radioGroupCategory.AddNode(tabList);
		
		// A very basic text node with various default properties set
		var label = new SimpleLabelNode {
			String = "Some String",
		};
		
		tabList.AddNode(label);
		
		// Radio Group Node
		var radioGroup = new RadioButtonGroupNode {
			Width = radioGroupCategory.Width,
			IsVisible = true,
		};
		
		// Here we add options via text strings, and assign callback functions to do something when an option is clicked
		radioGroup.AddButton("First", () => { });
		radioGroup.AddButton("Second", () => { });
		radioGroup.AddButton("Third", () => { });
		
		// Adds a tab amount to all following nodes inserted
		tabList.AddTab(1);
		
		// Add a node at the current tab index
		tabList.AddNode(radioGroup);
		
		// Alternatively, to insert a node at specific tab,
		// this will still add the internal current tab index
		// tabList.AddNode(1, radioGroup);

		// Additionally, you can reduce the tab amount
		// tabList.SubtractTab(1);

		// Can add a negative tab amount
		// tabList.AddNode(-1);
		
		radioGroupCategory.RecalculateLayout();
	}
}