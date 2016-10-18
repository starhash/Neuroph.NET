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
	using MaxNormalizer = org.neuroph.util.data.norm.MaxNormalizer;
	using Normalizer = org.neuroph.util.data.norm.Normalizer;

	/*
	 * @author Ivana Bajovic
	 */

	/*
	 INTRODUCTION TO THE PROBLEM AND DATA SET INFORMATION:
	 The objective is to train the neural network to predict relative performance of a CPU using some characteristics
	 that are used as input, and subsequently comparing that result with existing performance that is published and 
	 relative performance that is estimated using linear regression method.
	 The original data set that will be used in this experiment can be found at link http://archive.ics.uci.edu/ml/datasets/Computer+Hardware.
	 The data set contains 209 instances with the total of 9 attributes.
	 
	 ATTRIBUTE INFORMATION:
	 
	 1. (ignored) vendor name: 30 
	 (adviser, amdahl,apollo, basf, bti, burroughs, c.r.d, cambex, cdc, dec, 
	 dg, formation, four-phase, gould, honeywell, hp, ibm, ipl, magnuson, 
	 microdata, nas, ncr, nixdorf, perkin-elmer, prime, siemens, sperry, 
	 sratus, wang) 
	 2. (input) Model Name: many unique symbols 
	 3. (input) MYCT: machine cycle time in nanoseconds (integer) 
	 4. (input) MMIN: minimum main memory in kilobytes (integer) 
	 5. (input) MMAX: maximum main memory in kilobytes (integer) 
	 6. (input) CACH: cache memory in kilobytes (integer) 
	 7. (input) CHMIN: minimum channels in units (integer) 
	 8. (input) CHMAX: maximum channels in units (integer) 
	 9. (input) PRP: published relative performance (integer) 
	 10. (output) ERP: estimated relative performance from the original article (integer)
	
	 */
	//beskonacna petlja
	public class PredictingPerformanceOfCPUSample : LearningEventListener
	{

		/// <param name="args"> the command line arguments </param>
		public static void Main(string[] args)
		{
			(new PredictingPerformanceOfCPUSample()).run();
		}

		public virtual void run()
		{

			Console.WriteLine("Creating training set...");
			string trainingSetFileName = "data_sets/cpu_data.txt";
			int inputsCount = 7;
			int outputsCount = 1;

			// create training set from file
			DataSet dataSet = DataSet.createFromFile(trainingSetFileName, inputsCount, outputsCount, ",", false);
			Normalizer normalizer = new MaxNormalizer();
			normalizer.normalize(dataSet);


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
			neuralNet.save("MyNeuralNetCPU.nnet");

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