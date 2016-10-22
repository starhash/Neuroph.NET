using System;

namespace org.neuroph.contrib.graphml
{

	using Connection = org.neuroph.core.Connection;
	using org.neuroph.core;
	using Neuron = org.neuroph.core.Neuron;

	/// <summary>
	/// The parent XML element holding nodes and edges of a (sub)graph.  
	/// 
	/// Characterized by two attributes: 
	/// 1. The Id of the graph. 
	/// 2. The edgetype. For neural network representation directed is chosen. 
	/// 
	/// Contains all edges and nodes of the network as child elements. 
	/// 
	/// @author fernando carrillo (fernando@carrillo.at)
	/// 
	/// </summary>
	public class Graph : XMLElement
	{
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public Graph(final String id)
		public Graph(string id)
		{
			addAttribute(new XMLAttribute("id", id));
			addAttribute(new XMLAttribute("edgedefault", "directed"));
		}

		/// <summary>
		/// Adds a neuroph neural network to the graphml reprentation. </summary>
		/// <param name="ann"> </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public void addNetwork(final org.neuroph.core.NeuralNetwork ann)
		public virtual void addNetwork(NeuralNetwork ann)
		{

			for (int layer = 0; layer < ann.LayersCount; layer++)
			{
				foreach (Neuron neuron in ann.getLayerAt(layer).Neurons)
				{
					addNode(neuron);
					addEdges(neuron);
				}
			}
		}

		/// <summary>
		/// Adds a child element of type node for the given neuron </summary>
		/// <param name="neuron"> </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: private void addNode(final org.neuroph.core.Neuron neuron)
		private void addNode(Neuron neuron)
		{
			appendChild(new Node(neuron.Label));
		}

		/// <summary>
		/// Adds a child element of type edge for each connection in the given neuron. </summary>
		/// <param name="neuron"> </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: private void addEdges(final org.neuroph.core.Neuron neuron)
		private void addEdges(Neuron neuron)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String source = neuron.getLabel();
			string source = neuron.Label;

			string target;
			string weight;
			string weightKeyId = "d1";

			foreach (Connection con in neuron.OutConnections)
			{
				target = con.ToNeuron.Label;
				weight = Convert.ToString(con.Weight);

				appendChild(new Edge(source, target, weightKeyId, weight));
			}
		}


		public static void Main(string[] args)
		{
			Graph graph = new Graph("graph1");
			System.Console.WriteLine(graph);
		}

		public override string Tag
		{
			get
			{
				return "graph";
			}
		}

	}

}