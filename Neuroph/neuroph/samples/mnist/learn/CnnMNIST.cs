using System;

namespace org.neuroph.samples.mnist.learn
{

	using Evaluation = org.neuroph.contrib.eval.Evaluation;
	using org.neuroph.core;
	using MeanSquaredError = org.neuroph.core.learning.error.MeanSquaredError;
	using ConvolutionalNetwork = org.neuroph.nnet.ConvolutionalNetwork;
	using Kernel = org.neuroph.nnet.comp.Kernel;
	using FeatureMapLayer = org.neuroph.nnet.comp.layer.FeatureMapLayer;
	using DataSet = org.neuroph.core.data.DataSet;
	using LearningEvent = org.neuroph.core.events.LearningEvent;
	using LearningEventListener = org.neuroph.core.events.LearningEventListener;
	using Dimension2D = org.neuroph.nnet.comp.Dimension2D;
	using BackPropagation = org.neuroph.nnet.learning.BackPropagation;
	using ConvolutionalBackpropagation = org.neuroph.nnet.learning.ConvolutionalBackpropagation;
	using MomentumBackpropagation = org.neuroph.nnet.learning.MomentumBackpropagation;
	using MNISTDataSet = org.neuroph.samples.convolution.mnist.MNISTDataSet;
	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	/// <summary>
	/// Utility class which can be used from command prompt to train ConvolutionalNetwork
	/// </summary>
	public class CnnMNIST
	{

		private static Logger LOG = LoggerFactory.getLogger(typeof(CnnMNIST));


		/// <param name="args"> Command line parameters used to initialize parameters of convolutional network
		///             [0] - maximal number of epochs during learning
		///             [1] - learning error stop condition
		///             [2] - learning rate used during learning process
		///             [3] - number of feature maps in 1st convolutional layer
		///             [4] - number of feature maps in 2nd convolutional layer
		///             [5] - number of feature maps in 3rd convolutional layer </param>
		public static void Main(string[] args)
		{
			try
			{
				int maxIter = 10000; // Integer.parseInt(args[0]);
				double maxError = 0.01; //Double.parseDouble(args[1]);
				double learningRate = 0.2; //  Double.parseDouble(args[2]);

				int layer1 = Convert.ToInt32(args[3]);
				int layer2 = Convert.ToInt32(args[4]);
				int layer3 = Convert.ToInt32(args[5]);

				LOG.info("{}-{}-{}", layer1, layer2, layer3);

				DataSet trainSet = MNISTDataSet.createFromFile(MNISTDataSet.TRAIN_LABEL_NAME, MNISTDataSet.TRAIN_IMAGE_NAME, 100);
				DataSet testSet = MNISTDataSet.createFromFile(MNISTDataSet.TEST_LABEL_NAME, MNISTDataSet.TEST_IMAGE_NAME, 10000);

				Dimension2D inputDimension = new Dimension2D(32, 32);
				Dimension2D convolutionKernel = new Dimension2D(5, 5);
				Dimension2D poolingKernel = new Dimension2D(2, 2);

				ConvolutionalNetwork convolutionNetwork = (new ConvolutionalNetwork.Builder()).withInputLayer(32, 32, 1).withConvolutionLayer(5, 5, layer1).withPoolingLayer(2, 2).withConvolutionLayer(5, 5, layer2).withPoolingLayer(2, 2).withConvolutionLayer(5, 5, layer3).withFullConnectedLayer(10).build();

				ConvolutionalBackpropagation backPropagation = new ConvolutionalBackpropagation();
				backPropagation.LearningRate = learningRate;
				backPropagation.MaxError = maxError;
				backPropagation.MaxIterations = maxIter;
				backPropagation.addListener(new LearningListener(convolutionNetwork, testSet));
				backPropagation.ErrorFunction = new MeanSquaredError();

				convolutionNetwork.LearningRule = backPropagation;
				convolutionNetwork.learn(trainSet);

				Evaluation.runFullEvaluation(convolutionNetwork, testSet);


			}
			catch (IOException e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
			}
		}


		private class LearningListener : LearningEventListener
		{

			internal readonly NeuralNetwork neuralNetwork;
			internal DataSet testSet;

			public LearningListener(NeuralNetwork neuralNetwork, DataSet testSet)
			{
				this.testSet = testSet;
				this.neuralNetwork = neuralNetwork;
			}


			internal long start = DateTimeHelperClass.CurrentUnixTimeMillis();

			public virtual void handleLearningEvent(LearningEvent @event)
			{
				BackPropagation bp = (BackPropagation) @event.Source;
				LOG.info("Epoch no#: [{}]. Error [{}]", bp.CurrentIteration, bp.TotalNetworkError);
				LOG.info("Epoch execution time: {} sec", (DateTimeHelperClass.CurrentUnixTimeMillis() - start) / 1000.0);
			   // neuralNetwork.save(bp.getCurrentIteration() + "_MNIST_CNN-MIC.nnet");

				start = DateTimeHelperClass.CurrentUnixTimeMillis();
			  //  if (bp.getCurrentIteration() % 5 == 0)
			  //      Evaluation.runFullEvaluation(neuralNetwork, testSet);
			}

		}


	}

}