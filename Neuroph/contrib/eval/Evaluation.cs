using System;
using System.Collections.Generic;

namespace org.neuroph.contrib.eval
{

	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;
	using MeanSquaredError = org.neuroph.core.learning.error.MeanSquaredError;
	using BackPropagation = org.neuroph.nnet.learning.BackPropagation;
	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	using ClassificationMetrics = org.org.neuroph.contrib.eval.classification.ClassificationMetrics;
	using ConfusionMatrix = org.org.neuroph.contrib.eval.classification.ConfusionMatrix;

	/// <summary>
	/// Evaluation service used to run different evaluators on trained neural network
	/// </summary>
	public class Evaluation
	{

		private static Logger LOGGER = LoggerFactory.getLogger("neuroph");

		private IDictionary<Type, IEvaluator> evaluators = new Dictionary<Type, IEvaluator>();

		public Evaluation()
		{
			  addEvaluator(new ErrorEvaluator(new MeanSquaredError()));
		}

		/// <summary>
		/// Runs evaluation procedure for given neural network and data set through all evaluatoors
		/// Evaluation results are stored in evaluators
		/// </summary>
		/// <param name="neuralNetwork"> trained neural network </param>
		/// <param name="dataSet">       test data set used for evaluation </param>
		public virtual EvaluationResult evaluateDataSet(NeuralNetwork neuralNetwork, DataSet dataSet)
		{
			// first reset all evaluators
			foreach (IEvaluator evaluator in evaluators.Values) // for now we have only classification metrics and mse
			{
					evaluator.reset();
			}

			foreach (DataSetRow dataRow in dataSet.Rows) // iterate all dataset rows
			{
				 neuralNetwork.Input = dataRow.Input; // apply input to neural network
				 neuralNetwork.calculate(); // and calculate neural network

				// feed actual neural network along with desired output to all evaluators
				foreach (IEvaluator evaluator in evaluators.Values) // for now we have only kfold and mse
				{
					evaluator.processNetworkResult(neuralNetwork.Output, dataRow.DesiredOutput);
				}
			}

			// we should iterate all evaluators and getresults here- its hardcoded for now
			ConfusionMatrix confusionMatrix = ((ClassifierEvaluator.MultiClass)getEvaluator(typeof(ClassifierEvaluator.MultiClass))).Result;
			double meanSquaredError = ((ErrorEvaluator)getEvaluator(typeof(ErrorEvaluator))).Result;

			EvaluationResult result = new EvaluationResult();
			result.DataSet = dataSet;
			result.ConfusionMatrix = confusionMatrix;
			result.MeanSquareError = meanSquaredError;

			 // add neural network here too and maybe dataset too?

			return result;
		}

		/// 
		public virtual void addEvaluator<T>(Evaluator<T> evaluator) // <T extends Evaluator>     |  Class<T> type, T instance
		{
			if (evaluator == null)
			{
				throw new System.ArgumentException("Evaluator cannot be null!");
			}

			evaluators[evaluator.GetType()] = evaluator;
		}

		/// <param name="type"> concrete evaluator class </param>
		/// <returns> result of evaluation for given Evaluator type </returns>
		public virtual IEvaluator getEvaluator(Type type)
		{
			return evaluators[type];
		}

		/// <summary>
		/// Return all evaluators used for evaluation </summary>
		/// <returns>  </returns>
		public virtual IDictionary<Type, IEvaluator> Evaluators
		{
			get
			{
				return evaluators;
			}
		}

		public virtual double MeanSquareError
		{
			get
			{
			   return ((ErrorEvaluator)getEvaluator(typeof(ErrorEvaluator))).Result;
			}
		}


		/// <summary>
		/// Out of the box method (util) which computes all metrics for given neural network and test data set
		/// </summary>
		public static void runFullEvaluation(NeuralNetwork neuralNet, DataSet dataSet)
		{

			Evaluation evaluation = new Evaluation();
			// take onlu output column names here
			evaluation.addEvaluator(new ClassifierEvaluator.MultiClass(dataSet.ColumnNames)); // these two should be added by default

			evaluation.evaluateDataSet(neuralNet, dataSet);
		   // use logger here  - see how to make it print out
			// http://saltnlight5.blogspot.com/2013/08/how-to-configure-slf4j-with-different.html
			LOGGER.info("##############################################################################");
	//        LOGGER.info("Errors: ");
			LOGGER.info("MeanSquare Error: " + ((ErrorEvaluator)evaluation.getEvaluator(typeof(ErrorEvaluator))).Result);
			LOGGER.info("##############################################################################");
			ClassifierEvaluator classificationEvaluator = ((ClassifierEvaluator.MultiClass)evaluation.getEvaluator(typeof(ClassifierEvaluator.MultiClass)));
			ConfusionMatrix confusionMatrix = classificationEvaluator.Result;

			LOGGER.info("Confusion Matrix: \r\n" + confusionMatrix.ToString());


			LOGGER.info("##############################################################################");
			LOGGER.info("Classification metrics: ");
			ClassificationMetrics[] metrics = ClassificationMetrics.createFromMatrix(confusionMatrix);
			foreach (ClassificationMetrics cm in metrics)
			{
				LOGGER.info(cm.ToString());
			}

			LOGGER.info("##############################################################################");
		}

	}

}