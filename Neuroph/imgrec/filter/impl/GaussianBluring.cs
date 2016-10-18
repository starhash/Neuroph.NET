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
	/// @author Mihailo Stupar
	/// </summary>

	//http://www.swageroo.com/wordpress/how-to-program-a-gaussian-blur-without-using-3rd-party-libraries/
	[Serializable]
	public class GaussianBluring : ImageFilter
	{

		[NonSerialized]
		private BufferedImage originalImage;
		[NonSerialized]
		private BufferedImage filteredImage;

		private int radius;
		private double sigma;

		private double[][] kernel;

		public GaussianBluring()
		{
			radius = 7;
			sigma = 10;
		}




		public virtual BufferedImage processImage(BufferedImage image)
		{

			originalImage = image;

			int oldWidth = image.Width;
			int oldHeight = image.Height;

			int width = image.Width - 2 * radius;
			int height = image.Height - 2 * radius;





			filteredImage = new BufferedImage(width, height, originalImage.Type);

			createKernel();


			for (int i = radius; i < oldWidth - radius; i++)
			{
				for (int j = radius; j < oldHeight - radius; j++)
				{
					int alpha = (new Color(originalImage.getRGB(i, j))).Alpha;
					int newColor = getNewColor(i, j);
					int rgb = ImageUtilities.colorToRGB(alpha, newColor, newColor, newColor);

					int x = i - radius;
					int y = j - radius;
					filteredImage.setRGB(x,y, rgb);

				}
			}


			return filteredImage;
		}




		protected internal virtual void createKernel()
		{

			int size = radius * 2 + 1;
			int center = radius;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: kernel = new double [size][size];
			kernel = RectangularArrays.ReturnRectangularDoubleArray(size, size);

			for (int i = 0; i < kernel.Length; i++)
			{
				for (int j = 0; j < kernel[0].Length; j++)
				{
					int distanceX = Math.Abs(center - i);
					int distanceY = Math.Abs(center - j);
					kernel [i][j] = gaussianFormula(distanceX, distanceY);
				}
			}

			double noralizationValue = getNormalizationValue(kernel);

			for (int i = 0; i < kernel.Length; i++)
			{
				for (int j = 0; j < kernel[0].Length; j++)
				{
					kernel[i][j] = kernel[i][j] * noralizationValue;
				}
			}

		}

		public virtual double gaussianFormula(double x, double y)
		{
			double one = 1.0;
			double value = one / (2 * Math.PI * sigma * sigma);
			double exp = -(x * x + y * y) / (2 * sigma * sigma);
			exp = Math.Pow(Math.E, exp);
			value = value * exp;
			return value;
		}

		public virtual double getNormalizationValue(double[][] kernel)
		{
			double sum = 0;
			for (int i = 0; i < kernel.Length; i++)
			{
				for (int j = 0; j < kernel[0].Length; j++)
				{
					sum = sum + kernel [i][j];
				}
			}
			double one = 1.0;
			return one / sum;
		}

		public virtual int getNewColor(int x, int y)
		{
			if (!checkConditios(x, y))
			{
				return (new Color(originalImage.getRGB(x, y))).Red;
			}

			int size = 2 * radius + 1;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] matrix = new double [size][size];
			double[][] matrix = RectangularArrays.ReturnRectangularDoubleArray(size, size);

			int newI = 0;
			int newJ = 0;
			for (int i = x - radius; i <= x + radius; i++)
			{
				for (int j = y - radius; j <= y + radius; j++)
				{

					//System.out.println("size:" +size+", radius:"+radius+", x:"+x+",y:"+y+", i:"+i+",j:"+j+ ", newI:"+newI+"newJ:"+newJ);
					int oldColor = (new Color(originalImage.getRGB(i, j))).Red;
					matrix[newI][newJ] = oldColor * kernel[newI][newJ];
					newJ++;
				}
				newI++;
				newJ = 0;
			}

			double sum = 0;

			for (int i = 0; i < matrix.Length; i++)
			{
				for (int j = 0; j < matrix[0].Length; j++)
				{
					sum = sum + matrix[i][j];
				}
			}

			return (int) Math.Round(sum);




		}

		public virtual bool checkConditios(int x, int y)
		{
			if (x - radius >= 0 && x + radius < originalImage.Width && y - radius >= 0 && y + radius < originalImage.Height)
			{
				return true;
			}
			return false;
		}


		public override string ToString()
		{
			return "Gaussian bluring";
		}

		public virtual int Radius
		{
			set
			{
				if (value % 2 != 0)
				{
					this.radius = value;
				}
				this.radius = value - 1;
			}
		}

		public virtual double Sigma
		{
			set
			{
				this.sigma = value;
			}
		}






	}

}