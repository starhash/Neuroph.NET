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
	public class GuoHallThiningFilter : ImageFilter
	{

		private BufferedImage originalImage;
		private BufferedImage filteredImage;

		private bool blackLetters = true;

		internal int[][] imageM;
		internal int width;
		internal int height;

		/// 
		/// <param name="image"> The input image should be binary
		/// @return </param>
		public virtual BufferedImage processImage(BufferedImage image)
		{

			originalImage = image;
			width = originalImage.Width;
			height = originalImage.Height;

			filteredImage = new BufferedImage(width, height, originalImage.Type);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: imageM = new int[width][height];
			imageM = RectangularArrays.ReturnRectangularIntArray(width, height);

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{

					int col = (new Color(originalImage.getRGB(i, j))).Red;
					if (blackLetters)
					{
						imageM[i][j] = 1 - (col / 255);
					}
					else
					{
						imageM[i][j] = col / 255;
					}

				}

			}


			while (true)
			{

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: int[][] start = new int[width][height];
				int[][] start = RectangularArrays.ReturnRectangularIntArray(width, height);

				for (int i = 0; i < width; i++)
				{
					for (int j = 0; j < height; j++)
					{
						start[i][j] = imageM[i][j];
					}
				}

				thiningGuoHallIteration(0);
				thiningGuoHallIteration(1);

				bool same = true;
				for (int i = 0; i < width; i++)
				{
					for (int j = 0; j < height; j++)
					{
						if (start[i][j] != imageM[i][j])
						{
							same = false;
							goto MainforLoopBreak;
						}

					}
					MainforLoopContinue:;
				}
				MainforLoopBreak:
				if (same)
				{
					break;
				}

			}

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{

					int alpha = (new Color(originalImage.getRGB(i, j))).Alpha;
					int col;
					if (blackLetters)
					{
						col = 255 - imageM[i][j] * 255;
					}
					else
					{
						col = imageM[i][j] * 255;
					}
					int rgb = ImageUtilities.colorToRGB(alpha, col, col, col);

					filteredImage.setRGB(i, j, rgb);
				}
			}

			return filteredImage;

		}


		public override string ToString()
		{
			return "Guo Hall Thin Method";
		}

		public virtual void thiningGuoHallIteration(int iter)
		{
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: int[][] marker = new int[width][height];
			int[][] marker = RectangularArrays.ReturnRectangularIntArray(width, height);
			for (int i = 1; i < width - 1; i++)
			{
				for (int j = 1; j < height - 1; j++)
				{

					int p2 = imageM[i - 1][j];
					int p3 = imageM[i - 1][j + 1];
					int p4 = imageM[i][j + 1];
					int p5 = imageM[i + 1][j + 1];
					int p6 = imageM[i + 1][j];
					int p7 = imageM[i + 1][j - 1];
					int p8 = imageM[i][j - 1];
					int p9 = imageM[i - 1][j - 1];

					int C = (~p2 & (p3 | p4)) + (~p4 & (p5 | p6)) + (~p6 & (p7 | p8)) + (~p8 & (p9 | p2));
					int N1 = (p9 | p2) + (p3 | p4) + (p5 | p6) + (p7 | p8);
					int N2 = (p2 | p3) + (p4 | p5) + (p6 | p7) + (p8 | p9);
					int N = N1 < N2 ? N1 : N2;
					int m = iter == 0 ? ((p6 | p7 | ~p9) & p8) : ((p2 | p3 | ~p5) & p4);

					if (C == 1 && (N >= 2 && N <= 3) & m == 0)
					{
						marker[i][j] = 1;
					}

				}

			}
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{

					int tmp = 1 - marker[i][j];
					if (imageM[i][j] == tmp && imageM[i][j] == 1)
					{
						imageM[i][j] = 1;
					}
					else
					{
						imageM[i][j] = 0;
					}

				}
			}

		}

		public virtual bool BlackLetters
		{
			set
			{
				this.blackLetters = value;
			}
		}



	}

}