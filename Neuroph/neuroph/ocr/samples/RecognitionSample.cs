using System;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
namespace org.neuroph.ocr.samples
{

	using ImageFilterChain = org.neuroph.imgrec.filter.ImageFilterChain;
	using GrayscaleFilter = org.neuroph.imgrec.filter.impl.GrayscaleFilter;
	using OtsuBinarizeFilter = org.neuroph.imgrec.filter.impl.OtsuBinarizeFilter;
	using Letter = org.neuroph.ocr.util.Letter;
	using Text = org.neuroph.ocr.util.Text;

	/// 
	/// <summary>
	/// @author Mihailo
	/// </summary>
	public class RecognitionSample
	{

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws java.io.IOException
		public static void Main(string[] args)
		{

			// User input parameters
	//*******************************************************************************************************************************************       
			string imagePath = "C:/Users/Mihailo/Desktop/OCR/tekst.png"; //path to the image with letters (document) for recognition
			string textPath = "C:/Users/Mihailo/Desktop/OCR/tekst.txt"; // path to the .txt file where the recognized text will be stored
			string networkPath = "C:/Users/Mihailo/Desktop/OCR/network.nnet"; // locatoin of the trained network
			int fontSize = 12; // fontSize, predicted by height of the letters, minimum font size is 12 pt
			int scanQuality = 300; // scan quality, minimum quality is 300 dpi
	//*******************************************************************************************************************************************

			BufferedImage image = ImageIO.read(new File(imagePath));
			ImageFilterChain chain = new ImageFilterChain();
			chain.addFilter(new GrayscaleFilter());
			chain.addFilter(new OtsuBinarizeFilter());
			BufferedImage binarizedImage = chain.processImage(image);

			// Information about letters and text
			Letter letterInfo = new Letter(scanQuality, binarizedImage);
	//        letterInfo.recognizeDots(); // call this method only if you want to recognize dots and other litle characters, TODO
			Text textInfo = new Text(binarizedImage, letterInfo);

			OCRTextRecognition recognition = new OCRTextRecognition(letterInfo, textInfo);

			recognition.NetworkPath = networkPath;

			recognition.recognize();

			//if you want to save recognized text
	//        recognition.setRecognizedTextPath(textPath); 
	//        recognition.saveText();

			Console.WriteLine(recognition.RecognizedText);
		}

	}

}