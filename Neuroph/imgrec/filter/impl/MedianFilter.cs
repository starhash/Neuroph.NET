using System;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
namespace org.neuroph.imgrec.filter.impl
{


	/// <summary>
	/// Median filter is used for noise reduction on the grayscale image. The filter 
	/// works on way that for each pixel in the image one window is set around it. 
	/// Radius of the window by default is set to 4. Then all the values of the pixels 
	/// belonging to the window are being sorted and values are used to calculate new 
	/// value that represents the median. The value of that pixels in filtered image 
	/// is replaced with one that is obtained as the median. </summary>
	/// <param name="radius"> radius of the window
	/// 
	/// @author Mihailo Stupar </param>
	[Serializable]
	public class MedianFilter : ImageFilter
	{

		[NonSerialized]
		private BufferedImage originalImage;
		[NonSerialized]
		private BufferedImage filteredImage;

		private int radius;

		public MedianFilter()
		{
		radius = 1;
		}


		public virtual BufferedImage processImage(BufferedImage image)
		{

			originalImage = image;

		int width = originalImage.Width;
		int height = originalImage.Height;

		filteredImage = new BufferedImage(width, height, originalImage.Type);

		int[] arrayOfPixels;
		int median;
		int alpha;
		int newColor;

		for (int i = 0; i < width; i++)
		{
				for (int j = 0; j < height; j++)
				{

			arrayOfPixels = getArrayOfPixels(i, j);
			median = findMedian(arrayOfPixels);
			alpha = (new Color(originalImage.getRGB(i, j))).Alpha;

			newColor = ImageUtilities.colorToRGB(alpha, median, median, median);
			filteredImage.setRGB(i, j, newColor);
				}
		}

		return filteredImage;
		}

		public virtual int[] getArrayOfPixels(int i, int j)
		{

			int startX = i - radius;
		int goalX = i + radius;
		int startY = j - radius;
		int goalY = j + radius;

		if (startX < 0)
		{
				startX = 0;
		}
		if (goalX > originalImage.Width - 1)
		{
				goalX = originalImage.Width - 1;
		}
			if (startY < 0)
			{
				startY = 0;
			}
		if (goalY > originalImage.Height - 1)
		{
				goalY = originalImage.Height - 1;
		}

		int arraySize = (goalX - startX + 1) * (goalY - startY + 1);
		int[] pixels = new int [arraySize];

		int position = 0;
		int color;
			for (int p = startX; p <= goalX; p++)
			{
				for (int q = startY; q <= goalY; q++)
				{
			color = (new Color(originalImage.getRGB(p, q))).Red;
			pixels[position] = color;
			position++;
				}
			}

		return pixels;
		}

		public virtual int findMedian(int[] arrayOfPixels)
		{
		Arrays.sort(arrayOfPixels);
		int middle = arrayOfPixels.Length / 2;
		return arrayOfPixels[middle];
		}

		/// 
		/// <param name="radius"> radius of the window. Current pixel is in center of this 
		/// window  </param>

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




		public override string ToString()
		{
			return "Median Filter";
		}




	}
}