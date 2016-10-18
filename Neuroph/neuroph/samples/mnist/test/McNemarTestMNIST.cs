namespace org.neuroph.samples.mnist.test
{

	using McNemarTest = org.neuroph.contrib.eval.classification.McNemarTest;
	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;
	using MNISTDataSet = org.neuroph.samples.convolution.mnist.MNISTDataSet;


	/// <summary>
	/// McNemar test calculated for MNIST data set
	/// </summary>
	public class McNemarTestMNIST
	{


		/// <param name="args"> command line arguments which represent paths to persisted neural networks
		///             [0] - location of first neural network
		///             [1] - location of second neural network </param>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws java.io.IOException
		public static void Main(string[] args)
		{

			DataSet testSet = MNISTDataSet.createFromFile(MNISTDataSet.TEST_LABEL_NAME, MNISTDataSet.TEST_IMAGE_NAME, 10000);

			NeuralNetwork nn1 = NeuralNetwork.load(new FileInputStream(args[0]));
			NeuralNetwork nn2 = NeuralNetwork.load(new FileInputStream(args[1]));

			(new McNemarTest()).evaluateNetworks(nn1, nn2, testSet);
		}

	}

}