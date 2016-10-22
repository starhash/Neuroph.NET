using System.Text;

namespace org.neuroph.contrib.eval
{

	using ClassificationMetrics = org.neuroph.contrib.eval.classification.ClassificationMetrics;
	using ConfusionMatrix = org.neuroph.contrib.eval.classification.ConfusionMatrix;
	using DataSet = org.neuroph.core.data.DataSet;

	/// <summary>
	/// Create class that will hold statistics for all evaluated datasets - avgs, mx, min, std, variation
	/// @author zoran
	/// </summary>
	public class EvaluationResult
	{
		// for now this aggregates hardcoded results from all evaluators
		internal ConfusionMatrix confusionMatrix;
		internal double meanSquareError;
		internal DataSet dataSet;
		// include neural net and data set?

		public virtual ConfusionMatrix ConfusionMatrix
		{
			get
			{
				return confusionMatrix;
			}
			set
			{
				this.confusionMatrix = value;
			}
		}


		public virtual ClassificationMetrics[] ClassificationMetricses
		{
			get
			{
				return ClassificationMetrics.createFromMatrix(confusionMatrix);
			}
		}

		public virtual double MeanSquareError
		{
			get
			{
				return meanSquareError;
			}
			set
			{
				this.meanSquareError = value;
			}
		}


		public virtual DataSet DataSet
		{
			get
			{
				return dataSet;
			}
			set
			{
				this.dataSet = value;
			}
		}


		public override string ToString()
		{
			//-- also display getClassificationMetricses here
			  ClassificationMetrics[] cms = ClassificationMetricses;
			  StringBuilder sb = new StringBuilder();
			  foreach (ClassificationMetrics c in cms)
			  {
				  sb.Append(c).AppendLine();
			  }

			return "EvaluationResult{" + "dataSet=" + dataSet.Label + ", meanSquareError=" + meanSquareError + ", \r\n confusionMatrix=\r\n" + confusionMatrix + "\r\n" + sb.ToString() + "}\r\n";
		}

	}

}