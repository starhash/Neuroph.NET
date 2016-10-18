namespace org.neuroph.samples.evaluation.optimization
{

	using Evaluation = org.neuroph.contrib.eval.Evaluation;
	using org.neuroph.contrib.model.modelselection;
	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;
	using BackPropagation = org.neuroph.nnet.learning.BackPropagation;
	using MNISTDataSet = org.neuroph.samples.convolution.mnist.MNISTDataSet;

	/// <summary>
	/// Example which demonstrated how to use MultilayerPerceptronOptimazer in order to create optimal
	/// network architecture for MNIST dataset
	/// <p/>
	/// Default optimization parameters are used.
	/// </summary>
	public class MLPMNISTOptimization
	{

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws java.io.IOException
		public static void Main(string[] args)
		{

			DataSet trainSet = MNISTDataSet.createFromFile(MNISTDataSet.TRAIN_LABEL_NAME, MNISTDataSet.TRAIN_IMAGE_NAME, 200);
			DataSet testSet = MNISTDataSet.createFromFile(MNISTDataSet.TEST_LABEL_NAME, MNISTDataSet.TEST_IMAGE_NAME, 10000);
			BackPropagation learningRule = createLearningRule();


			NeuralNetwork neuralNet = (new MultilayerPerceptronOptimazer<>()).withLearningRule(learningRule).createOptimalModel(trainSet);

			Evaluation.runFullEvaluation(neuralNet, testSet);
		}

		private static BackPropagation createLearningRule()
		{
			BackPropagation learningRule = new BackPropagation();
			learningRule.MaxIterations = 100;
			learningRule.MaxError = 0.0001;
			return learningRule;
		}


	}

}