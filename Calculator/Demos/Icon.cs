using System.Numerics;
using FFXIVClientStructs.FFXIV.Client.System.String;
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
			IconId = 2, // Inventory
			AcceptedType = DragDropType.MainCommand,
			Payload = new DragDropPayload {
				Type = DragDropType.MainCommand,
				Int2 = 10,
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

				switch(payload.Type) {
				case DragDropType.MainCommand
				when Services.DataManager.GetExcelSheet<MainCommand>().TryGetRow((uint)payload.Int2, out var row):
					node.Payload = payload;
					node.IconId = (uint)row.Icon;
					unsafe { node.IsIconDisabled = !UIModule.Instance()->IsMainCommandUnlocked((uint)payload.Int2); }
					break;
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
				unsafe {
					var dragDropData = data.GetDragDropData();
					var payload = DragDropPayload.FromDragDropInterface(dragDropData.DragDropInterface);

					switch(dragDropData.MouseButtonId) {
					case 0:
						Serilog.Log.Debug("[DragDropNode] Clicked left mouse button: {type} {int1} {int2}", payload.Type, payload.Int1, payload.Int2);
						break;

					case 1:
						Serilog.Log.Debug("[DragDropNode] Clicked right mouse button: {type} {int1} {int2}", payload.Type, payload.Int1, payload.Int2);

						switch(payload.Type) {
						case DragDropType.MainCommand:
							UIModule.Instance()->ExecuteMainCommand((uint)payload.Int2);
							break;
						}
						break;
					}
				}
			},
			OnRollOver = (node, data) => {
				Serilog.Log.Debug("[DragDropNode] RollOver");
				unsafe {
					var addon = RaptureAtkUnitManager.Instance()->GetAddonByNode((AtkResNode*)node);
					if (addon != null) {
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
