using System;
using System.Collections.Generic;

namespace org.neuroph.contrib.model.errorestimation
{

	using Evaluation = org.neuroph.contrib.eval.Evaluation;
	using ClassificationMetrics = org.org.neuroph.contrib.eval.classification.ClassificationMetrics;
	using ClassifierEvaluator = org.neuroph.contrib.eval.ClassifierEvaluator;
	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;
	using BackPropagation = org.neuroph.nnet.learning.BackPropagation;
	using Sampling = org.neuroph.util.data.sample.Sampling;
	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	using ErrorEvaluator = org.neuroph.contrib.eval.ErrorEvaluator;
	using EvaluationResult = org.neuroph.contrib.eval.EvaluationResult;
	using org.neuroph.contrib.eval;
	using ConfusionMatrix = org.org.neuroph.contrib.eval.classification.ConfusionMatrix;
	using SubSampling = org.neuroph.util.data.sample.SubSampling;

	/// <summary>
	/// This class implements cross validation procedure.
	/// Splits data set into several subsets, trains with one and tests with all other subsets.
	/// Repeats that procedure with each subset
	/// 
	/// At the end runs evaluation
	/// 
	/// </summary>
	public class CrossValidation
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger LOGGER = LoggerFactory.getLogger(typeof(CrossValidation).FullName);

		/// <summary>
		/// Neural network to train
		/// </summary>
		private NeuralNetwork neuralNetwork;

		/// <summary>
		/// Data set to use for training
		/// </summary>
		private DataSet dataSet;

		/// <summary>
		/// Data set sampling algorithm used. 
		/// By default uses random subsampling without repetition
		/// </summary>
		private Sampling sampling;

		/// <summary>
		/// Evaluation procedure. Holds a collection of evaluators which can be automaticaly added
		/// </summary>
		private Evaluation evaluation = new Evaluation();


		private CrossValidationResult results;


		/// <summary>
		/// Default constructor for creating KFold error estimation
		/// </summary>
		/// <param name="subsetCount"> defines number of folds used in sampling algorithm </param>
		public CrossValidation(NeuralNetwork neuralNetwork, DataSet dataSet, int subSetCount) // number of folds/subsets
		{
			this.neuralNetwork = neuralNetwork;
			this.dataSet = dataSet;
			this.sampling = new SubSampling(subSetCount); // new RandomSamplingWithoutRepetition(numberOfSamples
		}


		public CrossValidation(NeuralNetwork neuralNetwork, DataSet dataSet, params int[] subSetSizes) // number of folds
		{
			this.neuralNetwork = neuralNetwork;
			this.dataSet = dataSet;
			this.sampling = new SubSampling(subSetSizes);
		}

		public CrossValidation(NeuralNetwork neuralNetwork, DataSet dataSet, Sampling sampling) // number of folds/subsets
		{
			this.neuralNetwork = neuralNetwork;
			this.dataSet = dataSet;
			this.sampling = sampling;
		}


		public virtual Sampling Sampling
		{
			get
			{
				return sampling;
			}
			set
			{
				this.sampling = value;
			}
		}


		public virtual Evaluation Evaluation
		{
			get
			{
				return evaluation;
			}
		}

		// kfolding is done here
		// provide neural network and data set - thi is the main entry point for crossvalidation
		public virtual void run()
		{
		 //   evaluation.addEvaluator(ClassificationEvaluator.createForDataSet(dataSet)); // this should be added elseewhere

			// create subsets of the entire datasets that will be used for k-folding
			List<DataSet> dataSets = sampling.sample(dataSet);
			results = new CrossValidationResult();

			//TODO Good place for parallelization. // But in order to make this possible NeuralNetwork must be cloneable or immutable
			for (int i = 0; i < dataSets.Count; i++)
			{
				neuralNetwork.randomizeWeights(); // we shouldnt do this - we should clone the original network
				dataSets[i].Label = dataSet.Label + "-subset-" + i;
				neuralNetwork.learn(dataSets[i]); // train neural network with i-th data set fold

				for (int j = 0; j < dataSets.Count; j++) // next do the testing with all other dataset folds
				{
					if (j == i)
					{
						continue; // dont use for testing the same dataset that was used for training
					}

				  // testNetwork(neuralNetwork, dataSets.get(j));
					EvaluationResult evaluationResult = evaluation.evaluateDataSet(neuralNetwork, dataSets[j]); // this method should return all evaluation results
					results.addEvaluationResult(evaluationResult);
		   //       results.add(result);
				   // get all the results from the single evaluation - for each evaluator Classifiaction and Error
				   // store it somewhere with neural network

				   // save evaluation results from multiple runs  and then calculateaverages

				   // we should also save all these trained network along w ith their evaluation results or at least store them intor array...
				   // ne need to store evaluation results and neural network for each run 
				}
			}
			results.calculateStatistics();

		}

		public virtual void addEvaluator<T>(eval.Evaluator<T> eval)
		{
			evaluation.addEvaluator(eval);
		}

		public virtual T getEvaluator<T>(Type type)
		{
			return ((T)evaluation.getEvaluator(type));
		}

		public virtual CrossValidationResult Result
		{
			get
			{
				return results;
			}
		}



		// TODO: dont sysout - store somewhere these results so they can be displayed
		// 
		private void testNetwork(NeuralNetwork neuralNetwork, DataSet testSet)
		{
			evaluation.evaluateDataSet(neuralNetwork, testSet);
			// works for binary what if we have multiple classes - how to get results for multiple classes here? 
	  //      results.add(evaluation.getEvaluator(ClassificationMetricsEvaluator.class).getResult()[0]); // MUST BE FIXED!!!!! get all and add thm all to results

			System.Console.WriteLine("##############################################################################");
			System.Console.WriteLine("MeanSquare Error: " + ((ErrorEvaluator)evaluation.getEvaluator(typeof(ErrorEvaluator))).Result);
			System.Console.WriteLine("##############################################################################");

			// TODO: deal with BinaryClassifiers too here
			ClassifierEvaluator evaluator = ((ClassifierEvaluator.MultiClass)evaluation.getEvaluator(typeof(ClassifierEvaluator.MultiClass)));
			ConfusionMatrix confusionMatrix = evaluator.Result;

			System.Console.WriteLine("Confusion Matrix: \r\n" + confusionMatrix.ToString());

			System.Console.WriteLine("##############################################################################");
			System.Console.WriteLine("Classification metrics: ");
			ClassificationMetrics[] metrics = ClassificationMetrics.createFromMatrix(confusionMatrix); // add all of these to result

			foreach (ClassificationMetrics cm in metrics)
			{
				System.Console.WriteLine(cm.ToString());
			}

			System.Console.WriteLine("##############################################################################");
		}

	}

}