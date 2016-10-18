using System;

/// <summary>
/// Copyright 2013 Neuroph Project http://neuroph.sourceforge.net
/// 
/// Licensed under the Apache License, Version 2.0 (the "License"); you may not
/// use this file except in compliance with the License. You may obtain a copy of
/// the License at
/// 
/// http://www.apache.org/licenses/LICENSE-2.0
/// 
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
/// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
/// License for the specific language governing permissions and limitations under
/// the License.
/// </summary>

namespace org.neuroph.samples.uci
{

	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;
	using LearningEvent = org.neuroph.core.events.LearningEvent;
	using LearningEventListener = org.neuroph.core.events.LearningEventListener;
	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;
	using BackPropagation = org.neuroph.nnet.learning.BackPropagation;
	using MomentumBackpropagation = org.neuroph.nnet.learning.MomentumBackpropagation;

	/// 
	/// <summary>
	/// @author Ivana Bajovic
	/// </summary>

	/*
	INTRODUCTION TO THE PROBLEM AND DATA SET INFORMATION:
	 * This data set was generated to model psychological experimental results. 
	 * Each example is classified as having the balance scale tip to the right, tip to the left, or be balanced.
	 * The attributes are the left weight, the left distance, the right weight, and the right distance. 
	 * The correct way to find the class is the greater of (left-distance * left-weight) and (right-distance * right-weight). If they are equal, it is balanced.
	
	ATTRIBUTE INFORMATION:
	
	Inputs:
	1. Left-Weight: 5 (1, 2, 3, 4, 5), nominal values
	2. Left-Distance: 5 (1, 2, 3, 4, 5), nominal values 
	3. Right-Weight: 5 (1, 2, 3, 4, 5), nominal values 
	4. Right-Distance: 5 (1, 2, 3, 4, 5), nominal values
	
	Output:
	5. Class Name: 3 (L, B, R), nominal values
	 
	The original data set that will be used in this experiment can be found at link http://archive.ics.uci.edu/ml/datasets/Balance+Scale 
	*/


	public class BalanceScaleSample : LearningEventListener
	{

		/// <param name="args"> the command line arguments </param>
		public static void Main(string[] args)
		{
			(new BalanceScaleSample()).run();
		}

		public virtual void run()
		{

			Console.WriteLine("Creating training set...");
			string trainingSetFileName = "data_sets/balance_scale_data.txt";
			int inputsCount = 20;
			int outputsCount = 3;

			// create training set from file
			DataSet dataSet = DataSet.createFromFile(trainingSetFileName, inputsCount, outputsCount, "\t", false);


			Console.WriteLine("Creating neural network...");
			// create MultiLayerPerceptron neural network
			MultiLayerPerceptron neuralNet = new MultiLayerPerceptron(inputsCount, 22, outputsCount);


			// attach listener to learning rule
			MomentumBackpropagation learningRule = (MomentumBackpropagation) neuralNet.LearningRule;
			learningRule.addListener(this);

			// set learning rate and max error
			learningRule.LearningRate = 0.2;
			learningRule.MaxError = 0.01;

			Console.WriteLine("Training network...");
			// train the network with training set
			neuralNet.learn(dataSet);

			Console.WriteLine("Training completed.");
			Console.WriteLine("Testing network...");

			testNeuralNetwork(neuralNet, dataSet);

			Console.WriteLine("Saving network");
			// save neural network to file
			neuralNet.save("MyNeuralNetBalanceScale.nnet");

			Console.WriteLine("Done.");
		}

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
			BackPropagation bp = (BackPropagation) @event.Source;
			Console.WriteLine(bp.CurrentIteration + ". iteration | Total network error: " + bp.TotalNetworkError);
		}


	}


}