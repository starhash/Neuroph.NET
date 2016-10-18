using System;

namespace org.neuroph.samples.brestCancer
{

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

	/// 
	/// <summary>
	/// @author Ivan Petrovic
	/// </summary>
	/*
	 INTRODUCTION TO THE PROBLEM AND DATA SET INFORMATION:
	
	 *Data set that will be used in this experiment: Wisconsin Diagnostic Breast Cancer (WDBC)
	 The original data set that will be used in this experiment can be found at link: 
	 https://archive.ics.uci.edu/ml/machine-learning-databases/breast-cancer-wisconsin/wdbc.data
	
	 *Creators: 
	 -   r. William H. Wolberg, General Surgery Dept., University of Wisconsin,  Clinical Sciences Center, 
	 Madison, WI 53792 wolberg@eagle.surgery.wisc.edu
	    
	 -   W. Nick Street, Computer Sciences Dept., University of Wisconsin, 1210 West Dayton St., 
	 Madison, WI 53706 treet@cs.wisc.edu  608-262-6619
	
	 -   Olvi L. Mangasarian, Computer Sciences Dept., University of Wisconsin, 1210 West Dayton St., 
	 Madison, WI 53706 olvi@cs.wisc.edu 
	
	 *See also: 
	 -   http://www.cs.wisc.edu/~olvi/uwmp/mpml.html
	 -   http://www.cs.wisc.edu/~olvi/uwmp/cancer.html
	
	 *Result: 
	 -   predicting field 2, diagnosis: B = benign, M = malignant 
	 -   sets are linearly separable using all 30 input features
	
	 *Relevant information: 
	 Features are computed from a digitized image of a fine needle aspirate (FNA) of a breast mass. 
	 They describe characteristics of the cell nuclei present in the image. Separating plane described above 
	 was obtained using Multisurface Method-Tree (MSM-T), a classification method which uses linear
	 programming to construct a decision tree. Relevant features were selected using an exhaustive search 
	 in the space of 1-4	features and 1-3 separating planes.
	
	 *Number of instances: 569
	
	 *Number of attributes: 
	 32 (30 real-valued input features, 
	 2 output features - 1,0 for M = malignant cancer and 0,1 for B = benign cancer)
	
	 *Missing attribute values: none
	
	 *Class distribution: 357 benign, 212 malignant
	
	 ATTRIBUTE INFORMATION:
	
	 Inputs:
	 1-30 attributes: 
	 Ten real-valued features are computed for each cell nucleus:
	 a) radius (mean of distances from center to points on the perimeter)
	 b) texture (standard deviation of gray-scale values)
	 c) perimeter
	 d) area
	 e) smoothness (local variation in radius lengths)
	 f) compactness (perimeter^2 / area - 1.0)
	 g) concavity (severity of concave portions of the contour)
	 h) concave points (number of concave portions of the contour)
	 i) symmetry 
	 j) fractal dimension ("coastline approximation" - 1)
	
	 Output:
	 31 and 32: 
	 1,0 M = malignant
	 0,1 B = benign
	
	 *The original data set description can be found at link:
	 https://archive.ics.uci.edu/ml/machine-learning-databases/breast-cancer-wisconsin/wdbc.names
	
	 */
	public class BrestCancerSample : LearningEventListener
	{

		//Important for evaluating network result
		public int[] count = new int[3];
		public int[] correct = new int[3];
		internal int unpredicted = 0;

		/// <param name="args"> the command line arguments </param>
		public static void Main(string[] args)
		{

			(new BrestCancerSample()).run();
		}

		public virtual void run()
		{

			Console.WriteLine("Creating training and test set from file...");
			string trainingSetFileName = "data_sets/breast cancer.txt";
			int inputsCount = 30;
			int outputsCount = 2;

			//Create data set from file
			DataSet dataSet = DataSet.createFromFile(trainingSetFileName, inputsCount, outputsCount, ",");
			dataSet.shuffle();

			//Normalizing data set
			Normalizer normalizer = new MaxNormalizer();
			normalizer.normalize(dataSet);

			//Creatinig training set (70%) and test set (30%)
			DataSet[] trainingAndTestSet = dataSet.createTrainingAndTestSubsets(70, 30);
			DataSet trainingSet = trainingAndTestSet[0];
			DataSet testSet = trainingAndTestSet[1];

			Console.WriteLine("Creating neural network...");
			//Create MultiLayerPerceptron neural network
			MultiLayerPerceptron neuralNet = new MultiLayerPerceptron(inputsCount, 16, outputsCount);

			//attach listener to learning rule
			MomentumBackpropagation learningRule = (MomentumBackpropagation) neuralNet.LearningRule;
			learningRule.addListener(this);

			learningRule.LearningRate = 0.3;
			learningRule.MaxError = 0.001;
			learningRule.MaxIterations = 5000;

			Console.WriteLine("Training network...");
			//train the network with training set
			neuralNet.learn(trainingSet);

			Console.WriteLine("Testing network...\n\n");
			testNeuralNetwork(neuralNet, testSet);

			Console.WriteLine("Done.");

			Console.WriteLine("**************************************************");

		}

		public virtual void testNeuralNetwork(NeuralNetwork neuralNet, DataSet testSet)
		{

			Console.WriteLine("**************************************************");
			Console.WriteLine("**********************RESULT**********************");
			Console.WriteLine("**************************************************");
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

			Console.WriteLine("Total cases: " + this.count[2] + ". ");
			Console.WriteLine("Correctly predicted cases: " + this.correct[2] + ". ");
			Console.WriteLine("Incorrectly predicted cases: " + (this.count[2] - this.correct[2] - unpredicted) + ". ");
			Console.WriteLine("Unrecognized cases: " + unpredicted + ". ");
			double percentTotal = (double) this.correct[2] * 100 / (double) this.count[2];
			Console.WriteLine("Predicted correctly: " + formatDecimalNumber(percentTotal) + "%. ");

			double percentM = (double) this.correct[0] * 100.0 / (double) this.count[0];
			Console.WriteLine("Prediction for 'M (malignant)' => (Correct/total): " + this.correct[0] + "/" + count[0] + "(" + formatDecimalNumber(percentM) + "%). ");

			double percentB = (double) this.correct[1] * 100.0 / (double) this.count[1];
			Console.WriteLine("Prediction for 'B (benign)' => (Correct/total): " + this.correct[1] + "/" + count[1] + "(" + formatDecimalNumber(percentB) + "%). ");
		}

		public virtual void handleLearningEvent(LearningEvent @event)
		{
			BackPropagation bp = (BackPropagation) @event.Source;
			if (@event.EventType.Equals(LearningEvent.Type.LEARNING_STOPPED))
			{
				double error = bp.TotalNetworkError;
				Console.WriteLine("Training completed in " + bp.CurrentIteration + " iterations, ");
				Console.WriteLine("With total error: " + formatDecimalNumber(error));
			}
			else
			{
				Console.WriteLine("Iteration: " + bp.CurrentIteration + " | Network error: " + bp.TotalNetworkError);
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
		public virtual void keepScore(int prediction, int ideal)
		{
			count[ideal]++;
			count[2]++;

			if (prediction == ideal)
			{
				correct[ideal]++;
				correct[2]++;
			}
			if (prediction == -1)
			{
				unpredicted++;
			}
		}

		//Formating decimal number to have 3 decimal places
		public virtual string formatDecimalNumber(double number)
		{
			return (new decimal(number)).setScale(4, RoundingMode.HALF_UP).ToString();
		}
	}

}