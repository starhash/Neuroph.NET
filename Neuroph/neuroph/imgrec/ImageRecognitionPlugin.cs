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


	using Dimension = org.neuroph.imgrec.image.Dimension;
	using Image = org.neuroph.imgrec.image.Image;
	using ImageFactory = org.neuroph.imgrec.image.ImageFactory;

	using Neuron = org.neuroph.core.Neuron;
	using VectorSizeMismatchException = org.neuroph.core.exceptions.VectorSizeMismatchException;
	using ImageJ2SE = org.neuroph.imgrec.image.ImageJ2SE;
	using PluginBase = org.neuroph.util.plugins.PluginBase;

	/// <summary>
	/// Provides image recognition specific properties like sampling resolution, and easy to
	/// use image recognition interface for neural network.
	/// 
	/// @author Jon Tait
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	[Serializable]
	public class ImageRecognitionPlugin : PluginBase
	{
		private const long serialVersionUID = 1L;

		public const string IMG_REC_PLUGIN_NAME = "Image Recognition Plugin";

		/// <summary>
		/// Image sampling resolution (image dimensions)
		/// </summary>
		private Dimension samplingResolution;

			/// <summary>
			/// Color mode used for recognition (full color or black and white)
			/// </summary>
			private ColorMode colorMode;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="samplingResolution">
		///            image sampling resolution (dimensions) </param>
		public ImageRecognitionPlugin(Dimension samplingResolution) : base(IMG_REC_PLUGIN_NAME)
		{
			this.samplingResolution = samplingResolution;
					this.colorMode = ColorMode.COLOR_RGB;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="samplingResolution">
		///            image sampling resolution (dimensions) </param>
		/// <param name="colorMode"> recognition color mode  </param>
		public ImageRecognitionPlugin(Dimension samplingResolution, ColorMode colorMode) : base(IMG_REC_PLUGIN_NAME)
		{
			this.samplingResolution = samplingResolution;
					this.colorMode = colorMode;
		}

		/// <summary>
		/// Returns image sampling resolution (dimensions)
		/// </summary>
		/// <returns> image sampling resolution (dimensions) </returns>
		public virtual Dimension SamplingResolution
		{
			get
			{
				return samplingResolution;
			}
		}

			/// <summary>
			/// Returns color mode used for image recognition </summary>
			/// <returns> color mode used for image recognition </returns>
			public virtual ColorMode ColorMode
			{
				get
				{
					return this.colorMode;
				}
			}

		/// <summary>
		/// Sets network input (image to recognize) from the specified BufferedImage
		/// object
		/// </summary>
		/// <param name="img">
		///            image to recognize </param>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void setInput(org.neuroph.imgrec.image.Image img) throws ImageSizeMismatchException
		public virtual Image Input
		{
			set
			{
    
				double[] input;
    
				if (this.colorMode == ColorMode.COLOR_RGB)
				{
							FractionRgbData imgRgb = new FractionRgbData(ImageSampler.downSampleImage(samplingResolution, value, value.Type));
					input = imgRgb.FlattenedRgbValues;
				}
						else if (this.colorMode == ColorMode.COLOR_HSL)
						{
							FractionHSLData imgHsl = new FractionHSLData(ImageSampler.downSampleImage(samplingResolution, value, value.Type));
					input = imgHsl.FlattenedHSLValues;
						}
						else if (this.colorMode == ColorMode.BLACK_AND_WHITE)
						{
							FractionRgbData imgRgb = new FractionRgbData(ImageSampler.downSampleImage(samplingResolution, value, value.Type));
					input = FractionRgbData.convertRgbInputToBinaryBlackAndWhite(imgRgb.FlattenedRgbValues);
						}
						else
						{
					throw new Exception("Unknown color mode!");
						}
    
						try
						{
							this.ParentNetwork.Input = input;
						}
						catch (VectorSizeMismatchException vsme)
						{
							throw new ImageSizeMismatchException(vsme);
						}
			}
		}

		/// <summary>
		/// Sets network input (image to recognize) from the specified File object
		/// </summary>
		/// <param name="imgFile">
		///            file of the image to recognize </param>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void setInput(java.io.File imgFile) throws java.io.IOException, ImageSizeMismatchException
		public virtual File Input
		{
			set
			{
				this.Input = ImageFactory.getImage(value);
			}
		}

		/// <summary>
		/// Sets network input (image to recognize) from the specified URL object
		/// </summary>
		/// <param name="imgURL">
		///            url of the image </param>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void setInput(java.net.URL imgURL) throws java.io.IOException, ImageSizeMismatchException
		public virtual URL Input
		{
			set
			{
				this.Input = ImageFactory.getImage(value);
			}
		}

			public virtual void processInput()
			{
					ParentNetwork.calculate();
			}

		/// <summary>
		/// Returns image recognition result as map with image labels as keys and
		/// recogition result as value
		/// </summary>
		/// <returns> image recognition result </returns>
		public virtual Dictionary<string, double?> Output
		{
			get
			{
				Dictionary<string, double?> networkOutput = new Dictionary<string, double?>();
    
				foreach (Neuron neuron in this.ParentNetwork.OutputNeurons)
				{
					string neuronLabel = neuron.Label;
					networkOutput[neuronLabel] = neuron.Output;
				}
    
				return networkOutput;
			}
		}


		/// <summary>
		/// This method performs the image recognition for specified image.
		/// Returns image recognition result as map with image labels as keys and
		/// recogition result as value
		/// </summary>
		/// <returns> image recognition result </returns>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public java.util.HashMap<String, Double> recognizeImage(org.neuroph.imgrec.image.Image img) throws ImageSizeMismatchException
			public virtual Dictionary<string, double?> recognizeImage(Image img)
			{
			Input = img;
			processInput();
					return Output;
			}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public java.util.HashMap<String, Double> recognizeImage(java.awt.image.BufferedImage img) throws ImageSizeMismatchException
			public virtual Dictionary<string, double?> recognizeImage(BufferedImage img)
			{
				return recognizeImage(new ImageJ2SE(img));
			}

	//        public HashMap<String, Double> recognizeImage(Bitmap img) throws ImageSizeMismatchException {
	//            return recognizeImage(ImageFactory. new ImageAndroid(img));
	//        }


		/// <summary>
		/// This method performs the image recognition for specified image file.
		/// Returns image recognition result as map with image labels as keys and
		/// recogition result as value
		/// </summary>
		/// <returns> image recognition result </returns>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public java.util.HashMap<String, Double> recognizeImage(java.io.File imgFile) throws java.io.IOException, ImageSizeMismatchException
			public virtual Dictionary<string, double?> recognizeImage(File imgFile)
			{
			Input = imgFile;
			processInput();
					return Output;
			}

		/// <summary>
		/// This method performs the image recognition for specified image URL.
		/// Returns image recognition result as map with image labels as keys and
		/// recogition result as value
		/// </summary>
		/// <returns> image recognition result </returns>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public java.util.HashMap<String, Double> recognizeImage(java.net.URL imgURL) throws java.io.IOException, ImageSizeMismatchException
			public virtual Dictionary<string, double?> recognizeImage(URL imgURL)
			{
			Input = imgURL;
			processInput();
					return Output;
			}

		/// <summary>
		/// Returns one or more image labels with the maximum output - recognized
		/// images
		/// </summary>
		/// <returns> one or more image labels with the maximum output </returns>
		public virtual Dictionary<string, Neuron> MaxOutput
		{
			get
			{
				Dictionary<string, Neuron> maxOutput = new Dictionary<string, Neuron>();
				Neuron maxNeuron = this.ParentNetwork.OutputNeurons[0];
    
				foreach (Neuron neuron in this.ParentNetwork.OutputNeurons)
				{
					if (neuron.Output > maxNeuron.Output)
					{
						maxNeuron = neuron;
					}
				}
    
    
				maxOutput[maxNeuron.Label] = maxNeuron;
    
				foreach (Neuron neuron in this.ParentNetwork.OutputNeurons)
				{
					if (neuron.Output == maxNeuron.Output)
					{
						maxOutput[neuron.Label] = neuron;
					}
				}
    
				return maxOutput;
			}
		}

	}
}