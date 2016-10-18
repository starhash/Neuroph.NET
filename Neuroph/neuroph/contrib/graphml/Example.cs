namespace org.neuroph.contrib.graphml
{

	using org.neuroph.core;


	/// <summary>
	/// Example on how to export a neural network to graphml. 
	///  
	/// @author fernando carrillo (fernando@carrillo.at)
	/// 
	/// </summary>
	public class Example
	{
		/// <summary>
		/// 1. Generate trained artificial neural network.
		/// 2. Create GraphmlExport instance. 
		/// 3. Parse the artificial neural network. 
		/// 4. Print to STDOUT
		///   
		/// </summary>
		public Example()
		{
			NeuralNetwork ann = ExampleNetworXOR.Network;
			GraphmlExport ge = new GraphmlExport(ann);
			ge.parse();
			ge.printToStdout();
		}

		public static void Main(string[] args)
		{
			new Example();
		}
	}

}