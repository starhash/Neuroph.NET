namespace org.neuroph.util.data.norm
{

	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;

	/// <summary>
	/// This class does normalization of a data set to specified range
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class RangeNormalizer : Normalizer
	{
		private double lowLimit = 0, highLimit = 1;
		internal double[] maxIn, maxOut; // contains max values for in and out columns
		internal double[] minIn, minOut; // contains min values for in and out columns

		public RangeNormalizer(double lowLimit, double highLimit)
		{
			this.lowLimit = lowLimit;
			this.highLimit = highLimit;
		}

		public virtual void normalize(DataSet dataSet)
		{
			findMaxAndMinVectors(dataSet);

			foreach (DataSetRow row in dataSet.Rows)
			{
				double[] normalizedInput = normalizeToRange(row.Input, minIn, maxIn);
				row.Input = normalizedInput;

				if (dataSet.Supervised)
				{
					double[] normalizedOutput = normalizeToRange(row.DesiredOutput, minOut, maxOut);
					row.DesiredOutput = normalizedOutput;
				}

			}

		}

		private double[] normalizeToRange(double[] vector, double[] min, double[] max)
		{
			double[] normalizedVector = new double[vector.Length];

			for (int i = 0; i < vector.Length; i++)
			{
				normalizedVector[i] = ((vector[i] - min[i]) / (max[i] - min[i])) * (highLimit - lowLimit) + lowLimit;
			}

			return normalizedVector;
		}



		private void findMaxAndMinVectors(DataSet dataSet)
		{
			int inputSize = dataSet.InputSize;
			int outputSize = dataSet.OutputSize;

			maxIn = new double[inputSize];
			minIn = new double[inputSize];

			for (int i = 0; i < inputSize; i++)
			{
				maxIn[i] = double.Epsilon;
				minIn[i] = double.MaxValue;
			}

			maxOut = new double[outputSize];
			minOut = new double[outputSize];

			for (int i = 0; i < outputSize; i++)
			{
				maxOut[i] = double.Epsilon;
				minOut[i] = double.MaxValue;
			}

			foreach (DataSetRow dataSetRow in dataSet.Rows)
			{
				double[] input = dataSetRow.Input;
				for (int i = 0; i < inputSize; i++)
				{
					if (input[i] > maxIn[i])
					{
						maxIn[i] = input[i];
					}
					if (input[i] < minIn[i])
					{
						minIn[i] = input[i];
					}
				}

				double[] output = dataSetRow.DesiredOutput;
				for (int i = 0; i < outputSize; i++)
				{
					if (output[i] > maxOut[i])
					{
						maxOut[i] = output[i];
					}
					if (output[i] < minOut[i])
					{
						minOut[i] = output[i];
					}
				}

			}
		}

	}

}