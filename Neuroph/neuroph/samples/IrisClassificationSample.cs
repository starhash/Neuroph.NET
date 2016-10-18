using System;

/// <summary>
/// Copyright 2010 Neuroph Project http://neuroph.sourceforge.net
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
/// 
///    http://www.apache.org/licenses/LICENSE-2.0
/// 
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// </summary>

namespace org.neuroph.samples
{

	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;
	using LearningEvent = org.neuroph.core.events.LearningEvent;
	using LearningEventListener = org.neuroph.core.events.LearningEventListener;
	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;
	using BackPropagation = org.neuroph.nnet.learning.BackPropagation;

	/// <summary>
	/// This sample shows how to train MultiLayerPerceptron neural network for iris classification problem using Neuroph
	/// For more details about training process, error, iterations use NeurophStudio which provides rich environment  for
	/// training and inspecting neural networks
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class IrisClassificationSample
	{


		/// <summary>
		///  Runs this sample
		/// </summary>
		public static void Main(string[] args)
		{
			// get the path to file with data
			string inputFileName = "data_sets/iris_data_normalised.txt";

			// create MultiLayerPerceptron neural network
			MultiLayerPerceptron neuralNet = new MultiLayerPerceptron(4, 16, 3);
			// create training set from file
			DataSet irisDataSet = DataSet.createFromFile(inputFileName, 4, 3, ",", false);
			// train the network with training set

			neuralNet.LearningRule.addListener(new LearningListener());
			neuralNet.LearningRule.LearningRate = 0.2;
		//    neuralNet.getLearningRule().setMaxIterations(30000);

			neuralNet.learn(irisDataSet);

			neuralNet.save("irisNet.nnet");

			Console.WriteLine("Done training.");
			Console.WriteLine("Testing network...");
		}


		/// <summary>
		/// Prints network output for the each element from the specified training set. </summary>
		/// <param name="neuralNet"> neural network </param>
		/// <param name="testSet"> test data set </param>
		public static void testNeuralNetwork(NeuralNetwork neuralNet, DataSet testSet)
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

		internal class LearningListener : LearningEventListener
		{

			public virtual void handleLearningEvent(LearningEvent @event)
			{
				BackPropagation bp = (BackPropagation) @event.Source;
				Console.WriteLine("Current iteration: " + bp.CurrentIteration);
				Console.WriteLine("Error: " + bp.TotalNetworkError);
			}

		}


	}
}