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

namespace org.neuroph.nnet
{

	using Layer = org.neuroph.core.Layer;
	using org.neuroph.core;
	using InputOutputNeuron = org.neuroph.nnet.comp.neuron.InputOutputNeuron;
	using BinaryHebbianLearning = org.neuroph.nnet.learning.BinaryHebbianLearning;
	using ConnectionFactory = org.neuroph.util.ConnectionFactory;
	using LayerFactory = org.neuroph.util.LayerFactory;
	using NeuralNetworkFactory = org.neuroph.util.NeuralNetworkFactory;
	using NeuralNetworkType = org.neuroph.util.NeuralNetworkType;
	using NeuronProperties = org.neuroph.util.NeuronProperties;
	using TransferFunctionType = org.neuroph.util.TransferFunctionType;

	/// <summary>
	/// Hopfield neural network.
	/// Notes: try to use [1, -1] activation levels, sgn as transfer function, or real numbers for activation
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>

	public class Hopfield : NeuralNetwork
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 2L;

		/// <summary>
		/// Creates new Hopfield network with specified neuron number
		/// </summary>
		/// <param name="neuronsCount">
		///            neurons number in Hopfied network </param>
		public Hopfield(int neuronsCount)
		{

			// init neuron settings for hopfield network
			NeuronProperties neuronProperties = new NeuronProperties();
			neuronProperties.setProperty("neuronType", typeof(InputOutputNeuron));
			neuronProperties.setProperty("bias", 0);
			neuronProperties.setProperty("transferFunction", TransferFunctionType.STEP);
			neuronProperties.setProperty("transferFunction.yHigh", 1);
			neuronProperties.setProperty("transferFunction.yLow", 0);

			this.createNetwork(neuronsCount, neuronProperties);
		}

		/// <summary>
		/// Creates new Hopfield network with specified neuron number and neuron
		/// properties
		/// </summary>
		/// <param name="neuronsCount">
		///            neurons number in Hopfied network </param>
		/// <param name="neuronProperties">
		///            neuron properties </param>
		public Hopfield(int neuronsCount, NeuronProperties neuronProperties)
		{
			this.createNetwork(neuronsCount, neuronProperties);
		}

		/// <summary>
		/// Creates Hopfield network architecture
		/// </summary>
		/// <param name="neuronsCount">
		///            neurons number in Hopfied network </param>
		/// <param name="neuronProperties">
		///            neuron properties </param>
		private void createNetwork(int neuronsCount, NeuronProperties neuronProperties)
		{

			// set network type
			this.NetworkType = NeuralNetworkType.HOPFIELD;

			// createLayer neurons in layer
			Layer layer = LayerFactory.createLayer(neuronsCount, neuronProperties);

			// createLayer full connectivity in layer
			ConnectionFactory.fullConnect(layer, 0.1);

			// add layer to network
			this.addLayer(layer);

			// set input and output cells for this network
			NeuralNetworkFactory.DefaultIO = this;

			// set Hopfield learning rule for this network
			//this.setLearningRule(new HopfieldLearning(this));	
			this.LearningRule = new BinaryHebbianLearning();
		}

	}

}