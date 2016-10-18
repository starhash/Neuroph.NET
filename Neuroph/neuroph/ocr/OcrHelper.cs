using System.Collections.Generic;

/// <summary>
/// Copyright 2010 Neuroph Project http://neuroph.sourceforge.net
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
/// 
///    http://www.apache.org/licenses/LICENSE-2.0
/// 
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// </summary>

namespace org.neuroph.ocr
{

	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;
	using ColorMode = org.neuroph.imgrec.ColorMode;
	using FractionRgbData = org.neuroph.imgrec.FractionRgbData;
	using ImageRecognitionHelper = org.neuroph.imgrec.ImageRecognitionHelper;
	using ImageUtilities = org.neuroph.imgrec.ImageUtilities;
	using Dimension = org.neuroph.imgrec.image.Dimension;
	using ImageJ2SE = org.neuroph.imgrec.image.ImageJ2SE;
	using TransferFunctionType = org.neuroph.util.TransferFunctionType;


	/// <summary>
	/// Provides methods to create Neural Network and Training set for OCR.
	/// @author zoran
	/// </summary>
	public class OcrHelper : ImageRecognitionHelper
	{

		/// <summary>
		/// Creates neural network for OCR, which contains OCR plugin. OCR plugin provides interface for character recognition. </summary>
		/// <param name="label"> neural network label </param>
		/// <param name="samplingResolution"> character size in pixels (all characters will be scaled to this dimensions during recognition) </param>
		/// <param name="colorMode"> color mode used fr recognition </param>
		/// <param name="characterLabels"> character labels for output neurons </param>
		/// <param name="layersNeuronsCount"> number of neurons ih hidden layers </param>
		/// <param name="transferFunctionType"> neurons transfer function type </param>
		/// <returns> returns NeuralNetwork with the OCR plugin </returns>
		public static NeuralNetwork createNewNeuralNetwork(string label, Dimension samplingResolution, ColorMode colorMode, List<string> characterLabels, List<int?> layersNeuronsCount, TransferFunctionType transferFunctionType)
		{
			NeuralNetwork neuralNetwork = ImageRecognitionHelper.createNewNeuralNetwork(label, samplingResolution, colorMode, characterLabels, layersNeuronsCount, transferFunctionType);
			neuralNetwork.addPlugin(new OcrPlugin(samplingResolution, colorMode));

			return neuralNetwork;
		}

	   /// <summary>
	   /// Create training set 
	   /// </summary>
	   /// <param name="imageWithChars"> </param>
	   /// <param name="chars"> </param>
	   /// <param name="scaleToDim"> </param>
	   /// <param name="trainingSetName">  </param>
		public static DataSet createTrainingSet(string trainingSetName, BufferedImage imageWithChars, string chars, Dimension scaleToDim, List<string> imageLabels)
		{

			// convert chars from string to list 
			List<string> charList = Arrays.asList(chars.Split(" ", true)); // izgleda da ovo zeza...
			// extract individual char images which will be used to create training set
			CharExtractor charExtractor = new CharExtractor(imageWithChars);
			Dictionary<string, BufferedImage> charImageMap = charExtractor.extractCharImagesToLearn(imageWithChars, charList, scaleToDim);


		   // prepare image labels (we need them to label output neurons...)
		   // ArrayList<String> imageLabels = new ArrayList<String>();   
			foreach (string imgName in charImageMap.Keys)
			{
				StringTokenizer st = new StringTokenizer(imgName, "._");
				string imgLabel = st.nextToken();
				if (!imageLabels.Contains(imgLabel)) // check for duplicates ...
				{
					imageLabels.Add(imgLabel);
				}
			}
			imageLabels.Sort();

			// get RGB image data - map chars and their their rgb data
			IDictionary<string, FractionRgbData> imageRgbData = ImageUtilities.getFractionRgbDataForImages(charImageMap);

			// also put junk all black and white image in training set (for black n whit emode)
			BufferedImage allWhite = new BufferedImage(scaleToDim.Width, scaleToDim.Height, BufferedImage.TYPE_INT_RGB);
			Graphics g = allWhite.Graphics;
			g.Color = Color.WHITE;
			g.fillRect(0, 0, allWhite.Width, allWhite.Height);
			imageRgbData["allWhite"] = new FractionRgbData(allWhite);

	//        BufferedImage allBlack = new BufferedImage(charDimension.getWidth(), charDimension.getHeight(), BufferedImage.TYPE_INT_RGB);
	//        g = allBlack.getGraphics();
	//        g.setColor(Color.BLACK);
	//        g.fillRect(0, 0, allBlack.getWidth(), allBlack.getHeight());
	//        imageRgbData.put("allBlack", new FractionRgbData(allBlack));        

			// put junk images (all red, blue, green) to avoid errors (used for full color mode)
	//        BufferedImage allRed = new BufferedImage(charDimension.getWidth(), charDimension.getHeight(), BufferedImage.TYPE_INT_RGB);
	//        Graphics g = allRed.getGraphics();
	//        g.setColor(Color.RED);
	//        g.fillRect(0, 0, allRed.getWidth(), allRed.getHeight());
	//        imageRgbData.put("allRed", new FractionRgbData(allRed));        
	//        
	//        BufferedImage allBlue = new BufferedImage(charDimension.getWidth(), charDimension.getHeight(), BufferedImage.TYPE_INT_RGB);
	//        g = allBlue.getGraphics(); 
	//        g.setColor(Color.BLUE);
	//        g.fillRect(0, 0, allBlue.getWidth(), allBlue.getHeight());
	//        imageRgbData.put("allBlue", new FractionRgbData(allBlue));        
	//        
	//        BufferedImage allGreen = new BufferedImage(charDimension.getWidth(), charDimension.getHeight(), BufferedImage.TYPE_INT_RGB);
	//        g = allGreen.getGraphics(); 
	//        g.setColor(Color.GREEN);
	//        g.fillRect(0, 0, allGreen.getWidth(), allGreen.getHeight());
	//        imageRgbData.put("allGreen", new FractionRgbData(allGreen));                

			// create training set using image rgb data
			DataSet dataSet = ImageRecognitionHelper.createBlackAndWhiteTrainingSet(imageLabels, imageRgbData);
			 //DataSet dataSet = ImageRecognitionHelper.createTrainingSet(this.imageLabels, imageRgbData);
			dataSet.Label = trainingSetName;

			return dataSet;
		}

		/// <summary>
		/// Recognize characters in given text images and returns character list </summary>
		/// <param name="neuralNet"> </param>
		/// <param name="image"> </param>
		/// <param name="charDimension"> </param>
		/// <returns>  </returns>
		public static List<char?> recognizeText(NeuralNetwork neuralNet, BufferedImage image, Dimension charDimension)
		{
			CharExtractor charExtractor = new CharExtractor(image);
			List<BufferedImage> charImages = charExtractor.extractCharImagesToRecognize();
			List<char?> characters = recognize(neuralNet, charImages, charDimension);
			return characters;
		}


		public static List<char?> recognize(NeuralNetwork nnet, List<BufferedImage> images, Dimension charDimension)
		{
			OcrPlugin ocrPlugin = (OcrPlugin) nnet.getPlugin(typeof(OcrPlugin));
			List<char?> letters = new List<char?>();

			foreach (BufferedImage img in images)
			{
				char? letter = ocrPlugin.recognizeCharacter(new ImageJ2SE(img), charDimension);
				letters.Add(letter);
			}
			return letters;
		}


	}
}