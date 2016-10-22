using System;
using System.Collections;
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


	using Neuron = org.neuroph.core.Neuron;
	using ColorMode = org.neuroph.imgrec.ColorMode;
	using ImageRecognitionPlugin = org.neuroph.imgrec.ImageRecognitionPlugin;
	using ImageUtilities = org.neuroph.imgrec.ImageUtilities;
	using Dimension = org.neuroph.imgrec.image.Dimension;
	using Image = org.neuroph.imgrec.image.Image;
	using PluginBase = org.neuroph.util.plugins.PluginBase;

	/// <summary>
	/// Provides OCR interface for neural network.
	/// 
	/// @author Zoran Sevarac, Ivana Jovicic, Vladimir Kolarevic, Marko Ivanovic,
	/// Boris Horvat, Damir Kocic, Nemanja Jovanovic
	/// </summary>
	[Serializable]
	public class OcrPlugin : PluginBase
	{

		private const long serialVersionUID = 1L;

		/// <summary>
		/// Ocr plugin name field used for getting plugin from parent neural network.
		/// </summary>
		public const string OCR_PLUGIN_NAME = "OCR Plugin";

		/// <summary>
		/// Image sampling resolution (image dimensions)
		/// </summary>
		private Dimension samplingResolution;
		/// <summary>
		/// Color mode used for recognition (full color or black and white)
		/// </summary>
		private ColorMode colorMode;


		/// <summary>
		/// Constructor, creates new OCR Plugin for specified sampling resolution and color mode
		/// </summary>
		/// <param name="samplingResolution">
		///            image sampling resolution (dimensions) </param>
		/// <param name="colorMode"> recognition color mode </param>
		public OcrPlugin(Dimension samplingResolution, ColorMode colorMode) : base(OCR_PLUGIN_NAME)
		{
			this.samplingResolution = samplingResolution;
			this.colorMode = colorMode;
		}

		/// <summary>
		/// This method scales character image to the given dimensions and then does the character recognition.
		/// Returns recognized character. </summary>
		/// <param name="charImage"> character image </param>
		/// <param name="scaleToDim"> dimensions to scale the image before character recognition is done </param>
		/// <returns> recognized character </returns>
		public virtual char? recognizeCharacter(Image charImage, Dimension scaleToDim)
		{
			Image resizedImage = this.resizeImage(charImage, scaleToDim.Width, scaleToDim.Height);
			return recognizeCharacter(resizedImage);
		}

		/// <summary>
		/// Recognizes character from the image and returns character </summary>
		/// <param name="charImage"> character image </param>
		/// <returns> recognized character </returns>
		public virtual char? recognizeCharacter(Image charImage)
		{
			// get the image recognition plugin from neural network
			ImageRecognitionPlugin imageRecognition = (ImageRecognitionPlugin) this.ParentNetwork.getPlugin(typeof(ImageRecognitionPlugin));

			Dictionary<string, double?> output = imageRecognition.recognizeImage(charImage);
			Dictionary<string, Neuron> n = imageRecognition.MaxOutput;

			string ch = n.ToString().Substring(1, 1);
			return Convert.ToChar(ch[0]);
		}

		/// <summary>
		/// Recogize the character from the image and returns HashMap with keys as 
		/// characters and recognition probability as values sorted descending by probability.
		/// </summary>
		/// <param name="charImage"> character image </param>
		/// <returns> HashMap with keys as characters and recognition probability as values </returns>
		public virtual Hashtable recognizeCharacterProbabilities(Image charImage)
		{
			// get the image recognition plugin from neural network
			ImageRecognitionPlugin imageRecognition = (ImageRecognitionPlugin) this.ParentNetwork.getPlugin(typeof(ImageRecognitionPlugin));

			// image recognition is done here
			Dictionary<string, double?> output = imageRecognition.recognizeImage(charImage);
			Dictionary<char?, double?> recognized = sortHashMapByValues(output);

			return recognized;
		}


		/// <summary>
		/// Resize image to given dimensions (width and height) </summary>
		/// <param name="image"> image to resize </param>
		/// <param name="width"> width to resize to </param>
		/// <param name="height"> height to resize to </param>
		/// <returns> scaled image </returns>
		private Image resizeImage(Image image, int width, int height)
		{
			return ImageUtilities.resizeImage(image, width, height);
	//    	return ImageFactory.resizeImage(image, width, height); // FIX:  use ImageUtilities instead
		}

		/// <summary>
		/// This private method sorts the result of the recogntion, in order to
		/// see which letter has the highest probability
		/// </summary>
		/// <param name="passedMap"> the HashMap that holds the resault of the recognition process
		/// </param>
		/// <returns> LinkedHashMap that represents the combination of letters with the
		///                       probability of the correct recognition </returns>
		private LinkedHashMap sortHashMapByValues(Hashtable passedMap)
		{
			IList mapKeys = new ArrayList(passedMap.Keys);
			IList mapValues = new ArrayList(passedMap.Values);
			mapValues.Sort();
			mapKeys.Sort();
			mapValues.Reverse();

			LinkedHashMap sortedMap = new LinkedHashMap();

			IEnumerator valueIt = mapValues.GetEnumerator();
			while (valueIt.hasNext())
			{
				object val = valueIt.next();
				IEnumerator keyIt = mapKeys.GetEnumerator();

				while (keyIt.hasNext())
				{
					object key = keyIt.next();
					string comp1 = passedMap[key].ToString();
					string comp2 = val.ToString();

					if (comp1.Equals(comp2))
					{
						passedMap.Remove(key);
						mapKeys.Remove(key);
						char? charKey = Convert.ToChar(key.ToString()[0]);
						sortedMap.put(charKey, (double?) val);
						break;
					}

				}

			}
			return sortedMap;
		}

		/// <summary>
		/// Returns color mode used for OCR </summary>
		/// <returns> color mode used for OCR </returns>
		public virtual ColorMode ColorMode
		{
			get
			{
				return colorMode;
			}
		}

	   /// <summary>
	   /// Returns sampling resolution used for OCR </summary>
	   /// <returns> sampling resolution used for OCR </returns>
		public virtual Dimension SamplingResolution
		{
			get
			{
				return samplingResolution;
			}
		}



	}

}