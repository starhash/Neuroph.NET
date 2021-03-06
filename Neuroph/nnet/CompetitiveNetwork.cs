﻿/// <summary>
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
	using WeightedSum = org.neuroph.core.input.WeightedSum;
	using CompetitiveLayer = org.neuroph.nnet.comp.layer.CompetitiveLayer;
	using CompetitiveNeuron = org.neuroph.nnet.comp.neuron.CompetitiveNeuron;
	using CompetitiveLearning = org.neuroph.nnet.learning.CompetitiveLearning;
	using ConnectionFactory = org.neuroph.util.ConnectionFactory;
	using LayerFactory = org.neuroph.util.LayerFactory;
	using NeuralNetworkFactory = org.neuroph.util.NeuralNetworkFactory;
	using NeuralNetworkType = org.neuroph.util.NeuralNetworkType;
	using NeuronProperties = org.neuroph.util.NeuronProperties;
	using TransferFunctionType = org.neuroph.util.TransferFunctionType;

	/// <summary>
	/// Two layer neural network with competitive learning rule.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class CompetitiveNetwork : NeuralNetwork
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Creates new competitive network with specified neuron number
		/// </summary>
		/// <param name="inputNeuronsCount">
		///            number of input neurons </param>
		/// <param name="outputNeuronsCount">
		///            number of output neurons </param>
		public CompetitiveNetwork(int inputNeuronsCount, int outputNeuronsCount)
		{
			this.createNetwork(inputNeuronsCount, outputNeuronsCount);
		}

		/// <summary>
		/// Creates Competitive network architecture
		/// </summary>
		/// <param name="inputNeuronsCount">
		///            input neurons number </param>
		/// <param name="outputNeuronsCount">
		///            output neurons number </param>
		/// <param name="neuronProperties">
		///            neuron properties </param>
		private void createNetwork(int inputNeuronsCount, int outputNeuronsCount)
		{
			// set network type
			this.NetworkType = NeuralNetworkType.COMPETITIVE;

			// createLayer input layer
			Layer inputLayer = LayerFactory.createLayer(inputNeuronsCount, new NeuronProperties());
			this.addLayer(inputLayer);

			// createLayer properties for neurons in output layer
			NeuronProperties neuronProperties = new NeuronProperties();
			neuronProperties.setProperty("neuronType", typeof(CompetitiveNeuron));
			neuronProperties.setProperty("inputFunction", typeof(WeightedSum));
			neuronProperties.setProperty("transferFunction",TransferFunctionType.Ramp.ToString());

			// createLayer full connectivity in competitive layer
			CompetitiveLayer competitiveLayer = new CompetitiveLayer(outputNeuronsCount, neuronProperties);

			// add competitive layer to network
			this.addLayer(competitiveLayer);

			double competitiveWeight = -(1 / (double) outputNeuronsCount);
			// createLayer full connectivity within competitive layer
			ConnectionFactory.fullConnect(competitiveLayer, competitiveWeight, 1);

			// createLayer full connectivity from input to competitive layer
			ConnectionFactory.fullConnect(inputLayer, competitiveLayer);

			// set input and output cells for this network
			NeuralNetworkFactory.DefaultIO = this;

			this.LearningRule = new CompetitiveLearning();
		}

	}

}