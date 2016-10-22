using System.Collections;
using System.Collections.Generic;

namespace org.neuroph.adapters.jml
{

	using Dataset = net.sf.javaml.core.Dataset;
	using DefaultDataset = net.sf.javaml.core.DefaultDataset;
	using DenseInstance = net.sf.javaml.core.DenseInstance;
	using Instance = net.sf.javaml.core.Instance;
	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;

	/// <summary>
	/// Provides methods to convert dataset instances from Neuroph to JML,
	/// and from JML to Neuroph
	/// @author Zoran Sevarac
	/// @author Vladimir Markovic
	/// </summary>
	public class JMLDataSetConverter
	{

		/// <summary>
		/// Converts Java-ML data set to Neuroph data set </summary>
		/// <param name="jmlDataset"> Dataset Java-ML data set </param>
		/// <param name="numInputs"> int Number of inputs </param>
		/// <param name="numOutputs"> int Number of outputs </param>
		/// <returns> Neuroph data set </returns>
		public static DataSet convertJMLToNeurophDataset(Dataset jmlDataset, int numInputs, int numOutputs)
		{

			if (numInputs <= 0)
			{
				throw new System.ArgumentException("Number of inputs  in DataSet cannot be zero or negative!");
			}

			if (numOutputs < 0)
			{
				throw new System.ArgumentException("Number of outputs  in DataSet cannot be negative!");
			}

			// get number of attributes + 1 if class counts as attribute
			int rowSize = jmlDataset.noAttributes() + 1;
			if (numOutputs + numInputs < rowSize)
			{
				throw new System.ArgumentException("Number of outputs and inputs should be equal to number of attributes from data set!");
			}

			// create dataset
			DataSet neurophDataset;

			if (numOutputs == 0)
			{
				neurophDataset = new DataSet(rowSize);
			}
			else
			{
				neurophDataset = new DataSet(numInputs, numOutputs);
			}


			List<string> outputClasses = new List<string>();

			for (int i = 0; i < jmlDataset.size(); i++)
			{
				if (!outputClasses.Contains(jmlDataset.get(i).classValue().ToString()))
				{
					outputClasses.Add(jmlDataset.get(i).classValue().ToString());
				}
			}

			// fill neuroph dataset from jml dataset
			for (int i = 0; i < jmlDataset.size(); i++)
			{
				IEnumerator attributeIterator = jmlDataset.get(i).GetEnumerator();

				double[] values = new double[rowSize];
				int index = 0;

				while (attributeIterator.hasNext())
				{
					double? attrValue = (double?) attributeIterator.next();
					values[index] = (double)attrValue;
					index++;
				}

				DataSetRow row = null;
				if (numOutputs == 0)
				{
					row = new DataSetRow(values);
				}
				else
				{
					double[] inputs = new double[numInputs];
					double[] outputs = new double[outputClasses.Count];
					int k = 0;
					int j = 0;
					for (int v = 0; v < values.Length; v++)
					{
						if (v < numInputs)
						{
							inputs[j] = values[v];
							j++;
						}
					}

					foreach (string cla in outputClasses)
					{
						if (cla.Equals(jmlDataset.get(i).classValue().ToString()))
						{
							outputs[k] = 1;
						}
						else
						{
							outputs[k] = 0;
						}
						k++;
					}

					row = new DataSetRow(inputs, outputs);
				}
				row.Label = jmlDataset.get(i).classValue().ToString();
				neurophDataset.addRow(row);
			}

			return neurophDataset;
		}

		/// <summary>
		/// Converts Neuroph data set to Java-ML data set </summary>
		/// <param name="neurophDataset"> Dataset Neuroph data set </param>
		/// <returns> Dataset Java-ML data set </returns>
		public static Dataset convertNeurophToJMLDataset(DataSet neurophDataset)
		{
			Dataset jmlDataset = new DefaultDataset();

			int numInputs = neurophDataset.InputSize;
			int numOutputs = neurophDataset.OutputSize;

			foreach (DataSetRow row in neurophDataset.Rows)
			{

				if (numOutputs > 0)
				{
					double[] mergedIO = new double[numInputs + numOutputs];
					for (int i = 0; i < numInputs; i++)
					{
						mergedIO[i] = row.Input[i];
					}

					for (int i = 0; i < numOutputs; i++)
					{
						mergedIO[numInputs + i] = row.DesiredOutput[i];
					}

					Instance instance = new DenseInstance(mergedIO);
					instance.ClassValue = row.Label;
					jmlDataset.add(instance);
				}
				else
				{
					Instance instance = new DenseInstance(row.Input);
					instance.ClassValue = row.Label;
					jmlDataset.add(instance);
				}
			}

			return jmlDataset;
		}
	}
}