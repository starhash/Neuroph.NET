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
	public class OCRSeparationFilter : ImageFilter
	{

		private BufferedImage originalImage;
		private int width;
		private int height;

		private int cropHeight;
		private int cropWidth;

		private bool[][] visited;

		private int letterWidth;
		private int letterHeight;
		private string location;

		private int[] counts;

		private int[] linePositions = null;
	//    private ArrayList<String> letterLabels;

		private string text;
		private int seqNum = 0;



		public OCRSeparationFilter()
		{
			letterWidth = 0;
			letterHeight = 0;
	//        letterLabels = new ArrayList<>(); 
			cropHeight = 0;
			cropWidth = 0;
		}

		public virtual BufferedImage processImage(BufferedImage image)
		{

			originalImage = image;
			width = originalImage.Width;
			height = originalImage.Height;

			prepare();

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: visited = new bool[height][width];
			visited = RectangularArrays.ReturnRectangularBoolArray(height, width);

			int color;
			int white = 255;

			for (int line = 0; line < linePositions.Length; line++)
			{
				for (int k = -1; k <= 1; k++)
				{
					int i = linePositions[line] + k;
					if (i == -1 || i == height)
					{
						continue;
					}
	//        for (int i = 0; i < height; i++) {

					for (int j = 0; j < width; j++)
					{

						color = (new Color(originalImage.getRGB(j, i))).Red;
						if (color == white)
						{
							visited[i][j] = true;
						}
						else
						{
							BFStraverseAndSave(i, j);

						}

					}
				}
			}
			return originalImage;
		}

		private void BFStraverseAndSave(int startI, int startJ)
		{

			int gapWidth = letterWidth / 5 * 2; //start x coordinate of letter, 2/5 itended
			int gapHeight = letterHeight / 5 * 2; //start y coordinate of letter

			LinkedList<string> queue = new LinkedList<string>();

			BufferedImage letter = new BufferedImage(letterWidth, letterHeight, originalImage.Type);
			int alpha = (new Color(originalImage.getRGB(startJ, startI))).Alpha;
			int white = ImageUtilities.colorToRGB(alpha, 255, 255, 255);
			int black = ImageUtilities.colorToRGB(alpha, 0, 0, 0);

			// fill all letter image with white pixels
			for (int i = 0; i < letterHeight; i++)
			{
				for (int j = 0; j < letterWidth; j++)
				{
					letter.setRGB(j, i, white);
				}
			}

			int countPixels = 0; // ignore dots
			string positions = startI + " " + startJ;
			visited[startI][startJ] = true;
			queue.AddLast(positions);
			while (queue.Count > 0)
			{
				string pos = queue.RemoveFirst();
				string[] posArray = pos.Split(" ", true);
				int H = Convert.ToInt32(posArray[0]); // H-height
				int W = Convert.ToInt32(posArray[1]); // W-width
				visited[H][W] = true;

				int posW = W - startJ + gapWidth;
				int posH = H - startI + gapHeight;

				countPixels++;

				letter.setRGB(posW, posH, black);

				int color;
				int blackInt = 0;
				for (int i = H - 1; i <= H + 1; i++)
				{
					for (int j = W - 1; j <= W + 1; j++)
					{
						if (i >= 0 && j >= 0 && i < originalImage.Height && j < originalImage.Width)
						{
							if (!visited[i][j])
							{
								color = (new Color(originalImage.getRGB(j, i))).Red;
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

			if (countPixels < 35) //da ne bi uzimao male crtice, tacke
			{
				return;
			}

			string name = createName();
			saveToFile(letter, name); //potrebno je izbaciti seqNum i ostaviti samo name

			seqNum++;

		}

		private void saveToFile(BufferedImage img, string letterName)
		{
			File outputfile = new File(location + letterName + ".png");
			BufferedImage crop = img;
			if (cropHeight != 0 || cropWidth != 0)
			{
				OCRCropImage ci = new OCRCropImage();
				ci.setDimension(cropWidth, cropHeight);
				crop = ci.processImage(img);
			}


			try
			{
				ImageIO.write(crop, "png", outputfile);
			}
			catch (IOException ex)
			{
				Console.WriteLine(ex.ToString());
				Console.Write(ex.StackTrace);
			}
		}

		/// <summary>
		///  pretopstavka da s ekoriste samo slova, mala i velika
		///  26 mali i 26 velikih, zato je counts[52]
		/// </summary>
		private void prepare()
		{
			counts = new int[52];
			for (int i = 0; i < counts.Length; i++)
			{
				counts[i] = 1;
			}
			string pom = "";
			for (int i = 0; i < text.Length; i++)
			{
				if (char.IsLetter(text[i]))
				{
					pom += text[i];
				}
			}
			text = pom;
			//====================================================
			// ako nije setovan linepostions proci kroz sve linije
			if (linePositions == null)
			{
				linePositions = new int[height];

				for (int i = 0; i < linePositions.Length; i++)
				{
					linePositions[i] = i;
				}
			}

		}


		/// <summary>
		/// trenutno radi samo sa slovima, malim i velikim
		/// promeniti da prepoznaje i druge karaktere </summary>
		/// <returns> naziv slova, npr A ili c </returns>
		private string createName()
		{

			int offsetBIG = 65;
			int offsetSMALL = 97;
			int offsetARRAY = 26;

			char c = text[seqNum];
			int key = c;
	//        System.out.println(key+" "+c);
			int number;
			if (key < 95) //smallLetter
			{
				number = counts[key - offsetBIG];
				counts[key - offsetBIG]++;
			} //big letter
			else
			{
				number = counts[key - offsetSMALL + offsetARRAY];
				counts[key - offsetSMALL + offsetARRAY]++;
			}
			string name = c + "_" + number;
	//        letterLabels.add(c+"");
			return name;
		}

		public virtual int[] LinePositions
		{
			set
			{
				this.linePositions = value;
			}
		}

	//    public ArrayList<String> getLetterLabels() {
	//        return letterLabels;
	//    }

		/// <summary>
		/// The dimension of the image with a single letter
		/// Letter will be in the center of the image
		/// If the dimension is too small, the letter will be cropped
		/// treba dodati preporucene velicine za svaki font </summary>
		/// <param name="cropHeight"> </param>
		/// <param name="cropWidth">  </param>
		public virtual void setDimension(int cropHeight, int cropWidth)
		{
			this.cropHeight = cropHeight;
			this.cropWidth = cropWidth;
			letterWidth = 3 * cropWidth;
			letterHeight = 3 * cropHeight;
		}

		/// <param name="location"> Location path/folder where the images with letters
		/// will be saved </param>
		public virtual string LocationFolder
		{
			set
			{
				this.location = value;
			}
		}


		/// <summary>
		/// The text that corresponds to the text on image
		/// Used for name of each image </summary>
		/// <param name="text">  </param>
		public virtual string Text
		{
			set
			{
				this.text = value;
			}
		}



	}

}