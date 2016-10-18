namespace org.neuroph.samples.mnist.test
{

	using Evaluation = org.neuroph.contrib.eval.Evaluation;
	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;
	using MNISTDataSet = org.neuroph.samples.convolution.mnist.MNISTDataSet;


	/// <summary>
	/// Utility class used for metrics evaluation of neural network
	/// </summary>
	public class MetricTestMNIST
	{

		/// <param name="args"> command line arguments which represent paths to persisted neural network
		///             [0] - location of  neural network </param>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws java.io.IOException
		public static void Main(string[] args)
		{

			DataSet testSet = MNISTDataSet.createFromFile(MNISTDataSet.TEST_LABEL_NAME, MNISTDataSet.TEST_IMAGE_NAME, 10000);
			NeuralNetwork nn = NeuralNetwork.load(new FileInputStream(args[0]));

			Evaluation.runFullEvaluation(nn, testSet);
		}

	}

}