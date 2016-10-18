using System;

namespace org.neuroph.samples
{

	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;
	using LearningEvent = org.neuroph.core.events.LearningEvent;
	using LearningEventListener = org.neuroph.core.events.LearningEventListener;
	using RBFNetwork = org.neuroph.nnet.RBFNetwork;
	using LMS = org.neuroph.nnet.learning.LMS;
	using RBFLearning = org.neuroph.nnet.learning.RBFLearning;

	/// <summary>
	/// TODO: use k-nearest neighbour to tweak sigma
	/// @author zoran
	/// </summary>
	public class RBFClassificationSample : LearningEventListener
	{

		/// <summary>
		///  Runs this sample
		/// </summary>
		public static void Main(string[] args)
		{
			  (new RBFClassificationSample()).run();
		}


		public virtual void run()
		{
			// get the path to file with data
			string inputFileName = "data_sets/sine.csv";

			// create MultiLayerPerceptron neural network
			RBFNetwork neuralNet = new RBFNetwork(1, 15, 1);

			// create training set from file
			DataSet dataSet = DataSet.createFromFile(inputFileName, 1, 1, ",", false);

			RBFLearning learningRule = ((RBFLearning)neuralNet.LearningRule);
			learningRule.LearningRate = 0.02;
			learningRule.MaxError = 0.01;
			learningRule.addListener(this);

			// train the network with training set
			neuralNet.learn(dataSet);

			Console.WriteLine("Done training.");
			Console.WriteLine("Testing network...");

			testNeuralNetwork(neuralNet, dataSet);
		}

		/// <summary>
		/// Prints network output for the each element from the specified training set. </summary>
		/// <param name="neuralNet"> neural network </param>
		/// <param name="testSet"> test data set </param>
		public virtual void testNeuralNetwork(NeuralNetwork neuralNet, DataSet testSet)
		{
			foreach (DataSetRow testSetRow in testSet.Rows)
			{
				neuralNet.Input = testSetRow.Input;
				neuralNet.calculate();
				double[] networkOutput = neuralNet.Output;

				Console.Write("Input: " + Arrays.ToString(testSetRow.Input));
				Console.WriteLine(" Output: " + Arrays.ToString(networkOutput));
			}
		}

		public virtual void handleLearningEvent(LearningEvent @event)
		{
			LMS lr = (LMS) @event.Source;
			Console.WriteLine(lr.CurrentIteration + ". iteration | Total network error: " + lr.TotalNetworkError);
		}

	}

}