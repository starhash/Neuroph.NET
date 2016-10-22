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
	using Neuron = org.neuroph.core.Neuron;
	using Weight = org.neuroph.core.Weight;
	using DelayedConnection = org.neuroph.nnet.comp.DelayedConnection;
	using BiasNeuron = org.neuroph.nnet.comp.neuron.BiasNeuron;

	/// <summary>
	/// Provides methods to connect neurons by creating Connection objects.
	/// </summary>
	 public class ConnectionFactory
	 {

		/// <summary>
		/// Creates connection between two specified neurons
		/// </summary>
		/// <param name="fromNeuron">
		///            output neuron </param>
		/// <param name="toNeuron">
		///            input neuron </param>
		public static void createConnection(Neuron fromNeuron, Neuron toNeuron)
		{
			Connection connection = new Connection(fromNeuron, toNeuron);
			toNeuron.addInputConnection(connection);
		}

		/// <summary>
		/// Creates connection between two specified neurons
		/// </summary>
		/// <param name="fromNeuron">
		///            neuron to connect (connection source) </param>
		/// <param name="toNeuron">
		///            neuron to connect to (connection target) </param>
		/// <param name="weightVal">
		///            connection weight value </param>
		public static void createConnection(Neuron fromNeuron, Neuron toNeuron, double weightVal)
		{
			Connection connection = new Connection(fromNeuron, toNeuron, weightVal);
			toNeuron.addInputConnection(connection);
		}

		public static void createConnection(Neuron fromNeuron, Neuron toNeuron, double weightVal, int delay)
		{
			DelayedConnection connection = new DelayedConnection(fromNeuron, toNeuron, weightVal, delay);
			toNeuron.addInputConnection(connection);
		}

		/// <summary>
		/// Creates connection between two specified neurons
		/// </summary>
		/// <param name="fromNeuron">
		///            neuron to connect (connection source) </param>
		/// <param name="toNeuron">
		///            neuron to connect to (connection target) </param>
		/// <param name="weight">
		///            connection weight </param>
		public static void createConnection(Neuron fromNeuron, Neuron toNeuron, Weight weight)
		{
			Connection connection = new Connection(fromNeuron, toNeuron, weight);
			toNeuron.addInputConnection(connection);
		}


			/// <summary>
			/// Creates  connectivity between specified neuron and all neurons in specified layer
			/// </summary>
			/// <param name="fromNeuron">
			///            neuron to connect </param>
			/// <param name="toLayer">
			///            layer to connect to </param>
		public static void createConnection(Neuron fromNeuron, Layer toLayer)
		{
					foreach (Neuron toNeuron in toLayer.Neurons)
					{
						ConnectionFactory.createConnection(fromNeuron, toNeuron);
					}
		}


		/// <summary>
		/// Creates full connectivity between the two specified layers
		/// </summary>
		/// <param name="fromLayer">
		///            layer to connect </param>
		/// <param name="toLayer">
		///            layer to connect to </param>
		public static void fullConnect(Layer fromLayer, Layer toLayer)
		{
			foreach (Neuron fromNeuron in fromLayer.Neurons)
			{
				foreach (Neuron toNeuron in toLayer.Neurons)
				{
					createConnection(fromNeuron, toNeuron);
				}
			}
		}

		/// <summary>
		/// Creates full connectivity between the two specified layers
		/// </summary>
		/// <param name="fromLayer">
		///            layer to connect </param>
		/// <param name="toLayer">
		///            layer to connect to </param>
		public static void fullConnect(Layer fromLayer, Layer toLayer, bool connectBiasNeuron)
		{
			foreach (Neuron fromNeuron in fromLayer.Neurons)
			{
						if (fromNeuron is BiasNeuron)
						{
							continue;
						}
						foreach (Neuron toNeuron in toLayer.Neurons)
						{
				createConnection(fromNeuron, toNeuron);
						}
			}
		}



		/// <summary>
		/// Creates full connectivity between two specified layers with specified
		/// weight for all connections
		/// </summary>
		/// <param name="fromLayer">
		///            output layer </param>
		/// <param name="toLayer">
		///            input layer </param>
		/// <param name="weightVal">
		///             connection weight value </param>
		public static void fullConnect(Layer fromLayer, Layer toLayer, double weightVal)
		{
			foreach (Neuron fromNeuron in fromLayer.Neurons)
			{
				foreach (Neuron toNeuron in toLayer.Neurons)
				{
					createConnection(fromNeuron, toNeuron, weightVal);
				}
			}
		}

		/// <summary>
		/// Creates full connectivity within layer - each neuron with all other
		/// within the same layer
		/// </summary>
		public static void fullConnect(Layer layer)
		{
			int neuronNum = layer.NeuronsCount;
			for (int i = 0; i < neuronNum; i++)
			{
				for (int j = 0; j < neuronNum; j++)
				{
					if (j == i)
					{
						continue;
					}
					Neuron from = layer.getNeuronAt(i);
					Neuron to = layer.getNeuronAt(j);
					createConnection(from, to);
				} // j
			} // i
		}

		/// <summary>
		/// Creates full connectivity within layer - each neuron with all other
		/// within the same layer with the specified weight values for all
		/// conections.
		/// </summary>
		public static void fullConnect(Layer layer, double weightVal)
		{
			int neuronNum = layer.NeuronsCount;
			for (int i = 0; i < neuronNum; i++)
			{
				for (int j = 0; j < neuronNum; j++)
				{
					if (j == i)
					{
						continue;
					}
					Neuron from = layer.getNeuronAt(i);
					Neuron to = layer.getNeuronAt(j);
					createConnection(from, to, weightVal);
				} // j
			} // i
		}

		/// <summary>
		/// Creates full connectivity within layer - each neuron with all other
		/// within the same layer with the specified weight and delay values for all
		/// conections.
		/// </summary>
		public static void fullConnect(Layer layer, double weightVal, int delay)
		{
			int neuronNum = layer.NeuronsCount;
			for (int i = 0; i < neuronNum; i++)
			{
				for (int j = 0; j < neuronNum; j++)
				{
					if (j == i)
					{
						continue;
					}
					Neuron from = layer.getNeuronAt(i);
					Neuron to = layer.getNeuronAt(j);
					createConnection(from, to, weightVal, delay);
				} // j
			} // i
		}

		/// <summary>
		/// Creates forward connectivity pattern between the specified layers
		/// </summary>
		/// <param name="fromLayer">
		///            layer to connect </param>
		/// <param name="toLayer">
		///            layer to connect to </param>
		public static void forwardConnect(Layer fromLayer, Layer toLayer, double weightVal)
		{
			for (int i = 0; i < fromLayer.NeuronsCount; i++)
			{
				Neuron fromNeuron = fromLayer.getNeuronAt(i);
				Neuron toNeuron = toLayer.getNeuronAt(i);
				createConnection(fromNeuron, toNeuron, weightVal);
			}
		}

		/// <summary>
		/// Creates forward connection pattern between specified layers
		/// </summary>
		/// <param name="fromLayer">
		///            layer to connect </param>
		/// <param name="toLayer">
		///            layer to connect to </param>
		public static void forwardConnect(Layer fromLayer, Layer toLayer)
		{
			for (int i = 0; i < fromLayer.NeuronsCount; i++)
			{
				Neuron fromNeuron = fromLayer.getNeuronAt(i);
				Neuron toNeuron = toLayer.getNeuronAt(i);
				createConnection(fromNeuron, toNeuron, 1);
			}
		}

	 }
}