using System.Numerics;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Component.GUI;
using KamiToolKit;
using KamiToolKit.Nodes;
using Lumina.Excel.Sheets;

namespace Calculator.Demos;

public static class Lists {
	private static NativeController NativeController => Services.NativeController;
	
	public static void ListDemo(TreeListCategoryNode treeListCategoryNode) {
		treeListCategoryNode.AddHeader("TextList");
		TextList(treeListCategoryNode);
		
		treeListCategoryNode.AddHeader("LuminaList");
		LuminaList(treeListCategoryNode);
	}

	private static void TextList(TreeListCategoryNode treeListCategoryNode) {
		ResNode containerNode;
		treeListCategoryNode.AddNode(containerNode = new ResNode {
			Width = treeListCategoryNode.Width,
			Height = 150.0f,
			IsVisible = true,
		});

		var demoTextNode = new TextNode {
			Position = new Vector2(325.0f, 0.0f),
			Size = new Vector2(300.0f, 28.0f),
			AlignmentType = AlignmentType.Left,
			Text = "No Option Selected",
		};
		
		NativeController.AttachNode(demoTextNode, containerNode);
		
		// Basic text list node, you can set various text options that the user can select
		// Note: the number of buttons generated is dependent on the higher of the dropdown element
		NativeController.AttachNode(new TextListNode {
			X = 20.0f,
			Size = new Vector2(300.0f, 150.0f),
			IsVisible = true,
			
			// Here we can select which texture we want to use for the button
			Options = [
				"Pick Me!",
				"No me!",
				"I only tell truth",
				"I only lie",
				"I'm not associate with those two",
				"This is the cool one",
				"Get in the car Morty! NOW!",
				"Sir, this is a Wendy's",
				"No! This is Patrick!",
			],
			
			// Lists provide an OnOptionSelected callback to trigger doing something when the user selects an option
			OnOptionSelected = option => {
				demoTextNode.Text = $"Option Selected: {option}";
			},
		}, containerNode);
	}

	private static void LuminaList(TreeListCategoryNode treeListCategoryNode) {
		ResNode containerNode;
		treeListCategoryNode.AddNode(containerNode = new ResNode {
			Width = treeListCategoryNode.Width,
			Height = 150.0f,
			IsVisible = true,
		});

		var demoTextNode = new TextNode {
			Position = new Vector2(325.0f, 0.0f),
			Size = new Vector2(300.0f, 28.0f),
			AlignmentType = AlignmentType.Left,
			Text = "No Option Selected",
		};
		
		NativeController.AttachNode(demoTextNode, containerNode);
		
		// LuminaLists extract data from the games internal datasheets
		// This allows you to let the user choose from a filtered list of data
		NativeController.AttachNode(new LuminaListNode<Aetheryte> {
			X = 20.0f,
			Size = new Vector2(300.0f, 150.0f),
			IsVisible = true,

			// Here we define a filter function that will limit the range of options
			FilterFunction = option => option.IsAetheryte && !option.AethernetName.Value.Name.ExtractText().IsNullOrEmpty(),
			
			// As each lumina object could have several different strings, we have to define what we want to show to the user
			LabelFunction = option => option.AethernetName.Value.Name.ExtractText(),
			
			// Finally, our callback will give us a reference to the full lumina object that represents the option the user chose
			OnOptionSelected = option => {
				demoTextNode.Text = $"Option Selected: {option.AethernetName.Value.Name.ExtractText()}";
			},
		}, containerNode);
	}
}