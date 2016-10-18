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
	using SupervisedLearning = org.neuroph.core.learning.SupervisedLearning;
	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;
	using ResilientPropagation = org.neuroph.nnet.learning.ResilientPropagation;
	using TransferFunctionType = org.neuroph.util.TransferFunctionType;

	/// <summary>
	/// This sample trains Multi Layer Perceptron network using Resilient Propagation
	/// learning rule for the XOR problem.
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class XorResilientPropagationSample
	{

		/// <summary>
		/// Runs this sample
		/// </summary>
		public static void Main(string[] args)
		{

			// create training set (logical XOR function)
			DataSet trainingSet = new DataSet(2, 1);
			trainingSet.addRow(new DataSetRow(new double[]{0, 0}, new double[]{0}));
			trainingSet.addRow(new DataSetRow(new double[]{0, 1}, new double[]{1}));
			trainingSet.addRow(new DataSetRow(new double[]{1, 0}, new double[]{1}));
			trainingSet.addRow(new DataSetRow(new double[]{1, 1}, new double[]{0}));

			// create multi layer perceptron
			MultiLayerPerceptron myMlPerceptron = new MultiLayerPerceptron(TransferFunctionType.SIGMOID, 2, 3, 1);
			// set ResilientPropagation learning rule
			myMlPerceptron.LearningRule = new ResilientPropagation();

			// learn the training set
			Console.WriteLine("Training neural network...");
			myMlPerceptron.learn(trainingSet);

			int iterations = ((SupervisedLearning)myMlPerceptron.LearningRule).CurrentIteration;
			Console.WriteLine("Learned in " + iterations + " iterations");

			// test perceptron
			Console.WriteLine("Testing trained neural network");
			testNeuralNetwork(myMlPerceptron, trainingSet);

		}

		/// <summary>
		/// Prints network output for each element from the specified training set. </summary>
		/// <param name="neuralNet"> neural network </param>
		/// <param name="trainingSet"> training set </param>
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

	}

}