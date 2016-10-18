using System;
using System.Collections.Generic;

/// <summary>
/// Copyright 2010 Neuroph Project http://neuroph.sourceforge.net
/// 
/// Licensed under the Apache License, Version 2.0 (the "License"); you may not
/// use this file except in compliance with the License. You may obtain a copy of
/// the License at
/// 
/// http://www.apache.org/licenses/LICENSE-2.0
/// 
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
/// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
/// License for the specific language governing permissions and limitations under
/// the License.
/// </summary>
namespace org.neuroph.imgrec
{

	using Image = org.neuroph.imgrec.image.Image;

	/// <summary>
	/// Contains various utility methods used for OCR.
	/// 
	/// @author Ivana Jovicic, Vladimir Kolarevic, Marko Ivanovic, Zoran Sevarac
	/// </summary>
	public class ImageUtilities
	{

		/// <summary>
		/// This method cleans input image by replacing all non black pixels with
		/// white pixels TODO: some should be used here
		/// </summary>
		/// <param name="image"> - input image that will be cleaned </param>
		/// <returns> - cleaned input image as BufferedImage </returns>
		public static BufferedImage blackAndWhiteCleaning(BufferedImage image)
		{
			for (int j = 0; j < image.Height; j++)
			{
				for (int i = 0; i < image.Width; i++)
				{
					if (image.getRGB(i, j) != -16777216)
					{
						image.setRGB(i, j, -1);
					}
				}
			}
			return image;
		}

		/// <summary>
		/// This method cleans input image by replacing all pixels with RGB values
		/// from -4473925 (gray) to -1 (white) with white pixels and from -4473925
		/// (gray) to -16777216 (black) with black pixels
		/// </summary>
		/// <param name="image"> - input image that will be cleaned </param>
		/// <returns> - cleaned input image as BufferedImage </returns>
		public static BufferedImage blackAndGrayCleaning(BufferedImage image)
		{
			for (int j = 0; j < image.Height; j++)
			{
				for (int i = 0; i < image.Width; i++)
				{
					if (image.getRGB(i, j) > -4473925)
					{
						image.setRGB(i, j, -1);
					}
					else
					{
						image.setRGB(i, j, -16777216);
					}
				}
			}
			return image;
		}

		/// <summary>
		/// This method cleans input image by replacing all pixels with RGB values
		/// from -3092272 (light gray) to -1 (white) with white pixels and from
		/// -3092272 (light gray) to -16777216 (black) with black pixels
		/// </summary>
		/// <param name="image"> - input image that will be cleaned </param>
		/// <returns> - cleaned input image as BufferedImage </returns>
		public static BufferedImage blackAndLightGrayCleaning(BufferedImage image)
		{
			for (int j = 0; j < image.Height; j++)
			{
				for (int i = 0; i < image.Width; i++)
				{
					if (image.getRGB(i, j) > -4473925)
					{
						image.setRGB(i, j, -1);
					}
					else
					{
						image.setRGB(i, j, -16777216);
					}
				}
			}
			return image;
		}

		/// <summary>
		/// This method cleans input image by replacing all pixels with RGB values
		/// from RGBcolor input (the input color) to -1 (white) with white pixels and
		/// from RGBcolor input (the input color) to -16777216 (black) with black
		/// pixels
		/// </summary>
		/// <param name="image"> - input image that will be cleaned </param>
		/// <param name="RGBcolor"> - input RGB value of wanted color as reference for
		/// celaning </param>
		/// <returns> - cleaned input image as BufferedImage </returns>
		public static BufferedImage colorCleaning(BufferedImage image, int RGBcolor)
		{
			for (int j = 0; j < image.Height; j++)
			{
				for (int i = 0; i < image.Width; i++)
				{
					if (image.getRGB(i, j) == RGBcolor)
					{
						image.setRGB(i, j, -16777216);
					}
					else
					{
						image.setRGB(i, j, -1);
					}
				}
			}
			return image;
		}

		/// <summary>
		/// This method loads the input Image and returns the cleaned version
		/// </summary>
		/// <param name="file"> - input file that will be loaded as image </param>
		/// <returns> - return cleaned loaded image as BufferedImage </returns>
		public static BufferedImage loadAndCleanImage(File file)
		{
			try
			{
				BufferedImage image = ImageIO.read(file);
				return blackAndLightGrayCleaning(image);
			}
			catch (IOException ex)
			{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
				Logger.getLogger(typeof(ImageUtilities).FullName).log(Level.SEVERE, null, ex);
				return null;
			}
		}

		/// <summary>
		/// Loads image from the file.
		/// </summary>
		/// <param name="file"> image file </param>
		/// <returns> loaded image </returns>
		public static BufferedImage loadImage(File file)
		{
			try
			{
				return ImageIO.read(file);
			}
			catch (IOException ex)
			{
				throw new Exception("IOException whle trying to load image file" + file.Name, ex);
			}
		}

		public static void save(BufferedImage image, string filename, string type)
		{
			try
			{
				ImageIO.write(image, type, new File(filename));
			}
			catch (IOException ex)
			{
				throw new Exception("IOException whle trying to save image file" + filename, ex);
			}
		}

		/// <summary>
		/// This method reads the image pixels until it reads the first black pixel
		/// by height and then returns that value
		/// </summary>
		/// <param name="Img"> - input image that will be read </param>
		/// <returns> - returns the value of height when conditions are true </returns>
		private static int trimLockup(BufferedImage img)
		{
			for (int j = 0; j < img.Height; j++)
			{
				for (int i = 0; i < img.Width; i++)
				{
					if (img.getRGB(i, j) == -16777216)
					{
						return j;
					}
				}
			}
			return 0;
		}

		/// <summary>
		/// This method reads the input image from the input from start pixel height
		/// (y1) until it reads the first next row where all pixel are white by
		/// height and return that value
		/// </summary>
		/// <param name="Img"> - input image that will be read </param>
		/// <param name="y1"> - input start height pixel of image </param>
		/// <returns> - returns the value of height when conditions are true </returns>
		private static int trimLockdown(BufferedImage img, int y1)
		{
			for (int j = y1 + 1; j < img.Height; j++)
			{
				int counterWhite = 0;
				for (int i = 0; i < img.Width; i++)
				{
					if (img.getRGB(i, j) == -1)
					{
						counterWhite++;
					}
				}
				if (counterWhite == img.Width)
				{
					//this is a chek for dots over the letters i and j
					//so they wont be missread as dots
					if (j > (img.Height / 2))
					{
						return j;
					}
				}
				if (j == img.Height - 1)
				{
					return j + 1;
				}
			}
			return 0;
		}

		/// <summary>
		/// This method trims the input image and returns it as a BufferedImage
		/// </summary>
		/// <param name="imageToTrim"> input image that will be trimed </param>
		/// <returns> return trimed input image as BufferedImage </returns>
		public static BufferedImage trimImage(BufferedImage imageToTrim)
		{
			int y1 = trimLockup(imageToTrim);
			int y2 = trimLockdown(imageToTrim, y1);
			int x1 = 0; // why zero? search white pixels from left...
			int x2 = imageToTrim.Width;
			return cropImage(imageToTrim, x1, y1, x2, y2);
		}

		/// <summary>
		/// Resize image to specified dimensions
		/// </summary>
		/// <param name="image"> image to resize </param>
		/// <param name="width"> new image width </param>
		/// <param name="height"> new image height </param>
		/// <returns> resized image </returns>
		public static BufferedImage resizeImage(BufferedImage image, int width, int height)
		{
			BufferedImage resizedImage = new BufferedImage(width, height, BufferedImage.TYPE_INT_RGB);
			Graphics2D g = resizedImage.createGraphics();
			g.drawImage(image, 0, 0, width, height, null);
			g.dispose();
			return resizedImage;
		}

		public static Image resizeImage(Image image, int width, int height)
		{
			return image.resize(width, height);
		}

		/// <summary>
		/// Crops (returns subimage) of specified input image at specified points.
		/// </summary>
		/// <param name="image"> image to crop </param>
		/// <param name="x1"> top left x coordinate </param>
		/// <param name="y1"> top left y coordinate </param>
		/// <param name="x2"> bottom right x coordinate </param>
		/// <param name="y2"> bottom right y coordinate
		/// </param>
		/// <returns> image croped at specified points </returns>
		public static BufferedImage cropImage(BufferedImage image, int x1, int y1, int x2, int y2)
		{
			return image.getSubimage(x1, y1, x2 - x1, y2 - y1);
		}

		/// <summary>
		/// Creates and returns image from the given text.
		/// </summary>
		/// <param name="text"> input text </param>
		/// <param name="font"> text font </param>
		/// <returns> image with input text </returns>
	//    public static BufferedImage createImageFromText(String text, Font font) {
	//        //You may want to change these setting, or make them parameters
	//        boolean isAntiAliased = true;
	//        boolean usesFractionalMetrics = false;
	//        FontRenderContext frc = new FontRenderContext(null, isAntiAliased, usesFractionalMetrics);
	//        TextLayout layout = new TextLayout(text, font, frc);
	//        Rectangle2D bounds = layout.getBounds();
	//        int w = (int) Math.ceil(bounds.getWidth());
	//        int h = (int) Math.ceil(bounds.getHeight()) + 2;
	//        BufferedImage image = new BufferedImage(w, h, BufferedImage.TYPE_INT_RGB); //for example;
	//        Graphics2D g = image.createGraphics();
	//        g.setColor(Color.WHITE);
	//        g.fillRect(0, 0, w, h);
	//        g.setColor(Color.BLACK);
	//        g.setFont(font);
	//        Object antiAliased = isAntiAliased
	//                ? RenderingHints.VALUE_TEXT_ANTIALIAS_ON : RenderingHints.VALUE_TEXT_ANTIALIAS_OFF;
	//        g.setRenderingHint(RenderingHints.KEY_TEXT_ANTIALIASING, antiAliased);
	//        Object fractionalMetrics = usesFractionalMetrics
	//                ? RenderingHints.VALUE_FRACTIONALMETRICS_ON : RenderingHints.VALUE_FRACTIONALMETRICS_OFF;
	//        g.setRenderingHint(RenderingHints.KEY_FRACTIONALMETRICS, fractionalMetrics);
	//        g.drawString(text, (float) -bounds.getX(), (float) -bounds.getY());
	//        g.dispose();
	//
	//        return image;
	//    }
	//    public static Bitmap createImageFromText(String text) {
	//		TextView txtView = new TextView(null);
	//		txtView.setBackgroundColor(Color.WHITE);
	//		txtView.setTextColor(Color.BLACK);
	//		txtView.setText(text);
	//		txtView.setWidth(LayoutParams.WRAP_CONTENT);
	//		txtView.setHeight(LayoutParams.WRAP_CONTENT);
	//        Bitmap image = Bitmap.createBitmap(txtView.getWidth(), txtView.getHeight(), Bitmap.Config.ALPHA_8);
	//        Canvas canvas = new Canvas(image);
	//        txtView.draw(canvas);
	//        
	//        return image;
	//    }
		/// <summary>
		/// Returns RGB data for all input images
		/// </summary>
		/// <param name="imagesData"> data map with characters as keys and charcter images as
		/// values </param>
		/// <returns> data map with characters as keys and image rgb data as values </returns>
		public static IDictionary<string, FractionRgbData> getFractionRgbDataForImages(Dictionary<string, BufferedImage> imagesData)
		{

			IDictionary<string, FractionRgbData> rgbDataMap = new Dictionary<string, FractionRgbData>();

			foreach (string imageName in imagesData.Keys)
			{
				StringTokenizer st = new StringTokenizer(imageName, ".");
				BufferedImage image = imagesData[imageName];
				rgbDataMap[st.nextToken()] = new FractionRgbData(image);
			}

			return rgbDataMap;
		}

		public static int colorToRGB(int alpha, int red, int green, int blue)
		{

			int newPixel = 0;
			newPixel += alpha;
			newPixel = newPixel << 8;
			newPixel += red;
			newPixel = newPixel << 8;
			newPixel += green;
			newPixel = newPixel << 8;
			newPixel += blue;

			return newPixel;
		}

	}

}