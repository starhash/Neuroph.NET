﻿using System;

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
	Given are the variable name, variable type, the measurement unit and a brief description. The concrete compressive strength is the regression problem. The order of this listing corresponds to the order of numerals along the rows of the database. 
	
	Name -- Data Type -- Measurement -- Description 
	
	Cement (component 1) -- quantitative -- kg in a m3 mixture -- Input Variable 
	Blast Furnace Slag (component 2) -- quantitative -- kg in a m3 mixture -- Input Variable 
	Fly Ash (component 3) -- quantitative -- kg in a m3 mixture -- Input Variable 
	Water (component 4) -- quantitative -- kg in a m3 mixture -- Input Variable 
	Superplasticizer (component 5) -- quantitative -- kg in a m3 mixture -- Input Variable 
	Coarse Aggregate (component 6) -- quantitative -- kg in a m3 mixture -- Input Variable 
	Fine Aggregate (component 7)	 -- quantitative -- kg in a m3 mixture -- Input Variable 
	Age -- quantitative -- Day (1~365) -- Input Variable 
	Concrete compressive strength -- quantitative -- MPa -- Output Variable 
	 
	 The original data set that will be used in this experiment can be found at link http://archive.ics.uci.edu/ml/datasets/Concrete+Compressive+Strength
	 */

	public class ConcreteStrenghtTestSample : LearningEventListener
	{

		/// <param name="args"> the command line arguments </param>
		public static void Main(string[] args)
		{
			(new ConcreteStrenghtTestSample()).run();
		}

		public virtual void run()
		{

			Console.WriteLine("Creating training set...");
			string trainingSetFileName = "data_sets/concrete_strenght_test_data.txt";
			int inputsCount = 8;
			int outputsCount = 1;

			// create training set from file
			DataSet dataSet = DataSet.createFromFile(trainingSetFileName, inputsCount, outputsCount, ",", false);


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
			neuralNet.save("MyNeuralConcreteStrenght.nnet");

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