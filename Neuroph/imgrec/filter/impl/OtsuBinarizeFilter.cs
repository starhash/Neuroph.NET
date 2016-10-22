using System;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
namespace org.neuroph.imgrec.filter.impl
{


	/// <summary>
	/// Otsu binarize filter serves to dynamically determine the threshold based on 
	/// the whole image and for later binarization on black (0) and white (255) pixels. 
	/// In determining threshold a image histogram is created in way that the value of 
	/// each pixel of image affects on the histogram appearance. Then, depending upon 
	/// the look of the histogram threshold counts and based on that, the real image 
	/// which is binarized is made.The image before this filter must be grayscale and 
	/// at the end image will contain only two colors - black and white. 
	/// 
	/// reference to: http://zerocool.is-a-geek.net/?p=376
	///  http://www.labbookpages.co.uk/software/imgProc/otsuThreshold.html
	/// 
	/// @author Mihailo Stupar
	/// </summary>
	[Serializable]
	public class OtsuBinarizeFilter : ImageFilter
	{

		[NonSerialized]
		private BufferedImage originalImage;
		[NonSerialized]
		private BufferedImage filteredImage;
		public virtual BufferedImage processImage(BufferedImage image)
		{

			originalImage = image;

		int width = originalImage.Width;
		int height = originalImage.Height;

		filteredImage = new BufferedImage(width, height, originalImage.Type);

		int[] histogram = imageHistogram(originalImage);

		int totalNumberOfpixels = height * width;

		int treshold = treshold(histogram, totalNumberOfpixels);

		int black = 0;
		int white = 255;

		int alpha;
		int gray;
		int newColor;

		for (int i = 0; i < width; i++)
		{
				for (int j = 0; j < height; j++)
				{
			gray = (new Color(originalImage.getRGB(i, j))).Red;
			alpha = (new Color(originalImage.getRGB(i, j))).Alpha;

			if (gray > treshold)
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

		private int treshold(int[] histogram, int total)
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

		public override string ToString()
		{
			return "Otsu Binarize Filter";
		}


	}
}