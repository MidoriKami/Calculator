using System.Numerics;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Component.GUI;
using KamiToolKit.Classes;
using KamiToolKit.Nodes;
using Lumina.Excel.Sheets;

namespace Calculator.Demos;

public static class Icons {
	private static HorizontalFlexNode GetContainer(TreeListCategoryNode treeListCategoryNode) => new() {
		Width = treeListCategoryNode.Width,
		AlignmentFlags = FlexFlags.FitContentHeight | FlexFlags.CenterVertically | FlexFlags.CenterHorizontally,
		IsVisible = true,
	};

	public static void IconDemo(TreeListCategoryNode treeListCategoryNode) {
		treeListCategoryNode.AddHeader("Icon");
		Icon(treeListCategoryNode);

		treeListCategoryNode.AddHeader("DragDropIcon");
		DragDropIcon(treeListCategoryNode);
	}

	private static void Icon(TreeListCategoryNode treeListCategoryNode) {
		var flexGrid = GetContainer(treeListCategoryNode);

		// IconNode's are complex composite nodes involving a **lot** of data from the game
		// If you only need to show a game icon, see IconImageNode or IconButtonNode
		var iconNode = new IconNode {
			Size = new Vector2(44.0f, 44.0f),
			IsVisible = true,
			IconId = 2551,
			IsMacro = false,
			IsIconDisabled = true,
			IsRecipe = false,
		};

		var button = new TextButtonNode {
			Position = new Vector2(100.0f, 0.0f),
			Size = new Vector2(150.0f, 28.0f),
			IsVisible = true,
			Label = "Toggle Icon Enabled",
			OnClick = () => {
				iconNode.IsIconDisabled = !iconNode.IsIconDisabled;
			},
		};

		flexGrid.AddNode(iconNode);
		flexGrid.AddNode(button);

		treeListCategoryNode.AddNode(flexGrid);
	}

	private static unsafe void DragDropIcon(TreeListCategoryNode treeListCategoryNode) {
		var flexGrid = GetContainer(treeListCategoryNode);

		// DragDropNode's are even more complex nodes for accepting and providing actions and icons
		var dragDropNode = new DragDropNode {
			Size = new Vector2(44.0f, 44.0f),
			IsVisible = true,
			
			// IconId to display (this example is the iconId of the MainCommand for Inventory)
			IconId = 2,
			
			// What category should this icon interact with
			AcceptedType = DragDropType.Everything,
			
			// Set to false to prevent click-dragging
			IsDraggable = true,
			
			// Payload information is what's used to set other drag drop slots.
			Payload = new DragDropPayload {
				
				// Type you want to set in target
				Type = DragDropType.MainCommand,
				
				// Data id you want to set in target (this example is the MainCommand.RowId for the Inventory Main Command) 
				Int2 = 10,
			},

			// Allow left-clicking to trigger OnClicked
			IsClickable = false,
			
			// Event that is called when the button is moused over, this example shows a fancy tooltip for the action in the slot
			OnRollOver = (node, _) => node.ShowTooltip(AtkTooltipManager.AtkTooltipType.Action, ActionKind.MainCommand),
			
			// Event that is called once the button is click-dragged
			OnBegin = (_, _) => { },
			
			// Event that is called once the button is no longer being click-dragged
			OnEnd = (_, _) => { },
			
			// Event that is called when a drag drop element is dropped into our node
			// If this node's accepted type allows this payload, then OnPayloadAccepted is invoked
			OnPayloadAccepted = (node, _, payload) => {
				
				// You will need to set this node to the correct data for the payload being received
				
				if (payload.Type.Accepts(DragDropType.MainCommand) && Services.DataManager.GetExcelSheet<MainCommand>().TryGetRow((uint)payload.Int2, out var row)) {
					// manually copy payload data to avoid pulling in DragDropType.ActionBar_MainCommand
					node.Payload.Type = DragDropType.MainCommand;
					node.Payload.Int2 = payload.Int2;
					node.IconId = (uint)row.Icon;
					node.IsIconDisabled = !UIModule.Instance()->IsMainCommandUnlocked((uint)payload.Int2);
				}
			},
			
			// Event that is fired when drag is dropped into the world to discard the thing
			OnDiscard = (node, _) => node.Clear(),
			
			// Event that is fired when the button is clicked on
			OnClicked = (node, _) => UIModule.Instance()->ExecuteMainCommand((uint)node.Payload.Int2),

			// Event that is fired when the cursor is no longer hovering over the drag drop node
			OnRollOut = (node, _) => node.HideTooltip(),
		};

		flexGrid.AddNode(dragDropNode);
		flexGrid.AddDummy(new ResNode(), 0.0f);

		treeListCategoryNode.AddNode(flexGrid);
	}
}
