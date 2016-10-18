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
	public class OCRCropImage : ImageFilter
	{

		private BufferedImage originalImage;
		private BufferedImage filteredImage;
		private int width;
		private int height;

		private int newWidth;
		private int newHeight;

		/// <summary>
		/// Dimension of the cropped image
		/// </summary>
		/// <param name="width"> </param>
		/// <param name="height"> </param>
		public virtual void setDimension(int width, int height)
		{
			newHeight = height;
			newWidth = width;
		}

		public virtual BufferedImage processImage(BufferedImage image)
		{

			width = image.Width;
			height = image.Height;

			originalImage = image;
			filteredImage = new BufferedImage(newWidth, newHeight, image.Type);

			int startH = createStartH();
			int startW = createStartW();
			int endH = createEndH();
			int endW = createEndW();

			fillImage(startH, startW, endH, endW);


			return filteredImage;
		}

		private int createStartH()
		{
			int color;
			int black = 0;
			int startH = 0;
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					color = (new Color(originalImage.getRGB(j, i))).Red;
					if (color == black)
					{
						startH = i;
						goto loopBreak;
					}
				}
				loopContinue:;
			}
			loopBreak:
			return startH;
		}

		private int createStartW()
		{
			int color;
			int black = 0;
			int startW = 0;
			for (int j = 0; j < width; j++)
			{
				for (int i = 0; i < height; i++)
				{
					color = (new Color(originalImage.getRGB(j, i))).Red;
					if (color == black)
					{
						startW = j;
						goto loopBreak;
					}
				}
				loopContinue:;
			}
			loopBreak:
			return startW;
		}

		private int createEndH()
		{
			int color;
			int black = 0;
			int endH = 0;
			for (int i = height - 1; i >= 0; i--)
			{
				for (int j = width - 1; j >= 0; j--)
				{
					color = (new Color(originalImage.getRGB(j, i))).Red;
					if (color == black)
					{
						endH = i;
						goto loopBreak;
					}
				}
				loopContinue:;
			}
			loopBreak:
			return endH;
		}

		private int createEndW()
		{
			int color;
			int black = 0;
			int endW = 0;
			for (int j = width - 1; j >= 0; j--)
			{
				for (int i = height - 1; i >= 0; i--)
				{
					color = (new Color(originalImage.getRGB(j, i))).Red;
					if (color == black)
					{
						endW = j;
						goto loopBreak;
					}
				}
				loopContinue:;
			}
			loopBreak:
			return endW;
		}

		private void fillImage(int startH, int startW, int endH, int endW)
		{



			// fill the image with white color
			int alpha = (new Color(originalImage.getRGB(width / 2, height / 2))).Red;
			int whiteRGB = ImageUtilities.colorToRGB(alpha, 255, 255, 255);
			for (int i = 0; i < newHeight; i++)
			{
				for (int j = 0; j < newWidth; j++)
				{
					filteredImage.setRGB(j, i, whiteRGB);
				}
			}

			// fill black pixels

			int oldCenterH = (startH + endH) / 2;
			int oldCenterW = (startW + endW) / 2;

			int newCenterH = newHeight / 2;
			int newCenterW = newWidth / 2;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: bool[][] visited = new bool[newHeight][newWidth];
			bool[][] visited = RectangularArrays.ReturnRectangularBoolArray(newHeight, newWidth);

			LinkedList<string> queue = new LinkedList<string>();
			string pos = newCenterH + " " + newCenterW + " " + oldCenterH + " " + oldCenterW;
			queue.AddLast(pos);
			visited[newCenterH][newCenterW] = true;
			try
			{
				while (queue.Count > 0)
				{
					string tmp = queue.RemoveFirst();
					int nh = Convert.ToInt32(tmp.Split(" ", true)[0]);
					int nw = Convert.ToInt32(tmp.Split(" ", true)[1]);
					int oh = Convert.ToInt32(tmp.Split(" ", true)[2]);
					int ow = Convert.ToInt32(tmp.Split(" ", true)[3]);

					filteredImage.setRGB(nw, nh, originalImage.getRGB(ow, oh));

					for (int i = -1; i <= 1; i++)
					{
						for (int j = -1; j <= 1; j++)
						{
							int n_tmpH = nh + i;
							int n_tmpW = nw + j;
							int o_tmpH = oh + i;
							int o_tmpW = ow + j;
							if (!visited[n_tmpH][n_tmpW])
							{
								visited[n_tmpH][n_tmpW] = true;
								queue.AddLast(n_tmpH + " " + n_tmpW + " " + o_tmpH + " " + o_tmpW);
							}
						}
					}
				}

			}
			catch (System.IndexOutOfRangeException)
			{
			}
		}
	}

}