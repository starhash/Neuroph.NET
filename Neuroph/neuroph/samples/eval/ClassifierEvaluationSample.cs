using System;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
namespace org.neuroph.samples.eval
{

	using ClassifierEvaluator = org.neuroph.contrib.eval.ClassifierEvaluator;
	using ErrorEvaluator = org.neuroph.contrib.eval.ErrorEvaluator;
	using Evaluation = org.neuroph.contrib.eval.Evaluation;
	using ClassificationMetrics = org.neuroph.contrib.eval.classification.ClassificationMetrics;
	using ConfusionMatrix = org.neuroph.contrib.eval.classification.ConfusionMatrix;
	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;
	using MeanSquaredError = org.neuroph.core.learning.error.MeanSquaredError;
	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;

	/// 
	/// <summary>
	/// @author zoran
	/// </summary>
	public class ClassifierEvaluationSample
	{

		public static void Main(string[] args)
		{
			Evaluation evaluation = new Evaluation();
			evaluation.addEvaluator(new ErrorEvaluator(new MeanSquaredError()));

			string[] classNames = new string[] {"Virginica", "Setosa", "Versicolor"};


			MultiLayerPerceptron neuralNet = (MultiLayerPerceptron) NeuralNetwork.createFromFile("irisNet.nnet");
			DataSet dataSet = DataSet.createFromFile("data_sets/iris_data_normalised.txt", 4, 3, ",");

			evaluation.addEvaluator(new ClassifierEvaluator.MultiClass(classNames));
			evaluation.evaluateDataSet(neuralNet, dataSet);

			ClassifierEvaluator evaluator = evaluation.getEvaluator(typeof(ClassifierEvaluator.MultiClass));
			ConfusionMatrix confusionMatrix = evaluator.Result;
			Console.WriteLine("Confusion matrrix:\r\n");
			Console.WriteLine(confusionMatrix.ToString() + "\r\n\r\n");
			Console.WriteLine("Classification metrics\r\n");
			ClassificationMetrics[] metrics = ClassificationMetrics.createFromMatrix(confusionMatrix);
			ClassificationMetrics.Stats average = ClassificationMetrics.average(metrics);
			foreach (ClassificationMetrics cm in metrics)
			{
				Console.WriteLine(cm.ToString() + "\r\n");
			}
			Console.WriteLine(average.ToString());

		}

	}

}