using System.Drawing;
using System.IO;
using System.Numerics;
using Dalamud.Interface;
using KamiToolKit;
using KamiToolKit.NodeParts;
using KamiToolKit.Nodes;

namespace Calculator.Demos;

public static class Images {
	private static NativeController NativeController => Services.NativeController;
	
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
		ResNode containerNode;
		imageCategory.AddNode(containerNode = new ResNode {
			Width = imageCategory.Width,
			Height = 64.0f,
			IsVisible = true,
		});
		
		// This is a basic node that is easily colorable to act as a background to any element you desire
		NativeController.AttachNode(new BackgroundImageNode {
			X = 20.0f,
			Size = new Vector2(64.0f, 64.0f),
			IsVisible = true,

			// Here we can set any color we want, this will accept any values between 0.0f and 1.0f
			Color = KnownColor.OrangeRed.Vector(),
			
		}, containerNode);
	}

	private static void GifImage(TreeListCategoryNode imageCategory) {
		ResNode containerNode;
		imageCategory.AddNode(containerNode = new ResNode {
			Width = imageCategory.Width,
			Height = 640.0f,
			IsVisible = true,
		});
		
		// This is a basic node that is easily colorable to act as a background to any element you desire
		NativeController.AttachNode(new GifImageNode {
			X = 20.0f,
			IsVisible = true,
			
			// Sets the file path to load the gif from, this must be loaded from filesystem, as the game does not natively support gifs
			FilePath = Path.Combine(Services.PluginInterface.AssemblyLocation.Directory?.FullName!, @"assets\niko-oneshot.gif"),
			
			// This field will cause the node to resize to fit the loaded frames maintaining aspect ratio
			FitNodeToGif = true,
			
		}, containerNode);
	}

	private static void IconImage(TreeListCategoryNode imageCategory) {
		ResNode containerNode;
		imageCategory.AddNode(containerNode = new ResNode {
			Width = imageCategory.Width,
			Height = 64.0f,
			IsVisible = true,
		});
		
		// This is a basic image node with a single texture part and helpers to load icon textures
		NativeController.AttachNode(new IconImageNode {
			X = 20.0f,
			IsVisible = true,
			Size = new Vector2(64.0f, 64.0f),
			
			// The icon texture we want to load
			IconId = 2551,
			
		}, containerNode);
	}

	private static void Image(TreeListCategoryNode imageCategory) {
		ResNode containerNode;
		imageCategory.AddNode(containerNode = new ResNode {
			Width = imageCategory.Width,
			Height = 64.0f,
			IsVisible = true,
		});
		
		// Base type of image that does nothing for you automatically, you are required to allocate parts to make this node work
		var imageNode = new ImageNode {
			X = 20.0f, 
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
		
		NativeController.AttachNode(imageNode, containerNode);
		
		NativeController.AttachNode(new TextButtonNode {
			Position = new Vector2(100.0f, 0.0f),
			Size = new Vector2(150.0f, 28.0f),
			IsVisible = true,
			Label = "Swap Part Id",
			OnClick = () => {
				imageNode.PartId = (uint) (imageNode.PartId is 0 ? 1 : 0);
			},
		}, containerNode);
	}

	private static void ImGuiImage(TreeListCategoryNode imageCategory) {
		ResNode containerNode;
		imageCategory.AddNode(containerNode = new ResNode {
			Width = imageCategory.Width,
			Height = 150.0f,
			IsVisible = true,
		});
		
		// ImGuiImageNode allows us to load any custom image into the game as a simple texture
		NativeController.AttachNode(new ImGuiImageNode {
			X = 20.0f,
			Size = new Vector2(101.95f, 144.7f),
			IsVisible = true,
			
			// Here we set the path to load the texture from, this can also be a path to a existing game texture
			TexturePath = Path.Combine(Services.PluginInterface.AssemblyLocation.Directory?.FullName!, @"assets\ff14_midorikami._no_BG.png"),
			
		}, containerNode);
	}

	private static void SimpleImage(TreeListCategoryNode imageCategory) {
		ResNode containerNode;
		imageCategory.AddNode(containerNode = new ResNode {
			Width = imageCategory.Width,
			Height = 260.0f,
			IsVisible = true,
		});
		
		// A simple image node with a single texture part already allocated, you only need to set texture information
		NativeController.AttachNode(new SimpleImageNode {
			X = 20.0f,
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
			
		}, containerNode);
	}
}