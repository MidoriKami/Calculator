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
		
		var demoTextNode = new TextNode {
			Position = new Vector2(120.0f, 12.0f),
		};
			
		NativeController.AttachNode(demoTextNode, containerNode);
		
		// Sliders let the user choose values between a set range
		NativeController.AttachNode(new SliderNode {
			X = 20.0f,
			Size = new Vector2(300.0f, 32.0f),
			IsVisible = true,
			
			// Minimum value to allow
			Min = 5,
			
			// Maximum value to allow
			Max = 30,
			
			// Event that is called when the value changes
			OnValueChanged = newValue => demoTextNode.Text = $"Value: {newValue}",
			
		}, containerNode);
	}
}