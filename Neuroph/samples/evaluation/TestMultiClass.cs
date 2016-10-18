namespace org.neuroph.samples.evaluation
{

	using Evaluation = org.neuroph.contrib.eval.Evaluation;
	using DataSet = org.neuroph.core.data.DataSet;
	using MeanSquaredError = org.neuroph.core.learning.error.MeanSquaredError;
	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;


	/// <summary>
	/// Simple example which shows how to use EvaluationService on Multi-class classification problem (IRIS dataset)
	/// </summary>
	public class TestMultiClass
	{

		private const string inputFileName = "/iris_data.txt";


		public static void Main(string[] args)
		{

			DataSet irisDataSet = loadDataSet();

			MultiLayerPerceptron neuralNet = new MultiLayerPerceptron(4, 15, 3);

			configureLearningRule(neuralNet);
			neuralNet.learn(irisDataSet);

			Evaluation.runFullEvaluation(neuralNet, irisDataSet);
		}

		private static DataSet loadDataSet()
		{
			DataSet irisDataSet = DataSet.createFromFile(inputFileName, 4, 3, ",", false);
			irisDataSet.shuffle();
			return irisDataSet;
		}

		private static void configureLearningRule(MultiLayerPerceptron neuralNet)
		{
			neuralNet.LearningRule.LearningRate = 0.02;
			neuralNet.LearningRule.MaxError = 0.01;
			neuralNet.LearningRule.ErrorFunction = new MeanSquaredError();
		}

	}

}