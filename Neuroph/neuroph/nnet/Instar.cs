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
	using InstarLearning = org.neuroph.nnet.learning.InstarLearning;
	using ConnectionFactory = org.neuroph.util.ConnectionFactory;
	using LayerFactory = org.neuroph.util.LayerFactory;
	using NeuralNetworkFactory = org.neuroph.util.NeuralNetworkFactory;
	using NeuralNetworkType = org.neuroph.util.NeuralNetworkType;
	using NeuronProperties = org.neuroph.util.NeuronProperties;
	using TransferFunctionType = org.neuroph.util.TransferFunctionType;

	/// <summary>
	/// Instar neural network with Instar learning rule.
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class Instar : NeuralNetwork
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Creates new Instar with specified number of input neurons.
		/// </summary>
		/// <param name="inputNeuronsCount">
		///            number of neurons in input layer </param>
		public Instar(int inputNeuronsCount)
		{
			this.createNetwork(inputNeuronsCount);
		}

		/// <summary>
		/// Creates Instar architecture with specified number of input neurons
		/// </summary>
		/// <param name="inputNeuronsCount">
		///            number of neurons in input layer </param>
		private void createNetwork(int inputNeuronsCount)
		{

			// set network type
			this.NetworkType = NeuralNetworkType.INSTAR;

			// init neuron settings for this type of network
			NeuronProperties neuronProperties = new NeuronProperties();
			neuronProperties.setProperty("transferFunction", TransferFunctionType.STEP);

			// create input layer
			Layer inputLayer = LayerFactory.createLayer(inputNeuronsCount, neuronProperties);
			this.addLayer(inputLayer);

			// createLayer output layer
			neuronProperties.setProperty("transferFunction", TransferFunctionType.STEP);
			Layer outputLayer = LayerFactory.createLayer(1, neuronProperties);
			this.addLayer(outputLayer);

			// create full conectivity between input and output layer
			ConnectionFactory.fullConnect(inputLayer, outputLayer);

			// set input and output cells for this network
			NeuralNetworkFactory.DefaultIO = this;

			// set appropriate learning rule for this network
			this.LearningRule = new InstarLearning();
		}

	}

}