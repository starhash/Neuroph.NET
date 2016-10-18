using System;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace org.neuroph.imgrec.filter.impl
{



	/// <summary>
	/// Dilation filter is used for making lines on the image little bit wider. It convolves through whole image
	/// and every black pixel replaces with 9 pixels. 
	/// @author Mihailo Stupar
	/// </summary>
	[Serializable]
	public class Dilation : ImageFilter
	{


		[NonSerialized]
		private BufferedImage originalImage;
		[NonSerialized]
		private BufferedImage filteredImage;

		private int width;
		private int height;

		private int[][] kernel;
		public virtual BufferedImage processImage(BufferedImage image)
		{

			originalImage = image;

			width = originalImage.Width;
			height = originalImage.Height;

			filteredImage = new BufferedImage(width, height, originalImage.Type);

			kernel = createKernel();

			int white = 255;
			int black = 0;

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					int color = (new Color(originalImage.getRGB(i, j))).Red;
					if (color == black)
					{
						convolve(i, j);
					}
					else
					{
						int alpha = (new Color(originalImage.getRGB(i, j))).Alpha;
						int rgb = ImageUtilities.colorToRGB(alpha, white, white, white);
						filteredImage.setRGB(i, j, rgb);
					}
				}
			}
			return filteredImage;
		}

		private int [][] createKernel()
		{
			int[][] kernel = new int[][] {new int[] {0,1,1,1,0}, new int[] {1,1,1,1,1}, new int[] {1,1,1,1,1}, new int[] {1,1,1,1,1}, new int[] {0,1,1,1,0}};
			return kernel;
		}

		private void convolve(int i, int j)
		{
			for (int x = i - 2; x <= i + 2; x++)
			{
				for (int y = j - 2; y <= j + 2; y++)
				{
					if (x >= 0 && y >= 0 && x < width && y < height)
					{
						int black = 0;
						int alpha = (new Color(originalImage.getRGB(x, y))).Alpha;
						int rgb = ImageUtilities.colorToRGB(alpha, black, black, black);
						filteredImage.setRGB(x, y, rgb);
					}
				}
			}
		}

		public override string ToString()
		{
			return "Dilation";
		}




	}

}