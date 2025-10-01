using FFXIVClientStructs.FFXIV.Component.GUI;
using KamiToolKit.Addon;
using KamiToolKit.Nodes;

namespace KamiToolKitDemo.Demos;

public class AddonWidgetDemo : NativeAddon {
	
	private ScrollingAreaNode<TreeListNode>? scrollingAreaNode;
	private TreeListCategoryNode? buttonCategory;
	private TreeListCategoryNode? dropDownCategory;
	private TreeListCategoryNode? iconCategory;
	private TreeListCategoryNode? imageCategory;
	private TreeListCategoryNode? inputTextCategory;
	private TreeListCategoryNode? listCategory;
	private TreeListCategoryNode? sliderCategory;
	private TreeListCategoryNode? progressBarCategory;
	private TreeListCategoryNode? checkboxCategory;
	private TreeListCategoryNode? counterCategory;
	private TreeListCategoryNode? textCategory;
	private TreeListCategoryNode? tabBarCategory;
	private TreeListCategoryNode? textNineGridCategory;
	private TreeListCategoryNode? radioGroupCategory;
	
	protected override unsafe void OnSetup(AtkUnitBase* addon) {
		// ScrollingAreaNode Demo, creates a content node that you can attach your elements to and will scroll the contents
		AttachNode(scrollingAreaNode = new ScrollingAreaNode<TreeListNode> {
			
			// Size and Position is the area that you want to be visible
			Position = ContentStartPosition,
			Size = ContentSize,
			
			// Content Height is how tall you want the entire scrolling area to be 
			ContentHeight = 2000.0f,
			
			// Sets how much the node should move for each tick of scroll (default 24)
			ScrollSpeed = 25,
			
			IsVisible = true,
		});

		scrollingAreaNode.ContentNode.OnLayoutUpdate = newHeight => scrollingAreaNode.ContentHeight = newHeight;

		var treeListNode = scrollingAreaNode.ContentAreaNode;

		// We add each category directly from the root tree node
		treeListNode.AddCategoryNode(buttonCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			String = "Button",
		});
		
		Buttons.ButtonDemo(buttonCategory);
		
		treeListNode.AddCategoryNode(dropDownCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			String = "Drop Down",
		});

		DropDowns.DropDownDemo(dropDownCategory);
		
		treeListNode.AddCategoryNode(iconCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			String = "Icon",
		});
		
		Icons.IconDemo(iconCategory);
		
		treeListNode.AddCategoryNode(imageCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			String = "Image",
		});
		
		Images.ImageDemo(imageCategory);
		
		treeListNode.AddCategoryNode(inputTextCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			String = "Keyboard Input",
		});
		
		KeyboardInput.InputDemo(inputTextCategory);
		
		treeListNode.AddCategoryNode(listCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			String = "List",
		});
		
		Lists.ListDemo(listCategory);
		
		treeListNode.AddCategoryNode(sliderCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			String = "Slider",
		});
		
		Sliders.SliderDemo(sliderCategory);
		
		treeListNode.AddCategoryNode(progressBarCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			String = "Progress Bar",
		});
		
		ProgressBars.ProgressBarDemo(progressBarCategory);
		
		treeListNode.AddCategoryNode(checkboxCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			String = "Checkbox",
		});
		
		CheckBoxes.CheckBoxDemo(checkboxCategory);
		
		treeListNode.AddCategoryNode(counterCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			String = "Counter",
		});
		
		Counters.CountersDemo(counterCategory);
		
		treeListNode.AddCategoryNode(textCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			String = "Text",
		});
		
		Texts.TextDemo(textCategory);
		
		treeListNode.AddCategoryNode(tabBarCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			String = "Tab Bar",
		});
		
		TabBars.TabBarDemo(tabBarCategory);
		
		treeListNode.AddCategoryNode(textNineGridCategory  = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			String = "Text NineGrid",
		});
		
		TextNineGrids.ImageDemo(textNineGridCategory);
		
		treeListNode.AddCategoryNode(radioGroupCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			String = "Radio Group",
		});
		
		RadioGroups.RadioGroupDemo(radioGroupCategory);
	}
}
