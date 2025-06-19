using System.Numerics;
using KamiToolKit;
using KamiToolKit.Nodes;
using KamiToolKit.Nodes.Slider;

namespace Calculator.Demos;

public static class Sliders {
	private static NativeController NativeController => Services.NativeController;

	public static void SliderDemo(TreeListCategoryNode treeListCategoryNode) {
		treeListCategoryNode.AddHeader("Slider");
		Slider(treeListCategoryNode);
	}

	private static void Slider(TreeListCategoryNode treeListCategoryNode) {
		ResNode containerNode;
		treeListCategoryNode.AddNode(containerNode = new ResNode {
			Width = treeListCategoryNode.Width,
			Height = 32.0f,
			IsVisible = true,
		});
		
		// Sliders let the user choose values between a set range
		NativeController.AttachNode(new SliderNode {
			X = 20.0f,
			Size = new Vector2(300.0f, 32.0f),
			IsVisible = true,
			
			// Minimum value to allow
			Min = 5,
			
			// Maximum value to allow
			Max = 30,
			
		}, containerNode);
	}
}