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
	 DATA SET INFORMATION:
	 * Car Evaluation Database was derived from a simple hierarchical decision model originally developed for the demonstration of DEX, M. Bohanec, V. Rajkovic: Expert system for decision making.
	 * The model evaluates cars according to the following concept structure: 
	
	 CAR car acceptability 
	 . PRICE overall price 
	 . . buying buying price 
	 . . maint price of the maintenance 
	 . TECH technical characteristics 
	 . . COMFORT comfort 
	 . . . doors number of doors 
	 . . . persons capacity in terms of persons to carry 
	 . . . lug_boot the size of luggage boot 
	 . . safety estimated safety of the car 
	
	 ATTRIBUTE INFORMATION:
	 1. buying: vhigh, high, med, low
	 1,0,0,0 instead of vhigh, 0,1,0,0 instead of high, 0,0,1,0 instead of med, 0,0,0,1 instead of low
	 2. maint: vhigh, high, med, low
	 1,0,0,0 instead of vhigh, 0,1,0,0 instead of high, 0,0,1,0 instead of med, 0,0,0,1 instead of low
	 3. doors: 2, 3, 4, more
	 0,0,0,1 instead of 2, 0,0,1,0 instead of 3, 0,1,0,0 instead of 4, 1,0,0,0 instead of more
	 4. persons: 2, 4, more
	 0,0,1 instead of 2, 0,1,0 instead of 4, 1,0,0 instead of more
	 5. lug_boot: small, med, big
	 0,0,1 instead of small, 0,1,0 instead of med, 1,0,0 instead of big
	 6. safety: low, med, high
	 0,0,1 instead of low, 0,1,0 instead of med, 1,0,0 instead of high
	 7. class values: unacc, acc, good, vgood 
	 0,0,0,1 instead of unacc, 0,0,1,0 instead of acc, 0,1,0,0 instead of good, 1,0,0,0 instead of vgood. 
	 
	 1.,2.,3.,4.,5.,6. - inputs
	 7. - output
	 
	 The original data set that will be used in this experiment can be found at link http://archive.ics.uci.edu/ml/datasets/Car+Evaluation
	 */
	public class CarEvaluationSample : LearningEventListener
	{

		/// <param name="args"> the command line arguments </param>
		public static void Main(string[] args)
		{
			(new CarEvaluationSample()).run();
		}

		public virtual void run()
		{

			Console.WriteLine("Creating training set...");
			string trainingSetFileName = "data_sets/car_evaluation_data.txt";
			int inputsCount = 21;
			int outputsCount = 4;

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
			neuralNet.save("MyNeuralNetCarEvaluation.nnet");

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