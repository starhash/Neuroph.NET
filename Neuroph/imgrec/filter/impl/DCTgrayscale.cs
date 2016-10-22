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

	//http://www.lokminglui.com/dct.pdf
	//http://dspace.thapar.edu:8080/dspace/bitstream/10266/2753/1/Tara+Dissertation.pdf


	[Serializable]
	public class DCTgrayscale : ImageFilter
	{

		[NonSerialized]
		private BufferedImage originalImage;
		[NonSerialized]
		private BufferedImage filteredImage;

		private int N_Renamed;
		private int qualityLevel;

		public DCTgrayscale()
		{
			N_Renamed = 16;
			qualityLevel = 50;
		}




		 public virtual BufferedImage processImage(BufferedImage image)
		 {

			int width = image.Width;
		int height = image.Height;

			while (width % N_Renamed != 0)
			{
				width--;
			}

			while (height % N_Renamed != 0)
			{
				height--;
			}

			originalImage = resize(image, width, height);

			filteredImage = new BufferedImage(width, height, originalImage.Type);



			int numXpatches = width / N_Renamed;
			int numYpatches = height / N_Renamed;


			double[][] T = createT();

			double[][] Tinv = null;
			if (N_Renamed == 8)
			{
				Tinv = createTinv();
			}
			if (N_Renamed == 16)
			{
				Tinv = createTinv16X16();
			}



			 for (int i = 0; i < numXpatches; i++)
			 {
				 for (int j = 0; j < numYpatches; j++)
				 {

					 double[][] M = createM(i, j);
					 double[][] D = multiply(multiply(T, M), Tinv);
					 int[][] Q = null;
					 if (N_Renamed == 8)
					 {
						Q = createQ50();
						updateQ(Q);
					 }
					 if (N_Renamed == 16)
					 {
						Q = createQ16X16();
					 }

					 int[][] C = createC(D, Q);
					 double[][] R = createR(Q, C);
					 int[][] Nmatrix = createN(Tinv, R, T);
					 fillFilteredImage(i, j, Nmatrix);
				 }
			 }
			return filteredImage;
		 }

		public virtual BufferedImage resize(BufferedImage img, int newW, int newH)
		{
			int w = img.Width;
			int h = img.Height;
			BufferedImage dimg = dimg = new BufferedImage(newW, newH, img.Type);
			Graphics2D g = dimg.createGraphics();
			g.setRenderingHint(RenderingHints.KEY_INTERPOLATION, RenderingHints.VALUE_INTERPOLATION_BILINEAR);
			g.drawImage(img, 0, 0, newW, newH, 0, 0, w, h, null);
			g.dispose();
			return dimg;
		}

		public virtual double[][] createT()
		{
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] T = new double [N_Renamed][N_Renamed];
			double[][] T = RectangularArrays.ReturnRectangularDoubleArray(N_Renamed, N_Renamed);
			for (int i = 0; i < N_Renamed; i++)
			{
				T[0][i] = roundFourDecimals(1.0 / Math.Sqrt(N_Renamed));
			}
			for (int i = 1; i < N_Renamed; i++)
			{
				for (int j = 0; j < N_Renamed; j++)
				{

					T[i][j] = roundFourDecimals(Math.Sqrt(2.0 / N_Renamed) * Math.Cos(((2.0 * j + 1) * i * Math.PI) / (2.0 * N_Renamed)));
				}
			}
			return T;
		}

		public virtual double [][] createM(int i, int j)
		{
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] M = new double [N_Renamed][N_Renamed];
			double[][] M = RectangularArrays.ReturnRectangularDoubleArray(N_Renamed, N_Renamed);
			int xx = 0;
			int yy = 0;
			for (int x = i * N_Renamed; x < i * N_Renamed + N_Renamed; x++)
			{
				for (int y = j * N_Renamed; y < j * N_Renamed + N_Renamed; y++)
				{
					M[xx][yy] = (new Color(originalImage.getRGB(x, y))).Red - 128;
					yy++;
				}
				xx++;
				yy = 0;
			}
			return M;
		}

		public virtual double roundFourDecimals(double d)
		{
			DecimalFormat fourDForm = new DecimalFormat("#.####");
			return Convert.ToDouble(fourDForm.format(d));
		}

		public virtual double [][] createTinv()
		{
			double[][] Tinv = new double[][] {new double[] {0.3536,0.4904,0.4619,0.4157,0.3536,0.2778,0.1913,0.0975}, new double[] {0.3536,0.4157,0.1913,-0.0975,-0.3536,-0.4904,-0.4619,-0.2778}, new double[] {0.3536,0.2778,-0.1913,-0.4904,-0.3536,0.0975,0.4619,0.4157}, new double[] {0.3536,0.0975,-0.4619,-0.2778,0.3536,0.4157,-0.1913,-0.4904}, new double[] {0.3536,-0.0975,-0.4619,0.2778,0.3536,-0.4157,-0.1913,0.4904}, new double[] {0.3536,-0.2778,-0.1913,0.4904,-0.3536,-0.0975,0.4619,-0.4157}, new double[] {0.3536,-0.4157,0.1913,0.0975,-0.3536,0.4904,-0.4619,0.2778}, new double[] {0.3536,-0.4904,0.4619,-0.4157,0.3536,-0.2778,0.1913,-0.0975}};

			return Tinv;
		}

		public virtual double[][] createTinv16X16()
		{
			double[][] Tinv = new double[][] {new double[] {0.2500,0.3518,0.3467,0.3384,0.3267,0.3118,0.2939,0.2733,0.2500,0.2243,0.1964,0.1667,0.1353,0.1026,0.0690,0.0346}, new double[] {0.2500,0.3384,0.2939,0.2243,0.1353,0.0346,-0.0690,-0.1667,-0.2500,-0.3118,-0.3467,-0.3518,-0.3267,-0.2733,-0.1964,-0.1026}, new double[] {0.2500,0.3118,0.1964,0.0346,-0.1353,-0.2733,-0.3467,-0.3384,-0.2500,-0.1026,0.0690,0.2243,0.3267,0.3518,0.2939,0.1667}, new double[] {0.2500,0.2733,0.0690,-0.1667,-0.3267,-0.3384,-0.1964,0.0346,0.2500,0.3518,0.2939,0.1026,-0.1353,-0.3118,-0.3467,-0.2243}, new double[] {0.2500,0.2243,-0.0690,-0.3118,-0.3267,-0.1026,0.1964,0.3518,0.2500,-0.0346,-0.2939,-0.3384,-0.1353,0.1667,0.3467,0.2733}, new double[] {0.2500,0.1667,-0.1964,-0.3518,-0.1353,0.2243,0.3467,0.1026,-0.2500,-0.3384,-0.0690,0.2733,0.3267,0.0346,-0.2939,-0.3118}, new double[] {0.2500,0.1026,-0.2939,-0.2733,0.1353,0.3518,0.0690,-0.3118,-0.2500,0.1667,0.3467,0.0346,-0.3267,-0.2243,0.1964,0.3384}, new double[] {0.2500,0.0346,-0.3467,-0.1026,0.3267,0.1667,-0.2939,-0.2243,0.2500,0.2733,-0.1964,-0.3118,0.1353,0.3384,-0.0690,-0.3518}, new double[] {0.2500,-0.0346,-0.3467,0.1026,0.3267,-0.1667,-0.2939,0.2243,0.2500,-0.2733,-0.1964,0.3118,0.1353,-0.3384,-0.0690,0.3518}, new double[] {0.2500,-0.1026,-0.2939,0.2733,0.1353,-0.3518,0.0690,0.3118,-0.2500,-0.1667,0.3467,-0.0346,-0.3267,0.2243,0.1964,-0.3384}, new double[] {0.2500,-0.1667,-0.1964,0.3518,-0.1353,-0.2243,0.3467,-0.1026,-0.2500,0.3384,-0.0690,-0.2733,0.3267,-0.0346,-0.2939,0.3118}, new double[] {0.2500,-0.2243,-0.0690,0.3118,-0.3267,0.1026,0.1964,-0.3518,0.2500,0.0346,-0.2939,0.3384,-0.1353,-0.1667,0.3467,-0.2733}, new double[] {0.2500,-0.2733,0.0690,0.1667,-0.3267,0.3384,-0.1964,-0.0346,0.2500,-0.3518,0.2939,-0.1026,-0.1353,0.3118,-0.3467,0.2243}, new double[] {0.2500,-0.3118,0.1964,-0.0346,-0.1353,0.2733,-0.3467,0.3384,-0.2500,0.1026,0.0690,-0.2243,0.3267,-0.3518,0.2939,-0.1667}, new double[] {0.2500,-0.3384,0.2939,-0.2243,0.1353,-0.0346,-0.0690,0.1667,-0.2500,0.3118,-0.3467,0.3518,-0.3267,0.2733,-0.1964,0.1026}, new double[] {0.2500,-0.3518,0.3467,-0.3384,0.3267,-0.3118,0.2939,-0.2733,0.2500,-0.2243,0.1964,-0.1667,0.1353,-0.1026,0.0690,-0.0346}};

			return Tinv;
		}


		public virtual double[][] multiply(double[][] m1, double[][] m2)
		{
		int m1rows = m1.Length;
		int m1cols = m1[0].Length;
		int m2rows = m2.Length;
		int m2cols = m2[0].Length;
		if (m1cols != m2rows)
		{
		  throw new System.ArgumentException("matrices don't match: " + m1cols + " != " + m2rows);
		}
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] result = new double[m1rows][m2cols];
		double[][] result = RectangularArrays.ReturnRectangularDoubleArray(m1rows, m2cols);

		// multiply
		for (int i = 0; i < m1rows; i++)
		{
		  for (int j = 0; j < m2cols; j++)
		  {
			for (int k = 0; k < m1cols; k++)
			{
			result[i][j] += m1[i][k] * m2[k][j];
			}
		  }
		}
		return result;
		}

		public virtual int[][] createQ50()
		{
			int[][] Q = new int[][] {new int[] {16, 11, 10, 16, 24, 40, 51, 61}, new int[] {12, 12, 14, 19, 26, 58, 60, 55}, new int[] {14, 13, 16, 24, 40, 57, 69, 56}, new int[] {14, 17, 22, 29, 51, 87, 80, 62}, new int[] {18, 22, 37, 56, 68, 109, 103, 77}, new int[] {24, 35, 55, 64, 81, 104, 113, 92}, new int[] {49, 64, 78, 87, 103, 121, 120, 101}, new int[] {72, 92, 95, 98, 112, 100, 103, 99}};
		 return Q;
		}

		public virtual int [][] createC(double[][] D, int[][] Q)
		{
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: int[][] C = new int[N_Renamed][N_Renamed];
			int[][] C = RectangularArrays.ReturnRectangularIntArray(N_Renamed, N_Renamed);
			for (int i = 0; i < N_Renamed; i++)
			{
				for (int j = 0; j < N_Renamed; j++)
				{
					C[i][j] = (int) Math.Round(D[i][j] / Q[i][j]);
				}
			}
			return C;
		}

		public virtual double [][] createR(int[][] Q, int[][] C)
		{
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] R = new double[N_Renamed][N_Renamed];
			double[][] R = RectangularArrays.ReturnRectangularDoubleArray(N_Renamed, N_Renamed);
			for (int i = 0; i < N_Renamed; i++)
			{
				for (int j = 0; j < N_Renamed; j++)
				{
					R[i][j] = Q[i][j] * C[i][j];
				}
			}
			return R;
		}

		public virtual int [][] createN(double[][] Tinv, double[][] R, double[][] T)
		{
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: int[][] Nmatrix = new int [N_Renamed][N_Renamed];
			int[][] Nmatrix = RectangularArrays.ReturnRectangularIntArray(N_Renamed, N_Renamed);
			double[][] tmp = multiply(multiply(Tinv, R), T);
			for (int i = 0; i < N_Renamed; i++)
			{
				for (int j = 0; j < N_Renamed; j++)
				{
					Nmatrix[i][j] = (int)(Math.Round(tmp[i][j]) + 128);
				}
			}
			return Nmatrix;
		}

		public virtual void fillFilteredImage(int i, int j, int[][] Nmatrix)
		{
			int xx = 0;
			int yy = 0;
			for (int x = i * N_Renamed; x < i * N_Renamed + N_Renamed; x++)
			{
				for (int y = j * N_Renamed; y < j * N_Renamed + N_Renamed; y++)
				{
					int alpha = (new Color(originalImage.getRGB(x, y))).Alpha;
					int color = Nmatrix[xx][yy];
					int rgb = ImageUtilities.colorToRGB(alpha, color, color, color);
					yy++;
					filteredImage.setRGB(x, y, rgb);
				}
				xx++;
				yy = 0;
			}
		}

		public virtual void updateQ(int[][] Q)
		{
			if (qualityLevel == 50)
			{
				return;
			}
			if (qualityLevel > 50)
			{
				for (int i = 0; i < N_Renamed; i++)
				{
					for (int j = 0; j < N_Renamed; j++)
					{
						Q[i][j] = (int) Math.Round(Q[i][j] * (100 - qualityLevel) * 1.0 / 50);
					}
				}
			}
			if (qualityLevel < 50)
			{
				for (int i = 0; i < N_Renamed; i++)
				{
					for (int j = 0; j < N_Renamed; j++)
					{
						Q[i][j] = (int)Math.Round(Q[i][j] * 50.0 / qualityLevel);
					}
				}
			}
		}

		public virtual int [][] createQ16X16()
		{
			int[][] Q = new int[][] {new int[] {8,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, new int[] {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,30}, new int[] {8,1,1,1,1,1,1,1,1,1,1,1,1,1,30,28}, new int[] {1,1,1,1,1,1,1,1,1,1,1,1,1,32,35,29}, new int[] {1,1,1,1,1,1,1,1,1,1,1,1,32,35,32,28}, new int[] {1,1,1,1,1,1,1,1,1,1,1,35,40,42,40,35}, new int[] {1,1,1,1,1,1,1,1,1,1,35,44,42,40,35,31}, new int[] {1,1,1,1,1,1,1,1,1,35,44,44,50,53,52,45}, new int[] {1,1,1,1,1,1,1,1,31,34,44,55,53,52,45,39}, new int[] {1,1,1,1,1,1,1,31,34,40,41,47,52,45,52,50}, new int[] {1,1,1,1,1,1,30,32,36,41,47,52,54,57,50,46}, new int[] {1,1,1,1,1,36,32,36,44,47,52,57,60,57,55,47}, new int[] {1,1,1,1,36,39,42,44,48,52,57,61,60,60,55,51}, new int[] {1,1,1,39,42,47,48,46,49,57,56,55,52,61,54,51}, new int[] {1,1,42,46,47,48,48,49,53,56,53,50,51,52,51,50}, new int[] {1,45,46,47,48,49,57,56,56,50,52,52,51,51,51,50}};
			return Q;
		}

		public override string ToString()
		{
			return "DCT-grayscale";
		}

		public virtual int QualityLevel
		{
			set
			{
				if (value > 97)
				{
					this.qualityLevel = 97;
					return;
				}
				if (value < 1)
				{
					this.qualityLevel = 1;
					return;
				}
				this.qualityLevel = value;
			}
		}

		public virtual int N
		{
			set
			{
				if (value >= 12)
				{
					this.N_Renamed = 16;
					return;
				}
				this.N_Renamed = 8;
			}
		}






	}

}