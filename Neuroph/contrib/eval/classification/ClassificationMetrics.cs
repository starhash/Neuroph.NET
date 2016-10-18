using System;
using System.Collections.Generic;
using System.Text;

namespace org.org.neuroph.contrib.eval.classification
{


//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to .NET:

	/// <summary>
	/// Container class for all metrics which use confusion matrix for their computation 
	/// 
	/// Based on:
	/// http://java-ml.sourceforge.net/api/0.1.7/net/sf/javaml/classification/evaluation/PerformanceMeasure.html
	/// http://sourceforge.net/p/java-ml/java-ml-code/ci/a25ddde7c3677da44e47a643f88e32e2c8bbc32f/tree/net/sf/javaml/classification/evaluation/PerformanceMeasure.java
	/// 
	/// http://en.wikipedia.org/wiki/Matthews_correlation_coefficient
	/// 
	/// </summary>
	public class ClassificationMetrics
	{

		internal double falseNegative;
		internal double falsePositive;
		internal double trueNegative;
		internal double truePositive;
		internal double total;

		internal string classLabel;


	   /// <summary>
	   /// Constructs a new measure using arguments
	   /// TODO: add class to which measure corresponds?
	   /// </summary>
	   /// <param name="truePositive"> </param>
	   /// <param name="trueNegative"> </param>
	   /// <param name="falsePositive"> </param>
	   /// <param name="falseNegative"> </param>
		public ClassificationMetrics(int truePositive, int trueNegative, int falsePositive, int falseNegative)
		{
			this.truePositive = truePositive;
			this.trueNegative = trueNegative;
			this.falsePositive = falsePositive;
			this.falseNegative = falseNegative;
			this.total = falseNegative + falsePositive + trueNegative + truePositive;
		}

		public virtual string ClassLabel
		{
			get
			{
				return classLabel;
			}
			set
			{
				this.classLabel = value;
			}
		}


		/// <summary>
		/// Calculate and return classification accuracy measure </summary>
		/// <returns>  </returns>
		public virtual double Accuracy
		{
			get
			{
				return (truePositive + trueNegative) / total;
			}
		}

		public virtual double Precision
		{
			get
			{
				return truePositive / (double)(truePositive + falsePositive);
			}
		}

		public virtual double Recall
		{
			get
			{
				return this.truePositive / (double)(this.truePositive + this.falseNegative);
			}
		}

		/// <summary>
		/// Calculate and return classification true positive rate, recall, sensitivity </summary>
		/// <returns>  </returns>
		public virtual double Sensitivity
		{
			get
			{
				return truePositive / (truePositive + falseNegative);
			}
		}

		//Specifity

		public virtual double Specificity
		{
			get
			{
				return trueNegative / (trueNegative + falsePositive);
			}
		}

		public virtual double FalsePositiveRate
		{
			get
			{
				return falsePositive / (falsePositive + trueNegative);
			}
		}

		//False negative rate,

		public virtual double FalseNegativeRate
		{
			get
			{
				return falseNegative / (falseNegative + truePositive);
			}
		}

		public virtual double ErrorRate
		{
			get
			{
				return (this.falsePositive + this.falseNegative) / total;
			}
		}

		//Total
		public virtual double Total
		{
			get
			{
				return total;
			}
		}

		public virtual double FalseDiscoveryRate
		{
			get
			{
				return falsePositive / (truePositive + falsePositive);
			}
		}

		// http://en.wikipedia.org/wiki/Matthews_correlation_coefficient
		//  measure of the quality of binary (two-class) classifications. It takes into account true and false positives and negatives and is generally regarded as a balanced measure which can be used even if the classes are of very different sizes.     
		public virtual double MatthewsCorrelationCoefficient
		{
			get
			{
				return (truePositive * trueNegative - falsePositive * falseNegative) / (System.Math.Sqrt((truePositive + falsePositive) * (truePositive + falseNegative) * (trueNegative + falsePositive) * (trueNegative + falseNegative)));
			}
		}



	   /// <summary>
	   /// Calculates F-score for beta equal to 1.
	   /// </summary>
	   /// <returns> f-score </returns>
		public virtual double FMeasure
		{
			get
			{
				return getFMeasure(1);
			}
		}

		/// <summary>
		/// Returns the F-score. When recall and precision are zero, this method will
		/// return 0.
		/// </summary>
		/// <param name="beta"> </param>
		/// <returns> f-score </returns>
		public virtual double getFMeasure(int beta)
		{
			double f = ((beta * beta + 1) * Precision * Recall) / (double)(beta * beta * Precision + Recall);
			if (double.IsNaN(f))
			{
				return 0;
			}
			else
			{
				return f;
			}
		}

		public virtual double Q9
		{
			get
			{
				if (truePositive + falseNegative == 0)
				{
					return (trueNegative - falsePositive) / (trueNegative + falsePositive);
				}
				else if (trueNegative + falsePositive == 0)
				{
					return (truePositive - falseNegative) / (truePositive + falseNegative);
				}
				else
				{
					return 1 - Math.Sqrt(2) * Math.Sqrt(Math.Pow(falseNegative / (truePositive + falseNegative), 2) + Math.Pow(falsePositive / (trueNegative + falsePositive), 2));
				}
			}
		}


		public virtual double BalancedClassificationRate
		{
			get
			{
				if (trueNegative == 0 && falsePositive == 0)
				{
					return truePositive / (truePositive + falseNegative);
				}
				if (truePositive == 0 && falseNegative == 0)
				{
					return trueNegative / (trueNegative + falsePositive);
				}
    
				return 0.5 * (truePositive / (truePositive + falseNegative) + trueNegative / (trueNegative + falsePositive));
			}
		}




		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("Class: " + classLabel).Append("\n");
			sb.Append("Total items: ").Append(Total).Append("\n");
			sb.Append("True positive:").Append(truePositive).Append("\n");
			sb.Append("True negative:").Append(trueNegative).Append("\n");
			sb.Append("False positive:").Append(falsePositive).Append("\n");
			sb.Append("False negative:").Append(falseNegative).Append("\n");
			sb.Append("Sensitivity or true positive rate (TPR): ").Append(Sensitivity).Append("\n");
			sb.Append("Specificity (SPC) or true negative rate (TNR): ").Append(Specificity).Append("\n");
			sb.Append("Fall-out or false positive rate (FPR): ").Append(FalsePositiveRate).Append("\n");
			sb.Append("False negative rate (FNR): ").Append(FalseNegativeRate).Append("\n");
			sb.Append("Accuracy (ACC): ").Append(Accuracy).Append("\n");
			sb.Append("Precision or positive predictive value (PPV): ").Append(Precision).Append("\n");
			sb.Append("Recall: ").Append(Recall).Append("\n");
			sb.Append("F-measure: ").Append(FMeasure).Append("\n");
			sb.Append("False discovery rate (FDR): ").Append(FalseDiscoveryRate).Append("\n");
			sb.Append("Matthews correlation Coefficient (MCC): ").Append(MatthewsCorrelationCoefficient).Append("\n");
			return sb.ToString();
		}




		public class Stats
		{
			public double accuracy = 0;
			public double precision = 0;
			public double recall = 0;
			public double fScore = 0;
			public double mserror = 0;

			public override string ToString()
			{
				return "Stats{" + "accuracy=" + accuracy + ", precision=" + precision + ", recall=" + recall + ", fScore=" + fScore + ", mserror=" + mserror + '}';
			}


		}


		public static ClassificationMetrics[] createFromMatrix(ConfusionMatrix confusionMatrix)
		{
			// Create Classification measure for each class 
			ClassificationMetrics[] measures = new ClassificationMetrics[confusionMatrix.ClassCount];
			string[] classLabels = confusionMatrix.ClassLabels;

			for (int clsIdx = 0; clsIdx < confusionMatrix.ClassCount; clsIdx++) // for each class
			{
				// ove metode mozda ubaciti u matricu Confusion matrix - najbolje tako
				int tp = confusionMatrix.getTruePositive(clsIdx);
				int tn = confusionMatrix.getTrueNegative(clsIdx);
				int fp = confusionMatrix.getFalsePositive(clsIdx);
				int fn = confusionMatrix.getFalseNegative(clsIdx);

				measures[clsIdx] = new ClassificationMetrics(tp, tn, fp, fn);
				measures[clsIdx].ClassLabel = classLabels[clsIdx];
			}

			return measures;
		}



		/// 
		/// <param name="results"> list of different metric results computed on different sets of data </param>
		/// <returns> average metrics computed different MetricResults </returns>
		public static ClassificationMetrics.Stats average(ClassificationMetrics[] results)
		{
			List<string> classLabels = new List<string>();
			 ClassificationMetrics.Stats average = new ClassificationMetrics.Stats();
			  double count = 0;
				foreach (ClassificationMetrics cm in results)
				{
					average.accuracy += cm.Accuracy;
					average.precision += cm.Precision;
					average.recall += cm.Recall;
					average.fScore += cm.FMeasure;
	//                average.mserror += er.getMeanSquareError();

					if (!classLabels.Contains(cm.ClassLabel))
					{
						classLabels.Add(cm.ClassLabel);
					}
				}
				count++;

			count = count * classLabels.Count; // * classes count
			average.accuracy = average.accuracy / count;
			average.precision = average.precision / count;
			average.recall = average.recall / count;
			average.fScore = average.fScore / count;
			average.mserror = average.mserror / count;

			return average;
		}

		/// 
		/// <param name="results"> list of different metric results computed on different sets of data </param>
		/// <returns> maximum metrics computed different MetricResults </returns>
	//    public static ClassificationMetrics maxFromMultipleRuns(List<ClassificationMetrics> results) {
	//        double maxAccuracy = 0;
	//        double maxError = 0;
	//        double maxPrecision = 0;
	//        double maxRecall = 0;
	//        double maxFScore = 0;
	//
	//        for (ClassificationMetrics metricResult : results) {
	//            maxAccuracy = Math.max(maxAccuracy, metricResult.getAccuracy());
	//            maxError = Math.max(maxError, metricResult.getError());
	//            maxPrecision = Math.max(maxPrecision, metricResult.getPrecision());
	//            maxRecall = Math.max(maxRecall, metricResult.getRecall());
	//            maxFScore = Math.max(maxFScore, metricResult.getFScore());
	//        }
	//
	//        ClassificationMetrics averageMetricsResult = new ClassificationMetrics();
	//
	//        averageMetricsResult.accuracy = maxAccuracy;
	//        averageMetricsResult.error = maxError;
	//        averageMetricsResult.precision = maxPrecision;
	//        averageMetricsResult.recall = maxRecall;
	//        averageMetricsResult.fScore = maxFScore;
	//
	//        return averageMetricsResult;
	//    }



	//    private static double[] createFScoresForEachClass(double[] precisions, double[] recalls) {
	//        double[] fScores = new double[precisions.length];
	//
	//        for (int i = 0; i < precisions.length; i++) {
	//            fScores[i] = 2 * (precisions[i] * recalls[i]) / (precisions[i] + recalls[i]);
	//        }
	//
	//        return fScores;
	//    }


	//    private static double safelyDivide(double x, double y) {
	//        double divisor = x == 0.0 ? 1 : x;
	//        double divider = y == 0.0 ? 1.0 : y;
	//        return divisor / divider;
	//    }


	}
}