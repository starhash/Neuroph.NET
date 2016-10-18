using System;
using System.Collections.Generic;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
namespace org.neuroph.ocr.util
{

	using Histogram = org.neuroph.ocr.util.histogram.Histogram;
	using OCRHistogram = org.neuroph.ocr.util.histogram.OCRHistogram;

	/// 
	/// <summary>
	/// @author Mihailo Stupar
	/// </summary>
	public class OCRUtilities
	{

		/// <summary>
		/// Output contains all character labels with probabilities. This method
		/// finds the biggest probability and returns character with that
		/// probability.
		/// </summary>
		/// <param name="output"> output from network </param>
		/// <returns> character as string </returns>
		public static string getCharacter(IDictionary<string, double?> output)
		{
			double maxValue = -1;
			KeyValuePair<string, double?> maxElement = null;
			foreach (KeyValuePair<string, double?> element in output)
			{
				if (maxValue < element.Value)
				{
					maxElement = element;
					maxValue = element.Value;
				}
			}
			return maxElement.Key;
		}

		/// <summary>
		/// Find the center of each row measured in pixels.
		/// </summary>
		/// <param name="image"> input image, should be black-white image; </param>
		/// <param name="heightThresh"> the height of the sign (dots or trash) that should not
		/// be recognized as letter </param>
		/// <returns> list with pixel position of each row </returns>
		public static List<int?> rowPositions(BufferedImage image, int heightThresh)
		{
			int[] histogram = Histogram.heightHistogram(image);
			int[] gradient = Histogram.gradient(histogram);
			return linePositions(gradient, heightThresh);
		}

		/// <summary>
		/// Word is class with two parameters, startPixel and endPixel. This method
		/// calculates these pixels for given row and return them as List of Word
		/// </summary>
		/// <param name="image"> input image, should be black-white </param>
		/// <param name="row"> given row </param>
		/// <param name="letterHeight"> predicted letter size </param>
		/// <param name="spaceGap"> predicted space size, spaces smaller that spaceGap are
		/// not spaces between word, they are spaces between letter. Ignore spaces
		/// between letters.
		/// @return </param>
		public static List<WordPosition> wordsPositions(BufferedImage image, int row, int letterHeight, int spaceGap)
		{
			List<WordPosition> words = new List<WordPosition>();
			int[] histogram = OCRHistogram.widthRowHistogram(image, row, letterHeight);
			int[] histogramWLS = OCRHistogram.histogramWithoutLetterSpaces(histogram, spaceGap);

			int count = 0;
			for (int i = 0; i < histogramWLS.Length; i++)
			{
				if (histogramWLS[i] != 0)
				{
					count++;
				} //(histogram[i] == 0) drugim recima vece je od nule
				else
				{
					if (count > 0)
					{
						int start = i - count;
						int end = i - 1;
						WordPosition w = new WordPosition(start, end);
						words.Add(w);
					}
					count = 0;
				}
			}
			return words;
		}



		/// <summary>
		/// Save the image to the file </summary>
		/// <param name="image"> should be cropped before the saving. Use OCRCropImage class </param>
		/// <param name="path"> path to the folder, ie C:/Users/.../ it should ended with / </param>
		/// <param name="letterName"> letter of the name </param>
		/// <param name="extension"> some of .png .jpg ... </param>
		public static void saveToFile(BufferedImage image, string path, string letterName, string extension)
		{
			string imagePath = path + letterName + "." + extension;
			File outputfile = new File(imagePath);
			try
			{
				ImageIO.write(image, extension, outputfile);
			}
			catch (IOException ex)
			{
				Console.WriteLine(ex.ToString());
				Console.Write(ex.StackTrace);
			}
		}

		public static string createImageName(string character)
		{
			int number = character.GetHashCode() * ((new Random()).Next(100));
			return character + "_" + number;
		}

		 /// <param name="gradient"> gradient array calculated with method gradient(int []) </param>
		 /// <param name="ignoredSize"> - noise - what is the minimum size of letter to be
		 /// recognized <br/>
		 /// With lower value you will probably find trash as separate line <br/>
		 /// With higher value you will probably miss the letter <br/>
		 /// Ideal value is less that the letter size </param>
		 /// <returns> List of integers where each element represent center of line.
		 /// First element corresponds to the first line etc. </returns>
		public static List<int?> linePositions(int[] gradient, int ignoredSize)
		{
			List<int?> lines = new List<int?>();
			int sum = 0;
			int count = 0;
			for (int row = 0; row < gradient.Length; row++)
			{

				sum += gradient[row];
				if (sum != 0)
				{
					count++;
					continue;
				}
				if (sum == 0)
				{
					if (count < ignoredSize)
					{
						count = 0;
					} //count >= lineHeightThresh // found line!
					else
					{
						int startLetter = row - count;
						int endLetter = row;
						int line = (startLetter + endLetter) / 2;
						lines.Add(line);
						count = 0;
					}
				}
			}
			return lines;

		}


		public static List<int?> rowHeights(int[] gradient, int ignoredSize)
		{
			 List<int?> heights = new List<int?>();
			int sum = 0;
			int count = 0;
			for (int row = 0; row < gradient.Length; row++)
			{

				sum += gradient[row];
				if (sum != 0)
				{
					count++;
					continue;
				}
				if (sum == 0)
				{
					if (count < ignoredSize)
					{
						count = 0;
					} //count >= lineHeightThresh // found line!
					else
					{
						heights.Add(count);
						count = 0;
					}
				}
			}
			return heights;
		}

	}

}