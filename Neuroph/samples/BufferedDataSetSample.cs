using System;

namespace org.neuroph.samples
{

	using BufferedDataSet = org.neuroph.core.data.BufferedDataSet;
	using LearningEvent = org.neuroph.core.events.LearningEvent;
	using LearningEventListener = org.neuroph.core.events.LearningEventListener;
	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;
	using BackPropagation = org.neuroph.nnet.learning.BackPropagation;

	/// 
	/// <summary>
	/// @author zoran
	/// </summary>
	public class BufferedDataSetSample : LearningEventListener
	{


//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws java.io.FileNotFoundException
		public static void Main(string[] args)
		{
			(new BufferedDataSetSample()).run();

		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void run() throws java.io.FileNotFoundException
		public virtual void run()
		{
			string inputFileName = typeof(BufferedDataSetSample).getResource("data/iris_data_normalised.txt").File;

			// create MultiLayerPerceptron neural network
			MultiLayerPerceptron neuralNet = new MultiLayerPerceptron(4, 16, 3);

			BufferedDataSet irisDataSet = new BufferedDataSet(new File(inputFileName), 4, 3, ",");

			neuralNet.LearningRule.addListener(this);
			neuralNet.learn(irisDataSet);

		   // neuralNet.getLearningRule().setMaxError(0.001);
		}

		public virtual void handleLearningEvent(LearningEvent @event)
		{
			BackPropagation bp = (BackPropagation)@event.Source;
			Console.WriteLine(bp.CurrentIteration + ". iteration : " + bp.TotalNetworkError);
		}


	}

}