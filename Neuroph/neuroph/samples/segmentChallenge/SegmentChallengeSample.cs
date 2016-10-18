using System;

namespace org.neuroph.samples.segmentChallenge
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
	 The original training set that will be used in this experiment can be found at link: 
	 https://archive.ics.uci.edu/ml/machine-learning-databases/image/segmentation.data
	
	 The original test set that will be used in this experiment can be found at link: 
	 https://archive.ics.uci.edu/ml/machine-learning-databases/image/segmentation.test
	
	 1. Title: Image Segmentation data
	
	 2. Source Information
	 -- Creators: Vision Group, University of Massachusetts
	 -- Donor: Vision Group (Carla Brodley, brodley@cs.umass.edu)
	 -- Date: November, 1990
	 
	 3. Past Usage: None yet published
	
	 4. Relevant Information:
	
	 The instances were drawn randomly from a database of 7 outdoor 
	 images.  The images were handsegmented to create a classification
	 for every pixel.  
	
	 Each instance is a 3x3 region.
	
	 5. Number of Instances: Training data: 210  Test data: 2100
	
	 6. Number of Attributes: 19 continuous attributes
	
	 7. Attribute Information:
	
	 1.  region-centroid-col:  the column of the center pixel of the region.
	 2.  region-centroid-row:  the row of the center pixel of the region.
	 3.  region-pixel-count:  the number of pixels in a region = 9.
	 4.  short-line-density-5:  the results of a line extractoin algorithm that 
	 counts how many lines of length 5 (any orientation) with
	 low contrast, less than or equal to 5, go through the region.
	 5.  short-line-density-2:  same as short-line-density-5 but counts lines
	 of high contrast, greater than 5.
	 6.  vedge-mean:  measure the contrast of horizontally
	 adjacent pixels in the region.  There are 6, the mean and 
	 standard deviation are given.  This attribute is used as
	 a vertical edge detector.
	 7.  vegde-sd:  (see 6)
	 8.  hedge-mean:  measures the contrast of vertically adjacent
	 pixels. Used for horizontal line detection. 
	 9.  hedge-sd: (see 8).
	 10. intensity-mean:  the average over the region of (R + G + B)/3
	 11. rawred-mean: the average over the region of the R value.
	 12. rawblue-mean: the average over the region of the B value.
	 13. rawgreen-mean: the average over the region of the G value.
	 14. exred-mean: measure the excess red:  (2R - (G + B))
	 15. exblue-mean: measure the excess blue:  (2B - (G + R))
	 16. exgreen-mean: measure the excess green:  (2G - (R + B))
	 17. value-mean:  3-d nonlinear transformation
	 of RGB. (Algorithm can be found in Foley and VanDam, Fundamentals
	 of Interactive Computer Graphics)
	 18. saturatoin-mean:  (see 17)
	 19. hue-mean:  (see 17)
	
	 8. Missing Attribute Values: None
	
	 9. Class Distribution: 
	
	 Classes:   1,0,0,0,0,0,0 -- brickface
	            0,1,0,0,0,0,0 -- sky
	            0,0,1,0,0,0,0 -- foliage
	            0,0,0,1,0,0,0 -- cement
	            0,0,0,0,1,0,0 -- window
	            0,0,0,0,0,1,0 -- path
	            0,0,0,0,0,0,1 -- grass
	        
	 30 instances per class for training data.
	 300 instances per class for test data.
	
	 *The original data set description can be found at link:
	 https://archive.ics.uci.edu/ml/machine-learning-databases/image/segmentation.names
	
	 */
	public class SegmentChallengeSample : LearningEventListener
	{

		//Important for evaluating network result
		public int[] count = new int[8];
		public int[] correct = new int[8];
		internal int unpredicted = 0;

		/// <param name="args"> the command line arguments </param>
		public static void Main(string[] args)
		{

			(new SegmentChallengeSample()).run();
		}

		public virtual void run()
		{

			Console.WriteLine("Creating training and test set from file...");
			string trainingSetFileName = "data_sets/segment challenge.txt";
			string testSetFileName = "data_sets/segment test.txt";
			int inputsCount = 19;
			int outputsCount = 7;

			//Create training data set from file
			DataSet trainingSet = DataSet.createFromFile(trainingSetFileName, inputsCount, outputsCount, ",");
			Console.WriteLine("Training set size: " + trainingSet.Rows.Count);
			trainingSet.shuffle();
			trainingSet.shuffle();

			//Normalizing training data set
			Normalizer normalizer = new MaxNormalizer();
			normalizer.normalize(trainingSet);

			//Create test data set from file
			DataSet testSet = DataSet.createFromFile(testSetFileName, inputsCount, outputsCount, ",");
			Console.WriteLine("Test set size: " + testSet.Rows.Count);
			Console.WriteLine("--------------------------------------------------");
			testSet.shuffle();
			testSet.shuffle();

			//Normalizing training data set
			normalizer.normalize(testSet);

			Console.WriteLine("Creating neural network...");
			//Create MultiLayerPerceptron neural network
			MultiLayerPerceptron neuralNet = new MultiLayerPerceptron(inputsCount, 17, 10, outputsCount);
			//attach listener to learning rule
			MomentumBackpropagation learningRule = (MomentumBackpropagation) neuralNet.LearningRule;
			learningRule.addListener(this);

			learningRule.LearningRate = 0.01;
			learningRule.MaxError = 0.001;
			learningRule.MaxIterations = 12000;

			Console.WriteLine("Training network...");
			//train the network with training set
			neuralNet.learn(trainingSet);

			Console.WriteLine("Testing network...\n\n");
			testNeuralNetwork(neuralNet, testSet);

			Console.WriteLine("Done.");
			Console.WriteLine("**************************************************");
	//        }
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

			Console.WriteLine("Total cases: " + this.count[7] + ". ");
			Console.WriteLine("Correctly predicted cases: " + this.correct[7] + ". ");
			Console.WriteLine("Incorrectly predicted cases: " + (this.count[7] - this.correct[7] - unpredicted) + ". ");
			Console.WriteLine("Unrecognized cases: " + unpredicted + ". ");
			double percentTotal = (double) this.correct[7] * 100 / (double) this.count[7];
			Console.WriteLine("Predicted correctly: " + formatDecimalNumber(percentTotal) + "%. ");

			for (int i = 0; i < correct.Length - 1; i++)
			{
				double p = (double) this.correct[i] * 100.0 / (double) this.count[i];
				Console.WriteLine("Segment class: " + getClasificationClass(i + 1) + " - Correct/total: " + this.correct[i] + "/" + count[i] + "(" + formatDecimalNumber(p) + "%). ");
			}

			this.count = new int[8];
			this.correct = new int[8];
			unpredicted = 0;
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
			count[7]++;

			if (prediction == ideal)
			{
				correct[ideal]++;
				correct[7]++;
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

		public virtual string getClasificationClass(int i)
		{
			switch (i)
			{
				case 1:
					return "brickface";
				case 2:
					return "sky";
				case 3:
					return "foliage";
				case 4:
					return "cement";
				case 5:
					return "window";
				case 6:
					return "path";
				case 7:
					return "grass";
				default:
					return "error";
			}
		}
	}

}