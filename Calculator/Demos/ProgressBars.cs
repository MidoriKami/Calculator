using System;
using System.Drawing;
using System.Numerics;
using Dalamud.Interface;
using KamiToolKit.Classes;
using KamiToolKit.Nodes;
using KamiToolKit.Nodes.Slider;

namespace Calculator.Demos;

public static class ProgressBars {
	private static HorizontalFlexNode GetContainer(TreeListCategoryNode treeListCategoryNode) => new() {
		Width = treeListCategoryNode.Width / 2.0f,
		AlignmentFlags = FlexFlags.CenterVertically | FlexFlags.CenterHorizontally | FlexFlags.FitContentHeight,
		IsVisible = true,
	};
	
	public static void ProgressBarDemo(TreeListCategoryNode treeListCategoryNode) {
		treeListCategoryNode.AddHeader("CastbarProgressBar");
		CastBarProgressBar(treeListCategoryNode);
		
		treeListCategoryNode.AddHeader("ProgressBar");
		ProgressBar(treeListCategoryNode);
	}

	private static void CastBarProgressBar(TreeListCategoryNode treeListCategoryNode) {
		var flexGrid = GetContainer(treeListCategoryNode);
		
		// Node that looks like a castbar
		var castBarNode = new CastBarProgressBarNode {
			Size = new Vector2(300.0f, 32.0f),
			IsVisible = true,
			
			// Value between 0.0f and 1.0f (inclusive)
			Progress = 0.20f,
			
			BarColor = KnownColor.Aqua.Vector(),
			
			BackgroundColor = KnownColor.Black.Vector(),
		};

		flexGrid.AddNode(castBarNode);
		
		var subFlexGird = GetContainer(treeListCategoryNode);
		
		var progressSlider = new SliderNode {
			Size = new Vector2(150.0f, 32.0f),
			IsVisible = true,
			Range = .. 100,
			Value = 20,
			OnValueChanged = newValue => castBarNode.Progress = newValue / 100.0f,
		};

		var randomColorButton = new TextButtonNode {
			Size = new Vector2(150.0f, 32.0f),
			IsVisible = true,
			String = "Randomize Color",
			OnClick = () => {
				var rng = new Random();
				castBarNode.BarColor = new Vector4(rng.Next() % 255, rng.Next() % 255, rng.Next() % 255, 255) / 255.0f;
			},
		};
		
		subFlexGird.AddNode(progressSlider);
		subFlexGird.AddNode(randomColorButton);
		
		flexGrid.AddNode(subFlexGird);
		
		treeListCategoryNode.AddNode(flexGrid);
	}

	private static void ProgressBar(TreeListCategoryNode treeListCategoryNode) {
		var flexGrid = GetContainer(treeListCategoryNode);
		
		// Node that looks like the active quest list's progress bar
		var castBarNode = new ProgressBarNode {
			Size = new Vector2(300.0f, 32.0f),
			IsVisible = true,
			
			// Value between 0.0f and 1.0f (inclusive)
			Progress = 0.35f,
		};

		flexGrid.AddNode(castBarNode);
		
		var subFlexGird = GetContainer(treeListCategoryNode);
		
		var progressSlider = new SliderNode {
			Size = new Vector2(150.0f, 32.0f),
			IsVisible = true,
			Range = .. 100,
			Value = 35,
			OnValueChanged = newValue => castBarNode.Progress = newValue / 100.0f,
		};

		var randomColorButton = new TextButtonNode {
			Size = new Vector2(150.0f, 32.0f),
			IsVisible = true,
			String = "Randomize Color",
			OnClick = () => {
				var rng = new Random();
				castBarNode.BarColor = new Vector4(rng.Next() % 255, rng.Next() % 255, rng.Next() % 255, 1);
			},
		};
		
		subFlexGird.AddNode(progressSlider);
		subFlexGird.AddNode(randomColorButton);
		
		flexGrid.AddNode(subFlexGird);
		
		treeListCategoryNode.AddNode(flexGrid);
	}
}