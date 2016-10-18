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
	using Perceptron = org.neuroph.nnet.Perceptron;

	/// <summary>
	/// This sample shows how to create, train, save and load simple Perceptron neural network
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class PerceptronSample
	{

		/// <summary>
		/// Runs this sample
		/// </summary>
		public static void Main(string[] args)
		{

				// create training set (logical AND function)
				DataSet trainingSet = new DataSet(2, 1);
				trainingSet.addRow(new DataSetRow(new double[]{0, 0}, new double[]{0}));
				trainingSet.addRow(new DataSetRow(new double[]{0, 1}, new double[]{0}));
				trainingSet.addRow(new DataSetRow(new double[]{1, 0}, new double[]{0}));
				trainingSet.addRow(new DataSetRow(new double[]{1, 1}, new double[]{1}));

				// create perceptron neural network
				NeuralNetwork myPerceptron = new Perceptron(2, 1);
				// learn the training set
				myPerceptron.learn(trainingSet);
				// test perceptron
				Console.WriteLine("Testing trained perceptron");
				testNeuralNetwork(myPerceptron, trainingSet);
				// save trained perceptron
				myPerceptron.save("mySamplePerceptron.nnet");
				// load saved neural network
				NeuralNetwork loadedPerceptron = NeuralNetwork.load("mySamplePerceptron.nnet");
				// test loaded neural network
				Console.WriteLine("Testing loaded perceptron");
				testNeuralNetwork(loadedPerceptron, trainingSet);
		}

		/// <summary>
		/// Prints network output for the each element from the specified training set. </summary>
		/// <param name="neuralNet"> neural network </param>
		/// <param name="testSet"> data set used for testing </param>
		public static void testNeuralNetwork(NeuralNetwork neuralNet, DataSet testSet)
		{

			foreach (DataSetRow trainingElement in testSet.Rows)
			{
				neuralNet.Input = trainingElement.Input;
				neuralNet.calculate();
				double[] networkOutput = neuralNet.Output;

				Console.Write("Input: " + Arrays.ToString(trainingElement.Input));
				Console.WriteLine(" Output: " + Arrays.ToString(networkOutput));
			}
		}
	}

}