using System;

namespace org.neuroph.samples.crossval
{

	using ClassifierEvaluator = org.neuroph.contrib.eval.ClassifierEvaluator;
	using CrossValidation = org.neuroph.contrib.model.errorestimation.CrossValidation;
	using CrossValidationResult = org.neuroph.contrib.model.errorestimation.CrossValidationResult;
	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;
	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;

	/// 
	/// <summary>
	/// @author zoran
	/// </summary>
	public class IrisCrossValidationSample
	{

		public static void Main(string[] args)
		{
			// create data set from csv file
			MultiLayerPerceptron neuralNet = (MultiLayerPerceptron) NeuralNetwork.createFromFile("irisNet.nnet");
			DataSet dataSet = DataSet.createFromFile("data_sets/iris_data_normalised.txt", 4, 3, ",");
			string[] classNames = new string[] {"Virginica", "Setosa", "Versicolor"};

			CrossValidation crossval = new CrossValidation(neuralNet, dataSet, 5);
			crossval.addEvaluator(new ClassifierEvaluator.MultiClass(classNames));

			crossval.run();
			CrossValidationResult results = crossval.Result;
			Console.WriteLine(results);

		}




	}

}