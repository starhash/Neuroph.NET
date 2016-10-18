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
	 * Each record is an example of a hand consisting of five playing cards drawn from a standard deck of 52.
	 * Each card is described using two attributes (suit and rank), for a total of 10 predictive attributes. 
	 * There is one Class attribute that describes the "Poker Hand".
	 * The order of cards is important, which is why there are 480 possible Royal Flush hands as compared to 4.
	
	 ATTRIBUTE INFORMATION:
	 1. S1 "Suit of card #1" 
	 Ordinal (1-4) representing {Hearts, Spades, Diamonds, Clubs} 
	
	 2. C1 "Rank of card #1" 
	 Numerical (1-13) representing (Ace, 2, 3, ... , Queen, King) 
	
	 3. S2 "Suit of card #2" 
	 Ordinal (1-4) representing {Hearts, Spades, Diamonds, Clubs} 
	
	 4. C2 "Rank of card #2" 
	 Numerical (1-13) representing (Ace, 2, 3, ... , Queen, King) 
	
	 5. S3 "Suit of card #3" 
	 Ordinal (1-4) representing {Hearts, Spades, Diamonds, Clubs} 
	
	 6. C3 "Rank of card #3" 
	 Numerical (1-13) representing (Ace, 2, 3, ... , Queen, King) 
	
	 7. S4 "Suit of card #4" 
	 Ordinal (1-4) representing {Hearts, Spades, Diamonds, Clubs} 
	
	 8. C4 "Rank of card #4" 
	 Numerical (1-13) representing (Ace, 2, 3, ... , Queen, King) 
	
	 9. S5 "Suit of card #5" 
	 Ordinal (1-4) representing {Hearts, Spades, Diamonds, Clubs} 
	
	 10. C5 "Rank of card 5" 
	 Numerical (1-13) representing (Ace, 2, 3, ... , Queen, King) 
	
	 11. CLASS "Poker Hand" 
	 Ordinal (0-9) :
	 0: Nothing in hand; not a recognized poker hand 
	 1: One pair; one pair of equal ranks within five cards 
	 2: Two pairs; two pairs of equal ranks within five cards 
	 3: Three of a kind; three equal ranks within five cards 
	 4: Straight; five cards, sequentially ranked with no gaps 
	 5: Flush; five cards with the same suit 
	 6: Full house; pair + different rank three of a kind 
	 7: Four of a kind; four equal ranks within five cards 
	 8: Straight flush; straight + flush 
	 9: Royal flush; {Ace, King, Queen, Jack, Ten} + flush 
	 
	 1.,2.,3.,4.,5.,6.,7.,8.,9.,10. - inputs
	 11. - output
	
	 The original data set that will be used in this experiment can be found at link http://archive.ics.uci.edu/ml/datasets/Poker+Hand
	 */
	public class PredictingPokerHandsSample : LearningEventListener
	{

		/// <param name="args"> the command line arguments </param>
		public static void Main(string[] args)
		{
			(new PredictingPokerHandsSample()).run();
		}

		public virtual void run()
		{

			Console.WriteLine("Creating training set...");

			string trainingSetFileName = "data_sets/predicting_poker_hands_data.txt";
			int inputsCount = 85;
			int outputsCount = 9;

			// create training set from file
			DataSet dataSet = DataSet.createFromFile(trainingSetFileName, inputsCount, outputsCount, "\t", false);

			Console.WriteLine("Creating neural network...");
			// create MultiLayerPerceptron neural network
			MultiLayerPerceptron neuralNet = new MultiLayerPerceptron(inputsCount, 65, outputsCount);


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
			neuralNet.save("MyNeuralNetPokerHands.nnet");

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