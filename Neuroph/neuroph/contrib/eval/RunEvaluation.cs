namespace org.neuroph.contrib.eval
{

	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;
	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;

	/// 
	/// <summary>
	/// @author zoran
	/// </summary>
	public class RunEvaluation
	{

		/// <param name="args"> the command line arguments </param>
		public static void Main(string[] args)
		{
			NeuralNetwork nnet = NeuralNetwork.createFromFile("MicrNetwork.nnet");
			DataSet dataSet = DataSet.load("MicrDataColor.tset");

			Evaluation.runFullEvaluation(nnet, dataSet);

		}

	}

}