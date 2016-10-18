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
namespace org.neuroph.samples.forestCover
{

	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;
	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;

	public class Evaluate
	{

		private Config config;

		internal int[] count = new int[8];
		internal int[] correct = new int[8];
		internal int unpredicted = 0;

		public Evaluate(Config config)
		{
			this.config = config;
		}

		public virtual void evaluate()
		{
			Console.WriteLine("Evaluating neural network...");
			//Loading neural network from file
			MultiLayerPerceptron neuralNet = (MultiLayerPerceptron) NeuralNetwork.createFromFile(config.TrainedNetworkFileName);

			//Load normalized balanced data set from file
			DataSet dataSet = DataSet.load(config.TestFileName);

			//Testing neural network
			testNeuralNetwork(neuralNet, dataSet);

		}

		//Testing neural network
		public virtual void testNeuralNetwork(NeuralNetwork neuralNet, DataSet testSet)
		{

			foreach (DataSetRow testSetRow in testSet.Rows)
			{

				neuralNet.Input = testSetRow.Input;
				neuralNet.calculate();

				//Finding network output
				double[] networkOutput = neuralNet.Output;
				int predicted = maxOutput(networkOutput);

				//Finding actual output
				double[] networkDesiredOutput = testSetRow.DesiredOutput;
				int ideal = maxOutput(networkDesiredOutput);

				//Colecting data for network evaluation
				keepScore(predicted, ideal);
			}

			Console.WriteLine("Total cases: " + this.count[7] + ". ");
			Console.WriteLine("Correct cases: " + this.correct[7] + ". ");
			Console.WriteLine("Incorrectly predicted cases: " + (this.count[7] - this.correct[7] - unpredicted) + ". ");
			Console.WriteLine("Unrecognized cases: " + unpredicted + ". ");

			double percentTotal = (double) this.correct[7] * 100 / (double) this.count[7];
			Console.WriteLine("Predicted correctly: " + formatDecimalNumber(percentTotal) + "%. ");

			for (int i = 0; i < correct.Length - 1; i++)
			{
				double p = (double) this.correct[i] * 100.0 / (double) this.count[i];
				Console.WriteLine("Tree type: " + (i + 1) + " - Correct/total: " + this.correct[i] + "/" + count[i] + "(" + formatDecimalNumber(p) + "%). ");
			}
		}

		//Metod determines the maximum output. Maximum output is network prediction for one row. 
		public static int maxOutput(double[] array)
		{
			double max = array[0];
			int index = 0;

			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] > max)
				{
					index = i;
					max = array[i];
				}
			}
			//If maximum is less than 0.5, that prediction will not count.
			if (max < 0.5)
			{
				return -1;
			}
			return index;
		}

		//Colecting data to evaluate network.
		public virtual void keepScore(int actual, int ideal)
		{
			count[ideal]++;
			count[7]++;

			if (actual == ideal)
			{
				correct[ideal]++;
				correct[7]++;
			}
			if (actual == -1)
			{
				unpredicted++;
			}
		}

		//Formating decimal number to have 3 decimal places
		private string formatDecimalNumber(double number)
		{
			return (new decimal(number)).setScale(2, RoundingMode.HALF_UP).ToString();
		}

	}

}