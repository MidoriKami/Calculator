using System.Drawing;
using System.IO;
using System.Numerics;
using Dalamud.Interface;
using KamiToolKit.Classes;
using KamiToolKit.NodeParts;
using KamiToolKit.Nodes;
using KamiToolKit.System;

namespace Calculator.Demos;

public static class Images {
	private static HorizontalFlexNode<NodeBase> GetContainer(TreeListCategoryNode treeListCategoryNode) => new() {
		Width = treeListCategoryNode.Width,
		AlignmentFlags = FlexFlags.FitContentHeight | FlexFlags.CenterVertically | FlexFlags.CenterHorizontally,
		IsVisible = true,
	};
	
	public static void ImageDemo(TreeListCategoryNode imageCategory) {
		imageCategory.AddHeader("BackgroundImage");
		BackgroundImage(imageCategory);
		
		imageCategory.AddHeader("GifImage");
		GifImage(imageCategory);
		
		imageCategory.AddHeader("IconImage");
		IconImage(imageCategory);
		
		imageCategory.AddHeader("Image");
		Image(imageCategory);
		
		imageCategory.AddHeader("ImGuiImage");
		ImGuiImage(imageCategory);
		
		imageCategory.AddHeader("SimpleImage");
		SimpleImage(imageCategory);
	}

	private static void BackgroundImage(TreeListCategoryNode imageCategory) {
		var flexGrid = GetContainer(imageCategory);
		
		// This is a basic node that is easily colorable to act as a background to any element you desire
		var backgroundImageNode = new BackgroundImageNode {
			Size = new Vector2(64.0f, 64.0f),
			IsVisible = true,

			// Here we can set any color we want, this will accept any values between 0.0f and 1.0f
			Color = KnownColor.OrangeRed.Vector(),
		};
		
		flexGrid.AddNode(backgroundImageNode);
		imageCategory.AddNode(flexGrid);
	}

	private static void GifImage(TreeListCategoryNode imageCategory) {
		var flexGrid = GetContainer(imageCategory);
		
		// This is a basic node that is easily colorable to act as a background to any element you desire
		var gifImageNode = new GifImageNode {
			IsVisible = true,
			
			// Sets the file path to load the gif from, this must be loaded from filesystem, as the game does not natively support gifs
			FilePath = Path.Combine(Services.PluginInterface.AssemblyLocation.Directory?.FullName!, @"assets\niko-oneshot.gif"),
			
			// This field will cause the node to resize to fit the loaded frames maintaining aspect ratio
			FitNodeToGif = true,
			
			// Event that is called when the gif has fully loaded, as its size will be unknown until it loads
			OnGifLoaded = () => {
				flexGrid.RecalculateLayout();
				imageCategory.RecalculateLayout();
			},
		};
		
		flexGrid.AddNode(gifImageNode);
		imageCategory.AddNode(flexGrid);
	}

	private static void IconImage(TreeListCategoryNode imageCategory) {
		var flexGrid = GetContainer(imageCategory);

		// This is a basic image node with a single texture part and helpers to load icon textures
		var iconImageNode = new IconImageNode {
			IsVisible = true,
			Size = new Vector2(64.0f, 64.0f),

			// The icon texture we want to load
			IconId = 2551,
		};
		
		flexGrid.AddNode(iconImageNode);
		imageCategory.AddNode(flexGrid);
	}

	private static void Image(TreeListCategoryNode imageCategory) {
		var flexGrid = GetContainer(imageCategory);
		
		// Base type of image that does nothing for you automatically, you are required to allocate parts to make this node work
		var imageNode = new ImageNode {
			IsVisible = true, 
			
			// This size doesn't have to match the part size, they will be stretched to fit
			Size = new Vector2(64.0f, 64.0f),
			
			// Mode 2 will stretch the parts to fit our current size
			WrapMode = 2,
			
			// Clear image flags, by default autofit is set which will stretch the entire texture over our node, this is not what we want here
			ImageNodeFlags = 0,
			
			// Set a default part id, this will be the part that shows first
			PartId = 0,
		};
		
		// Allocate a part
		imageNode.AddPart(new Part {
			TexturePath = "ui/uld/Character.tex",
			TextureCoordinates = new Vector2(0.0f, 136.0f),
			Size = new Vector2(36.0f, 36.0f),
			Id = 0,
		});
		
		// Allocate another part
		imageNode.AddPart(new Part {
			TexturePath = "ui/uld/Character.tex",
			TextureCoordinates = new Vector2(36.0f, 136.0f),
			Size = new Vector2(36.0f, 36.0f),
			Id = 1,
		});

		var swapPartButton = new TextButtonNode {
			Size = new Vector2(150.0f, 28.0f),
			IsVisible = true,
			Label = "Swap Part Id",
			OnClick = () => {
				imageNode.PartId = (uint) (imageNode.PartId is 0 ? 1 : 0);
			},
		};
		
		flexGrid.AddNode(imageNode);
		flexGrid.AddNode(swapPartButton);
		imageCategory.AddNode(flexGrid);
	}

	private static void ImGuiImage(TreeListCategoryNode imageCategory) {
		var flexGrid = GetContainer(imageCategory);
		
		// ImGuiImageNode allows us to load any custom image into the game as a simple texture
		var imGuiImageNoe = new ImGuiImageNode {
			Size = new Vector2(101.95f, 144.7f),
			IsVisible = true,

			// Here we set the path to load the texture from, this can also be a path to a existing game texture
			TexturePath = Path.Combine(Services.PluginInterface.AssemblyLocation.Directory?.FullName!, @"assets\ff14_midorikami._no_BG.png"),
		};
		
		flexGrid.AddNode(imGuiImageNoe);
		imageCategory.AddNode(flexGrid);
	}

	private static void SimpleImage(TreeListCategoryNode imageCategory) {
		var flexGrid = GetContainer(imageCategory);
		
		// A simple image node with a single texture part already allocated, you only need to set texture information
		var imageNode = new SimpleImageNode {
			IsVisible = true,
			Size = new Vector2(486.0f, 260.0f),

			// Disable texture wrapping
			WrapMode = 0,

			// Disable auto-fit, and other flags
			ImageNodeFlags = 0,

			// Set the file path to load the image from
			TexturePath = "ui/uld/lotterydaily.tex",

			// The texture coordinates of the part you want to load
			TextureCoordinates = new Vector2(0.0f, 0.0f),

			// The texture size of that part
			TextureSize = new Vector2(486.0f, 260.0f),
		};
		
		flexGrid.AddNode(imageNode);
		imageCategory.AddNode(flexGrid);
	}
}