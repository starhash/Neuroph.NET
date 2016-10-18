using System;

namespace org.neuroph.imgrec.filter.impl
{


	/// <summary>
	/// Adaptive threshold binarization filter is primarily used for binarizing images 
	/// with text. If the image has such a brightness that one piece of text looks good 
	/// and the other part is in the dark, calculation of threshold based on the whole 
	/// image will not give a good result.This filter for each pixel in the image counts 
	/// a special threshold, and then decides whether the pixel transforms into black 
	/// or white color. Another purpose of this filter is to edge detection, especially 
	/// if the following filters that can be used is MedianFilter. In both cases, the 
	/// input should be a grayscale image.
	/// </summary>
	/// <param name="wSize"> size of window for calculating treshold, default value is 31 </param>
	/// <param name="k"> constant k, values between 0 and 1, default value is 0.02
	/// 
	/// reference to: http://arxiv.org/ftp/arxiv/papers/1201/1201.5227.pdf
	/// 
	/// @author Mihailo Stupar </param>
	[Serializable]
	public class AdaptiveThresholdBinarizeFilter : ImageFilter
	{

		[NonSerialized]
		private BufferedImage originalImage;
		[NonSerialized]
		private BufferedImage filteredImage;

		/// <summary>
		/// Size of window for calculating treshold, default value is 31
		/// </summary>
		private int windowSize = 31;

		/// <summary>
		/// Constant k, values between 0 and 1, default value is 0.02
		/// </summary>
		private double k = 0.02;

		public virtual BufferedImage processImage(BufferedImage image)
		{

			originalImage = image;

			int width = originalImage.Width;
			int height = originalImage.Height;

			filteredImage = new BufferedImage(width, height, originalImage.Type);

			int alpha;
			double gray;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] G = new double[width][height]; //Integral sum G
			double[][] G = RectangularArrays.ReturnRectangularDoubleArray(width, height); //Integral sum G

			gray = (new Color(originalImage.getRGB(0, 0))).Red;
			G[0][0] = gray / 255;

			for (int i = 1; i < width; i++)
			{
				gray = (new Color(originalImage.getRGB(i, 0))).Red;
				G[i][0] = G[i - 1][0] + gray / 255;
			}
			for (int j = 1; j < height; j++)
			{
				gray = (new Color(originalImage.getRGB(0, j))).Red;
				G[0][j] = G[0][j - 1] + gray / 255;
			}
			for (int i = 1; i < width; i++)
			{
				for (int j = 1; j < height; j++)
				{
					gray = (new Color(originalImage.getRGB(i, j))).Red;
					G[i][j] = gray / 255 + G[i][j - 1] + G[i - 1][j] - G[i - 1][j - 1];
				}
			}

			int d = windowSize / 2;

			int A = 0;
			int B = 0;
			int C = 0;
			int D = 0;

			double s;
			double m;
			double delta;
			double treshold;

			int newColor;

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{

					if (i + d - 1 >= width)
					{
						A = width - 1;
					}
					else
					{
						A = i + d - 1;
					}

					if (j + d - 1 >= height)
					{
						B = height - 1;
					}
					else
					{
						B = j + d - 1;
					}

					if (i - d < 0)
					{
						C = 0;
					}
					else
					{
						C = i - d;
					}

					if (j - d < 0)
					{
						D = 0;
					}
					else
					{
						D = j - d;
					}

					s = (G[A][B] + G[C][D]) - (G[C][B] + G[A][D]);
					m = s / (windowSize * windowSize);

					gray = (new Color(originalImage.getRGB(i, j))).Red;

					delta = gray / 255 - m;

					treshold = m * (1 + k * (delta / (1.0 - delta) - 1));

					if (gray / 255 > treshold)
					{
						newColor = 255;
					}
					else
					{
						newColor = 0;
					}

					alpha = (new Color(originalImage.getRGB(i, j))).Alpha;
					newColor = ImageUtilities.colorToRGB(alpha, newColor, newColor, newColor);

					filteredImage.setRGB(i, j, newColor);
				}
			}

			return filteredImage;
		}

		public virtual double K
		{
			set
			{
				this.k = value;
			}
		}

		public virtual int WindowSize
		{
			set
			{
				this.windowSize = value;
			}
		}

		public override string ToString()
		{
			return "Adaptive Treshold Binarize Filter";
		}


	}

}