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
	using org.neuroph.core;
	using ImageRecognitionPlugin = org.neuroph.imgrec.ImageRecognitionPlugin;
	using OCRCropLetter = org.neuroph.ocr.filter.OCRCropLetter;
	using OCRUtilities = org.neuroph.ocr.util.OCRUtilities;
	using WordPosition = org.neuroph.ocr.util.WordPosition;
	using OCRExtractLetter = org.neuroph.ocr.filter.OCRExtractLetter;
	using OCRProperties = org.neuroph.ocr.properties.OCRProperties;

	/// 
	/// <summary>
	/// @author Mihailo Stupar
	/// </summary>
	public class OCRTextRecognition : OCRProperties
	{

		private string recognizedTextPath;
		private NeuralNetwork nnet;
		private ImageRecognitionPlugin plugin;
		private string text;

		private bool[][] visited; //sluzi samo za procesiranje slike, za BFS

		public OCRTextRecognition(Letter letterInformation, Text textInformation) : base(letterInformation, textInformation)
		{
		}

		/// <summary>
		/// Path to the .txt folder where the recognized text will be stored
		/// </summary>
		/// <param name="recognizedTextPath"> </param>
		public virtual string RecognizedTextPath
		{
			set
			{
				this.recognizedTextPath = value;
			}
		}

		/// <param name="nnet"> trained neural network </param>
		public virtual NeuralNetwork NeuralNetwork
		{
			set
			{
				this.nnet = value;
				plugin = (ImageRecognitionPlugin) value.getPlugin(typeof(ImageRecognitionPlugin));
			}
		}

		/// <param name="networkPath"> path of the trained neural network </param>
		public virtual string NetworkPath
		{
			set
			{
				nnet = NeuralNetwork.createFromFile(value);
				plugin = (ImageRecognitionPlugin) nnet.getPlugin(typeof(ImageRecognitionPlugin));
			}
		}

		/// <summary>
		/// recognize the text on the image (document)
		/// </summary>
		public virtual void recognize()
		{
			int imageHeight = image.Height;
			int imageWidth = image.Width;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: visited = new bool[imageHeight][imageWidth];
			visited = RectangularArrays.ReturnRectangularBoolArray(imageHeight, imageWidth);
			text = "";

			for (int i = 0; i < textInformation.numberOfRows(); i++)
			{
				string rowText = recognizeRow(i);
				if (rowText.Length > 0)
				{
					text += rowText + "\n";
				}
			}

			visited = null; //prevent lottering
		}

		private string recognizeRow(int row)
		{
			string rowText = "";
			List<WordPosition> words = textInformation.getWordsAtRow(row);

			for (int i = 0; i < words.Count; i++)
			{
				int rowPixel = textInformation.getRowAt(row);
				WordPosition word = words[i];
				rowText += recognizeWord(word, rowPixel);

				if ((i + 1) == words.Count) //trenutno smo na poslednjoj reci u redu
				{
					rowText += addSpaces(word, null);
				}
				else
				{
					WordPosition next = words[i + 1];
					rowText += addSpaces(word, next);
				}
			}
			return rowText;

		}

		private string recognizeWord(WordPosition word, int rowPixel)
		{
			string wordText = "";


			int tmpWidth = 3 * letterInformation.CropWidth;
			int tmpHeight = 3 * letterInformation.CropHeight;
			int trashsize = letterInformation.TrashSize;

			OCRExtractLetter extractionLetter = new OCRExtractLetter(tmpWidth, tmpHeight, trashsize);

			int letterSize = letterInformation.LetterSize;

			int start = word.StartPixel;
			int end = word.EndPixel;

			Color white = Color.WHITE;
			Color color;
			//======================================================================
			for (int j = start; j < end; j++)
			{
				for (int k = -(letterSize / 4); k < (letterSize / 4); k++)
				{
					int i = rowPixel + k;
					if (i < 0 || i > image.Height)
					{
						continue;
					}

			//======================================================================
	//        gornja vrzija je ispravna ova ne radi kako treba
	//            for (int k = -(letterSize / 4); k < (letterSize / 4); k++) {
	//            int i = rowPixel + k;
	//            if (i < 0 || i > image.getHeight()) {
	//                continue;
	//            }
	//            for (int j = start; j < end; j++) {
			//======================================================================
					color = new Color(image.getRGB(j, i));
					if (color.Equals(white))
					{
						visited[i][j] = true;
					}
					else if (visited[i][j] == false)
					{
						BufferedImage letter = extractionLetter.extraxtLetter(image, visited, i, j); //OCRUtilities.extraxtCharacterImage(image, visited, i, j, tmpWidth, tmpHeight, letterInformation.getTrashSize());
						if (letter != null)
						{
							OCRCropLetter crop = new OCRCropLetter(letter, letterInformation.CropWidth, letterInformation.CropHeight);
							BufferedImage croped = crop.processImage();
							wordText += recognizeLetter(croped);
						}
					}
				}
			}
			return wordText;
		}

		private string recognizeLetter(BufferedImage image)
		{
			// samo za test 
	//        OCRUtilities.saveToFile(image, "C:\\Users\\Mihailo\\Desktop\\OCR\\test-letters", new Random().nextInt()+"", "png");
			//
			IDictionary<string, double?> output = plugin.recognizeImage(image);
			return OCRUtilities.getCharacter(output);
		}

		private string addSpaces(WordPosition first, WordPosition second)
		{
			if (second == null)
			{
				return "";
			}
			string space = "";
			int gap = second.StartPixel - first.EndPixel;
			int num = gap / letterInformation.SpaceGap;

			for (int i = 0; i < num; i++)
			{
				space += " ";
			}
			return space;
		}

		/// <returns> recognized text from the image </returns>
		public virtual string RecognizedText
		{
			get
			{
				return text;
			}
		}

		/// <summary>
		/// save the recognized text to the file specified earlier in location folder
		/// </summary>
		public virtual void saveText()
		{
			try
			{
				File file = new File(recognizedTextPath);
				if (!file.exists())
				{
					file.createNewFile();
				}
				string[] lines = text.Split("\n", true);
				FileWriter fw = new FileWriter(file.AbsoluteFile);
				BufferedWriter bw = new BufferedWriter(fw);
				foreach (string line in lines)
				{
					bw.write(line);
					bw.newLine();
				}
				bw.close();
			}
			catch (IOException ex)
			{
				Console.WriteLine(ex.ToString());
				Console.Write(ex.StackTrace);
			}

		}

	}

}