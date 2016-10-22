using System;

/// <summary>
/// Copyright 2013 Neuroph Project http://neuroph.sourceforge.net
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

namespace org.neuroph.nnet
{

	using Layer = org.neuroph.core.Layer;
	using WeightedSum = org.neuroph.core.input.WeightedSum;
	using ConvolutionalUtils = org.neuroph.nnet.comp.ConvolutionalUtils;
	using org.neuroph.nnet.comp.layer;
	using ConvolutionalBackpropagation = org.neuroph.nnet.learning.ConvolutionalBackpropagation;
	using org.neuroph.core;
	using Neuron = org.neuroph.core.Neuron;
	using NeurophException = org.neuroph.core.exceptions.NeurophException;
	using VectorSizeMismatchException = org.neuroph.core.exceptions.VectorSizeMismatchException;
	using TransferFunction = org.neuroph.core.transfer.TransferFunction;
	using BiasNeuron = org.neuroph.nnet.comp.neuron.BiasNeuron;
	using ConnectionFactory = org.neuroph.util.ConnectionFactory;
	using NeuronProperties = org.neuroph.util.NeuronProperties;
	using TransferFunctionType = org.neuroph.util.TransferFunctionType;

	/// <summary>
	/// Convolutional neural network with backpropagation algorithm modified for
	/// convolutional networks.
	/// <p/>
	/// TODO: provide Hiton, LeCun, AndrewNg implementation specific features
	/// 
	/// @author Boris Fulurija
	/// @author Zoran Sevarac </summary>
	/// <seealso cref= ConvolutionalBackpropagation </seealso>
	public class ConvolutionalNetwork : NeuralNetwork
	{

		private const long serialVersionUID = -1393907449047650509L;


		public ConvolutionalNetwork()
		{

		}

		/// <summary>
		/// Sets network input, to all feature maps in input layer
		/// </summary>
		/// <param name="inputVector"> </param>
		/// <exception cref="VectorSizeMismatchException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public void setInput(double... inputVector) throws org.neuroph.core.exceptions.VectorSizeMismatchException
		public override double[] Input
		{
			set
			{
				FeatureMapsLayer inputLayer = (FeatureMapsLayer) getLayerAt(0);
				int currentNeuron = 0;
				for (int i = 0; i < inputLayer.NumberOfMaps; i++)
				{
					FeatureMapLayer map = inputLayer.getFeatureMap(i);
					foreach (Neuron neuron in map.Neurons)
					{
						if (!(neuron is BiasNeuron))
						{
							neuron.Input = value[currentNeuron++];
						}
					}
				}
			}
		}

		public class Builder
		{

			public static readonly NeuronProperties DEFAULT_FULL_CONNECTED_NEURON_PROPERTIES = new NeuronProperties();
			internal ConvolutionalNetwork network;

			static Builder()
			{
				DEFAULT_FULL_CONNECTED_NEURON_PROPERTIES.setProperty("useBias", true);
				DEFAULT_FULL_CONNECTED_NEURON_PROPERTIES.setProperty("transferFunction", TransferFunctionType.Sigmoid.ToString().ToString());
				DEFAULT_FULL_CONNECTED_NEURON_PROPERTIES.setProperty("inputFunction", typeof(WeightedSum));
			}

			public Builder()
			{
				network = new ConvolutionalNetwork();
			}

			public virtual Builder withInputLayer(int width, int height, int numberOfMaps)
			{
				if (network.LayersCount > 0)
				{
					throw new NeurophException("Input layer must be the first layer in network");
				}

				InputMapsLayer inputLayer = new InputMapsLayer(new comp.Dimension2D(width, height), numberOfMaps);
				inputLayer.Label = "Input Layer";
				network.addLayer(inputLayer);

				return this;
			}

			public virtual Builder withConvolutionLayer(int kernelWidth, int kernelHeight, int numberOfMaps)
			{
				FeatureMapsLayer prevLayer = LastFeatureMapLayer;
				ConvolutionalLayer convolutionLayer = new ConvolutionalLayer(prevLayer, new comp.Dimension2D(kernelWidth, kernelHeight), numberOfMaps);

				network.addLayer(convolutionLayer);
				ConvolutionalUtils.fullConnectMapLayers(prevLayer, convolutionLayer);

				return this;
			}

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public Builder withConvolutionLayer(final org.neuroph.nnet.comp.Dimension2D kernelDimension, int numberOfMaps, Class transferFunction)
			public virtual Builder withConvolutionLayer(comp.Dimension2D kernelDimension, int numberOfMaps, Type transferFunction)
			{
				FeatureMapsLayer prevLayer = LastFeatureMapLayer;
				ConvolutionalLayer convolutionLayer = new ConvolutionalLayer(prevLayer, kernelDimension, numberOfMaps, transferFunction);

				network.addLayer(convolutionLayer);
				ConvolutionalUtils.fullConnectMapLayers(prevLayer, convolutionLayer);

				return this;
			}

			public virtual Builder withPoolingLayer(int width, int height)
			{
				FeatureMapsLayer lastLayer = LastFeatureMapLayer;
				PoolingLayer poolingLayer = new PoolingLayer(lastLayer, new comp.Dimension2D(width, height));

				network.addLayer(poolingLayer);
				ConvolutionalUtils.fullConnectMapLayers(lastLayer, poolingLayer);

				return this;
			}

			public virtual Builder withFullConnectedLayer(int numberOfNeurons)
			{
				Layer lastLayer = LastLayer;

				Layer fullConnectedLayer = new Layer(numberOfNeurons, DEFAULT_FULL_CONNECTED_NEURON_PROPERTIES);
				network.addLayer(fullConnectedLayer);

				ConnectionFactory.fullConnect(lastLayer, fullConnectedLayer);

				return this;
			}

			public virtual Builder withFullConnectedLayer(Layer layer)
			{
				Layer lastLayer = LastLayer;
				network.addLayer(layer);
				ConnectionFactory.fullConnect(lastLayer, layer);
				return this;
			}

			public virtual ConvolutionalNetwork build()
			{
				network.InputNeurons = network.getLayerAt(0).Neurons;
				network.OutputNeurons = LastLayer.Neurons;
				network.LearningRule = new ConvolutionalBackpropagation();
				return network;
			}


			internal virtual FeatureMapsLayer LastFeatureMapLayer
			{
				get
				{
					Layer layer = LastLayer;
					if (layer is FeatureMapsLayer)
					{
						return (FeatureMapsLayer) layer;
					}
    
					throw new Exception("Unable to add next layer because previous layer is not FeatureMapLayer");
				}
			}

			internal virtual Layer LastLayer
			{
				get
				{
					return network.getLayerAt(network.LayersCount - 1);
				}
			}


		}


	}
}