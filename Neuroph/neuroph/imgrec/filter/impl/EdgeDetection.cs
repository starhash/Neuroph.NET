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
	/// @author Sanja
	/// </summary>
	[Serializable]
	public class EdgeDetection : ImageFilter
	{

		internal int width;
		internal int height;
		[NonSerialized]
		internal BufferedImage originalImage;
		[NonSerialized]
		internal BufferedImage filteredImage;

		public virtual BufferedImage processImage(BufferedImage image)
		{
			originalImage = image;
			Attributes = image;
			int width = originalImage.Width;
			int height = originalImage.Height;

			filteredImage = new BufferedImage(width, height, originalImage.Type);

			int[][] filter1 = new int[][] {new int[] {-1, 0, 1}, new int[] {-2, 0, 2}, new int[] {-1, 0, 1}};
			int[][] filter2 = new int[][] {new int[] {1, 2, 1}, new int[] {0, 0, 0}, new int[] {-1, -2, -1}};

			for (int y = 1; y < height - 1; y++)
			{
				for (int x = 1; x < width - 1; x++)
				{

					// get 3-by-3 array of colors in neighborhood
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: int[][] gray = new int[3][3];
					int[][] gray = RectangularArrays.ReturnRectangularIntArray(3, 3);
					for (int i = 0; i < 3; i++)
					{
						for (int j = 0; j < 3; j++)
						{
							gray[i][j] = (int) lum(new Color(originalImage.getRGB(x - 1 + i, y - 1 + j)));
						}
					}

					// apply filter
					int gray1 = 0, gray2 = 0;
					for (int i = 0; i < 3; i++)
					{
						for (int j = 0; j < 3; j++)
						{
							gray1 += gray[i][j] * filter1[i][j];
							gray2 += gray[i][j] * filter2[i][j];
						}
					}
					// int magnitude = 255 - truncate(Math.abs(gray1) + Math.abs(gray2));
					int magnitude = 255 - truncate((int) Math.Sqrt(gray1 * gray1 + gray2 * gray2));
					Color grayscale = new Color(magnitude, magnitude, magnitude);
					filteredImage.setRGB(x, y, grayscale.RGB);

				}
			}
			return filteredImage;
		}

		/// <summary>
		/// Truncate color component to be between 0 and 255. </summary>
		/// <param name="a"> </param>
		/// <returns>  </returns>
		public static int truncate(int a)
		{
			if (a < 0)
			{
				return 0;
			}
			else if (a > 255)
			{
				return 255;
			}
			else
			{
				return a;
			}
		}

		private BufferedImage Attributes
		{
			set
			{
				//this.originalImage = value;
				this.height = originalImage.Height;
				this.width = originalImage.Width;
			}
		}

		/// <summary>
		/// Return the  luminance of a given color </summary>
		/// <param name="color"> - color of one pixel in image. </param>
		/// <returns> - luminance of a color </returns>
		public virtual double lum(Color color)
		{
			int r = color.Red;
			int g = color.Green;
			int b = color.Blue;
			return.299 * r + .587 * g + .114 * b;
		}

		public override string ToString()
		{
			return "Edge Detection";
		}

	}

}