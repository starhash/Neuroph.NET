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
	using DataSet = org.neuroph.core.data.DataSet;
	using ColorMode = org.neuroph.imgrec.ColorMode;
	using FractionRgbData = org.neuroph.imgrec.FractionRgbData;
	using ImageRecognitionHelper = org.neuroph.imgrec.ImageRecognitionHelper;
	using ImageFilterChain = org.neuroph.imgrec.filter.ImageFilterChain;
	using GrayscaleFilter = org.neuroph.imgrec.filter.impl.GrayscaleFilter;
	using OtsuBinarizeFilter = org.neuroph.imgrec.filter.impl.OtsuBinarizeFilter;
	using Dimension = org.neuroph.imgrec.image.Dimension;
	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;
	using BackPropagation = org.neuroph.nnet.learning.BackPropagation;
	using Letter = org.neuroph.ocr.util.Letter;
	using Text = org.neuroph.ocr.util.Text;
	using TransferFunctionType = org.neuroph.util.TransferFunctionType;

	/// 
	/// <summary>
	/// @author Mihailo Stupar
	/// </summary>
	public class TrainingSample
	{

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws java.io.IOException
		public static void Main(string[] args)
		{

			//     User input parameteres       
	//*******************************************************************************************************************************       
			string imagePath = "C:/Users/Mihailo/Desktop/OCR/slova.png"; //path to the image with letters                        *
			string folderPath = "C:/Users/Mihailo/Desktop/OCR/ImagesDir/"; // loaction folder for storing segmented letters           *
			string textPath = "C:/Users/Mihailo/Desktop/OCR/slova.txt"; // path to the .txt file with text on the image          *
			string networkPath = "C:/Users/Mihailo/Desktop/OCR/network.nnet"; // location where the network will be stored     *
			int fontSize = 12; // fontSize, predicted by height of the letters, minimum font size is 12 pt                          *
			int scanQuality = 300; // scan quality, minimum quality is 300 dpi                                                      *
	//*******************************************************************************************************************************

			BufferedImage image = ImageIO.read(new File(imagePath));
			ImageFilterChain chain = new ImageFilterChain();
			chain.addFilter(new GrayscaleFilter());
			chain.addFilter(new OtsuBinarizeFilter());
			BufferedImage binarizedImage = chain.processImage(image);





			Letter letterInfo = new Letter(scanQuality, binarizedImage);
	//        letterInfo.recognizeDots(); // call this method only if you want to recognize dots and other litle characters, TODO

			Text texTInfo = new Text(binarizedImage, letterInfo);

			OCRTraining ocrTraining = new OCRTraining(letterInfo, texTInfo);
			ocrTraining.FolderPath = folderPath;
			ocrTraining.TrainingTextPath = textPath;
			ocrTraining.prepareTrainingSet();



			List<string> characterLabels = ocrTraining.CharacterLabels;

			IDictionary<string, FractionRgbData> map = ImageRecognitionHelper.getFractionRgbDataForDirectory(new File(folderPath), new Dimension(20, 20));
			DataSet dataSet = ImageRecognitionHelper.createBlackAndWhiteTrainingSet(characterLabels, map);


			dataSet.FilePath = "C:/Users/Mihailo/Desktop/OCR/DataSet1.tset";
			dataSet.save();


			List<int?> hiddenLayers = new List<int?>();
			hiddenLayers.Add(12);

			NeuralNetwork nnet = ImageRecognitionHelper.createNewNeuralNetwork("someNetworkName", new Dimension(20, 20), ColorMode.BLACK_AND_WHITE, characterLabels, hiddenLayers, TransferFunctionType.SIGMOID);
			BackPropagation bp = (BackPropagation) nnet.LearningRule;
			bp.LearningRate = 0.3;
			bp.MaxError = 0.1;


	//        MultiLayerPerceptron mlp = new MultiLayerPerceptron(12,13);
	//        mlp.setOutputNeurons(null);

			Console.WriteLine("Start learning...");
			nnet.learn(dataSet);
			Console.WriteLine("NNet learned");

			nnet.save(networkPath);

		}

	}

}