using System;

namespace org.neuroph.imgrec.filter.impl
{


	/// 
	/// <summary>
	/// @author Mihailo Stupar
	/// </summary>
	public class GenericConvolution : ImageFilter
	{

		private BufferedImage originalImage;
		private BufferedImage filteredImage;

		private double[][] kernel;
		private bool normalize;

		public GenericConvolution(double[][] kernel)
		{
			this.kernel = kernel;
		}



		public virtual BufferedImage processImage(BufferedImage image)
		{

			originalImage = image;
			int width = originalImage.Width;
			int height = originalImage.Height;

			filteredImage = new BufferedImage(width, height, originalImage.Type);

			int radius = kernel.Length / 2;

			if (normalize)
			{
				normalizeKernel();
			}

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					double result = convolve(x, y, radius);
					int gray = (int)Math.Round(result);
					int alpha = (new Color(originalImage.getRGB(x, y))).Alpha;
					int rgb = ImageUtilities.colorToRGB(alpha, gray, gray, gray);
					filteredImage.setRGB(x, y, rgb);
				}
			}

			return filteredImage;
		}


		protected internal virtual double convolve(int x, int y, int radius)
		{
			double sum = 0;
			int kernelI = 0;
			for (int i = x - radius; i <= x + radius; i++)
			{
				int kernelJ = 0;
				for (int j = y - radius; j <= y + radius; j++)
				{
					if (i >= 0 && i < originalImage.Width && j>0 && j < originalImage.Height)
					{
						int color = (new Color(originalImage.getRGB(i, j))).Red;
						sum = sum + color * kernel[kernelI][kernelJ];
					}
					kernelJ++;
				}
				kernelI++;
			}

			return sum;
		}

		/*
		* Mak sure that kernel element sum is 1
		*/
		private void normalizeKernel()
		{
			int n = 0;
			for (int i = 0; i < kernel.Length; i++)
			{
				for (int j = 0; j < kernel.Length; j++)
				{
					n += (int)(kernel[i][j]);
				}

			}
			for (int i = 0; i < kernel.Length; i++)
			{
				for (int j = 0; j < kernel.Length; j++)
				{
					kernel[i][j] = kernel[i][j] / n;
				}

			}
		}

		public virtual bool Normalize
		{
			set
			{
				this.normalize = value;
			}
		}

		  public virtual double[][] Kernel
		  {
			  set
			  {
				if (value.Length % 2 == 0)
				{
					Console.WriteLine("ERROR!");
				}
				this.kernel = value;
			  }
		  }

	   public override string ToString()
	   {
			return "Generic convolution";
	   }



	}

}