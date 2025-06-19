using System.Numerics;
using KamiToolKit;
using KamiToolKit.Nodes;

namespace Calculator.Demos;

public static class Icons {
	private static NativeController NativeController => Services.NativeController;
	
	public static void IconDemo(TreeListCategoryNode treeListCategoryNode) {
		treeListCategoryNode.AddHeader("Icon");
		Icon(treeListCategoryNode);
		
		treeListCategoryNode.AddHeader("DragDropIcon");
		DragDropIcon(treeListCategoryNode);
	}

	public static void Icon(TreeListCategoryNode treeListCategoryNode) {
		ResNode containerNode;
		treeListCategoryNode.AddNode(containerNode = new ResNode {
			Width = treeListCategoryNode.Width,
			Height = 48.0f,
			IsVisible = true,
		});

		IconNode iconNode;
		
		// IconNode's are complex composite nodes involving a **lot** of data from the game
		// These represent interactable icons that the game shows
		// WARNING: WORK IN PROGRESS, INTERACTIONS MAY NOT BEHAVE AS EXPECTED
		//
		// If you only need to show a game icon, see IconImageNode or IconButtonNode
		NativeController.AttachNode(iconNode = new IconNode {
			X = 20.0f,
			Size = new Vector2(44.0f, 44.0f),
			IsVisible = true,
			
			IconId = 2551,
			
			IsMacro = false,
			IsIconDisabled = true,
			IsRecipe = false,

		}, containerNode);
		
		NativeController.AttachNode(new TextButtonNode {
			Position = new Vector2(100.0f, 0.0f),
			Size = new Vector2(150.0f, 28.0f),
			IsVisible = true,
			Label = "Toggle Icon Enabled",
			OnClick = () => {
				iconNode.IsIconDisabled = !iconNode.IsIconDisabled;
			},
		}, containerNode);
	}
	
	private static void DragDropIcon(TreeListCategoryNode treeListCategoryNode) {
		ResNode containerNode;
		treeListCategoryNode.AddNode(containerNode = new ResNode {
			Width = treeListCategoryNode.Width,
			Height = 48.0f,
			IsVisible = true,
		});

		// DragDropNode's are even more complex nodes for accepting and providing actions and icons
		// WARNING: WORK IN PROGRESS, INTERACTIONS MAY NOT BEHAVE AS EXPECTED
		NativeController.AttachNode(new DragDropNode {
			X = 20.0f,
			Size = new Vector2(44.0f, 44.0f),
			IsVisible = true,
			
			IconId = 2551,
			
		}, containerNode);
	}
}