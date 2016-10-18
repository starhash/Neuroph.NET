using System;

namespace org.neuroph.contrib.model.sampling
{

	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;

	/// <summary>
	/// Sampling algorithm where each element can be placed only in one sample
	/// </summary>
	public class RandomSamplingWithRepetition : AbstractSampling
	{

		private DataSet dataSet;

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public RandomSamplingWithRepetition(final int numberOfSamples)
		public RandomSamplingWithRepetition(int numberOfSamples) : base(numberOfSamples)
		{
		}

		protected internal override int SampleSize
		{
			get
			{
				return dataSet.size();
			}
		}

		protected internal override DataSetRow NextDataSetRow
		{
			get
			{
				return dataSet.getRowAt((new Random()).Next(dataSet.size()));
			}
		}

		protected internal override void populateInternalDataStructure(DataSet dataSet)
		{
			this.dataSet = dataSet;
		}

	}

}