/// <summary>
/// Copyright 2010 Neuroph Project http://neuroph.sourceforge.net
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
/// 
///    http://www.apache.org/licenses/LICENSE-2.0
/// 
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// </summary>

namespace org.neuroph.imgrec
{

	using Color = org.neuroph.imgrec.image.Color;
	using Image = org.neuroph.imgrec.image.Image;
	using ImageJ2SE = org.neuroph.imgrec.image.ImageJ2SE;


	/// <summary>
	/// The intention of this class is to allow you to pay up front (at construction) 
	/// the compute cost of converting the RGB values in a BufferedImage into a derived form.
	/// 
	/// The major benefit of using this class is a single loop that grabs all 3 color channels
	/// (red, green, and blue) from each (x,y) coordinate, as opposed to looping through each
	/// color channel (red, green, or blue) when you need it.
	/// 
	/// If you only need a single color from the channels (red, green, or blue) then
	/// using this class may be more expensive than a custom solution.
	/// 
	/// In the event that it needs to be parsed, the flattened rgb values array contains 
	/// all the red first, followed by all the green, followed by all the blue values.  
	/// The flattened array size should be divisible by 3. 
	/// 
	/// @author Jon Tait
	/// 
	/// </summary>
	public class FractionRgbData
	{
			/// <summary>
			/// Image width
			/// </summary>
		private int width;

			/// <summary>
			/// Image height
			/// </summary>
		private int height;

			/// <summary>
			/// Array which contains red componenet of the color for each image pixel
			/// </summary>
		protected internal double[][] redValues;

			/// <summary>
			/// Array which contains green componenet of the color for each image pixel
			/// </summary>
		protected internal double[][] greenValues;

			/// <summary>
			/// Array which contains blue componenet of the color for each image pixel
			/// </summary>
		protected internal double[][] blueValues;

			/// <summary>
			/// Single array with the red, green and blue componenets of the color for each image pixel
			/// </summary>
		protected internal double[] flattenedRgbValues;

			/// <summary>
			/// Creates rgb data for the specified image. </summary>
			/// <param name="img"> image to cretae rgb data for </param>
		public FractionRgbData(Image img)
		{
			width = img.Width;
			height = img.Height;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: redValues = new double[height][width];
			redValues = RectangularArrays.ReturnRectangularDoubleArray(height, width);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: greenValues = new double[height][width];
			greenValues = RectangularArrays.ReturnRectangularDoubleArray(height, width);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: blueValues = new double[height][width];
			blueValues = RectangularArrays.ReturnRectangularDoubleArray(height, width);
			flattenedRgbValues = new double[width * height * 3];

			populateRGBArrays(img);
		}

		public FractionRgbData(BufferedImage img) : this(new ImageJ2SE(img))
		{
		}

			/// <summary>
			/// Fills the rgb arrays from image </summary>
			/// <param name="image"> image to get rgb data from </param>
		protected internal void populateRGBArrays(Image image)
		{
			int color;

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					color = image.getPixel(x, y);

					double red = ((double) Color.getRed(color)) / 256d;
					redValues[y][x] = red;
					flattenedRgbValues[(y * width + x)] = red;

					double green = ((double) Color.getGreen(color)) / 256d;
					greenValues[y][x] = green;
					flattenedRgbValues[(width * height + y * width + x)] = green;

					double blue = ((double) Color.getBlue(color)) / 256d;
					blueValues[y][x] = blue;
					flattenedRgbValues[(2 * width * height + y * width + x)] = blue;
				}
			}
		}

			/// <summary>
			/// Converts image rgb data to binary black and white data (1 for black, 0 for white) </summary>
			/// <param name="inputRGB"> flatten rgb data </param>
			/// <returns> binary black and white representation of image </returns>
			public static double[] convertRgbInputToBinaryBlackAndWhite(double[] inputRGB)
			{
				int length = inputRGB.Length / 3;
				double[] inputBinary = new double[length];
				// black =1 , white = 0 which is opposite from color information
				for (int i = 0; i < length; i++) // problem sa ovime je sto pixele koji nisu skroz crni prebacuje u bele!
				{
					// if (r+g+b)/3 < thresh then black (1) else white (0)
					double rgbColorAvg = (inputRGB[i] + inputRGB[length + i] + inputRGB[2 * length + i]) / 3;
					if (rgbColorAvg < 0.19)
					{
						inputBinary[i] = 1; // then its 1 (black)
					}
					else
					{
						inputBinary[i] = 0; // otherwise its 0 (white)
					}
				}

				return inputBinary;
			}

			/// <summary>
			/// Get image width </summary>
			/// <returns> image width </returns>
		public virtual int Width
		{
			get
			{
				return width;
			}
		}

			/// <summary>
			/// Get image height </summary>
			/// <returns> image height </returns>
		public virtual int Height
		{
			get
			{
				return height;
			}
		}

		/// <summary>
		/// Returns red color component for the entire image </summary>
		/// <returns> 2d array in the form: [row][column] </returns>
		public virtual double[][] RedValues
		{
			get
			{
				return redValues;
			}
		}

		/// <summary>
		/// Returns green color component for the entire image </summary>
		/// <returns> 2d array in the form: [row][column] </returns>
		public virtual double[][] GreenValues
		{
			get
			{
				return greenValues;
			}
		}

		/// <summary>
		/// Returns blue color component for the entire image </summary>
		/// <returns> 2d array in the form: [row][column] </returns>
		public virtual double[][] BlueValues
		{
			get
			{
				return blueValues;
			}
		}

		/// <summary>
		/// Returns rgb data in a form: all red rows, all green rows, all blue rows </summary>
		/// <returns> All the red rows, followed by all the green rows, 
		/// followed by all the blue rows. </returns>
		public virtual double[] FlattenedRgbValues
		{
			get
			{
				return flattenedRgbValues;
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is FractionRgbData))
			{
				return false;
			}
			FractionRgbData other = (FractionRgbData) obj;
			return Arrays.Equals(flattenedRgbValues, other.FlattenedRgbValues);
		}

		public override int GetHashCode()
		{
			return Arrays.GetHashCode(flattenedRgbValues);
		}

		public override string ToString()
		{
			return Arrays.ToString(flattenedRgbValues);
		}
	}
}