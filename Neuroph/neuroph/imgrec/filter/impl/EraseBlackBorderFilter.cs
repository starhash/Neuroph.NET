using System;
using System.Collections.Generic;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
namespace org.neuroph.imgrec.filter.impl
{


	/// <summary>
	/// Erase Black border filter removes black border from the image. It assumes
	/// that the most important part of image is included in rectangle in the central
	/// part of the image. The rectangle is placed horizontally.All parts outside
	/// rectangle will be deleted from the image. The entrance to the filter must be
	/// binarized image. Good results are shown in images for lung cancer/cross
	/// section.
	/// 
	/// @author Mihailo Stupar
	/// </summary>
	[Serializable]
	public class EraseBlackBorderFilter : ImageFilter
	{

		[NonSerialized]
		private BufferedImage originalImage;
		[NonSerialized]
		private BufferedImage filteredImage;

		public virtual BufferedImage processImage(BufferedImage image)
		{

			originalImage = image;

			int width = originalImage.Width;
			int height = originalImage.Height;

			filteredImage = new BufferedImage(width, height, originalImage.Type);

			int centerI = width / 2;
			int centerJ = height / 2;

			int lengthI = width / 4;
			int lengthJ = height / 6;

			int startI = centerI - lengthI / 2;
			int goalI = centerI + lengthI / 2;

			int startJ = centerJ - lengthJ / 2;
			int goalJ = centerJ + lengthJ / 2;
			bool[][] visited;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: visited = new bool[width][height];
			visited = RectangularArrays.ReturnRectangularBoolArray(width, height);
			int color;

			for (int i = startI; i < goalI; i++)
			{
				for (int j = startJ; j < goalJ; j++)
				{

					color = (new Color(originalImage.getRGB(i, j))).Red;
					if (color == 0)
					{
						if (!visited[i][j])
						{
							BFS(i, j, visited);
						}
					}
				}
			}

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					int alpha = (new Color(originalImage.getRGB(i, j))).Alpha;
					if (!visited[i][j])
					{
						int white = 255;
						color = ImageUtilities.colorToRGB(alpha, white, white, white);
						filteredImage.setRGB(i, j, color);
					}
					else
					{
						int black = 0;
						color = ImageUtilities.colorToRGB(alpha, black, black, black);
						filteredImage.setRGB(i, j, color);
					}
				}
			}

			return filteredImage;
		}

		public virtual void BFS(int startI, int startJ, bool[][] visited)
		{
			LinkedList<string> queue = new LinkedList<string>();

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
				for (int i = x - 1; i <= x + 1; i++)
				{
					for (int j = y - 1; j <= y + 1; j++)
					{
						if (i >= 0 && j >= 0 && i < originalImage.Width && j < originalImage.Height && i != x && j != y)
						{
							if (!visited[i][j])
							{
								int color = (new Color(originalImage.getRGB(i, j))).Red;
								if (color == 0)
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

		}

		public override string ToString()
		{
			return "Erase Black Border Filter";
		}

	}

}