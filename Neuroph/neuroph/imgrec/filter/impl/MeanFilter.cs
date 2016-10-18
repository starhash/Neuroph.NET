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
	public class MeanFilter : ImageFilter
	{

		[NonSerialized]
		private BufferedImage originalImage;
		[NonSerialized]
		private BufferedImage filteredImage;

		private int radius;

		public MeanFilter()
		{
			radius = 4;
		}



		public virtual BufferedImage processImage(BufferedImage image)
		{

			originalImage = image;

			int width = originalImage.Width;
			int height = originalImage.Height;

			filteredImage = new BufferedImage(width, height, originalImage.Type);

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{

					int color = findMean(i, j);
					int alpha = (new Color(originalImage.getRGB(i, j))).Alpha;

					int rgb = ImageUtilities.colorToRGB(alpha, color, color, color);
					filteredImage.setRGB(i, j, rgb);

				}
			}


			return filteredImage;


		}

		public virtual int findMean(int x, int y)
		{
			double sum = 0;
			int n = 0;
			for (int i = x - radius; i <= x + radius; i++)
			{
				for (int j = y - radius; j <= y + radius; j++)
				{
					if (i > 0 && i < originalImage.Width && j>0 && j < originalImage.Height)
					{
						int color = (new Color(originalImage.getRGB(i, j))).Red; // why we use only red component here?
						sum = sum + color;
						n++;
					}
				}
			}
			return (int) Math.Round(sum / n);
		}

		public override string ToString()
		{
			return "Mean Filter";
		}

		public virtual int Radius
		{
			set
			{
				this.radius = value;
			}
			get
			{
				return radius;
			}
		}



	}

}