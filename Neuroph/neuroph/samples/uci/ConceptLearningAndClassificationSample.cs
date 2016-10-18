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

	/// <summary>
	/// NOTE: NOT WORKING!
	/// @author Ivana Bajovic
	/// </summary>

	/*
	 DATA SET INFORMATION:
	 * This database contains 5 numeric-valued attributes.
	 * Some instances could be placed in either category 0 or 1. 
	 * We have followed the authors' suggestion, placing them in each category with equal probability.
	 * We have replaced the actual values of the attributes (i.e., hobby has values chess, sports and stamps) with numeric values. 
	
	 ATTRIBUTE INFORMATION:
	 -- 1. name: distinct for each instance and represented numerically -- Input Variable
	 -- 2. hobby: nominal values ranging between 1 and 3 -- Input Variable
	 -- 3. age: nominal values ranging between 1 and 4 -- Input Variable
	 -- 4. educational level: nominal values ranging between 1 and 4 -- Input Variable
	 -- 5. marital status: nominal values ranging between 1 and 4 -- Input Variable
	 -- 6. class: nominal value between 1 and 3 -- Output Variable
	
	 The original data set that will be used in this experiment can be found at link http://archive.ics.uci.edu/ml/datasets/Hayes-Roth 
	 */
	public class ConceptLearningAndClassificationSample : LearningEventListener
	{

		/// <param name="args"> the command line arguments </param>
		public static void Main(string[] args)
		{
			(new ConceptLearningAndClassificationSample()).run();
		}

		public virtual void run()
		{

			Console.WriteLine("Creating training set...");
			string trainingSetFileName = "data_sets/concept_learning_and_classification_data_1.txt";
			int inputsCount = 15;
			int outputsCount = 3;

			// create training set from file
			DataSet dataSet = DataSet.createFromFile(trainingSetFileName, inputsCount, outputsCount, ",", false);
		   //dataSet.normalize();


			Console.WriteLine("Creating neural network...");
			// create MultiLayerPerceptron neural network
			MultiLayerPerceptron neuralNet = new MultiLayerPerceptron(inputsCount, 10, outputsCount);


			// attach listener to learning rule
			MomentumBackpropagation learningRule = (MomentumBackpropagation) neuralNet.LearningRule;
			learningRule.addListener(this);

			// set learning rate and max error
			learningRule.LearningRate = 0.2;
			learningRule.Momentum = 0.7;
			learningRule.MaxError = 0.01;

			Console.WriteLine("Training network...");
			// train the network with training set
			neuralNet.learn(dataSet);

			Console.WriteLine("Training completed.");
			Console.WriteLine("Testing network...");

			testNeuralNetwork(neuralNet, dataSet);

			Console.WriteLine("Saving network");
			// save neural network to file
			neuralNet.save("MyNeuralNetConceptLearning.nnet");

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