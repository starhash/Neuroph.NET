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
	using Difference = org.neuroph.core.input.Difference;
	using Gaussian = org.neuroph.core.transfer.Gaussian;
	using LMS = org.neuroph.nnet.learning.LMS;
	using RBFLearning = org.neuroph.nnet.learning.RBFLearning;
	using org.neuroph.util;

	/// <summary>
	/// Radial basis function neural network.
	/// 
	/// TODO: learning for rbf layer: k-means clustering
	/// weights between input and rbf layer are Ci vector
	/// each weight is a component of a Ci vector
	/// Ci are centroids of the clusters trained by k means clustering
	/// Each neuron in rbf layer corresponds to a single cluster
	/// neuronns in rbf layer are clusters
	/// 
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class RBFNetwork : NeuralNetwork
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Creates new RBFNetwork with specified number of neurons in input, rbf and output layer
		/// </summary>
		/// <param name="inputNeuronsCount">
		///		number of neurons in input layer </param>
		/// <param name="rbfNeuronsCount">
		///		number of neurons in rbf layer </param>
		/// <param name="outputNeuronsCount">
		///		number of neurons in output layer </param>
		public RBFNetwork(int inputNeuronsCount, int rbfNeuronsCount, int outputNeuronsCount)
		{
			this.createNetwork(inputNeuronsCount, rbfNeuronsCount, outputNeuronsCount);
		}

		/// <summary>
		/// Creates RBFNetwork architecture with specified number of neurons in input
		/// layer, output layer and transfer function
		/// </summary>
		/// <param name="inputNeuronsCount">
		///		number of neurons in input layer </param>
		/// <param name="rbfNeuronsCount">
		///		number of neurons in rbf layer </param>
		/// <param name="outputNeuronsCount">
		///		number of neurons in output layer </param>
		private void createNetwork(int inputNeuronsCount, int rbfNeuronsCount, int outputNeuronsCount)
		{
			// init neuron settings for this network
			NeuronProperties rbfNeuronProperties = new NeuronProperties();
			rbfNeuronProperties.setProperty("inputFunction", typeof(Difference));
			rbfNeuronProperties.setProperty("transferFunction", typeof(Gaussian));

			// set network type code
			this.NetworkType = NeuralNetworkType.RBF_NETWORK;

			// create input layer
			Layer inputLayer = LayerFactory.createLayer(inputNeuronsCount, TransferFunctionType.LINEAR);
			this.addLayer(inputLayer);

			// create rbf layer
			Layer rbfLayer = LayerFactory.createLayer(rbfNeuronsCount, rbfNeuronProperties);
			this.addLayer(rbfLayer);

			// create output layer
			Layer outputLayer = LayerFactory.createLayer(outputNeuronsCount, TransferFunctionType.LINEAR);
			this.addLayer(outputLayer);

			// create full conectivity between input and rbf layer
			ConnectionFactory.fullConnect(inputLayer, rbfLayer);
			// create full conectivity between rbf and output layer
			ConnectionFactory.fullConnect(rbfLayer, outputLayer);

			// set input and output cells for this network
			NeuralNetworkFactory.DefaultIO = this;

			// set appropriate learning rule for this network
			this.LearningRule = new RBFLearning();
		}

	}
}