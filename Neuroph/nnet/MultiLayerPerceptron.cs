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

namespace org.neuroph.nnet {



    using Layer = org.neuroph.core.Layer;
    using WeightedSum = org.neuroph.core.input.WeightedSum;
    using Linear = org.neuroph.core.transfer.Linear;
    using BiasNeuron = org.neuroph.nnet.comp.neuron.BiasNeuron;
    using InputNeuron = org.neuroph.nnet.comp.neuron.InputNeuron;
    using BackPropagation = org.neuroph.nnet.learning.BackPropagation;
    using MomentumBackpropagation = org.neuroph.nnet.learning.MomentumBackpropagation;
    using ConnectionFactory = org.neuroph.util.ConnectionFactory;
    using LayerFactory = org.neuroph.util.LayerFactory;
    using NeuralNetworkFactory = org.neuroph.util.NeuralNetworkFactory;
    using NeuralNetworkType = org.neuroph.util.NeuralNetworkType;
    using NeuronProperties = org.neuroph.util.NeuronProperties;
    using TransferFunctionType = org.neuroph.util.TransferFunctionType;
    using RangeRandomizer = org.neuroph.util.random.RangeRandomizer;
    using core;

    /// <summary>
    /// Multi Layer Perceptron neural network with Back propagation learning algorithm.
    /// 
    /// @author Zoran Sevarac <sevarac@gmail.com> </summary>
    /// <seealso cref= org.neuroph.nnet.learning.BackPropagation </seealso>
    /// <seealso cref= org.neuroph.nnet.learning.MomentumBackpropagation </seealso>
    [Serializable]
    public class MultiLayerPerceptron : NeuralNetwork
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 2L;

		/// <summary>
		/// Creates new MultiLayerPerceptron with specified number of neurons in layers
		/// </summary>
		/// <param name="neuronsInLayers"> collection of neuron number in layers </param>
		public MultiLayerPerceptron(List<int> neuronsInLayers)
		{
			// init neuron settings
			NeuronProperties neuronProperties = new NeuronProperties();
			neuronProperties.setProperty("useBias", true);
			neuronProperties.setProperty("transferFunction", TransferFunctionType.Sigmoid.ToString());

			this.createNetwork(neuronsInLayers, neuronProperties);
		}

		public MultiLayerPerceptron(params int[] neuronsInLayers)
		{
			// init neuron settings
			NeuronProperties neuronProperties = new NeuronProperties();
			neuronProperties.setProperty("useBias", true);
			neuronProperties.setProperty("transferFunction", TransferFunctionType.Sigmoid.ToString());
			neuronProperties.setProperty("inputFunction", typeof(WeightedSum));

			List<int> neuronsInLayersVector = new List<int>();
			for (int i = 0; i < neuronsInLayers.Length; i++)
			{
				neuronsInLayersVector.Add(Convert.ToInt32(neuronsInLayers[i]));
			}

			this.createNetwork(neuronsInLayersVector, neuronProperties);
		}

		public MultiLayerPerceptron(TransferFunctionType transferFunctionType, params int[] neuronsInLayers)
		{
			// init neuron settings
			NeuronProperties neuronProperties = new NeuronProperties();
			neuronProperties.setProperty("useBias", true);
			neuronProperties.setProperty("transferFunction", transferFunctionType.ToString());
			neuronProperties.setProperty("inputFunction", typeof(WeightedSum));


			List<int> neuronsInLayersVector = new List<int>();
			for (int i = 0; i < neuronsInLayers.Length; i++)
			{
				neuronsInLayersVector.Add(Convert.ToInt32(neuronsInLayers[i]));
			}

			this.createNetwork(neuronsInLayersVector, neuronProperties);
		}

		public MultiLayerPerceptron(List<int> neuronsInLayers, TransferFunctionType transferFunctionType)
		{
			// init neuron settings
			NeuronProperties neuronProperties = new NeuronProperties();
			neuronProperties.setProperty("useBias", true);
			neuronProperties.setProperty("transferFunction", transferFunctionType.ToString());

			this.createNetwork(neuronsInLayers, neuronProperties);
		}

		/// <summary>
		/// Creates new MultiLayerPerceptron net with specified number neurons in
		/// getLayersIterator
		/// </summary>
		/// <param name="neuronsInLayers">  collection of neuron numbers in layers </param>
		/// <param name="neuronProperties"> neuron properties </param>
		public MultiLayerPerceptron(List<int> neuronsInLayers, NeuronProperties neuronProperties)
		{
			this.createNetwork(neuronsInLayers, neuronProperties);
		}

		/// <summary>
		/// Creates MultiLayerPerceptron Network architecture - fully connected
		/// feed forward with specified number of neurons in each layer
		/// </summary>
		/// <param name="neuronsInLayers">  collection of neuron numbers in getLayersIterator </param>
		/// <param name="neuronProperties"> neuron properties </param>
		private void createNetwork(List<int> neuronsInLayers, NeuronProperties neuronProperties)
		{

			// set network type
			this.NetworkType = NeuralNetworkType.MULTI_LAYER_PERCEPTRON;

			// create input layer
			NeuronProperties inputNeuronProperties = new NeuronProperties(typeof(InputNeuron), typeof(Linear));
			Layer layer = LayerFactory.createLayer(neuronsInLayers[0], inputNeuronProperties);

			bool useBias = true; // use bias neurons by default
			if (neuronProperties.hasProperty("useBias"))
			{
				useBias = (bool) neuronProperties.getProperty("useBias");
			}

			if (useBias)
			{
				layer.addNeuron(new BiasNeuron());
			}

			this.addLayer(layer);

			// create layers
			Layer prevLayer = layer;

			//for(Integer neuronsNum : neuronsInLayers)
			for (int layerIdx = 1; layerIdx < neuronsInLayers.Count; layerIdx++)
			{
				int neuronsNum = neuronsInLayers[layerIdx];
				// createLayer layer
				layer = LayerFactory.createLayer(neuronsNum, neuronProperties);

				if (useBias && (layerIdx < (neuronsInLayers.Count - 1)))
				{
					layer.addNeuron(new BiasNeuron());
				}

				// add created layer to network
				this.addLayer(layer);
				// createLayer full connectivity between previous and this layer
				if (prevLayer != null)
				{
					ConnectionFactory.fullConnect(prevLayer, layer);
				}

				prevLayer = layer;
			}

			// set input and output cells for network
			NeuralNetworkFactory.DefaultIO = this;

			// set learnng rule
	//        this.setLearningRule(new BackPropagation());
			this.LearningRule = new MomentumBackpropagation();
			// this.setLearningRule(new DynamicBackPropagation());

			this.randomizeWeights(new RangeRandomizer(-0.7, 0.7));

		}

		public virtual void connectInputsToOutputs()
		{
			// connect first and last layer
			ConnectionFactory.fullConnect(getLayerAt(0), getLayerAt(LayersCount - 1), false);
		}

	}
}