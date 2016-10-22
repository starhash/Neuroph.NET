using System;

namespace org.neuroph.imgrec
{

	using Image = org.neuroph.imgrec.image.Image;
	using ImageJ2SE = org.neuroph.imgrec.image.ImageJ2SE;

	/// 
	/// <summary>
	/// @author Mihailo Stupar
	/// </summary>
	public class FractionHSLData
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
		/// Matrix which contains the hue componenet for each image pixel
		/// </summary>
		protected internal double[][] hueValues;

		/// <summary>
		/// Matrix which contains the saturation componenet for each image pixel
		/// </summary>
		protected internal double[][] saturationValues;

		/// <summary>
		/// Matrix which contains the intensity componenet for each image pixel
		/// </summary>
		protected internal double[][] lightnessValues;

		/// <summary>
		/// Single array with the hue, saturation and lightness components for each image pixel
		/// </summary>
		protected internal double[] flattenedHSLValues;


		/// <summary>
		/// Single array only with hue components for each pixel
		/// </summary>
		protected internal double[] flattenedHueValues;

		public FractionHSLData(BufferedImage img)
		{


		if (img != null)
		{
			width = img.Width;
			height = img.Height;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: hueValues = new double[height][width];
			hueValues = RectangularArrays.ReturnRectangularDoubleArray(height, width);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: saturationValues = new double[height][width];
			saturationValues = RectangularArrays.ReturnRectangularDoubleArray(height, width);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: lightnessValues = new double[height][width];
			lightnessValues = RectangularArrays.ReturnRectangularDoubleArray(height, width);
			flattenedHSLValues = new double[width * height * 3];
					flattenedHueValues = new double[width * height];

			populateHSLArrays(new ImageJ2SE(img));
		}
		else //no input image specified so default all values
		{
			width = 0;
			height = 0;
			hueValues = null;
			saturationValues = null;
			lightnessValues = null;
			flattenedHSLValues = null;
					flattenedHueValues = null;
		}
		}


		public FractionHSLData(Image img)
		{
			width = img.Width;
			height = img.Height;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: hueValues = new double[height][width];
			hueValues = RectangularArrays.ReturnRectangularDoubleArray(height, width);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: saturationValues = new double[height][width];
			saturationValues = RectangularArrays.ReturnRectangularDoubleArray(height, width);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: lightnessValues = new double[height][width];
			lightnessValues = RectangularArrays.ReturnRectangularDoubleArray(height, width);
			flattenedHSLValues = new double[width * height * 3];
					flattenedHueValues = new double[width * height];

			populateHSLArrays(img);
		}


		/// <summary>
		/// Fills the HSL matrices from image </summary>
		/// <param name="img"> image to get rgb data from
		///  </param>

		/// <summary>
		/// Fills the HSL matrices from image - this is where conversion from RGB to HSL is done </summary>
		/// <param name="image"> image to use     </param>
		protected internal virtual void populateHSLArrays(Image image)
		{

				double red;
				double green;
				double blue;

				double Cmax;
				double Cmin;

				double delta;

				for (int j = 0; j < width; j++)
				{
					for (int i = 0; i < height; i++)
					{

						Color color = new Color(image.getPixel(j, i)); // it was getRGB

						red = color.Red;
						green = color.Green;
						blue = color.Blue;

						red = red / 255;
						green = green / 255;
						blue = blue / 255;

						Cmax = Math.Max(red, Math.Max(green, blue));
						Cmin = Math.Min(red, Math.Min(green, blue));

						delta = Cmax - Cmin;

						double hue = 0;
						if (delta != 0)
						{
							if (Cmax == red)
							{
								hue = 60 * (((green - blue) / delta) % 6);
							}
							if (Cmax == green)
							{
								hue = 60 * (((blue - red) / delta) + 2);
							}
							if (Cmax == blue)
							{
								hue = 60 * ((red - green) / delta + 4);
							}
						}
						else
						{
							double a = (2 * red - green - blue) / 2;
							double b = (green - blue) * Math.Sqrt(3) / 2;
							hue = Math.Atan2(b, a);
						}
						hueValues[i][j] = hue / 360; //podeli sa 360 da vrednot bude izmedju 0-1

						double lightness = (Cmax + Cmin) / 2;
						lightnessValues[i][j] = lightness;

						double saturation = 0;
						if (delta == 0)
						{
							saturation = 0;
						}
						else
						{
							saturation = delta / (1 - Math.Abs(2 * lightness - 1));
						}
						saturationValues[i][j] = saturation;
					}
				}
		}

			   public virtual void fillFlattenedHueValues()
			   {

				int position = 0;
				for (int i = 0; i < height; i++)
				{
					for (int j = 0; j < height; j++)
					{
						flattenedHueValues[position++] = hueValues[i][j];
					}
				}
			   }

		public virtual void fillFlattenedHSLValues()
		{

			int positionHue = 0;
			int positionSaturation = 1;
			int positionLighteness = 2;

			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					flattenedHSLValues[positionHue] = hueValues[i][j];
					flattenedHSLValues[positionSaturation] = saturationValues[i][j];
					flattenedHSLValues[positionLighteness] = lightnessValues[i][j];
					positionHue = positionHue + 3;
					positionSaturation = positionSaturation + 3;
					positionLighteness = positionLighteness + 3;
				}
			}


		}

		public virtual double[][] HueValues
		{
			get
			{
				return hueValues;
			}
		}

		/// <summary>
		/// Returns saturation component for the entire image </summary>
		/// <returns> 2d array in the form: [row][column] </returns>
		public virtual double[][] SaturationValues
		{
			get
			{
				return saturationValues;
			}
		}

		/// <summary>
		/// Returns lightness component for the entire image </summary>
		/// <returns> 2d array in the form: [row][column] </returns>
		public virtual double[][] LightnessValues
		{
			get
			{
				return lightnessValues;
			}
		}

		public virtual double[] FlattenedHSLValues
		{
			get
			{
				return flattenedHSLValues;
			}
		}



	}
}