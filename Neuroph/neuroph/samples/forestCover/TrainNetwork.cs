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
	using LearningEvent = org.neuroph.core.events.LearningEvent;
	using LearningEventListener = org.neuroph.core.events.LearningEventListener;
	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;
	using BackPropagation = org.neuroph.nnet.learning.BackPropagation;
	using MomentumBackpropagation = org.neuroph.nnet.learning.MomentumBackpropagation;

	public class TrainNetwork : LearningEventListener
	{

		private Config config;

		public TrainNetwork(Config config)
		{
			this.config = config;
		}

		//Creating and saving neural network to file
		public virtual void createNeuralNetwork()
		{
			Console.WriteLine("Creating neural network... ");
			MultiLayerPerceptron neuralNet = new MultiLayerPerceptron(config.InputCount, config.FirstHiddenLayerCount, config.SecondHiddenLayerCount, config.OutputCount);
			MomentumBackpropagation learningRule = (MomentumBackpropagation) neuralNet.LearningRule;
			learningRule.LearningRate = 0.01;
			learningRule.MaxError = 0.1;
			learningRule.MaxIterations = 1000;
			Console.WriteLine("Saving neural network to file... ");
			neuralNet.save(config.TrainedNetworkFileName);
			Console.WriteLine("Neural network successfully saved!");
		}

		//Training neural network with normalized balanced training data set
		public virtual void train()
		{
			Console.WriteLine("Training neural network... ");
			MultiLayerPerceptron neuralNet = (MultiLayerPerceptron) NeuralNetwork.createFromFile(config.TrainedNetworkFileName);

			DataSet dataSet = DataSet.load(config.NormalizedBalancedFileName);
			neuralNet.LearningRule.addListener(this);
			neuralNet.learn(dataSet);
			Console.WriteLine("Saving trained neural network to file... ");
			neuralNet.save(config.TrainedNetworkFileName);
			Console.WriteLine("Neural network successfully saved!");
		}

		//Formating decimal number to have 3 decimal places
		private string formatDecimalNumber(double number)
		{
			return (new decimal(number)).setScale(5, RoundingMode.HALF_UP).ToString();
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

	}

}