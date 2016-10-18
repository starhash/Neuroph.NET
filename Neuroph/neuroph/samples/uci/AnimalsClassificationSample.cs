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
	INTRODUCTION TO THE PROBLEM AND DATA SET INFORMATION:
	 * The purpose of this experiment is to study the feasibility of classification animal species using neural networks.
	 * An animal class is made up of animal that are all alike in important ways.
	 * So we need to train a neural network to make it able to predict which species belong to a particular group.
	 * Once we have decided on a problem to solve using neural networks, we will need to gather data for training purposes.
	 * The training data set includes a number of cases, each containing values for a range of input and output variables.
	 * The data set that we use in this experiment can be found at http://archive.ics.uci.edu/ml/datasets.html under the category classification.
	 * This database includes 101 cases. Each case is the name of animal. It was found that each of these animals belonged to one of seven classes.
	
	 Class-Set of animals:
	1 - (41) aardvark, antelope, bear, boar, buffalo, calf, cavy, cheetah, deer, dolphin, elephant, fruitbat, giraffe, girl, goat, gorilla, hamster, hare, leopard, lion, lynx, mink, mole, mongoose, opossum, oryx, platypus, polecat, pony, porpoise, puma, pussycat, raccoon, reindeer, seal, sealion, squirrel, vampire, vole, wallaby, wolf
	2 - (20) chicken, crow, dove, duck, flamingo, gull, hawk, kiwi, lark, ostrich, parakeet, penguin, pheasant, rhea, skimmer, skua, sparrow, swan, vulture, wren
	3 - (5) pitviper, seasnake, slowworm, tortoise, tuatara
	4 - (13) bass, carp, catfish, chub, dogfish, haddock, herring, pike, piranha, seahorse, sole, stingray, tuna
	5 - frog, frog, newt, toad
	6 - flea, gnat, honeybee, housefly, ladybird, moth, termite, wasp
	7 - clam, crab, crayfish, lobster, octopus, scorpion, seawasp, slug, starfish, worm
	
	ATTRIBUTE INFORMATION:
	1. hair
	  Boolean = {true, false}. Value true will be replaced with value 1, and value false will be replaced with value 0.
	2. feathers
	  Boolean = {true, false}. Value true will be replaced with value 1, and value false will be replaced with value 0.
	3. eggs
	  Boolean = {true, false}. Value true will be replaced with value 1, and value false will be replaced with value 0.
	4. milk
	  Boolean = {true, false}. Value true will be replaced with value 1, and value false will be replaced with value 0.
	5. airborne
	  Boolean = {true, false}. Value true will be replaced with value 1, and value false will be replaced with value 0.
	6. aquatic
	  Boolean = {true, false}. Value true will be replaced with value 1, and value false will be replaced with value 0.
	7. predator
	  Boolean = {true, false}. Value true will be replaced with value 1, and value false will be replaced with value 0.
	8. toothed
	  Boolean = {true, false}. Value true will be replaced with value 1, and value false will be replaced with value 0.
	9. backbone
	  Boolean = {true, false}. Value true will be replaced with value 1, and value false will be replaced with value 0.
	10. bretahes
	  Boolean = {true, false}. Value true will be replaced with value 1, and value false will be replaced with value 0.
	11. venomous
	  Boolean = {true, false}. Value true will be replaced with value 1, and value false will be replaced with value 0.
	12. fins
	  Boolean = {true, false}. Value true will be replaced with value 1, and value false will be replaced with value 0.
	13. legs
	  Numeric variable (set of values: {0, 2, 4, 6, 8}).
	14. tail
	  Boolean = {true, false}. Value true will be replaced with value 1, and value false will be replaced with value 0.
	15. domestic
	  Boolean = {true, false}. Value true will be replaced with value 1, and value false will be replaced with value 0.
	16. catsize
	  Boolean = {true, false}. Value true will be replaced with value 1, and value false will be replaced with value 0.
	17.   Class-Set of animals
	  Numeric variable (set of values: {1, 2, 3, 4, 5, 6, 7}).
	1 - (41) aardvark, antelope, bear, boar, buffalo, calf, cavy, cheetah, deer, dolphin, elephant, fruitbat, giraffe, girl, goat, gorilla, hamster, hare, leopard, lion, lynx, mink, mole, mongoose, opossum, oryx, platypus, polecat, pony, porpoise, puma, pussycat, raccoon, reindeer, seal, sealion, squirrel, vampire, vole, wallaby, wolf
	2 - (20) chicken, crow, dove, duck, flamingo, gull, hawk, kiwi, lark, ostrich, parakeet, penguin, pheasant, rhea, skimmer, skua, sparrow, swan, vulture, wren
	3 - (5) pitviper, seasnake, slowworm, tortoise, tuatara
	4 - (13) bass, carp, catfish, chub, dogfish, haddock, herring, pike, piranha, seahorse, sole, stingray, tuna
	5 - frog, frog, newt, toad
	6 - flea, gnat, honeybee, housefly, ladybird, moth, termite, wasp
	7 - clam, crab, crayfish, lobster, octopus, scorpion, seawasp, slug, starfish, worm
	 
	1.,2.,...15.,16. - inputs
	17. - output
	
	The original data set that will be used in this experiment can be found at link http://archive.ics.uci.edu/ml/datasets/Zoo
	*/


	public class AnimalsClassificationSample : LearningEventListener
	{

		/// <param name="args"> the command line arguments </param>
		public static void Main(string[] args)
		{
			(new AnimalsClassificationSample()).run();
		}

		public virtual void run()
		{

			Console.WriteLine("Creating training set...");
			string trainingSetFileName = "data_sets/animals_data.txt";
			int inputsCount = 20;
			int outputsCount = 7;

			// create training set from file
			DataSet dataSet = DataSet.createFromFile(trainingSetFileName, inputsCount, outputsCount, "\t", true);


			Console.WriteLine("Creating neural network...");
			// create MultiLayerPerceptron neural network
			MultiLayerPerceptron neuralNet = new MultiLayerPerceptron(inputsCount, 22, outputsCount);


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
			neuralNet.save("MyNeuralNetAnimals.nnet");

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