using System;

namespace org.neuroph.imgrec.filter.impl
{


	/// 
	/// <summary>
	/// @author Mihailo Stupar
	/// 
	/// Filter who improves the quality of handwriting letters. 
	/// </summary>
	public class NormalizationFilter : ImageFilter
	{


		private BufferedImage originalImage;
		private BufferedImage filteredImage;

		private int blockSize = 5; //should be odd number (ex. 5)

		private double GOAL_MEAN_Renamed = 0;
		private double GOAL_VARIANCE_Renamed = 1;

		private int mean;
		private int @var;

		private int width;
		private int height;

		private int[][] imageMatrix;

		public virtual BufferedImage processImage(BufferedImage image)
		{

			originalImage = image;



			width = originalImage.Width;
			height = originalImage.Height;

			filteredImage = new BufferedImage(width, height, originalImage.Type);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: imageMatrix = new int[width][height];
			imageMatrix = RectangularArrays.ReturnRectangularIntArray(width, height);

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{

					imageMatrix[i][j] = (new Color(originalImage.getRGB(i, j))).Red;

				}
			}

			mean = calculateMean();
			@var = calculateVariance();

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{

					double normalizedPixel = 0;
					double squareError = 0;

					if (imageMatrix[i][j] > mean)
					{
						squareError = (imageMatrix[i][j] - mean) * (imageMatrix[i][j] - mean);
						normalizedPixel = (GOAL_MEAN_Renamed + Math.Sqrt(((GOAL_VARIANCE_Renamed * squareError / @var))));
					}
					else
					{
						squareError = (imageMatrix[i][j] - mean) * (imageMatrix[i][j] - mean);
						normalizedPixel = (GOAL_MEAN_Renamed - Math.Sqrt(((GOAL_VARIANCE_Renamed * squareError / @var))));
					}

					int alpha = (new Color(originalImage.getRGB(i, j))).Alpha;

					int rgb = (int)-normalizedPixel;

					int color = ImageUtilities.colorToRGB(alpha, rgb, rgb, rgb);

					filteredImage.setRGB(i, j, color);

				}
			}


			return filteredImage;
		}

		/// 
		/// <param name="x"> x coordinate of block </param>
		/// <param name="y"> y coordinate of block
		/// @return </param>
		public virtual int calculateVariance()
		{

			int @var = 0;
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					@var += (imageMatrix[i][j] - mean) * (imageMatrix[i][j] - mean);

				}
			}
			return (int)@var / (height * width * 255); //255 for white color
		}

		public virtual int calculateMean()
		{
			double mean = 0;

			for (int i = 0; i < width; i++)
			{
				for (int j = 0 ; j < height; j++)
				{
					mean += imageMatrix[i][j];

				}
			}

			return (int) mean / (width * height);
		}

		public override string ToString()
		{
			return "Normalization Filter";
		}

		public virtual double GOAL_MEAN
		{
			set
			{
				this.GOAL_MEAN_Renamed = value;
			}
		}

		public virtual double GOAL_VARIANCE
		{
			set
			{
				this.GOAL_VARIANCE_Renamed = value;
			}
		}








	}

}