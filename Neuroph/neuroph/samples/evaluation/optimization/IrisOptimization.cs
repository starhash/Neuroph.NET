namespace org.neuroph.samples.evaluation.optimization
{

	using Evaluation = org.neuroph.contrib.eval.Evaluation;
	using org.neuroph.contrib.model.modelselection;
	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;
	using BackPropagation = org.neuroph.nnet.learning.BackPropagation;

	/// <summary>
	/// Example which demonstrated how to use MultilayerPerceptronOptimazer in order to create optimal
	/// network architecture for IRIS dataset
	/// <p/>
	/// Default optimization parameters are used.
	/// </summary>
	public class IrisOptimization
	{


		public static void Main(string[] args)
		{
			string inputFileName = "/iris_data.txt";

			DataSet irisDataSet = DataSet.createFromFile(inputFileName, 4, 3, ",", false);
			BackPropagation learningRule = createLearningRule();

			NeuralNetwork neuralNet = (new MultilayerPerceptronOptimazer<>()).withLearningRule(learningRule).createOptimalModel(irisDataSet);

			neuralNet.learn(irisDataSet);
			Evaluation.runFullEvaluation(neuralNet, irisDataSet);

		}

		private static BackPropagation createLearningRule()
		{
			BackPropagation learningRule = new BackPropagation();
			learningRule.MaxIterations = 50;
			learningRule.MaxError = 0.0001;
			return learningRule;
		}

	}

}