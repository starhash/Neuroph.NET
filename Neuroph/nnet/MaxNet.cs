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
	using CompetitiveLayer = org.neuroph.nnet.comp.layer.CompetitiveLayer;
	using CompetitiveNeuron = org.neuroph.nnet.comp.neuron.CompetitiveNeuron;
	using ConnectionFactory = org.neuroph.util.ConnectionFactory;
	using LayerFactory = org.neuroph.util.LayerFactory;
	using NeuralNetworkFactory = org.neuroph.util.NeuralNetworkFactory;
	using NeuralNetworkType = org.neuroph.util.NeuralNetworkType;
	using NeuronProperties = org.neuroph.util.NeuronProperties;
	using TransferFunctionType = org.neuroph.util.TransferFunctionType;

	/// <summary>
	/// Max Net neural network with competitive learning rule.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class MaxNet : NeuralNetwork
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Creates new Maxnet network with specified neuron number
		/// </summary>
		/// <param name="neuronsCount">
		///            number of neurons in MaxNet network (same number in input and output layer) </param>
		public MaxNet(int neuronsCount)
		{
			this.createNetwork(neuronsCount);
		}

		/// <summary>
		/// Creates MaxNet network architecture
		/// </summary>
		/// <param name="neuronNum">
		///            neuron number in network </param>
		/// <param name="neuronProperties">
		///            neuron properties </param>
		private void createNetwork(int neuronsCount)
		{

			// set network type
			this.NetworkType = NeuralNetworkType.MAXNET;

			// createLayer input layer in layer
			Layer inputLayer = LayerFactory.createLayer(neuronsCount, new NeuronProperties());
			this.addLayer(inputLayer);

			// createLayer properties for neurons in output layer
			NeuronProperties neuronProperties = new NeuronProperties();
			neuronProperties.setProperty("neuronType", typeof(CompetitiveNeuron));
			neuronProperties.setProperty("transferFunction", TransferFunctionType.Ramp.ToString());

			// createLayer full connectivity in competitive layer
			CompetitiveLayer competitiveLayer = new CompetitiveLayer(neuronsCount, neuronProperties);

			// add competitive layer to network
			this.addLayer(competitiveLayer);

			double competitiveWeight = -(1 / (double) neuronsCount);
			// createLayer full connectivity within competitive layer
			ConnectionFactory.fullConnect(competitiveLayer, competitiveWeight, 1);

			// createLayer forward connectivity from input to competitive layer
			ConnectionFactory.forwardConnect(inputLayer, competitiveLayer, 1);

			// set input and output cells for this network
			NeuralNetworkFactory.DefaultIO = this;
		}

	}

}