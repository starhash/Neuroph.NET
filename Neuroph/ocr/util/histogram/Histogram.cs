/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
namespace org.neuroph.ocr.util.histogram
{


	/// 
	/// <summary>
	/// @author Mihailo
	/// </summary>
	public class Histogram
	{

		/// <param name="image"> binarized image, letters are black, background is white </param>
		/// <returns> array which length is height of image, every element of array
		/// represent count of black pixels in that row. </returns>
		public static int[] heightHistogram(BufferedImage image)
		{
			int height = image.Height;
			int width = image.Width;

			int[] histogram = new int[height];
			int black = 0;
			int color;
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					color = (new Color(image.getRGB(j, i))).Red;
					if (color == black)
					{
						histogram[i]++;
					}
				}
			}
			return histogram;
		}


		/// <param name="image"> binarized image, letters are black, background is white </param>
		/// <returns> array which length is width of image, every element of array
		/// represent count of black pixels in that column of pixels. </returns>
		public static int[] widthHistogram(BufferedImage image)
		{
			int height = image.Height;
			int width = image.Width;

			int[] histogram = new int[width];
			int black = 0;
			int color;
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					color = (new Color(image.getRGB(i, j))).Red;
					if (color == black)
					{
						histogram[i]++;
					}
				}
			}
			return histogram;
		}

		/// <param name="histogram"> histogram calculated by method
		/// <b>heightHistogram(BufferedImage)</b> or 
		/// <b>widthHistogram(BufferedImage)</b> </param>
		/// <returns> array that represents gradient Each element in array is
		/// calculated in the following way:<br/>
		/// gradient[i] = histogram[i] - histogram[i-1] </returns>
		public static int[] gradient(int[] histogram)
		{
			int[] gradient = new int[histogram.Length];
			for (int i = 1; i < gradient.Length; i++)
			{
				gradient[i] = histogram[i] - histogram[i - 1];
			}
			return gradient;
		}

	}

}