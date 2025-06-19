using System.IO;
using System.Numerics;
using KamiToolKit;
using KamiToolKit.Nodes;

namespace Calculator.Demos;

public static class Buttons {
	private static NativeController NativeController => Services.NativeController;
	
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
	}

	private static void CircleButton(TreeListCategoryNode treeListCategoryNode) {
		ResNode containerNode;
		treeListCategoryNode.AddNode(containerNode = new ResNode {
			Width = treeListCategoryNode.Width,
			Height = 48.0f,
			IsVisible = true,
		});

		var clickCount = 0;
		var demoTextNode = new TextNode {
			Position = new Vector2(120.0f, 12.0f), 
			Text = $"Button Has Been Pressed: {clickCount} times",
		};
			
		NativeController.AttachNode(demoTextNode, containerNode);
		
		// A circle button node is a button node that uses one the games default circle textures
		NativeController.AttachNode(new CircleButtonNode {
			X = 20.0f,
			Size = new Vector2(48.0f, 48.0f),
			IsVisible = true,
			
			// Here we can select which texture we want to use for the button
			Icon = ButtonIcon.GearCogWithChatBubble,
			
			// All button nodes have a "OnClick" handler you can subscribe to
			OnClick = () => {
				clickCount++;
				demoTextNode.Text = $"Button Has Been Pressed: {clickCount} times";
			},
		}, containerNode);
	}

	private static void IconButton(TreeListCategoryNode treeListCategoryNode) {
		ResNode containerNode;
		treeListCategoryNode.AddNode(containerNode = new ResNode {
			Width = treeListCategoryNode.Width,
			Height = 48.0f,
			IsVisible = true,
		});

		var clickCount = 0;
		var demoTextNode = new TextNode {
			Position = new Vector2(120.0f, 12.0f), 
			Text = $"Button Has Been Pressed: {clickCount} times",
		};
			
		NativeController.AttachNode(demoTextNode, containerNode);
		
		// IconButton's use a game icon id to load a game texture as a clickable icon
		NativeController.AttachNode(new IconButtonNode {
			X = 20.0f,
			Size = new Vector2(48.0f, 48.0f),
			IsVisible = true,
			
			// Here we can select which iconId we want to display as a button
			IconId = 2551,
			
			// All button nodes have a "OnClick" handler you can subscribe to
			OnClick = () => {
				clickCount++;
				demoTextNode.Text = $"Button Has Been Pressed: {clickCount} times";
			},
		}, containerNode);
	}

	private static void ImGuiIconButton(TreeListCategoryNode treeListCategoryNode) {
		ResNode containerNode;
		treeListCategoryNode.AddNode(containerNode = new ResNode {
			Width = treeListCategoryNode.Width,
			Height = 150.0f,
			IsVisible = true,
		});

		var clickCount = 0;
		var demoTextNode = new TextNode {
			Position = new Vector2(175.0f, 75.0f), 
			Text = $"Button Has Been Pressed: {clickCount} times",
		};
			
		NativeController.AttachNode(demoTextNode, containerNode);
		
		// ImGuiIconButton allows you to use any image file that you provide as a texture for the button
		NativeController.AttachNode(new ImGuiIconButtonNode {
			X = 20.0f,
			Size = new Vector2(101.95f, 144.7f),
			IsVisible = true,
			
			// Here we set the path to load the texture from, this can also be a path to a existing game texture
			TexturePath = Path.Combine(Services.PluginInterface.AssemblyLocation.Directory?.FullName!, @"assets\ff14_midorikami._no_BG.png"),
			
			// All button nodes have a "OnClick" handler you can subscribe to
			OnClick = () => {
				clickCount++;
				demoTextNode.Text = $"Button Has Been Pressed: {clickCount} times";
			},
		}, containerNode);
	}

	private static void TextButton(TreeListCategoryNode treeListCategoryNode) {
		ResNode containerNode;
		treeListCategoryNode.AddNode(containerNode = new ResNode {
			Width = treeListCategoryNode.Width,
			Height = 28.0f,
			IsVisible = true,
		});

		var clickCount = 0;
		var demoTextNode = new TextNode {
			Position = new Vector2(250.0f, 0.0f), 
			Text = $"Button Has Been Pressed: {clickCount} times",
		};
			
		NativeController.AttachNode(demoTextNode, containerNode);
		
		// TextButtons's are a standard button with a text label
		NativeController.AttachNode(new TextButtonNode {
			X = 20.0f,
			Size = new Vector2(200.0f, 28.0f),
			IsVisible = true,
			
			// Here we set the text string to use, this does support SeString
			Label = "BIG RED BUTTON",
			
			// All button nodes have a "OnClick" handler you can subscribe to
			OnClick = () => {
				clickCount++;
				demoTextNode.Text = $"Button Has Been Pressed: {clickCount} times";
			},
		}, containerNode);
	}

	private static void TextureButton(TreeListCategoryNode treeListCategoryNode) {
		ResNode containerNode;
		treeListCategoryNode.AddNode(containerNode = new ResNode {
			Width = treeListCategoryNode.Width,
			Height = 48.0f,
			IsVisible = true,
		});

		var clickCount = 0;
		var demoTextNode = new TextNode {
			Position = new Vector2(120.0f, 12.0f), 
			Text = $"Button Has Been Pressed: {clickCount} times",
		};
			
		NativeController.AttachNode(demoTextNode, containerNode);
		
		// TextureButtons are buttons that load a part of a existing game texture
		NativeController.AttachNode(new TextureButtonNode {
			X = 20.0f,
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
				demoTextNode.Text = $"Button Has Been Pressed: {clickCount} times";
			},
		}, containerNode);
	}
}