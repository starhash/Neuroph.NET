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
	using Neuron = org.neuroph.core.Neuron;
	using Difference = org.neuroph.core.input.Difference;
	using Linear = org.neuroph.core.transfer.Linear;
	using KohonenLearning = org.neuroph.nnet.learning.KohonenLearning;
	using org.neuroph.util;

	/// <summary>
	/// Kohonen neural network.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class Kohonen : NeuralNetwork
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Creates new Kohonen network with specified number of neurons in input and
		/// map layer
		/// </summary>
		/// <param name="inputNeuronsCount">
		///            number of neurons in input layer </param>
		/// <param name="outputNeuronsCount">
		///            number of neurons in output layer </param>
		public Kohonen(int inputNeuronsCount, int outputNeuronsCount)
		{
			this.createNetwork(inputNeuronsCount, outputNeuronsCount);
		}

		/// <summary>
		/// Creates Kohonen network architecture with specified number of neurons in
		/// input and map layer
		/// </summary>
		/// <param name="inputNeuronsCount">
		///            number of neurons in input layer </param>
		/// <param name="outputNeuronsCount">
		///            number of neurons in output layer </param>
		private void createNetwork(int inputNeuronsCount, int outputNeuronsCount)
		{

			// specify input neuron properties (use default: weighted sum input with
			// linear transfer)
			NeuronProperties inputNeuronProperties = new NeuronProperties();

			// specify map neuron properties
			NeuronProperties outputNeuronProperties = new NeuronProperties(typeof(Neuron), typeof(Difference), typeof(Linear)); // transfer function -  input function -  neuron type
			// set network type
			this.NetworkType = NeuralNetworkType.KOHONEN;

			// createLayer input layer
			Layer inLayer = LayerFactory.createLayer(inputNeuronsCount, inputNeuronProperties);
			this.addLayer(inLayer);

			// createLayer map layer
			Layer mapLayer = LayerFactory.createLayer(outputNeuronsCount, outputNeuronProperties);
			this.addLayer(mapLayer);

			// createLayer full connectivity between input and output layer
			ConnectionFactory.fullConnect(inLayer, mapLayer);

			// set network input and output cells
			NeuralNetworkFactory.DefaultIO = this;

			this.LearningRule = new KohonenLearning();
		}

	}

}