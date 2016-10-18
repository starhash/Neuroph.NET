namespace org.neuroph.samples.norm
{

	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;
	using Normalizer = org.neuroph.util.data.norm.Normalizer;

	public class ZeroMeanNormalizer : Normalizer
	{

		internal double[] mean;

		public virtual void normalize(DataSet dataSet)
		{

			double[] maxInput = DataSetStatistics.calculateMaxByColumns(dataSet);
			double[] minInput = DataSetStatistics.calculateMinByColumns(dataSet);
			double[] meanInput = DataSetStatistics.calculateMean(dataSet);

			foreach (DataSetRow row in dataSet.Rows)
			{
				double[] normalizedInput = row.Input;

				for (int i = 0; i < dataSet.InputSize; i++)
				{
					double divider = maxInput[i] - minInput[i] == 0 ? 1 : maxInput[i] - minInput[i];
					normalizedInput[i] = (normalizedInput[i] - meanInput[i]) / divider;
				}
				row.Input = normalizedInput;
			}

		}

	}

}