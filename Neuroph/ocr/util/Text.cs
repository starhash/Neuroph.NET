using System.Collections.Generic;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace org.neuroph.ocr.util
{


	/// 
	/// <summary>
	/// @author Mihailo
	/// </summary>
	public class Text
	{

		private List<int?> linePositions;
		private IDictionary<int?, List<WordPosition>> map;
		private Letter letterInfo;

		private BufferedImage image;

		public Text(BufferedImage image, Letter letterInformation)
		{
			this.letterInfo = letterInformation;
			this.image = image;
			linePositions = OCRUtilities.rowPositions(image, letterInformation.SmallestSizeLetter);
			createMap();
			populateMap();
		}


		private void populateMap()
		{
			foreach (int? row in linePositions)
			{
				map[row] = OCRUtilities.wordsPositions(image, row, letterInfo.LetterSize, letterInfo.SpaceGap);
			}
		}

		private void createMap()
		{
			map = new Dictionary<int?, List<WordPosition>>();
			foreach (int? row in linePositions)
			{
				map[row] = new List<WordPosition>();
			}
		}

		/// 
		/// <returns> number of rows  </returns>
		public virtual int numberOfRows()
		{
			return linePositions.Count;
		}

		/// <summary>
		/// pixel position of the row </summary>
		/// <param name="index"> start from 0 </param>
		/// <returns> pixel position of the row at the index </returns>
		public virtual int getRowAt(int index)
		{
			return linePositions[index];
		}

		/// <summary>
		/// return objects of class Word as List </summary>
		/// <param name="index"> index of row, start from 0 </param>
		/// <returns> list of Word </returns>
		/// <seealso cref= WordPosition </seealso>
		public virtual List<WordPosition> getWordsAtRow(int index)
		{
			int key = linePositions[index];
			return map[key];
		}

		/// <summary>
		/// return objects of class Word as List </summary>
		/// <param name="pixel"> pixel position of the row </param>
		/// <returns> list of Word </returns>
		/// <seealso cref= WordPosition </seealso>
		/// <exception cref="NullPointerException"> if the pixel is not result of method 
		/// getRowAt(index) </exception>
		public virtual List<WordPosition> getWordsAtPixel(int pixel)
		{
			return map[pixel];
		}

		/// 
		/// <returns> Image (document) </returns>
		public virtual BufferedImage Image
		{
			get
			{
				return image;
			}
		}


	}

}