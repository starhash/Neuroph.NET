using System;
using System.Collections.Generic;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
namespace org.neuroph.ocr.samples
{

	using org.neuroph.core;
	using ImageRecognitionPlugin = org.neuroph.imgrec.ImageRecognitionPlugin;
	using OCRUtilities = org.neuroph.ocr.util.OCRUtilities;

	/// 
	/// <summary>
	/// @author Mihailo Stupar
	/// </summary>
	public class RecognizeLetter
	{

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws java.io.IOException
		public static void Main(string[] args)
		{

			// User input parameters
	//***********************************************************************************************************************************
			string networkPath = "C:/Users/Mihailo/Desktop/OCR/nnet/nnet-12-0.01.nnet"; // path to the trained network                *
			string letterPath = "C:/Users/Mihailo/Desktop/OCR/letters/259.png"; // path to the letter for recognition                   *
	//***********************************************************************************************************************************

			NeuralNetwork nnet = NeuralNetwork.createFromFile(networkPath);
			ImageRecognitionPlugin imageRecognition = (ImageRecognitionPlugin) nnet.getPlugin(typeof(ImageRecognitionPlugin));
			IDictionary<string, double?> output = imageRecognition.recognizeImage(new File(letterPath));
			Console.WriteLine("Recognized letter: " + OCRUtilities.getCharacter(output));

		}

	}

}