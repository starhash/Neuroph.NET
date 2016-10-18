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
	using BiasNeuron = org.neuroph.nnet.comp.neuron.BiasNeuron;
	using LMS = org.neuroph.nnet.learning.LMS;
	using ConnectionFactory = org.neuroph.util.ConnectionFactory;
	using LayerFactory = org.neuroph.util.LayerFactory;
	using NeuralNetworkFactory = org.neuroph.util.NeuralNetworkFactory;
	using NeuralNetworkType = org.neuroph.util.NeuralNetworkType;
	using NeuronProperties = org.neuroph.util.NeuronProperties;
	using TransferFunctionType = org.neuroph.util.TransferFunctionType;

	/// <summary>
	/// Adaline neural network architecture with LMS learning rule.
	/// Uses bias input, bipolar inputs [-1, 1] and ramp transfer function
	/// It can be also created using binary inputs and linear transfer function,
	/// but that dont works for some problems.
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class Adaline : NeuralNetwork
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Creates new Adaline network with specified number of neurons in input
		/// layer
		/// </summary>
		/// <param name="inputNeuronsCount">
		///            number of neurons in input layer </param>
		public Adaline(int inputNeuronsCount)
		{
			this.createNetwork(inputNeuronsCount);
		}

		/// <summary>
		/// Creates adaline network architecture with specified number of input neurons
		/// </summary>
		/// <param name="inputNeuronsCount">
		///              number of neurons in input layer </param>
		private void createNetwork(int inputNeuronsCount)
		{
			// set network type code
			this.NetworkType = NeuralNetworkType.ADALINE;

					// create input layer neuron settings for this network
			NeuronProperties inNeuronProperties = new NeuronProperties();
			inNeuronProperties.setProperty("transferFunction", TransferFunctionType.LINEAR);

			// createLayer input layer with specified number of neurons
			Layer inputLayer = LayerFactory.createLayer(inputNeuronsCount, inNeuronProperties);
					inputLayer.addNeuron(new BiasNeuron()); // add bias neuron (always 1, and it will act as bias input for output neuron)
			this.addLayer(inputLayer);

				   // create output layer neuron settings for this network
			NeuronProperties outNeuronProperties = new NeuronProperties();
			outNeuronProperties.setProperty("transferFunction", TransferFunctionType.RAMP);
			outNeuronProperties.setProperty("transferFunction.slope", 1);
			outNeuronProperties.setProperty("transferFunction.yHigh", 1);
			outNeuronProperties.setProperty("transferFunction.xHigh", 1);
			outNeuronProperties.setProperty("transferFunction.yLow", -1);
			outNeuronProperties.setProperty("transferFunction.xLow", -1);

			// createLayer output layer (only one neuron)
			Layer outputLayer = LayerFactory.createLayer(1, outNeuronProperties);
			this.addLayer(outputLayer);

			// createLayer full conectivity between input and output layer
			ConnectionFactory.fullConnect(inputLayer, outputLayer);

			// set input and output cells for network
			NeuralNetworkFactory.DefaultIO = this;

			// set LMS learning rule for this network
			this.LearningRule = new LMS();
		}

	}
}