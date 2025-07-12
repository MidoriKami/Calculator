using System.Numerics;
using KamiToolKit.Classes;
using KamiToolKit.Nodes;
using KamiToolKit.Nodes.Slider;

namespace Calculator.Demos;

public static class Counters {
	private static HorizontalFlexNode GetContainer(TreeListCategoryNode treeListCategoryNode) => new() {
		Width = treeListCategoryNode.Width / 2.0f,
		AlignmentFlags = FlexFlags.CenterVertically | FlexFlags.FitContentHeight | FlexFlags.CenterHorizontally,
		IsVisible = true,
	};
	
	public static void CountersDemo(TreeListCategoryNode treeListCategoryNode) {
		treeListCategoryNode.AddHeader("Counter");
		Counter(treeListCategoryNode);
		
		treeListCategoryNode.AddHeader("SimpleCounter");
		SimpleCounter(treeListCategoryNode);
	}

	private static void Counter(TreeListCategoryNode treeListCategoryNode) {
		var flexGrid = GetContainer(treeListCategoryNode);

		// This version of counter node does not set a default texture for displaying the digits
		// You must set one for the count to be visible
		var counter = new CounterNode {
			Size = new Vector2(treeListCategoryNode.Width / 2.0f, 48.0f),
			IsVisible = true,
			
			// Specify the texture to load
			TexturePath = "ui/uld/RaceChocoboNum.tex",
			
			// The size of the texture to load
			TextureSize = new Vector2(30.0f, 60.0f),
			
			// The coordinates within the texture
			TextureCoordinates = Vector2.Zero,
			
			// Size in pixels for each digit, you can make this smaller to squish the numbers together
			NumberWidth = 32,
			
			// The value to display
			Number = 1234567890,
		};

		var slider = new SliderNode {
			Size = new Vector2(200.0f, 32.0f), 
			IsVisible = true,
			Min = 0,
			Max = int.MaxValue,
			OnValueChanged = newValue => counter.Number = newValue,
		};
		
		flexGrid.AddNode(counter);
		flexGrid.AddNode(slider);
		treeListCategoryNode.AddNode(flexGrid);
	}

	private static void SimpleCounter(TreeListCategoryNode treeListCategoryNode) {
		var flexGrid = GetContainer(treeListCategoryNode);

		// Simpler version of counter node with a default font and default params configured for you
		var counter = new SimpleCounterNode {
			Size = new Vector2(treeListCategoryNode.Width / 2.0f, 48.0f),
			IsVisible = true,
			
			// The value to display
			Number = 1234567890,
		};

		var slider = new SliderNode {
			Size = new Vector2(200.0f, 32.0f), 
			IsVisible = true,
			Min = 0,
			Max = int.MaxValue,
			OnValueChanged = newValue => counter.Number = newValue,
		};
		
		flexGrid.AddNode(counter);
		flexGrid.AddNode(slider);
		treeListCategoryNode.AddNode(flexGrid);
	}
}