using System;
using System.Collections.Generic;

namespace org.neuroph.contrib.licenceplaterecognition
{

	using CharacterExtractor = net.sourceforge.javaocr.ocrPlugins.CharacterExtractor;
	using org.neuroph.core;
	using ColorMode = org.neuroph.imgrec.ColorMode;
	using Dimension = org.neuroph.imgrec.image.Dimension;
	using ImageJ2SE = org.neuroph.imgrec.image.ImageJ2SE;
	using OcrPlugin = org.neuroph.ocr.OcrPlugin;


	/// <summary>
	/// Problemi: 1. Image i ImageJ2SE. BufferedImage 2. Problem ako su slova blizu
	/// ili ako je pozadina muljava CharExtractor ne radi kako treba resenje:
	/// omoguciti setovanje boje pozadine i slova CharExtractor -u i dozvoliti
	/// izvesnu toleranciju odnosno odstupanje od tih vrednosti, koje se takodje moze
	/// setovati Napravljeno resenje: slika se preprocesira sa graysacla i binarize i
	/// to onda radi; testirati threshold
	/// 
	/// 3. Dokumentovati OCR i image recognition API 4. Napisati neki kratak
	/// tutorijal uz ovaj demo primer
	/// 
	/// Sledeci korak: resiti crticu i sve znake manje od slova po visini
	/// 
	/// @author zoran
	/// </summary>
	public class OcrDemo
	{



		/// <summary>
		/// Image file with text to recognize
		/// </summary>
		private string textImageFile = "data/tablica.jpg";
		/// <summary>
		/// Image with all the font letters
		/// </summary>
		private string datasetImageFile = "data/svaslova.jpg";
		/// <summary>
		/// Trained neural network file created with OCR wizard from Neuroph Studio
		/// </summary>
		private string neuralNetworkFile = "data/mrezica.nnet";
		/*
		 * Output directory for dataset (individual letters)
		 */
		private string datasetOutputFile = "data/dataset";
		/// <summary>
		/// Location for storing extracted character images
		/// </summary>
		private string charOutputFile = "data";
		/// <summary>
		/// Trained neural network
		/// </summary>

		private NeuralNetwork nnet;

		/// <summary>
		/// Image with licence plate to recognize
		/// </summary>
		private BufferedImage image;

		/// <summary>
		/// String to store recognized characters;
		/// </summary>
		private string recognizedCharacters = "";


		 public OcrDemo()
		 {
		 }

		public OcrDemo(BufferedImage licencePlateImage, NeuralNetwork neuralNetwork)
		{
			image = licencePlateImage;
			nnet = neuralNetwork;

		}

		public virtual string RecognizedCharacters
		{
			get
			{
				return recognizedCharacters;
			}
		}

		/// <summary>
		/// Crop the part of an image with a white rectangle
		/// </summary>
		/// <returns> A cropped image File </returns>
		public virtual File crop(BufferedImage image)
		{
			// this will be coordinates of the upper left white pixel
			int upperLeftCornerx = int.MaxValue;
			int upperLeftCornery = int.MaxValue;
			//this will be coordinates of the lower right white pixel
			int lowerRightCornerx = int.MinValue;
			int lowerRightCornery = int.MinValue;
			//find the minimum and maximum white pixel coordinates
			for (int i = 0; i < image.Width; i++)
			{
				for (int j = 0; j < image.Height; j++)
				{
					if (image.getRGB(i, j) == WHITE.RGB && (i < upperLeftCornerx && j < upperLeftCornery) || (i <= upperLeftCornerx && j < upperLeftCornery) || (i < upperLeftCornerx && j <= upperLeftCornery))
					{
						upperLeftCornerx = i;
						upperLeftCornery = j;
					}
					if (image.getRGB(i, j) == WHITE.RGB && ((i > lowerRightCornerx && j >= lowerRightCornery) || (i >= lowerRightCornerx && j > lowerRightCornery) || (i > lowerRightCornerx && j >= lowerRightCornery)))
					{
						lowerRightCornerx = i;
						lowerRightCornery = j;
					}
				}
			}
			//crop the image to the white rectangle size
			BufferedImage croppedImage = image.getSubimage(upperLeftCornerx, upperLeftCornery, lowerRightCornerx - upperLeftCornerx, lowerRightCornery - upperLeftCornery);
		   //make a file from that cropped image
			File cropFile = new File("croppedimage.png");
			try
			{
				ImageIO.write(croppedImage, "png", cropFile);
			}
			catch (IOException ex)
			{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
				Logger.getLogger(typeof(OcrDemo).FullName).log(Level.SEVERE, null, ex);
			}
			return cropFile;
		}

		public virtual void run()
		{
			try
			{

				// load image with text to recognize
				if (image == null)
				{
				 image = ImageIO.read(new File(textImageFile));
				}
				//binarize the input image
				image = BinaryOps.binary(textImageFile);

				//dataset creation 
				/// <summary>
				/// CharacterExtractor ce1 = new CharacterExtractor(); File
				/// inputImage1 = new File(datasetImageFile); File outputDirectory1 =
				/// new File (datasetOutputFile); ce1.slice(inputImage1,
				/// outputDirectory1, 60, 60);
				/// </summary>
				// crop the white rectange from the image
				File cropFile = crop(image);

				// extract individual characters from text image
				CharacterExtractor ce = new CharacterExtractor();

			   //make the output file
				File outputDirectory = new File(charOutputFile);
				//slice the cropped file to individual character with the width and height of 60px
				ce.slice(cropFile, outputDirectory, 60, 60);

				//make a list of character images and add the images form char files
				List<BufferedImage> lista = new List<BufferedImage>();
				for (int i = 0; i <= 7; i++)
				{
					File f = new File("data/char_" + i + ".png");
					BufferedImage bi = ImageIO.read(f);
					lista.Add(bi);
				}

				// load neural network from file
			   if (nnet == null)
			   {
				NeuralNetwork nnet = NeuralNetwork.createFromFile(neuralNetworkFile);
			   }
				// get ocr plugin from neural network
				nnet.addPlugin(new OcrPlugin(new Dimension(10, 10), ColorMode.BLACK_AND_WHITE));
				OcrPlugin ocrPlugin = (OcrPlugin) nnet.getPlugin(typeof(OcrPlugin));

				// and recognize current character - ( have to use ImageJ2SE here to wrap BufferedImage)
				for (int i = 0; i < lista.Count; i++)
				{
					recognizedCharacters += ocrPlugin.recognizeCharacter(new ImageJ2SE(lista[i])) + " ";
					Console.Write(ocrPlugin.recognizeCharacter(new ImageJ2SE(lista[i])) + " ");
				}
				recognizedCharacters.Trim();
			}
			catch (IOException e)
			{
				//Let us know what happened  
				Console.WriteLine("Error reading dir: " + e.Message);
			}

		}

		public static void Main(string[] args)
		{
			(new OcrDemo()).run();
		}
	}

}