using System.Collections.Generic;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
namespace org.neuroph.ocr
{

	using Letter = org.neuroph.ocr.util.Letter;
	using Text = org.neuroph.ocr.util.Text;
	using OCRCropLetter = org.neuroph.ocr.filter.OCRCropLetter;
	using OCRExtractLetter = org.neuroph.ocr.filter.OCRExtractLetter;
	using OCRProperties = org.neuroph.ocr.properties.OCRProperties;
	using OCRUtilities = org.neuroph.ocr.util.OCRUtilities;

	/// 
	/// <summary>
	/// @author Mihailo Stupar
	/// </summary>
	public class OCRTraining : OCRProperties
	{

		private string folderPath;
		private string trainingText;
		private string imageExtension;

		private List<string> characterLabels;

		public OCRTraining(Letter letterInformation, Text textInformation) : base(letterInformation, textInformation)
		{
			imageExtension = "png";
		}

		/// <summary>
		/// Path to the text (.txt file) with letters that corresponds to the letters
		/// on the image
		/// </summary>
		/// <param name="trainingTextPath"> path to the .txt file </param>
		public virtual string TrainingTextPath
		{
			set
			{
				try
				{
					Path path = FileSystems.Default.getPath(value);
					trainingText = new string(Files.readAllBytes(path));
				}
				catch (IOException ex)
				{
					Console.WriteLine(ex.ToString());
					Console.Write(ex.StackTrace);
				}
			}
		}



		/// <summary>
		/// Path to the folder where the images (each with the single letter) will be
		/// stored
		/// 
		/// <P>
		/// Example: "C:/Users...../OCR/"</p>
		/// <para>
		/// Important thing is character '/' at the end of string</para>
		/// </summary>
		/// <param name="lettersPath"> path to the folder </param>
		public virtual string FolderPath
		{
			set
			{
				this.folderPath = value;
    
			}
			get
			{
				return folderPath;
			}
		}

		/// <summary>
		/// Extension of the images with letters. Ie: png, jpg
		/// </summary>
		/// <param name="imageExtension"> imageExtension of images </param>
		public virtual string ImageExtension
		{
			set
			{
				this.imageExtension = value;
			}
		}

		/// <summary>
		/// Text used for training. It should be formated as image.
		/// <para>
		/// Recommendations:</para>
		/// <para>
		/// -Use space, ie A B C a b c</para>
		/// <para>
		/// -Don't use A A A A, letters should be shaken</para>
		/// <para>
		/// -Use equal number of repetitions of each letter, as much as you can</para>
		/// </summary>
		/// <returns> text for training </returns>
		public virtual string TrainingText
		{
			get
			{
				return trainingText;
			}
			set
			{
				this.trainingText = value;
			}
		}



		/// <summary>
		/// Unique characters in the list. Extracted from the training text.
		/// Characters are represented as string
		/// </summary>
		/// <returns> list of characters (unique) </returns>
		public virtual List<string> CharacterLabels
		{
			get
			{
				return characterLabels;
			}
		}

		/// <summary>
		/// <para>
		/// 1. create character labels using text for training</para>
		/// <para>
		/// 2. create images, each with single letter and named correctly</para>
		/// <para>
		/// 3. store these image to the folder set before</para>
		/// </summary>
		public virtual void prepareTrainingSet()
		{
			prepateText();
			createCharacterLabels();
			createImagesWithLetters();
		}

		//=========================================================================
		private void createCharacterLabels()
		{
			characterLabels = new List<string>();
			for (int i = 0; i < trainingText.Length; i++)
			{
				string c = trainingText[i] + "";
				if (!characterLabels.Contains(c))
				{
					characterLabels.Add(trainingText[i] + "");
				}
			}
		}

		private void prepateText()
		{
			string tmp = "";
			for (int i = 0; i < trainingText.Length; i++)
			{
				char c = trainingText[i];
				if ((!char.isSpaceChar(c)) && (!char.IsWhiteSpace(c)))
				{
					tmp += c;
				}
			}
			trainingText = tmp;
		}

		private void createImagesWithLetters()
		{
			int cropWidth = letterInformation.CropWidth;
			int cropHeight = letterInformation.CropHeight;
			int tmpWidth = 3 * cropWidth;
			int tmpHeight = 3 * cropHeight;
			int trashSize = letterInformation.TrashSize;

			OCRExtractLetter extractionLetter = new OCRExtractLetter(tmpWidth, tmpHeight, trashSize);

			int letterSize = letterInformation.LetterSize;
			int imageHeight = image.Height;
			int imageWidth = image.Width;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: bool[][] visited = new bool[imageHeight][imageWidth];
			bool[][] visited = RectangularArrays.ReturnRectangularBoolArray(imageHeight, imageWidth);
			Color white = Color.WHITE;
			Color color;
			int seqNum = 0;

			for (int line = 0; line < textInformation.numberOfRows(); line++)
			{

	//==============================================================================
				for (int j = 0; j < imageWidth; j++)
				{
					for (int k = -(letterSize / 4); k < (letterSize / 4); k++)
					{
						int rowPixel = textInformation.getRowAt(line);
						int i = rowPixel + k;
						if (i < 0 || i >= imageHeight)
						{
							continue;
						}
	//==============================================================================
	//                   fornja verzija radi, ova ima gresku 
	//            for (int k = -(letterSize / 4); k < (letterSize / 4); k++) {
	//                int rowPixel = textInformation.getRowAt(line);
	//                int i = rowPixel + k;
	//                if (i < 0 || i >= imageHeight) {
	//                    continue;
	//                }
	//                for (int j = 0; j < imageWidth; j++) {
	//==============================================================================
						color = new Color(image.getRGB(j, i));
						if (color.Equals(white))
						{
							visited[i][j] = true;
						}
						else if (visited[i][j] == false)
						{
							BufferedImage letter = extractionLetter.extraxtLetter(image, visited, i, j); // OCRUtilities.extraxtCharacterImage(image, visited, i, j, tmpWidth, tmpHeight, letterInformation.getTrashSize());
							if (letter != null)
							{
								OCRCropLetter crop = new OCRCropLetter(letter, cropWidth, cropHeight);
								BufferedImage croped = crop.processImage();
								string character = trainingText[seqNum] + "";
								string name = character + "_" + seqNum;
								OCRUtilities.saveToFile(croped, folderPath, name, imageExtension);
								seqNum++;
							}
						}

					}
				}

			}

		}

	}

}