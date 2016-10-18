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
	public class GaussianNoise : ImageFilter
	{

		[NonSerialized]
		private BufferedImage originalImage;
		[NonSerialized]
		private BufferedImage filteredImage;

		private double mean;
		private double sigma;

		public GaussianNoise()
		{
			mean = 0;
			sigma = 30;
		}



		public virtual BufferedImage processImage(BufferedImage image)
		{


			double variance = sigma * sigma;

			originalImage = image;

			int width = originalImage.Width;
			int height = originalImage.Height;

			filteredImage = new BufferedImage(width, height, originalImage.Type);

			double a = 0.0;
			double b = 0.0;



			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{

					while (a == 0.0)
					{
						a = new Random(1).NextDouble();
					}
					b = new Random(2).NextDouble();

					double x = Math.Sqrt(-2 * Math.Log(a)) * Math.Cos(2 * Math.PI * b);
					double noise = mean + Math.Sqrt(variance) * x;

					//
					//

					int gray = (new Color(originalImage.getRGB(i, j))).Red;
					int alpha = (new Color(originalImage.getRGB(i, j))).Alpha;

					double color = gray + noise;
					if (color > 255)
					{
						color = 255;
					}
					if (color < 0)
					{
						color = 0;
					}

					int newColor = (int) Math.Round(color);

					int rgb = ImageUtilities.colorToRGB(alpha, newColor, newColor, newColor);

					filteredImage.setRGB(i, j, rgb);

				} //j
			} //i


			return filteredImage;
		}

		public virtual double Mean
		{
			set
			{
				this.mean = value;
			}
		}

		public virtual double Sigma
		{
			set
			{
				this.sigma = value;
			}
		}

		public override string ToString()
		{
			return "Gaussian noise";
		}






	}

}