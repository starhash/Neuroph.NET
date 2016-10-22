using System;
using System.Collections.Generic;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
namespace org.neuroph.ocr.filter
{


	/// <summary>
	/// Extraction of the letter in the text.
	/// Finds the pixel which corresponds to letter and whole letter extract to the new image. 
	/// @author Mihailo
	/// </summary>
	public class OCRExtractLetter
	{

		private int cropWidth;
		private int cropHeight;
		private int trashSize;

		/// <param name="cropWidth"> width of the cropped image, should be greater of the letter </param>
		/// <param name="cropHeight"> height of the cropped image, should be greater of the letter </param>
		/// <param name="trashSize"> number of pixels that is not recognized as letter </param>
		public OCRExtractLetter(int cropWidth, int cropHeight, int trashSize)
		{
			this.cropWidth = cropWidth;
			this.cropHeight = cropHeight;
			this.trashSize = trashSize;
		}

		/// <summary>
		/// You <b>must</b> set three parameters<br/>
		/// 1. cropWidth<br/>
		/// 2. cropHeight<br/>
		/// 3. trashSize
		/// </summary>
		public OCRExtractLetter()
		{
		}

		/// 
		/// <param name="cropHeight"> height of the cropped image, should be greater than letter height </param>
		public virtual int CropHeight
		{
			set
			{
				this.cropHeight = value;
			}
		}

		/// 
		/// <param name="cropWidth"> width of the cropped image, should be greater than letter width </param>
		public virtual int CropWidth
		{
			set
			{
				this.cropWidth = value;
			}
		}

		/// 
		/// <param name="trashSize"> number of pixels that is not recognized as letter, 
		/// number smaller than the number of pixels in some little letter, for example i </param>
		public virtual int TrashSize
		{
			set
			{
				this.trashSize = value;
			}
		}


		/// 
		/// <param name="image"> image with whole text </param>
		/// <param name="visited"> matrix of boolean, size of the matrix should correspond to size of the image with text. This matrix is used like in BFS traversal. </param>
		/// <param name="startX"> starting point on X coordinate where the black pixel is found </param>
		/// <param name="startY"> starting point on Y coordinate where the black pixel is found </param>
		/// <returns> new image with extracted letter only if number of pixel in that letter is greater than trashSize  </returns>
		public virtual BufferedImage extraxtLetter(BufferedImage image, bool[][] visited, int startX, int startY)
		{
			int gapWidth = cropWidth / 5 * 2; //start x coordinate of letter, 2/5 itended
			int gapHeight = cropHeight / 5 * 2; //start y coordinate of letter
			LinkedList<string> queue = new LinkedList<string>();
			BufferedImage letter = new BufferedImage(cropWidth, cropHeight, image.Type);
			Color white = Color.WHITE;
			Color black = Color.BLACK;

			// fill all letter image with white pixels
			for (int i = 0; i < cropHeight; i++)
			{
				for (int j = 0; j < cropWidth; j++)
				{
					letter.setRGB(j, i, white.RGB);
				}
			}
			int countPixels = 0; // ignore dots
			string positions = startX + " " + startY;
			visited[startX][startY] = true;
			queue.AddLast(positions);
			while (queue.Count > 0)
			{
				string pos = queue.RemoveFirst();
				string[] posArray = pos.Split(" ", true);
				int H = Convert.ToInt32(posArray[0]); // H-height
				int W = Convert.ToInt32(posArray[1]); // W-width
				visited[H][W] = true;

				int posW = W - startY + gapWidth;
				int posH = H - startX + gapHeight;

				countPixels++;

				letter.setRGB(posW, posH, black.RGB);

				int color;
				int blackInt = 0;
				for (int i = H - 1; i <= H + 1; i++)
				{
					for (int j = W - 1; j <= W + 1; j++)
					{
						if (i >= 0 && j >= 0 && i < image.Height && j < image.Width)
						{
							if (!visited[i][j])
							{
								color = (new Color(image.getRGB(j, i))).Red;
								if (color == blackInt)
								{
									visited[i][j] = true;
									string tmpPos = i + " " + j;
									queue.AddLast(tmpPos);
								}
							}
						}
					}
				}
			}
			if (countPixels < trashSize) //da ne bi uzimao male crtice, tacke
			{

				return null;
			}
			return letter;
		}

	}

}