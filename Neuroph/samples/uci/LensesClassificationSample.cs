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
	 * The lenses data set tries to predict whether a person will need soft contact lenses, hard contact lenses or no contacts, by determining relevant attributes of the client.
	 * The data set has 4 attributes (age of the patient, spectacle prescription, notion on astigmatism, and information on tear production rate) plus an associated three-valued class, that gives the appropriate lens prescription for patient (hard contact lenses, soft contact lenses, no lenses). 
	 * The data set contains 24 instances(class distribution: hard contact lenses- 4, soft contact lenses-5, no contact lenses- 15). 
	
	 ATTRIBUTE INFORMATION:
	 1. Age of the patient: 
	 Nominal variable: (1) young, (2) pre-presbyopic, (3) presbyopic
	 2. Spectacle prescription: 
	 Nominal variable: (1) myope, (2) hypermetrope
	 3. Astigmatic:
	 Nominal variable: (1) no, (2) yes
	 4. Tear production rate:
	 Nominal variable: (1) reduced, (2) normal
	 5. Class name: 
	 Nominal variable: (1) the patient should be fitted with hard contact lenses, (2) the patient should be fitted with soft contact lenses, (3) the patient should not be fitted with contact lenses.
	 
	 1.,2.,3.,4. - inputs
	 5. - output
	 
	 The original data set that will be used in this experiment can be found at link http://archive.ics.uci.edu/ml/datasets/Lenses 
	 */
	public class LensesClassificationSample : LearningEventListener
	{

		/// <param name="args"> the command line arguments </param>
		public static void Main(string[] args)
		{
			(new LensesClassificationSample()).run();
		}

		public virtual void run()
		{

			Console.WriteLine("Creating training set...");

			string trainingSetFileName = "data_sets/lenses_data.txt";
			int inputsCount = 9;
			int outputsCount = 3;

			Console.WriteLine("Creating training set...");
			DataSet dataSet = DataSet.createFromFile(trainingSetFileName, inputsCount, outputsCount, " ", false);


			Console.WriteLine("Creating neural network...");
			// create MultiLayerPerceptron neural network
			MultiLayerPerceptron neuralNet = new MultiLayerPerceptron(inputsCount, 16, outputsCount);


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
			neuralNet.save("MyNeuralNetLenses.nnet");

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