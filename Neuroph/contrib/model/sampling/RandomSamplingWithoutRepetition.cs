namespace org.neuroph.contrib.model.sampling
{
    using System.Collections.Generic;
    using DataSet = org.neuroph.core.data.DataSet;
    using DataSetRow = org.neuroph.core.data.DataSetRow;


    /// <summary>
    /// Sampling algorithm where each element can be placed in multiple samples
    /// </summary>
    public class RandomSamplingWithoutRepetition : AbstractSampling
	{

		internal Queue<DataSetRow> dataDeque;


//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public RandomSamplingWithoutRepetition(final int numberOfFolds)
		public RandomSamplingWithoutRepetition(int numberOfFolds) : base(numberOfFolds)
		{
			dataDeque = new Queue<core.data.DataSetRow>();
		}

		protected internal override int SampleSize
		{
			get
			{
				return dataDeque.Count / numberOfSamples;
			}
		}

		protected internal override DataSetRow NextDataSetRow
		{
			get
			{
                return dataDeque.Dequeue();
			}
		}

		protected internal override void populateInternalDataStructure(DataSet dataSet)
		{
            foreach (DataSetRow r in dataSet.Rows)
                dataDeque.Enqueue(r);

		}
	}

}