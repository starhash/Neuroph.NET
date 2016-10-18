using System;

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
	using ThresholdNeuron = org.neuroph.nnet.comp.neuron.ThresholdNeuron;
	using BinaryDeltaRule = org.neuroph.nnet.learning.BinaryDeltaRule;
	using ConnectionFactory = org.neuroph.util.ConnectionFactory;
	using LayerFactory = org.neuroph.util.LayerFactory;
	using NeuralNetworkFactory = org.neuroph.util.NeuralNetworkFactory;
	using NeuralNetworkType = org.neuroph.util.NeuralNetworkType;
	using NeuronProperties = org.neuroph.util.NeuronProperties;
	using TransferFunctionType = org.neuroph.util.TransferFunctionType;

	/// <summary>
	/// Perceptron neural network with some LMS based learning algorithm.
	/// </summary>
	/// <seealso cref= org.neuroph.nnet.learning.PerceptronLearning </seealso>
	/// <seealso cref= org.neuroph.nnet.learning.BinaryDeltaRule </seealso>
	/// <seealso cref= org.neuroph.nnet.learning.SigmoidDeltaRule
	/// @author Zoran Sevarac <sevarac@gmail.com> </seealso>
	public class Perceptron : NeuralNetwork
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Creates new Perceptron with specified number of neurons in input and
		/// output layer, with Step trqansfer function
		/// </summary>
		/// <param name="inputNeuronsCount">
		///            number of neurons in input layer </param>
		/// <param name="outputNeuronsCount">
		///            number of neurons in output layer </param>
		public Perceptron(int inputNeuronsCount, int outputNeuronsCount)
		{
			this.createNetwork(inputNeuronsCount, outputNeuronsCount, TransferFunctionType.STEP);
		}

		/// <summary>
		/// Creates new Perceptron with specified number of neurons in input and
		/// output layer, and specified transfer function
		/// </summary>
		/// <param name="inputNeuronsCount">
		///            number of neurons in input layer </param>
		/// <param name="outputNeuronsCount">
		///            number of neurons in output layer </param>
		/// <param name="transferFunctionType">
		///            transfer function type </param>
		public Perceptron(int inputNeuronsCount, int outputNeuronsCount, TransferFunctionType transferFunctionType)
		{
			this.createNetwork(inputNeuronsCount, outputNeuronsCount, transferFunctionType);
		}

		/// <summary>
		/// Creates perceptron architecture with specified number of neurons in input
		/// and output layer, specified transfer function
		/// </summary>
		/// <param name="inputNeuronsCount">
		///            number of neurons in input layer </param>
		/// <param name="outputNeuronsCount">
		///            number of neurons in output layer </param>
		/// <param name="transferFunctionType">
		///            neuron transfer function type </param>
		private void createNetwork(int inputNeuronsCount, int outputNeuronsCount, TransferFunctionType transferFunctionType)
		{
			// set network type
			this.NetworkType = NeuralNetworkType.PERCEPTRON;

			// init neuron settings for input layer
			NeuronProperties inputNeuronProperties = new NeuronProperties();
			   inputNeuronProperties.setProperty("transferFunction", TransferFunctionType.LINEAR);

			// create input layer
			Layer inputLayer = LayerFactory.createLayer(inputNeuronsCount, inputNeuronProperties);
			this.addLayer(inputLayer);

			NeuronProperties outputNeuronProperties = new NeuronProperties();
			outputNeuronProperties.setProperty("neuronType", typeof(ThresholdNeuron));
			outputNeuronProperties.setProperty("thresh", Math.Abs(new Random(1).NextDouble()));
			outputNeuronProperties.setProperty("transferFunction", transferFunctionType);
			// for sigmoid and tanh transfer functions set slope propery
			outputNeuronProperties.setProperty("transferFunction.slope", 1);

			// createLayer output layer
			Layer outputLayer = LayerFactory.createLayer(outputNeuronsCount, outputNeuronProperties);
			this.addLayer(outputLayer);

			// create full conectivity between input and output layer
			ConnectionFactory.fullConnect(inputLayer, outputLayer);

			// set input and output cells for this network
			NeuralNetworkFactory.DefaultIO = this;

					this.LearningRule = new BinaryDeltaRule();
			// set appropriate learning rule for this network
	//		if (transferFunctionType == TransferFunctionType.STEP) {
	//			this.setLearningRule(new BinaryDeltaRule(this));
	//		} else if (transferFunctionType == TransferFunctionType.SIGMOID) {
	//			this.setLearningRule(new SigmoidDeltaRule(this));
	//		} else if (transferFunctionType == TransferFunctionType.TANH) {
	//			this.setLearningRule(new SigmoidDeltaRule(this));
	//		} else {
	//			this.setLearningRule(new PerceptronLearning(this));
	//		}
		}

	}

}