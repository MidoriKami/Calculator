﻿using FFXIVClientStructs.FFXIV.Component.GUI;
using KamiToolKit.Addon;
using KamiToolKit.Nodes;

namespace Calculator.Demos;

public class AddonWidgetDemo : NativeAddon {
	
	private ScrollingAreaNode? scrollingAreaNode;
	private TreeListNode? treeListNode;
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
		AttachNode(scrollingAreaNode = new ScrollingAreaNode {
			
			// Size and Position is the area that you want to be visible
			Position = ContentStartPosition,
			Size = ContentSize,
			
			// Content Height is how tall you want the entire scrolling area to be 
			ContentHeight = 2000.0f,
			
			// Sets how much the node should move for each tick of scroll (default 24)
			ScrollSpeed = 100,
			
			IsVisible = true,
		});
		
		// Creates a new treelist node that will contain all of our custom category nodes
		treeListNode = new TreeListNode {
			Size = scrollingAreaNode.ContentNode.Size,
			IsVisible = true,
			
			// We can set an event to change the size of our scrolling area, depending on if the trees are collapsed
			OnLayoutUpdate = newHeight => scrollingAreaNode.ContentHeight = newHeight,
		};
		
		// We add each category directly from the root tree node
		treeListNode.AddCategoryNode(buttonCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			Label = "Button",
		});
		
		// When attaching nodes, KamiToolKit will attempt to register events and data from its parent, and then pass that to the children
		// This means that it *should* work no matter what order you attach things in, as long as eventually something is attached to native
		AttachNode(treeListNode, scrollingAreaNode.ContentNode);
		
		Buttons.ButtonDemo(buttonCategory);
		
		treeListNode.AddCategoryNode(dropDownCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			Label = "Drop Down",
		});

		DropDowns.DropDownDemo(dropDownCategory);
		
		treeListNode.AddCategoryNode(iconCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			Label = "Icon",
		});
		
		Icons.IconDemo(iconCategory);
		
		treeListNode.AddCategoryNode(imageCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			Label = "Image",
		});
		
		Images.ImageDemo(imageCategory);
		
		treeListNode.AddCategoryNode(inputTextCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			Label = "Keyboard Input",
		});
		
		KeyboardInput.InputDemo(inputTextCategory);
		
		treeListNode.AddCategoryNode(listCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			Label = "List",
		});
		
		Lists.ListDemo(listCategory);
		
		treeListNode.AddCategoryNode(sliderCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			Label = "Slider",
		});
		
		Sliders.SliderDemo(sliderCategory);
		
		treeListNode.AddCategoryNode(progressBarCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			Label = "Progress Bar",
		});
		
		ProgressBars.ProgressBarDemo(progressBarCategory);
		
		treeListNode.AddCategoryNode(checkboxCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			Label = "Checkbox",
		});
		
		CheckBoxes.CheckBoxDemo(checkboxCategory);
		
		treeListNode.AddCategoryNode(counterCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			Label = "Counter",
		});
		
		Counters.CountersDemo(counterCategory);
		
		treeListNode.AddCategoryNode(textCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			Label = "Text",
		});
		
		Texts.TextDemo(textCategory);
		
		treeListNode.AddCategoryNode(tabBarCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			Label = "Tab Bar",
		});
		
		TabBars.TabBarDemo(tabBarCategory);
		
		treeListNode.AddCategoryNode(textNineGridCategory  = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			Label = "Text NineGrid",
		});
		
		TextNineGrids.ImageDemo(textNineGridCategory);
		
		treeListNode.AddCategoryNode(radioGroupCategory = new TreeListCategoryNode {
			IsVisible = true,
			IsCollapsed = true,
			Label = "Radio Group",
		});
		
		RadioGroups.RadioGroupDemo(radioGroupCategory);
	}
}
