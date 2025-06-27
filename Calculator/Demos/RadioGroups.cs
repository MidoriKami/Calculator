using KamiToolKit.Nodes;

namespace Calculator.Demos;

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
			Text = "Some Label",
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
		
		tabList.AddTab(1);
		tabList.AddNode(radioGroup);
		
	}
}