using System;

namespace org.neuroph.samples.mnist.learn
{


	using CrossValidation = org.neuroph.contrib.model.errorestimation.CrossValidation;
	using CrossValidation = org.neuroph.contrib.model.errorestimation.CrossValidation;
	using Evaluation = org.neuroph.contrib.eval.Evaluation;
	using org.neuroph.contrib.model.modelselection;
	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;
	using BackPropagation = org.neuroph.nnet.learning.BackPropagation;
	using MNISTDataSet = org.neuroph.samples.convolution.mnist.MNISTDataSet;
	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	/// <summary>
	/// Utility class which can be used from command prompt to train MLP
	/// </summary>
	public class MultiLayerMNIST
	{

		private static Logger LOG = LoggerFactory.getLogger(typeof(MultiLayerMNIST));


		/// <param name="args"> Command line parameters used to initialize parameters of multi layer neural network optimizer
		///             [0] - maximal number of epochs during learning
		///             [1] - learning error stop condition
		///             [2] - learning rate used during learning process
		///             [3] - number of validation folds
		///             [4] - max number of layers in neural network
		///             [5] - min neuron count per layer
		///             [6] - max neuron count per layer
		///             [7] - neuron increment count </param>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws java.io.IOException
		public static void Main(string[] args)
		{

			int maxIter = 10000; //Integer.parseInt(args[0]);
			double maxError = 0.01; // Double.parseDouble(args[1]);
			double learningRate = 0.2; // Double.parseDouble(args[2]);

			int validationFolds = Convert.ToInt32(args[3]);

			int maxLayers = Convert.ToInt32(args[4]);
			int minNeuronCount = Convert.ToInt32(args[5]);
			int maxNeuronCount = Convert.ToInt32(args[6]);
			int neuronIncrement = Convert.ToInt32(args[7]);

			LOG.info("MLP learning for MNIST started.....");

			DataSet trainSet = MNISTDataSet.createFromFile(MNISTDataSet.TRAIN_LABEL_NAME, MNISTDataSet.TRAIN_IMAGE_NAME, 60000);
			DataSet testSet = MNISTDataSet.createFromFile(MNISTDataSet.TEST_LABEL_NAME, MNISTDataSet.TEST_IMAGE_NAME, 10000);

			BackPropagation bp = new BackPropagation();
			bp.MaxIterations = maxIter;
			bp.MaxError = maxError;
			bp.LearningRate = learningRate;
	// commented out due to errors
	//        KFoldCrossValidation errorEstimationMethod = new KFoldCrossValidation(neuralNet, trainSet, validationFolds);
	//
	//        NeuralNetwork neuralNet = new MultilayerPerceptronOptimazer<>()
	//                .withLearningRule(bp)
	//                .withErrorEstimationMethod(errorEstimationMethod)
	//                .withMaxLayers(maxLayers)
	//                .withMaxNeurons(maxNeuronCount)
	//                .withMinNeurons(minNeuronCount)
	//                .withNeuronIncrement(neuronIncrement)
	//                .createOptimalModel(trainSet);

			LOG.info("Evaluating model on Test Set.....");
	// commented out due to errors
		  //  Evaluation.runFullEvaluation(neuralNet, testSet);

			LOG.info("MLP learning for MNIST successfully finished.....");
		}
	}

}