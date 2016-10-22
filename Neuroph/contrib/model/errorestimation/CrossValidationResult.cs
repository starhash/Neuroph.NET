using System.Collections.Generic;

namespace org.neuroph.contrib.model.errorestimation
{

	using EvaluationResult = org.neuroph.contrib.eval.EvaluationResult;
	using ClassificationMetrics = org.neuroph.contrib.eval.classification.ClassificationMetrics;

	/// 
	/// <summary>
	/// @author zoran
	/// </summary>
	public class CrossValidationResult
	{
		internal List<EvaluationResult> results;
		internal ClassificationMetrics.Stats average;

		public CrossValidationResult()
		{
			results = new List<EvaluationResult>();
		}

		public virtual void addEvaluationResult(EvaluationResult result)
		{
			results.Add(result);
		}

		// add statistics here? 

		// calculate avg, max, min, variation, std,  for 

		// should we also calculate stats for each class???           

		/// <summary>
		/// Calculate average over all classes and datasets
		/// </summary>
		public virtual void calculateStatistics()
		{

			average = new ClassificationMetrics.Stats();
		   // ClassificationMetrics.Stats max = new ClassificationMetrics.Stats();

			double count = 0;

			List<string> classLabels = new List<string>();
			// avg max min variation std
			foreach (EvaluationResult er in results)
			{
				ClassificationMetrics[] ccm = er.ClassificationMetricses;
				foreach (ClassificationMetrics cm in ccm)
				{
					average.accuracy += cm.Accuracy;
					average.precision += cm.Precision;
					average.recall += cm.Recall;
					average.fScore += cm.FMeasure;
					average.mserror += er.MeanSquareError;

					if (!classLabels.Contains(cm.ClassLabel))
					{
						classLabels.Add(cm.ClassLabel);
					}
				}
				count++;
			}
			count = count * classLabels.Count; // * classes count
			average.accuracy = average.accuracy / count;
			average.precision = average.precision / count;
			average.recall = average.recall / count;
			average.fScore = average.fScore / count;
			average.mserror = average.mserror / count;

		}

		public virtual ClassificationMetrics.Stats Averages
		{
			get
			{
				return average;
			}
		}

		public override string ToString()
		{
			return "CrossValidationResult{" + "results=" + results + ", average=" + average + '}';
		}




	}

}