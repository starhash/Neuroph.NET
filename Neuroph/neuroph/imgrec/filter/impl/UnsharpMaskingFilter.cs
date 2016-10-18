using System;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace org.neuroph.imgrec.filter.impl
{


	/// 
	/// <summary>
	/// @author Mihailo Stupar
	/// </summary>
	[Serializable]
	public class UnsharpMaskingFilter : ImageFilter
	{

		[NonSerialized]
		private BufferedImage originalImage;
		[NonSerialized]
		private BufferedImage filteredImage;


		public virtual BufferedImage processImage(BufferedImage image)
		{

			originalImage = image;




			BufferedImage bluredImage = BluredImage;

			BufferedImage unsharpMask = getUnsharpMask(originalImage, bluredImage);

			filteredImage = getSharpImage(originalImage, unsharpMask);

			return filteredImage;
		}


		public virtual BufferedImage BluredImage
		{
			get
			{
    
				int width = originalImage.Width;
				int height = originalImage.Height;
    
				BufferedImage bluredImage = new BufferedImage(width, height, originalImage.Type);
				int alpha;
				int newColor;
    
				for (int i = 0; i < width; i++)
				{
					for (int j = 0; j < height; j++)
					{
						newColor = getAverageBluring(i, j);
						alpha = (new Color(originalImage.getRGB(i, j))).Alpha;
						int rgb = ImageUtilities.colorToRGB(alpha, newColor, newColor, newColor);
						bluredImage.setRGB(i, j, rgb);
					}
				}
    
				return bluredImage;
			}
		}

		public virtual int getAverageBluring(int i, int j)
		{

			double sum = 0;
			int n = 0;

			for (int x = i - 1; x <= i + 1; x++)
			{
				for (int y = j - 1; y <= j + 1; y++)
				{
					if (x >= 0 && x < originalImage.Width && y >= 0 && y < originalImage.Height)
					{
						int color = (new Color(originalImage.getRGB(x, y))).Red;
						sum = sum + color;
						n++;
					}
				}
			}

			int average = (int) Math.Round(sum / n);
			return average;
		}

		public virtual BufferedImage getUnsharpMask(BufferedImage originalImage, BufferedImage bluredImage)
		{

			int width = originalImage.Width;
			int height = originalImage.Height;

			BufferedImage unsharpMask = new BufferedImage(width, height, originalImage.Type);

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					int originalColor = (new Color(originalImage.getRGB(i, j))).Red;
					int blurColor = (new Color(bluredImage.getRGB(i, j))).Red;
					int alpha = (new Color(originalImage.getRGB(i, j))).Alpha;
					int newColor = originalColor - blurColor;
					int rgb = ImageUtilities.colorToRGB(alpha, newColor, newColor, newColor);
					unsharpMask.setRGB(i, j, rgb);
				}
			}
			return unsharpMask;
		}

		public virtual BufferedImage getSharpImage(BufferedImage originalImage, BufferedImage unsharpMask)
		{

			int width = originalImage.Width;
			int height = originalImage.Height;

			BufferedImage sharpImage = new BufferedImage(width, height, originalImage.Type);

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					int originalColor = (new Color(originalImage.getRGB(i, j))).Red;
					int unsharpColor = (new Color(unsharpMask.getRGB(i, j))).Red;
					int alpha = (new Color(originalImage.getRGB(i, j))).Alpha;
					int newColor = originalColor + unsharpColor;
					int rgb = ImageUtilities.colorToRGB(alpha, newColor, newColor, newColor);
					sharpImage.setRGB(i, j, rgb);
				}
			}
			return sharpImage;

		}

		public override string ToString()
		{
			return "Unsharp Masking Filter";
		}


	}

}