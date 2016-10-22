using System;

namespace org.neuroph.samples.adalineDigits
{

	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;
	using LearningEvent = org.neuroph.core.events.LearningEvent;
	using LearningEventListener = org.neuroph.core.events.LearningEventListener;
	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;
	using BackPropagation = org.neuroph.nnet.learning.BackPropagation;

	/// 
	/// <summary>
	/// @author Ivan Petrovic
	/// </summary>
	/*
	 In this sample MultiLayerPerceptron network is used for pattern recognition.  The 
	 input pattern must match EXACTLY with what the network was trained with.
	
	 This example teaches the adaline to recognize the 10 digits. 
	
	
	 This is based on a an example from Encog (Encog Examples/org.encog.examples.neural.adaline).
	 
	 Encog example is based on a an example by Karsten Kutza, 
	 written in C on 1996-01-24.
	 http://www.neural-networks-at-your-fingertips.com
	 */
	public class DigitsRecognition
	{

		public static void Main(string[] args)
		{

			//create training set from Data.DIGITS
			DataSet dataSet = generateTraining();

			int inputCount = Data.CHAR_HEIGHT * Data.CHAR_WIDTH;
			int outputCount = Data.DIGITS.Length;
			int hiddenNeurons = 19;


			//create neural network
			MultiLayerPerceptron neuralNet = new MultiLayerPerceptron(inputCount, hiddenNeurons, outputCount);
			//get backpropagation learning rule from network
			BackPropagation learningRule = neuralNet.LearningRule;

			learningRule.LearningRate = 0.5;
			learningRule.MaxError = 0.001;
			learningRule.MaxIterations = 5000;

			//add learning listener in order to print out training info
			learningRule.addListener(new LearningEventListenerAnonymousInnerClassHelper());

			//train neural network
			neuralNet.learn(dataSet);

			//train the network with training set
			testNeuralNetwork(neuralNet, dataSet);

		}

		private class LearningEventListenerAnonymousInnerClassHelper : LearningEventListener
		{
			public LearningEventListenerAnonymousInnerClassHelper()
			{
			}

			public virtual void handleLearningEvent(LearningEvent @event)
			{
				BackPropagation bp = (BackPropagation) @event.Source;
				if (@event.EventType.Equals(LearningEvent.Type.LEARNING_STOPPED))
				{
					Console.WriteLine();
					Console.WriteLine("Training completed in " + bp.CurrentIteration + " iterations");
					Console.WriteLine("With total error " + bp.TotalNetworkError + '\n');
				}
				else
				{
					Console.WriteLine("Iteration: " + bp.CurrentIteration + " | Network error: " + bp.TotalNetworkError);
				}
			}
		}

		/// <summary>
		/// Prints network output for the each element from the specified training
		/// set.
		/// </summary>
		/// <param name="neuralNet"> neural network </param>
		/// <param name="testSet"> test data set </param>
		public static void testNeuralNetwork(NeuralNetwork neuralNet, DataSet testSet)
		{

			Console.WriteLine("--------------------------------------------------------------------");
			Console.WriteLine("***********************TESTING NEURAL NETWORK***********************");
			foreach (DataSetRow testSetRow in testSet.Rows)
			{
				neuralNet.Input = testSetRow.Input;
				neuralNet.calculate();

				int izlaz = maxOutput(neuralNet.Output);

				string[] niz = Data.convertDataIntoImage(testSetRow.Input);

				for (int i = 0; i < niz.Length; i++)
				{
					if (i != niz.Length - 1)
					{
						Console.WriteLine(niz[i]);
					}
					else
					{
						Console.WriteLine(niz[i] + "----> " + izlaz);
					}
				}
				Console.WriteLine("");
			}
		}

		public static DataSet generateTraining()
		{

			DataSet dataSet = new DataSet(Data.CHAR_WIDTH * Data.CHAR_HEIGHT, Data.DIGITS.Length);

			for (int i = 0; i < Data.DIGITS.Length; i++)
			{

				// setup input 
				DataSetRow inputRow = Data.convertImageIntoData(Data.DIGITS[i]);
				double[] input = inputRow.Input;

				// setup output
				double[] output = new double[Data.DIGITS.Length];

				for (int j = 0; j < Data.DIGITS.Length; j++)
				{
					if (j == i)
					{
						output[j] = 1;
					}
					else
					{
						output[j] = 0;
					}
				}
				//creating new training element with specified input and output
				DataSetRow row = new DataSetRow(input, output);
				//adding row to data set
				dataSet.addRow(row);
			}
			return dataSet;
		}

		public static int maxOutput(double[] array)
		{

			double max = array[0];
			int index = 0;

			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] > max)
				{
					index = i;
					max = array[i];
				}
			}
			return index;
		}

	}

}