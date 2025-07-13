using System.Numerics;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Component.GUI;
using KamiToolKit.Classes;
using KamiToolKit.Extensions;
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

	public static void Icon(TreeListCategoryNode treeListCategoryNode) {
		var flexGrid = GetContainer(treeListCategoryNode);

		// IconNode's are complex composite nodes involving a **lot** of data from the game
		// These represent interactable icons that the game shows
		// WARNING: WORK IN PROGRESS, INTERACTIONS MAY NOT BEHAVE AS EXPECTED
		//
		// If you only need to show a game icon, see IconImageNode or IconButtonNode
		var iconNode = new IconNode {
			X = 20.0f,
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

	private static void DragDropIcon(TreeListCategoryNode treeListCategoryNode) {
		var flexGrid = GetContainer(treeListCategoryNode);

		// DragDropNode's are even more complex nodes for accepting and providing actions and icons
		var dragDropNode = new DragDropNode {
			X = 20.0f,
			Size = new Vector2(44.0f, 44.0f),
			IsVisible = true,
			IconId = 2, // Inventory Icon
			AcceptedType = DragDropType.MainCommand,
			Payload = new DragDropPayload {
				Type = DragDropType.MainCommand,
				Int2 = 10, // MainCommand.RowId of Inventory
			},
			IsClickable = true,
			OnBegin = (node, data) => {
				Serilog.Log.Debug("[DragDropNode] DragDrop started");
			},
			OnEnd = (node, data) => {
				Serilog.Log.Debug("[DragDropNode] DragDrop ended");
			},
			OnPayloadAccepted = (node, data, payload) => {
				Serilog.Log.Debug("[DragDropNode] Payload Accepted: {type} {int1} {int2}", payload.Type, payload.Int1, payload.Int2);

				node.Payload.Clear();
				node.IconId = 0;

				if (payload.Type.Accepts(DragDropType.MainCommand)
				&& Services.DataManager.GetExcelSheet<MainCommand>().TryGetRow((uint)payload.Int2, out var row)) {
					// manually copy payload data to avoid pulling in DragDropType.ActionBar_MainCommand
					node.Payload.Type = DragDropType.MainCommand;
					node.Payload.Int2 = payload.Int2;
					node.IconId = (uint)row.Icon;
					unsafe { node.IsIconDisabled = !UIModule.Instance()->IsMainCommandUnlocked((uint)payload.Int2); }
				}
			},
			OnPayloadRejected = (node, data, payload) => {
				Serilog.Log.Debug("[DragDropNode] Payload Rejected: {type} {int1} {int2}", payload.Type, payload.Int1, payload.Int2);
			},
			OnDiscard = (node, data) => {
				Serilog.Log.Debug("[DragDropNode] Payload Discarded");
				node.IconId = 0;
				node.Payload.Clear();
			},
			OnClicked = (node, data) => {
				var dragDropData = data.GetDragDropData();

				switch (dragDropData.MouseButtonId) {
				case 0:
					Serilog.Log.Debug("[DragDropNode] Clicked left mouse button: {type} {int1} {int2}", node.Payload.Type, node.Payload.Int1, node.Payload.Int2);
					break;

				case 1:
					Serilog.Log.Debug("[DragDropNode] Clicked right mouse button: {type} {int1} {int2}", node.Payload.Type, node.Payload.Int1, node.Payload.Int2);

					switch(node.Payload.Type) {
					case DragDropType.MainCommand:
						unsafe { UIModule.Instance()->ExecuteMainCommand((uint)node.Payload.Int2); }
						break;
					}
					break;
				}
			},
			OnRollOver = (node, data) => {
				Serilog.Log.Debug("[DragDropNode] RollOver");
				unsafe {
					if (AtkStage.Instance()->DragDropManager.IsDragging)
						return;

					var addon = RaptureAtkUnitManager.Instance()->GetAddonByNode((AtkResNode*)node);
					if (addon == null)
						return;

					var tooltipArgs = new AtkTooltipManager.AtkTooltipArgs();
					tooltipArgs.Ctor();
					tooltipArgs.TypeSpecificId = (ulong)node.Payload.Int2;
					tooltipArgs.Unk_16 = (byte)ActionKind.MainCommand;

					AtkStage.Instance()->TooltipManager.ShowTooltip(
						AtkTooltipManager.AtkTooltipType.Action,
						addon->Id,
						(AtkResNode*)node.InternalComponentNode,
						&tooltipArgs);
				}
			},
			OnRollOut = (node, data) => {
				Serilog.Log.Debug("[DragDropNode] RollOut");
				unsafe {
					var addon = RaptureAtkUnitManager.Instance()->GetAddonByNode((AtkResNode*)node);
					if(addon != null) {
						AtkStage.Instance()->TooltipManager.HideTooltip(addon->Id);
					}
				}
			},
		};

		flexGrid.AddNode(dragDropNode);
		flexGrid.AddDummy(new ResNode(), 0.0f);

		treeListCategoryNode.AddNode(flexGrid);
	}
}
