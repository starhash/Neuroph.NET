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
	/// Bidirectional Associative Memory
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class BAM : NeuralNetwork
	{
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Creates an instance of BAM network with specified number of neurons
		/// in input and output layers.
		/// </summary>
		/// <param name="inputNeuronsCount">
		///            number of neurons in input layer </param>
		/// <param name="outputNeuronsCount">
		///            number of neurons in output layer </param>
		public BAM(int inputNeuronsCount, int outputNeuronsCount)
		{

			// init neuron settings for BAM network
			NeuronProperties neuronProperties = new NeuronProperties();
			neuronProperties.setProperty("neuronType", typeof(InputOutputNeuron));
			neuronProperties.setProperty("bias", 0);
			neuronProperties.setProperty("transferFunction", TransferFunctionType.Step.ToString());
			neuronProperties.setProperty("transferFunction.yHigh", 1);
			neuronProperties.setProperty("transferFunction.yLow", 0);

			this.createNetwork(inputNeuronsCount, outputNeuronsCount, neuronProperties);
		}

		/// <summary>
		/// Creates BAM network architecture
		/// </summary>
		/// <param name="inputNeuronsCount">
		///            number of neurons in input layer </param>
		/// <param name="outputNeuronsCount">
		///            number of neurons in output layer </param>
		/// <param name="neuronProperties">
		///            neuron properties </param>
		private void createNetwork(int inputNeuronsCount, int outputNeuronsCount, NeuronProperties neuronProperties)
		{

					// set network type
			this.NetworkType = NeuralNetworkType.BAM;

			// create input layer
			Layer inputLayer = LayerFactory.createLayer(inputNeuronsCount, neuronProperties);
			// add input layer to network
			this.addLayer(inputLayer);

			// create output layer
			Layer outputLayer = LayerFactory.createLayer(outputNeuronsCount, neuronProperties);
			// add output layer to network
			this.addLayer(outputLayer);

			// create full connectivity from in to out layer	
			ConnectionFactory.fullConnect(inputLayer, outputLayer);
			// create full connectivity from out to in layer
			ConnectionFactory.fullConnect(outputLayer, inputLayer);

			// set input and output cells for this network
			NeuralNetworkFactory.DefaultIO = this;

			// set Hebbian learning rule for this network
			this.LearningRule = new BinaryHebbianLearning();
		}
	}

}