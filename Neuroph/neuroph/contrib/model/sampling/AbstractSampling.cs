using System.Collections.Generic;

namespace org.neuroph.contrib.model.sampling
{

	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;
	using Sampling = org.neuroph.util.data.sample.Sampling;


	/// <summary>
	/// Skeleton class which makes easy to implement concrete Sampling algorithms
	/// TODO: Remove this class
	/// </summary>
	public abstract class AbstractSampling : Sampling
	{

		protected internal readonly int numberOfSamples;

		public AbstractSampling(int numberOfSamples)
		{
			this.numberOfSamples = numberOfSamples;
		}


		/// <summary>
		/// Skeleton implementation for sample method
		/// </summary>
		/// <param name="dataSet"> initial data set </param>
		/// <returns> DataSets created using sampling algorithm </returns>
		public virtual List<DataSet> sample(DataSet dataSet)
		{
			populateInternalDataStructure(dataSet);

			List<DataSet> dataSets = new List<DataSet>(numberOfSamples);
			for (int i = 0; i < numberOfSamples; i++)
			{
				dataSets.Add(createDataSetFold(dataSet));
			}
			return dataSets;
		}


		private DataSet createDataSetFold(DataSet dataSet)
		{

			DataSet foldSet = new DataSet(dataSet.InputSize, dataSet.OutputSize);
			for (int j = 0; j < SampleSize; j++)
			{
				foldSet.addRow(NextDataSetRow);
			}

			return foldSet;
		}

		protected internal abstract int SampleSize {get;}


		/// <summary>
		/// SPI method which has to  be implemented in concrete Sampling algorithms
		/// </summary>
		/// <returns> DataSetRow using concrete sampling algorithm </returns>
		protected internal abstract DataSetRow NextDataSetRow {get;}

		/// <summary>
		/// SPI method which has to  be implemented in concrete Sampling algorithms
		/// </summary>
		/// <param name="dataSet"> creates and populates data structure required by sampling algorithm </param>
		protected internal abstract void populateInternalDataStructure(DataSet dataSet);


	}

}