using System;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
namespace org.neuroph.imgrec.filter.impl
{


	/// <summary>
	/// Grayscale filter from image in RGB format makes grayscale image in way that
	/// for each pixel, using value of red, green and blue color, calculates new
	/// value using formula: gray = 0.21*red + 0.71*green + 0.07*blue Grayscale
	/// filter is commonly used as first filter in Filter Chain and on that grayscale
	/// image other filters are added.
	/// 
	/// @author Mihailo Stupar
	/// </summary>
	[Serializable]
	public class GrayscaleFilter : ImageFilter
	{

		[NonSerialized]
		private BufferedImage originalImage;
		[NonSerialized]
		private BufferedImage filteredImage;

		public virtual BufferedImage processImage(BufferedImage image)
		{
			originalImage = image;
			int alpha;
			int red;
			int green;
			int blue;
			int gray;
			int width = originalImage.Width;
			int height = originalImage.Height;
			filteredImage = new BufferedImage(width, height, originalImage.Type);
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{

					alpha = (new Color(originalImage.getRGB(i, j))).Alpha;
					red = (new Color(originalImage.getRGB(i, j))).Red;
					green = (new Color(originalImage.getRGB(i, j))).Green;
					blue = (new Color(originalImage.getRGB(i, j))).Blue;

					gray = (int)(0.21 * red + 0.71 * green + 0.07 * blue);

					gray = ImageUtilities.colorToRGB(alpha, gray, gray, gray);

					filteredImage.setRGB(i, j, gray);
				}
			}
			return filteredImage;
		}
		public override string ToString()
		{
			return "Grayscale Filter";
		}
	}

}