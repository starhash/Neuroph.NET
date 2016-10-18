using System;

namespace org.neuroph.contrib.samples.timeseries
{

    using org.neuroph.core;
    using DataSet = org.neuroph.core.data.DataSet;
    using DataSetRow = org.neuroph.core.data.DataSetRow;
    using LearningEvent = org.neuroph.core.events.LearningEvent;
    using LearningEventListener = org.neuroph.core.events.LearningEventListener;
    using SupervisedLearning = org.neuroph.core.learning.SupervisedLearning;
    using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;
    using MomentumBackpropagation = org.neuroph.nnet.learning.MomentumBackpropagation;
    using TransferFunctionType = org.neuroph.util.TransferFunctionType;
    using System.Linq;

    /// 
    /// <summary>
    /// @author zoran
    /// </summary>
    public class TestTimeSeries : LearningEventListener
	{
//JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
//ORIGINAL LINE: org.neuroph.core.NeuralNetwork<?> neuralNet;
		internal NeuralNetwork neuralNet;
		internal DataSet trainingSet;

	  public static void Main(string[] args)
	  {
		  TestTimeSeries tts = new TestTimeSeries();
		  tts.train();
		  tts.testNeuralNetwork();
	  }

		public virtual void train()
		{
			// get the path to file with data
			string inputFileName = "C:\\timeseries\\BSW15";

			// create MultiLayerPerceptron neural network
			neuralNet = new MultiLayerPerceptron(TransferFunctionType.TANH, 5, 10, 1);
			MomentumBackpropagation learningRule = (MomentumBackpropagation)neuralNet.LearningRule;
			learningRule.LearningRate = 0.2;
			learningRule.Momentum = 0.5;
			// learningRule.addObserver(this);
			learningRule.addListener(this);

			// create training set from file
			 trainingSet = DataSet.createFromFile(inputFileName, 5, 1, "\t", false);
			// train the network with training set
			neuralNet.learn(trainingSet);

			System.Console.WriteLine("Done training.");
		}


		/// <summary>
		/// Prints network output for the each element from the specified training set. </summary>
		/// <param name="neuralNet"> neural network </param>
		/// <param name="trainingSet"> training set </param>
		public virtual void testNeuralNetwork()
		{
			System.Console.WriteLine("Testing network...");
			foreach (DataSetRow trainingElement in trainingSet.Rows)
			{
				neuralNet.Input = trainingElement.Input;
				neuralNet.calculate();
				double[] networkOutput = neuralNet.Output;

				Console.Write("Input: " + trainingElement.Input.Aggregate("", (x, y) => x + ", " + y).Trim(','));
				System.Console.WriteLine(" Output: " + networkOutput.Aggregate("", (x, y) => x + ", " + y).Trim(','));
			}
		}

	//	@Override
	//	public void update(Observable arg0, Object arg1) {
	//		SupervisedLearning rule = (SupervisedLearning)arg0;
	//		System.out.println( "Training, Network Epoch " + rule.getCurrentIteration() + ", Error:" + rule.getTotalNetworkError());
	//	}

		public virtual void handleLearningEvent(LearningEvent @event)
		{
			SupervisedLearning rule = (SupervisedLearning)@event.getSource();
			System.Console.WriteLine("Training, Network Epoch " + rule.CurrentIteration + ", Error:" + rule.TotalNetworkError);
		}

	}

}