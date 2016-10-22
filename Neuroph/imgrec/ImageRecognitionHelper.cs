using System;
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

namespace org.neuroph.imgrec
{

	using org.neuroph.core;
	using Neuron = org.neuroph.core.Neuron;
	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;
	using VectorSizeMismatchException = org.neuroph.core.exceptions.VectorSizeMismatchException;
	using Dimension = org.neuroph.imgrec.image.Dimension;
	using Image = org.neuroph.imgrec.image.Image;
	using ImageFactory = org.neuroph.imgrec.image.ImageFactory;
	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;
	using MomentumBackpropagation = org.neuroph.nnet.learning.MomentumBackpropagation;
	using TransferFunctionType = org.neuroph.util.TransferFunctionType;
	using PluginBase = org.neuroph.util.plugins.PluginBase;

	/// <summary>
	/// Provides methods to create neural network and training set for image recognition.
	/// This class is mostly based on the code from tileclassification utility by Jon Tait
	/// @author Jon Tait
	/// @author Zoran Sevarac
	/// </summary>
	public class ImageRecognitionHelper
	{

		/// <summary>
		/// Creates and returns new neural network for image recognition.
		/// Assumes that all of the FractionRgbData objects in the given map have identical 
		/// length arrays in them so that the input layer of the neural network can be 
		/// created here.
		/// </summary>
		/// <param name="label"> neural network label </param>
		/// <param name="samplingResolution"> sampling resolution (image size) </param>
		/// <param name="imageLabels"> image labels </param>
		/// <param name="layersNeuronsCount"> neuron counts in hidden layers </param>
		/// <param name="transferFunctionType"> type of transfer function to use for neurons in network </param>
		/// <param name="colorMode"> color mode </param>
		/// <returns>  </returns>
		public static NeuralNetwork createNewNeuralNetwork(string label, Dimension samplingResolution, ColorMode colorMode, List<string> imageLabels, List<int?> layersNeuronsCount, TransferFunctionType transferFunctionType)
		{

					int numberOfInputNeurons;
					if ((colorMode == ColorMode.COLOR_RGB) || (colorMode == ColorMode.COLOR_HSL)) // for full color rgb or hsl
					{
						numberOfInputNeurons = 3 * samplingResolution.Width * samplingResolution.Height;
					} // for black n white network
					else
					{
						numberOfInputNeurons = samplingResolution.Width * samplingResolution.Height;
					}

					int numberOfOuputNeurons = imageLabels.Count;

			layersNeuronsCount.Insert(0, numberOfInputNeurons);
			layersNeuronsCount.Add(numberOfOuputNeurons);

			Console.WriteLine("Neuron layer size counts vector = " + layersNeuronsCount);

			NeuralNetwork neuralNetwork = new MultiLayerPerceptron(layersNeuronsCount, transferFunctionType);

			neuralNetwork.Label = label;
			PluginBase imageRecognitionPlugin = new ImageRecognitionPlugin(samplingResolution, colorMode);
			neuralNetwork.addPlugin(imageRecognitionPlugin);

			assignLabelsToOutputNeurons(neuralNetwork, imageLabels);
					neuralNetwork.LearningRule = new MomentumBackpropagation();

				return neuralNetwork;
		}

			/// <summary>
			/// Assign labels to output neurons </summary>
			/// <param name="neuralNetwork"> neural network </param>
			/// <param name="imageLabels"> image labels </param>
		private static void assignLabelsToOutputNeurons(NeuralNetwork neuralNetwork, List<string> imageLabels)
		{
			List<Neuron> outputNeurons = neuralNetwork.OutputNeurons;

			for (int i = 0; i < outputNeurons.Count; i++)
			{
				Neuron neuron = outputNeurons[i];
				string label = imageLabels[i];
				neuron.Label = label;
			}
		}


			/// <summary>
			/// Creates training set for the specified image labels and rgb data. Thi method is now forwarded to createRGBTrainingSet </summary>
			/// <param name="imageLabels"> image labels </param>
			/// <param name="rgbDataMap"> map collection of rgb data </param>
			/// <returns> training set for the specified image data </returns>
			/// @deprecated Use createRGBTrainingSet instead 
			public static DataSet createTrainingSet(List<string> imageLabels, IDictionary<string, FractionRgbData> rgbDataMap)
			{
				return createRGBTrainingSet(imageLabels, rgbDataMap);
			}

			/// <summary>
			/// Creates training set for the specified image labels and rgb data </summary>
			/// <param name="imageLabels"> image labels </param>
			/// <param name="rgbDataMap"> map collection of rgb data </param>
			/// <returns> training set for the specified image data </returns>
		public static DataSet createRGBTrainingSet(List<string> imageLabels, IDictionary<string, FractionRgbData> rgbDataMap)
		{
					int inputCount = rgbDataMap.Values.GetEnumerator().next().FlattenedRgbValues.length;
					int outputCount = imageLabels.Count;
			DataSet trainingSet = new DataSet(inputCount, outputCount);

			foreach (KeyValuePair<string, FractionRgbData> entry in rgbDataMap)
			{
				double[] input = entry.Value.FlattenedRgbValues;
				double[] response = createResponse(entry.Key, imageLabels);
				trainingSet.addRow(new DataSetRow(input, response));
			}

					// set labels for output columns
					int inputSize = trainingSet.InputSize;
					for (int c = 0; c < trainingSet.OutputSize ; c++)
					{
						trainingSet.setColumnName(inputSize + c, imageLabels[c]);
					}

					return trainingSet;
		}

			/// <summary>
			/// Creates training set for the specified image labels and hsl data </summary>
			/// <param name="imageLabels"> image labels </param>
			/// <param name="hslDataMap"> map colletction of hsl data </param>
			/// <returns> training set for the specified image data </returns>
			public static DataSet createHSLTrainingSet(List<string> imageLabels, IDictionary<string, FractionHSLData> hslDataMap)
			{
					int inputCount = hslDataMap.Values.GetEnumerator().next().FlattenedHSLValues.length;
					int outputCount = imageLabels.Count;
			DataSet trainingSet = new DataSet(inputCount, outputCount);

			foreach (KeyValuePair<string, FractionHSLData> entry in hslDataMap)
			{
				double[] input = entry.Value.FlattenedHSLValues;
				double[] response = createResponse(entry.Key, imageLabels);
				trainingSet.addRow(new DataSetRow(input, response));
			}

					// set labels for output columns
					int inputSize = trainingSet.InputSize;
					for (int c = 0; c < trainingSet.OutputSize ; c++)
					{
						trainingSet.setColumnName(inputSize + c, imageLabels[c]);
					}


					return trainingSet;
			}


			/// <summary>
			/// Creates binary black and white training set for the specified image labels and rgb data
			/// white = 0 black = 1 </summary>
			/// <param name="imageLabels"> image labels </param>
			/// <param name="rgbDataMap"> map collection of rgb data </param>
			/// <returns> binary black and white training set for the specified image data </returns>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static org.neuroph.core.data.DataSet createBlackAndWhiteTrainingSet(java.util.List<String> imageLabels, java.util.Map<String, FractionRgbData> rgbDataMap) throws org.neuroph.core.exceptions.VectorSizeMismatchException
			public static DataSet createBlackAndWhiteTrainingSet(List<string> imageLabels, IDictionary<string, FractionRgbData> rgbDataMap)
			{
				// TODO: Use some binarization image filter to do this; currently it works  with averaging RGB values
					int inputCount = rgbDataMap.Values.GetEnumerator().next().FlattenedRgbValues.length / 3;
					int outputCount = imageLabels.Count;
			DataSet trainingSet = new DataSet(inputCount, outputCount);

			foreach (KeyValuePair<string, FractionRgbData> entry in rgbDataMap)
			{
				double[] inputRGB = entry.Value.FlattenedRgbValues;
							double[] inputBW = FractionRgbData.convertRgbInputToBinaryBlackAndWhite(inputRGB);
							double[] response = createResponse(entry.Key, imageLabels);
				trainingSet.addRow(new DataSetRow(inputBW, response));
			}

					// set labels for output columns
					int inputSize = trainingSet.InputSize;
					for (int c = 0; c < trainingSet.OutputSize ; c++)
					{
						trainingSet.setColumnName(inputSize + c, imageLabels[c]);
					}

				return trainingSet;
			}

		/// <summary>
		/// Loads images from the specified dir, scales to specified resolution and creates RGB data for each image
		/// Puts HSL data in a Map using filenames as keys, and returns that map </summary>
		/// <param name="imgDir"> </param>
		/// <param name="samplingResolution"> </param>
		/// <exception cref="java.io.IOException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static java.util.Map<String, FractionRgbData> getFractionRgbDataForDirectory(java.io.File imgDir, org.neuroph.imgrec.image.Dimension samplingResolution) throws java.io.IOException
		public static IDictionary<string, FractionRgbData> getFractionRgbDataForDirectory(File imgDir, Dimension samplingResolution)
		{
			if (!imgDir.Directory)
			{
				throw new System.ArgumentException("The given file must be a directory.  Argument is: " + imgDir);
			}

			IDictionary<string, FractionRgbData> rgbDataMap = new Dictionary<string, FractionRgbData>();

			ImageFilesIterator imagesIterator = new ImageFilesIterator(imgDir);
			while (imagesIterator.hasNext())
			{
				File imgFile = imagesIterator.next();
							Image img = ImageFactory.getImage(imgFile);
				img = ImageSampler.downSampleImage(samplingResolution, img);
				string filenameOfCurrentImage = imagesIterator.FilenameOfCurrentImage;
				StringTokenizer st = new StringTokenizer(filenameOfCurrentImage, ".");
				rgbDataMap[st.nextToken()] = new FractionRgbData(img);
			}
			return rgbDataMap;
		}

		 // creates hsl map from given image files - params should be files List<File> - or even better image files
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static java.util.Map<String, FractionHSLData> getFractionHSLDataForDirectory(java.io.File imgDir, org.neuroph.imgrec.image.Dimension samplingResolution) throws java.io.IOException
		 public static IDictionary<string, FractionHSLData> getFractionHSLDataForDirectory(File imgDir, Dimension samplingResolution)
		 {

			if (!imgDir.Directory)
			{
				throw new System.ArgumentException("The given file must be a directory.  Argument is: " + imgDir);
			}

		   IDictionary<string, FractionHSLData> map = new Dictionary<string, FractionHSLData>();
		   ImageFilesIterator imagesIterator = new ImageFilesIterator(imgDir);

			try
			{
				while (imagesIterator.hasNext())
				{
					File imgFile = imagesIterator.next();
					BufferedImage img = ImageIO.read(imgFile);
					BufferedImage image = ImageUtilities.resizeImage(img, samplingResolution.Width, samplingResolution.Height);

					string filenameOfCurrentImage = imgFile.Name;
					//String filenameOfCurrentImage = imagesIterator.getFilenameOfCurrentImage();
					StringTokenizer st = new StringTokenizer(filenameOfCurrentImage, ".");
					map[st.nextToken()] = new FractionHSLData(image);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
			}

			return map;
		 }

			/// <summary>
			/// Creates binary network output vector (response) for the specified list of images
			/// Each network output (neuron) corresponds to one image. </summary>
			/// <param name="inputLabel"> label of the input image </param>
			/// <param name="imageLabels"> labels used for output neurons </param>
			/// <returns> network response for the specified input </returns>
		private static double[] createResponse(string inputLabel, List<string> imageLabels)
		{
			double[] response = new double[imageLabels.Count];
			int i = 0;
			foreach (string imageLabel in imageLabels)
			{
				if (inputLabel.StartsWith(imageLabel, StringComparison.Ordinal))
				{
					response[i] = 1d;
				}
				else
				{
					response[i] = 0d;
				}
				i++;
			}
			return response;
		}
	}
}