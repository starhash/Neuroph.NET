/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
namespace org.neuroph.contrib.licenceplaterecognition
{

	using BinaryImageOps = boofcv.alg.filter.binary.BinaryImageOps;
	using Contour = boofcv.alg.filter.binary.Contour;
	using GThresholdImageOps = boofcv.alg.filter.binary.GThresholdImageOps;
	using ThresholdImageOps = boofcv.alg.filter.binary.ThresholdImageOps;
	using ConvertBufferedImage = boofcv.core.image.ConvertBufferedImage;
	using VisualizeBinaryData = boofcv.gui.binary.VisualizeBinaryData;
	using ShowImages = boofcv.gui.image.ShowImages;
	using UtilImageIO = boofcv.io.image.UtilImageIO;
	using ConnectRule = boofcv.@struct.ConnectRule;
	using ImageFloat32 = boofcv.@struct.image.ImageFloat32;
	using ImageSInt32 = boofcv.@struct.image.ImageSInt32;
	using ImageUInt8 = boofcv.@struct.image.ImageUInt8;

	/// 
	/// <summary>
	/// @author Megi
	/// </summary>
	public class BinaryOps
	{

		/// 
		/// <summary>
		/// Inverts the image colors from negative to positive
		/// </summary>
		/// <returns> the image with inverted colors </returns>
		public static BufferedImage invertImage(string imageName)
		{

			// read the image file
			BufferedImage inputFile = null;
			try
			{
				inputFile = ImageIO.read(new File(imageName));
			}
			catch (IOException e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
			}

			// go through image pixels and reverse their color       
			for (int x = 0; x < inputFile.Width; x++)
			{
				for (int y = 0; y < inputFile.Height; y++)
				{
					int rgba = inputFile.getRGB(x, y);
					Color col = new Color(rgba, true);
					col = new Color(255 - col.Red, 255 - col.Green, 255 - col.Blue);
					inputFile.setRGB(x, y, col.RGB);
				}
			}

			//write the image to a file blackandwhite.png
			try
			{
				File outputFile = new File("blackandwhite.png");
				ImageIO.write(inputFile, "png", outputFile);

			}
			catch (IOException e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
			}
			return inputFile;
		}

		public static BufferedImage binary(string textImageFile)
		{
			// load and convert the image into a usable format
			BufferedImage image = UtilImageIO.loadImage(textImageFile);

			// convert into a usable format
			ImageFloat32 input = ConvertBufferedImage.convertFromSingle(image, null, typeof(ImageFloat32));
			ImageUInt8 binary = new ImageUInt8(input.width, input.height);
			ImageSInt32 label = new ImageSInt32(input.width, input.height);

			// Select a global threshold using Otsu's method.
			double threshold = GThresholdImageOps.computeOtsu(input, 0, 256);

			// Apply the threshold to create a binary image
			ThresholdImageOps.threshold(input, binary, (float) threshold, true);

			// remove small blobs through erosion and dilation
			// The null in the input indicates that it should internally declare the work image it needs
			// this is less efficient, but easier to code.
			ImageUInt8 filtered = BinaryImageOps.erode8(binary, 1, null);
			filtered = BinaryImageOps.dilate8(filtered, 1, null);

			// get the binary image
			BufferedImage visualFiltered = VisualizeBinaryData.renderBinary(filtered, null);

			//write the negative image to a file
			File charFile = new File("whiteandblack.png");
			try
			{
				ImageIO.write(visualFiltered, "png", charFile);
			}
			catch (IOException ex)
			{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
				Logger.getLogger(typeof(BinaryOps).FullName).log(Level.SEVERE, null, ex);
			}

			//return the positive image
			return invertImage("whiteandblack.png");
		}
	}

}