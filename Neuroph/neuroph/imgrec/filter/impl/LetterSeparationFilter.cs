using System;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
namespace org.neuroph.imgrec.filter.impl
{


	/// <summary>
	/// Prvo izrcunav threshold na osnovu otsu i binrzije
	/// zatim na osnu bw i originalne slike podesaava(spusta) threshold tako da slova ne budu spojena
	/// 
	/// @author Mihailo Stupar
	/// </summary>
	public class LetterSeparationFilter : ImageFilter
	{

		private BufferedImage originalImage;
		private BufferedImage filteredImage;
	/// <summary>
	/// radi otsu da dobije spojena crna slova i ra </summary>
	/// <param name="image"> </param>
	/// <returns>  </returns>
		public virtual BufferedImage processImage(BufferedImage image)
		{

			originalImage = image;

			int width = originalImage.Width;
			int height = originalImage.Height;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: bool[][] matrix = new bool[width][height]; // black n white oolean matrix; true = blck, false = white
			bool[][] matrix = RectangularArrays.ReturnRectangularBoolArray(width, height); // black n white oolean matrix; true = blck, false = white

			filteredImage = new BufferedImage(width, height, originalImage.Type);

			int[] histogram = imageHistogram(originalImage);

			int totalNumberOfpixels = height * width;

			int threshold = threshold(histogram, totalNumberOfpixels);

			int black = 0;
			int white = 255;

			int gray;
			int alpha;
			int newColor;

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					gray = (new Color(originalImage.getRGB(i, j))).Red;

					if (gray > threshold)
					{
						matrix[i][j] = false;
					}
					else
					{
						matrix[i][j] = true;
					}

				}
			}

			int blackTreshold = letterThreshold(originalImage, matrix);

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					gray = (new Color(originalImage.getRGB(i, j))).Red;
					alpha = (new Color(originalImage.getRGB(i, j))).Alpha;

					if (gray > blackTreshold)
					{
						newColor = white;
					}
					else
					{
						newColor = black;
					}

					newColor = ImageUtilities.colorToRGB(alpha, newColor, newColor, newColor);
					filteredImage.setRGB(i, j, newColor);
				}
			}

			return filteredImage;
		}

		// hitoram from otsu method for grayscae dimage
		public virtual int[] imageHistogram(BufferedImage image)
		{

			int[] histogram = new int[256];

			for (int i = 0; i < histogram.Length; i++)
			{
				histogram[i] = 0;
			}

			for (int i = 0; i < image.Width; i++)
			{
				for (int j = 0; j < image.Height; j++)
				{
					int gray = (new Color(image.getRGB(i, j))).Red;
					histogram[gray]++;
				}
			}
			return histogram;
		}


		public virtual int letterThreshold(BufferedImage original, bool[][] matrix)
		{
			double sum = 0;
			int count = 0;

			for (int i = 0; i < original.Width; i++)
			{
				for (int j = 0; j < original.Height; j++)
				{

					if (matrix[i][j] == true)
					{
						int gray = (new Color(original.getRGB(i, j))).Red;
						sum += gray;
						count++;
					}
				}
			}

			if (count == 0)
			{
				return 0;
			}

			return (int) Math.Round((sum * 3) / (count * 2)); // 3 i 2 su plinkove konstnte
		}

		// thresold po otsu metodi
		private int threshold(int[] histogram, int total)
		{
			float sum = 0;
			for (int i = 0; i < 256; i++)
			{
				sum += i * histogram[i];
			}

			float sumB = 0;
			int wB = 0;
			int wF = 0;

			float varMax = 0;
			int threshold = 0;

			for (int i = 0; i < 256; i++)
			{
				wB += histogram[i];
				if (wB == 0)
				{
					continue;
				}
				wF = total - wB;

				if (wF == 0)
				{
					break;
				}

				sumB += (float)(i * histogram[i]);
				float mB = sumB / wB;
				float mF = (sum - sumB) / wF;

				float varBetween = (float) wB * (float) wF * (mB - mF) * (mB - mF);

				if (varBetween > varMax)
				{
					varMax = varBetween;
					threshold = i;
				}
			}
			return threshold;
		}

		public virtual int numberOfBlackPixels(bool[][] matrix)
		{
			int count = 0;
			for (int i = 0; i < originalImage.Width; i++)
			{
				for (int j = 0; j < originalImage.Height; j++)
				{

					if (matrix[i][j] == false)
					{
						count++;
					}

				}
			}
			return count;
		}

		public override string ToString()
		{
			return "Letter Separation Filter";
		}

	}

}