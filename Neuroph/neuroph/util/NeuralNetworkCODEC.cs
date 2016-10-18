using System.Collections.Generic;

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
namespace org.neuroph.util
{

	using Connection = org.neuroph.core.Connection;
	using Layer = org.neuroph.core.Layer;
	using org.neuroph.core;
	using Neuron = org.neuroph.core.Neuron;

	/// <summary>
	/// A CODEC encodes and decodes neural networks, much like the more standard
	/// definition of a CODEC encodes and decodes audio/video.
	/// 
	/// This CODEC can encode a neural network to an array of doubles. It can also
	/// decode this array of doubles back into a neural network. This is very useful
	/// for both simulated annealing and genetic algorithms.
	/// 
	/// @author Jeff Heaton (http://www.heatonresearch.com)
	/// </summary>
	public class NeuralNetworkCODEC
	{

		/// <summary>
		/// Private constructor.
		/// </summary>
		private NeuralNetworkCODEC()
		{

		}

		/// <summary>
		/// Encode a network to an array. </summary>
		/// <param name="network"> The network to encode. </param>
		public static void network2array(NeuralNetwork network, double[] array)
		{
			int index = 0;

					 List<Layer> layers = network.Layers;
			foreach (Layer layer in layers)
			{
				foreach (Neuron neuron in layer.Neurons)
				{
					foreach (Connection connection in neuron.OutConnections)
					{
						array[index++] = connection.Weight.Value;
					}
				}
			}
		}

		/// <summary>
		/// Decode a network from an array. </summary>
		/// <param name="array"> The array used to decode. </param>
		/// <param name="network"> The network to decode into. </param>
		public static void array2network(double[] array, NeuralNetwork network)
		{
			int index = 0;

					List<Layer> layers = network.Layers;
					foreach (Layer layer in layers)
					{
				foreach (Neuron neuron in layer.Neurons)
				{
					foreach (Connection connection in neuron.OutConnections)
					{
						connection.Weight.Value = array[index++];
						//connection.getWeight().setPreviousValue(array[index++]);
					}
				}
					}
		}

		/// <summary>
		/// Determine the array size for the given neural network. </summary>
		/// <param name="network"> The neural network to determine for. </param>
		/// <returns> The size of the array necessary to hold that network. </returns>
		public static int determineArraySize(NeuralNetwork network)
		{
			int result = 0;

					List<Layer> layers = network.Layers;
			foreach (Layer layer in layers)
			{
				foreach (Neuron neuron in layer.Neurons)
				{
					result += neuron.OutConnections.Count;
				}
			}
			return result;
		}
	}

}