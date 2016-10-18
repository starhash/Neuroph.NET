using System;

namespace org.neuroph.imgrec.filter.impl
{


	/// 
	/// <summary>
	/// @author Mihailo Stupar
	/// </summary>
	[Serializable]
	public class SobelEdgeDetection : ImageFilter
	{

		[NonSerialized]
		private BufferedImage originalImage;
		[NonSerialized]
		private BufferedImage filteredImage;

		private double[][] sobelX;
		private double[][] sobelY;

		private double treshold;


		public virtual BufferedImage processImage(BufferedImage image)
		{

			originalImage = image;

			int width = image.Width;
			int height = image.Height;

			filteredImage = new BufferedImage(width, height, image.Type);

			treshold = 0.1;
			generateSobelOperators();

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] Gx = new double[width][height];
			double[][] Gx = RectangularArrays.ReturnRectangularDoubleArray(width, height);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] Gy = new double[width][height];
			double[][] Gy = RectangularArrays.ReturnRectangularDoubleArray(width, height);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] G = new double[width][height];
			double[][] G = RectangularArrays.ReturnRectangularDoubleArray(width, height);

			double max = 0;

			for (int i = 1; i < width - 1; i++)
			{
				for (int j = 1; j < height - 1; j++)
				{

					Gx[i][j] = calculateGradient(i, j, sobelX);
					Gy[i][j] = calculateGradient(i, j, sobelY);

					G[i][j] = Math.Abs(Gx[i][j]) + Math.Abs(Gy[i][j]);

					if (G[i][j] > max)
					{
						max = G[i][j];
					}


				}

			}


			treshold = treshold * max;
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{

					int newColor;
					int alpha = (new Color(originalImage.getRGB(i, j))).Alpha;

					if (G[i][j] > treshold)
					{
						newColor = 0;
					}
					else
					{
						newColor = 255;
					}

					int rgb = ImageUtilities.colorToRGB(alpha, newColor, newColor, newColor);
					filteredImage.setRGB(i, j, rgb);

				}
			}

			return filteredImage;
		}

		protected internal virtual void generateSobelOperators()
		{

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: sobelX = new double[3][3];
			sobelX = RectangularArrays.ReturnRectangularDoubleArray(3, 3);
			sobelX [0][0] = -0.25;
			sobelX [0][1] = -0.5;
			sobelX [0][2] = -0.25;
			sobelX [1][0] = 0;
			sobelX [1][1] = 0;
			sobelX [1][2] = 0;
			sobelX [2][0] = 0.25;
			sobelX [2][1] = 0.5;
			sobelX [2][2] = 0.25;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: sobelY = new double[3][3];
			sobelY = RectangularArrays.ReturnRectangularDoubleArray(3, 3);
			sobelY [0][0] = -0.25;
			sobelY [0][1] = 0;
			sobelY [0][2] = 0.25;
			sobelY [1][0] = -0.5;
			sobelY [1][1] = 0;
			sobelY [1][2] = 0.5;
			sobelY [2][0] = -0.25;
			sobelY [2][1] = 0;
			sobelY [2][2] = 0.25;


			double one = 1;
			double oneThird = one / 3;


			sobelX [0][0] = -oneThird;
			sobelX [0][1] = -oneThird;
			sobelX [0][2] = -oneThird;
			sobelX [1][0] = 0;
			sobelX [1][1] = 0;
			sobelX [1][2] = 0;
			sobelX [2][0] = oneThird;
			sobelX [2][1] = oneThird;
			sobelX [2][2] = oneThird;

			sobelY [0][0] = -oneThird;
			sobelY [0][1] = 0;
			sobelY [0][2] = oneThird;
			sobelY [1][0] = -oneThird;
			sobelY [1][1] = 0;
			sobelY [1][2] = oneThird;
			sobelY [2][0] = -oneThird;
			sobelY [2][1] = 0;
			sobelY [2][2] = oneThird;


		}

		protected internal virtual double calculateGradient(int i, int j, double[][] sobelOperator)
		{
			double sum = 0;

			int posX = 0;
			for (int x = i - 1; x <= i + 1; x++)
			{

				int posY = 0;
				for (int y = j - 1; y <= j + 1; y++)
				{

					double gray = (new Color(originalImage.getRGB(x, y))).Red;

					sum = sum + gray * sobelOperator[posX][posY];
					posY++;
				}
				posX++;
			}

			return sum;
		}

		public virtual double Treshold
		{
			set
			{
				this.treshold = value;
			}
		}

		public override string ToString()
		{
			return "Sobel method";
		}


	}
}