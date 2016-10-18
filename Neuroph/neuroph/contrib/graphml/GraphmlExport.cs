using System;

namespace org.neuroph.contrib.graphml
{


    using org.neuroph.core;
    using Neuron = org.neuroph.core.Neuron;
    using BiasNeuron = org.neuroph.nnet.comp.neuron.BiasNeuron;
    using InputNeuron = org.neuroph.nnet.comp.neuron.InputNeuron;
    using java.io;

    /// <summary>
    /// The main class to export neural networks to graphml class. 
    /// To export: 
    /// 
    ///  1. Initiate class with Neural network. 
    ///  2. Parse neural network to graphml representation 
    ///  3. Print to file/STDOUT 
    /// 
    /// @author fernando carrillo (fernando@carrillo.at)
    /// </summary>
    public class GraphmlExport
	{
		private NeuralNetwork ann;
		private XMLElement graphml;

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public GraphmlExport(final org.neuroph.core.NeuralNetwork ann)
		public GraphmlExport(NeuralNetwork ann)
		{
			this.ann = ann;
			labelUnmarkedNeurons(this.ann);
		}

		/// <summary>
		/// Parses neural network to graphml object. 
		/// </summary>
		public virtual void parse()
		{
			graphml = new Graphml();
			graphml.appendChild(createGraph(this.ann));
		}


		/// <summary>
		/// Writes graphml object to specified file. </summary>
		/// <param name="filePathOut"> </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public void writeToFile(final String filePathOut)
		public virtual void writeToFile(string filePathOut)
		{

			try
			{
				File file = new File(filePathOut);
				file.createNewFile();
				print(new PrintStream(file));
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e.ToString());
                System.Console.Write(e.StackTrace);
			}
		}

		/// <summary>
		/// Prints graphml object to STDOUT
		/// </summary>
		public virtual void printToStdout()
		{
			print(java.lang.System.@out);
		}

		/// <summary>
		/// Print graphml representation to PrintStream </summary>
		/// <param name="out"> </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: private void print(final java.io.PrintStream out)
		private void print(PrintStream @out)
		{
			@out.println(XMLHeader.Header);
			@out.println(graphml);
		}

		/// <summary>
		/// Create XML graph from neuroph neural network. </summary>
		/// <param name="ann">
		/// @return </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: private Graph createGraph(final org.neuroph.core.NeuralNetwork ann)
		private Graph createGraph(NeuralNetwork ann)
		{
			string id = ann.Label;
			if (id == null || id.Length == 0)
			{
				id = "defaultId";
			}
			Graph graph = new Graph(id);
			graph.addNetwork(ann);

			return graph;
		}

		/// <summary>
		/// Labels neurons which are yet unlabelled. 
		/// </summary>
		/// <param name="ann"> </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: private void labelUnmarkedNeurons(final org.neuroph.core.NeuralNetwork ann)
		private void labelUnmarkedNeurons(NeuralNetwork ann)
		{

			for (int layer = 0; layer < ann.LayersCount; layer++)
			{

				int neuronCount = 0;
				foreach (Neuron neuron in ann.getLayerAt(layer).Neurons)
				{

					labelNeuron(layer, neuronCount, neuron);
					neuronCount++;
				}
			}
		}

		/// <summary>
		/// Labels unlabelled neuron according to following rules. 
		/// 1. If Input neuron: "Input-[neuronCount]"
		/// 2. If Bias neuron: "L[layer]-bias"
		/// 3. otherwise: L[layer]-[neuronCount] </summary>
		/// <param name="layer"> </param>
		/// <param name="neuronCount"> </param>
		/// <param name="neuron"> </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: private void labelNeuron(final int layer, final int neuronCount, final org.neuroph.core.Neuron neuron)
		private void labelNeuron(int layer, int neuronCount, Neuron neuron)
		{
			if (neuron.Label == null)
			{

				if (neuron.GetType() == typeof(InputNeuron))
				{
					neuron.Label = "Input-" + neuronCount;
				}
				else if (neuron.GetType() == typeof(BiasNeuron))
				{
					neuron.Label = "L" + layer + "-bias";
				}
				else
				{
					neuron.Label = "L" + layer + "-" + neuronCount;
				}

			}
		}

		//Getter
		public virtual NeuralNetwork NeuralNetwork
		{
			get
			{
				return this.ann;
			}
		}
		public virtual XMLElement Graphml
		{
			get
			{
				return this.graphml;
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws java.io.IOException
		public static void Main(string[] args)
		{
			GraphmlExport ge = new GraphmlExport(ExampleNetworXOR.Network);
			ge.parse();
			ge.printToStdout();
		}

	}

}