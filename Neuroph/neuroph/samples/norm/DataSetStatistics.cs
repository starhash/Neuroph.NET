using System;

namespace org.neuroph.samples.norm
{

	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;

	public class DataSetStatistics
	{

		public static double[] calculateMean(DataSet dataSet)
		{
			double[] mean = new double[dataSet.InputSize];

			foreach (DataSetRow row in dataSet.Rows)
			{
				double[] currentInput = row.Input;
				for (int i = 0; i < dataSet.InputSize; i++)
				{
					mean[i] += currentInput[i];
				}
			}
			for (int i = 0; i < dataSet.InputSize; i++)
			{
				mean[i] /= dataSet.Rows.Count;
			}
			return mean;
		}

		public static double[] calculateMaxByColumns(DataSet dataSet)
		{

			int inputSize = dataSet.InputSize;
			double[] maxColumnElements = new double[inputSize];

			for (int i = 0; i < inputSize; i++)
			{
				maxColumnElements[i] = double.Epsilon;
			}

			foreach (DataSetRow dataSetRow in dataSet.Rows)
			{
				double[] input = dataSetRow.Input;
				for (int i = 0; i < inputSize; i++)
				{
					maxColumnElements[i] = Math.Max(maxColumnElements[i], input[i]);
				}
			}

			return maxColumnElements;
		}

		public static double[] calculateMinByColumns(DataSet dataSet)
		{

			int inputSize = dataSet.InputSize;
			double[] minColumnElements = new double[inputSize];

			for (int i = 0; i < inputSize; i++)
			{
				minColumnElements[i] = double.MaxValue;
			}

			foreach (DataSetRow dataSetRow in dataSet.Rows)
			{
				double[] input = dataSetRow.Input;
				for (int i = 0; i < inputSize; i++)
				{
					minColumnElements[i] = Math.Min(minColumnElements[i], input[i]);
				}
			}
			return minColumnElements;
		}

	}

}