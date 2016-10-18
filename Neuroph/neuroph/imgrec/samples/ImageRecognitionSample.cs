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

namespace org.neuroph.imgrec.samples
{


	using org.neuroph.core;
	using VectorSizeMismatchException = org.neuroph.core.exceptions.VectorSizeMismatchException;

	/// <summary>
	/// This sample shows how to use the image recognition neural network in your applications.
	/// IMPORTANT NOTE: specify filenames for neural network and test image, or you'll get IOException
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class ImageRecognitionSample
	{

		public static void Main(string[] args)
		{
			  // load trained neural network saved with NeurophStudio (specify existing neural network file here)
			  NeuralNetwork nnet = NeuralNetwork.createFromFile("MyImageRecognition.nnet");
			  // get the image recognition plugin from neural network
			  ImageRecognitionPlugin imageRecognition = (ImageRecognitionPlugin)nnet.getPlugin(typeof(ImageRecognitionPlugin));

			  try
			  {
					// image recognition is done here
					Dictionary<string, double?> output = imageRecognition.recognizeImage(new File("someImage.jpg")); // specify some existing image file here
					Console.WriteLine(output.ToString());
			  }
			  catch (IOException)
			  {
				  Console.WriteLine("Error: could not read file!");
			  }
			  catch (VectorSizeMismatchException)
			  {
				  Console.WriteLine("Error: Image dimensions dont !");
			  }
		}
	}

}