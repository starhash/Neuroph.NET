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
	using UnsupervisedHebbianLearning = org.neuroph.nnet.learning.UnsupervisedHebbianLearning;
	using ConnectionFactory = org.neuroph.util.ConnectionFactory;
	using LayerFactory = org.neuroph.util.LayerFactory;
	using NeuralNetworkFactory = org.neuroph.util.NeuralNetworkFactory;
	using NeuralNetworkType = org.neuroph.util.NeuralNetworkType;
	using NeuronProperties = org.neuroph.util.NeuronProperties;
	using TransferFunctionType = org.neuroph.util.TransferFunctionType;

	/// <summary>
	/// Hebbian neural network with unsupervised Hebbian learning algorithm.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class UnsupervisedHebbianNetwork : NeuralNetwork
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 2L;

		/// <summary>
		/// Creates an instance of Unsuervised Hebian net with specified number 
		/// of neurons in input and output layer
		/// </summary>
		/// <param name="inputNeuronsNum">
		///            number of neurons in input layer </param>
		/// <param name="outputNeuronsNum">
		///            number of neurons in output layer </param>
		public UnsupervisedHebbianNetwork(int inputNeuronsNum, int outputNeuronsNum)
		{
			this.createNetwork(inputNeuronsNum, outputNeuronsNum, TransferFunctionType.LINEAR);

		}

		/// <summary>
		/// Creates an instance of Unsuervised Hebian net with specified number
		/// of neurons in input layer and output layer, and transfer function
		/// </summary>
		/// <param name="inputNeuronsNum">
		///            number of neurons in input layer </param>
		/// <param name="outputNeuronsNum">
		///            number of neurons in output layer </param>
		/// <param name="transferFunctionType">
		///            transfer function type id </param>
		public UnsupervisedHebbianNetwork(int inputNeuronsNum, int outputNeuronsNum, TransferFunctionType transferFunctionType)
		{
			this.createNetwork(inputNeuronsNum, outputNeuronsNum, transferFunctionType);
		}

		/// <summary>
		/// Creates an instance of Unsuervised Hebian net with specified number
		/// of neurons in input layer and output layer, and transfer function
		/// </summary>
		/// <param name="inputNeuronsNum">
		///            number of neurons in input layer </param>
		/// <param name="outputNeuronsNum">
		///            number of neurons in output layer </param>
		/// <param name="transferFunctionType">
		///            transfer function type </param>
		private void createNetwork(int inputNeuronsNum, int outputNeuronsNum, TransferFunctionType transferFunctionType)
		{

			// init neuron properties
			NeuronProperties neuronProperties = new NeuronProperties();
	//		neuronProperties.setProperty("bias", new Double(-Math
	//				.abs(Math.random() - 0.5))); // Hebbian network cann not work
			// without bias
			neuronProperties.setProperty("transferFunction", transferFunctionType);
			neuronProperties.setProperty("transferFunction.slope", 1);

			// set network type code
			this.NetworkType = NeuralNetworkType.UNSUPERVISED_HEBBIAN_NET;

			// createLayer input layer
			Layer inputLayer = LayerFactory.createLayer(inputNeuronsNum, neuronProperties);
			this.addLayer(inputLayer);

			// createLayer output layer
			Layer outputLayer = LayerFactory.createLayer(outputNeuronsNum, neuronProperties);
			this.addLayer(outputLayer);

			// createLayer full conectivity between input and output layer
			ConnectionFactory.fullConnect(inputLayer, outputLayer);

			// set input and output cells for this network
			NeuralNetworkFactory.DefaultIO = this;

			// set appropriate learning rule for this network
			this.LearningRule = new UnsupervisedHebbianLearning();
		//this.setLearningRule(new OjaLearning(this));
		}
	}


}