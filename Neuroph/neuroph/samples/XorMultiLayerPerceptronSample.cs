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
	using LearningEvent = org.neuroph.core.events.LearningEvent;
	using LearningEventListener = org.neuroph.core.events.LearningEventListener;
	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;
	using LearningRule = org.neuroph.core.learning.LearningRule;
	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;
	using BackPropagation = org.neuroph.nnet.learning.BackPropagation;
	using MomentumBackpropagation = org.neuroph.nnet.learning.MomentumBackpropagation;
	using TransferFunctionType = org.neuroph.util.TransferFunctionType;

	/// <summary>
	/// This sample shows how to create, train, save and load simple Multi Layer Perceptron for the XOR problem.
	/// This sample shows basics of Neuroph API.
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class XorMultiLayerPerceptronSample : LearningEventListener
	{

		public static void Main(string[] args)
		{
			(new XorMultiLayerPerceptronSample()).run();
		}

		/// <summary>
		/// Runs this sample
		/// </summary>
		public virtual void run()
		{

			// create training set (logical XOR function)
			DataSet trainingSet = new DataSet(2, 1);
			trainingSet.addRow(new DataSetRow(new double[]{0, 0}, new double[]{0}));
			trainingSet.addRow(new DataSetRow(new double[]{0, 1}, new double[]{1}));
			trainingSet.addRow(new DataSetRow(new double[]{1, 0}, new double[]{1}));
			trainingSet.addRow(new DataSetRow(new double[]{1, 1}, new double[]{0}));

			// create multi layer perceptron
			MultiLayerPerceptron myMlPerceptron = new MultiLayerPerceptron(TransferFunctionType.SIGMOID, 2, 3, 1);

			myMlPerceptron.LearningRule = new BackPropagation();

			// enable batch if using MomentumBackpropagation
	//        if( myMlPerceptron.getLearningRule() instanceof MomentumBackpropagation )
	//        	((MomentumBackpropagation)myMlPerceptron.getLearningRule()).setBatchMode(false);

			LearningRule learningRule = myMlPerceptron.LearningRule;
			learningRule.addListener(this);

			// learn the training set
			Console.WriteLine("Training neural network...");
			myMlPerceptron.learn(trainingSet);

			// test perceptron
			Console.WriteLine("Testing trained neural network");
			testNeuralNetwork(myMlPerceptron, trainingSet);

			// save trained neural network
			myMlPerceptron.save("myMlPerceptron.nnet");

			// load saved neural network
			NeuralNetwork loadedMlPerceptron = NeuralNetwork.createFromFile("myMlPerceptron.nnet");

			// test loaded neural network
			Console.WriteLine("Testing loaded neural network");
			testNeuralNetwork(loadedMlPerceptron, trainingSet);
		}

		/// <summary>
		/// Prints network output for the each element from the specified training set. </summary>
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

		public virtual void handleLearningEvent(LearningEvent @event)
		{
			BackPropagation bp = (BackPropagation)@event.Source;
			if (@event.EventType != LearningEvent.Type.LEARNING_STOPPED)
			{
				Console.WriteLine(bp.CurrentIteration + ". iteration : " + bp.TotalNetworkError);
			}
		}

	}

}