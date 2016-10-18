using System;
using System.Collections.Generic;

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
	public class LetterSegmentationFilter : ImageFilter
	{

		private BufferedImage originalImage;

		private int width;
		private int height;

		private bool[][] visited;

		public virtual BufferedImage processImage(BufferedImage image)
		{

			originalImage = image;
			width = originalImage.Width;
			height = originalImage.Height;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: visited = new bool[width][height];
			visited = RectangularArrays.ReturnRectangularBoolArray(width, height);

			int name = 1;

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{

					int color = (new Color(originalImage.getRGB(i, j))).Red;
					if (color == 255)
					{
						visited[i][j] = true;
					}
					else
					{
						if (name > 3000)
						{
							return originalImage;
						}
						BFS(i, j, name + "");
						name++;
					}

				}
			}

			return originalImage;
		}

		public virtual void BFS(int startI, int startJ, string imageName)
		{
			LinkedList<string> queue = new LinkedList<string>();

			//=============================================================================
			int letterWidth = 80;
			int letterHeight = 80;
			int gapX = 30;
			int gapY = 30;
			BufferedImage letter = new BufferedImage(letterWidth, letterHeight, BufferedImage.TYPE_BYTE_BINARY);
			int alpha = (new Color(originalImage.getRGB(startI, startJ))).Alpha;
			int white = ImageUtilities.colorToRGB(alpha, 255, 255, 255);
			int black = ImageUtilities.colorToRGB(alpha, 0, 0, 0);
			for (int i = 0; i < letterWidth; i++)
			{
				for (int j = 0; j < letterHeight; j++)
				{
					letter.setRGB(i, j, white);

				}
			}
			//=============================================================================

			int count = 0;
			string positions = startI + " " + startJ;
			visited[startI][startJ] = true;
			queue.AddLast(positions);

			while (queue.Count > 0)
			{
				string pos = queue.RemoveFirst();
				string[] posArray = pos.Split(" ", true);
				int x = Convert.ToInt32(posArray[0]);
				int y = Convert.ToInt32(posArray[1]);
				visited[x][y] = true;

				//set black pixel to letter image===================================
				int posX = startI - x + gapX;
				int posY = startJ - y + gapY;

				count++;
				try
				{
					letter.setRGB(posX, posY, black);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
					Console.Write(e.StackTrace);
					Console.WriteLine("posX " + posX);
					Console.WriteLine("posY " + posY);
					Console.WriteLine("letterWidth " + letter.Width);
					Console.WriteLine("letterHeight " + letter.Height);
					throw e;
				}
				//==================================================================
				for (int i = x - 1; i <= x + 1; i++)
				{
					for (int j = y - 1; j <= y + 1; j++)
					{
						if (i >= 0 && j >= 0 && i < originalImage.Width && j < originalImage.Height)
						{
							if (!visited[i][j])
							{
								int color = (new Color(originalImage.getRGB(i, j))).Red;
								if (color < 10)
								{
									visited[i][j] = true;
									string tmpPos = i + " " + j;
									queue.AddLast(tmpPos);
								}
							}
						}
					} //i
				} //j
			}

			Console.WriteLine("count = " + count);
			//save letter=========================================================== 
			if (count < 3)
			{
				return;
			}
			try
			{
				saveToFile(letter, imageName);
				//
			}
			catch (IOException ex)
			{
				Console.WriteLine(ex.ToString());
				Console.Write(ex.StackTrace);
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void saveToFile(java.awt.image.BufferedImage img, String name) throws java.io.FileNotFoundException, java.io.IOException
		public virtual void saveToFile(BufferedImage img, string name)
		{

			File outputfile = new File("C:/Users/Mihailo/Documents/NetBeansProjects/ImagePreprocessing/Segmented_letters/" + name + ".jpg");
			ImageIO.write(img, "jpg", outputfile);
		}

		public override string ToString()
		{
			return "Letter Segmentation filter";
		}

	}

}