using System.IO;
using System.Numerics;
using FFXIVClientStructs.FFXIV.Component.GUI;
using KamiToolKit.Classes;
using KamiToolKit.Nodes;
using KamiToolKit.System;

namespace Calculator.Demos;

public static class Buttons {
	private static HorizontalFlexNode<NodeBase> GetContainer(TreeListCategoryNode treeListCategoryNode) => new() {
		Width = treeListCategoryNode.Width,
		AlignmentFlags = FlexFlags.FitHeight | FlexFlags.CenterHorizontally | FlexFlags.FitContentHeight,
		IsVisible = true,
	};

	private static TextNode GetTextNode() => new() {
		TextFlags = TextFlags.AutoAdjustNodeSize,
		AlignmentType = AlignmentType.Left,
		Text = "Button Has Been Pressed: 0 times",
	};

	public static void ButtonDemo(TreeListCategoryNode treeListCategoryNode) {
		treeListCategoryNode.AddHeader("CircleButton");
		CircleButton(treeListCategoryNode);
		
		treeListCategoryNode.AddHeader("IconButton");
		IconButton(treeListCategoryNode);
		
		treeListCategoryNode.AddHeader("ImGuiIconButton");
		ImGuiIconButton(treeListCategoryNode);
		
		treeListCategoryNode.AddHeader("TextButton");
		TextButton(treeListCategoryNode);
		
		treeListCategoryNode.AddHeader("TextureButton");
		TextureButton(treeListCategoryNode);
		
		treeListCategoryNode.AddHeader("HoldButton");
		HoldButton(treeListCategoryNode);
	}

	private static void CircleButton(TreeListCategoryNode treeListCategoryNode) {
		var flexGrid = GetContainer(treeListCategoryNode);
		
		var textNode = GetTextNode();
		var clickCount = 0;
		
		// A circle button node is a button node that uses one the games default circle textures
		var circleButton = new CircleButtonNode {
			X = 20.0f,
			Size = new Vector2(48.0f, 48.0f),
			IsVisible = true,

			// Here we can select which texture we want to use for the button
			Icon = ButtonIcon.GearCogWithChatBubble,

			// All button nodes have a "OnClick" handler you can subscribe to
			OnClick = () => {
				clickCount++;
				textNode.Text = $"Button Has Been Pressed: {clickCount} times";
			},
		};
		
		flexGrid.AddNode(circleButton);
		flexGrid.AddNode(textNode);
		
		treeListCategoryNode.AddNode(flexGrid);
	}

	private static void IconButton(TreeListCategoryNode treeListCategoryNode) {
		var flexGrid = GetContainer(treeListCategoryNode);
		
		var textNode = GetTextNode();
		var clickCount = 0;
		
		// IconButton's use a game icon id to load a game texture as a clickable icon
		var iconButton = new IconButtonNode {
			Size = new Vector2(48.0f, 48.0f),
			IsVisible = true,

			// Here we can select which iconId we want to display as a button
			IconId = 2551,

			// All button nodes have a "OnClick" handler you can subscribe to
			OnClick = () => {
				clickCount++;
				textNode.Text = $"Button Has Been Pressed: {clickCount} times";
			},
		};
		
		flexGrid.AddNode(iconButton);
		flexGrid.AddNode(textNode);
		
		treeListCategoryNode.AddNode(flexGrid);
	}

	private static void ImGuiIconButton(TreeListCategoryNode treeListCategoryNode) {
		var flexGrid = GetContainer(treeListCategoryNode);
		
		var textNode = GetTextNode();
		var clickCount = 0;
		
		// ImGuiIconButton allows you to use any image file that you provide as a texture for the button
		var imGuiIconButton = new ImGuiIconButtonNode {
			Size = new Vector2(101.95f, 144.7f),
			IsVisible = true,

			// Here we set the path to load the texture from, this can also be a path to an existing game texture
			TexturePath = Path.Combine(Services.PluginInterface.AssemblyLocation.Directory?.FullName!, @"assets\ff14_midorikami._no_BG.png"),

			// All button nodes have a "OnClick" handler you can subscribe to
			OnClick = () => {
				clickCount++;
				textNode.Text = $"Button Has Been Pressed: {clickCount} times";
			},
		};
		
		flexGrid.AddNode(imGuiIconButton);
		flexGrid.AddNode(textNode);
		
		treeListCategoryNode.AddNode(flexGrid);
	}

	private static void TextButton(TreeListCategoryNode treeListCategoryNode) {
		var flexGrid = GetContainer(treeListCategoryNode);
		
		var textNode = GetTextNode();
		var clickCount = 0;
		
		// TextButtons's are a standard button with a text label
		var textButton = new TextButtonNode {
			Size = new Vector2(200.0f, 28.0f),
			IsVisible = true,

			// Here we set the text string to use, this does support SeString
			Label = "BIG RED BUTTON",

			// All button nodes have a "OnClick" handler you can subscribe to
			OnClick = () => {
				clickCount++;
				textNode.Text = $"Button Has Been Pressed: {clickCount} times";
			},
		};
		
		flexGrid.AddNode(textButton);
		flexGrid.AddNode(textNode);
		
		treeListCategoryNode.AddNode(flexGrid);
	}

	private static void TextureButton(TreeListCategoryNode treeListCategoryNode) {
		var flexGrid = GetContainer(treeListCategoryNode);
		
		var textNode = GetTextNode();
		var clickCount = 0;
		
		// TextureButtons are buttons that load a part of an existing game texture
		var textureButton = new TextureButtonNode {
			Size = new Vector2(48.0f, 48.0f),
			IsVisible = true,

			// First we set the texture path
			// Note, do not include "_hr1", the correct texture will be resolved depending on users 
			TexturePath = "ui/uld/Teleport.tex",

			// Next we set the texture coordinates, these are the x,y coordinates of the specific part in the image we want
			TextureCoordinates = new Vector2(69.0f, 75.0f),

			// Finally we set the part size, this is how big that specific texture part is
			TextureSize = new Vector2(32.0f, 32.0f),

			// All button nodes have a "OnClick" handler you can subscribe to
			OnClick = () => {
				clickCount++;
				textNode.Text = $"Button Has Been Pressed: {clickCount} times";
			},
		};
		
		flexGrid.AddNode(textureButton);
		flexGrid.AddNode(textNode);
		
		treeListCategoryNode.AddNode(flexGrid);
	}
	
	private static void HoldButton(TreeListCategoryNode treeListCategoryNode) {
		var flexGrid = GetContainer(treeListCategoryNode);
		
		var textNode = GetTextNode();
		var clickCount = 0;
		
		// TextureButtons are buttons that load a part of an existing game texture
		var textureButton = new HoldButtonNode {
			Size = new Vector2(100.0f, 28.0f),
			IsVisible = true,
			
			// All button nodes have a "OnClick" handler you can subscribe to
			OnClick = () => {
				clickCount++;
				textNode.Text = $"Button Has Been Pressed: {clickCount} times";
			},
		};

		textureButton.OnClick += () => textureButton.Reset();
		
		flexGrid.AddNode(textureButton);
		flexGrid.AddNode(textNode);
		
		treeListCategoryNode.AddNode(flexGrid);
	}
}