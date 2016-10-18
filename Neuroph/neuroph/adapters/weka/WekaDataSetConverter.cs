using System.Collections;
using System.Collections.Generic;

namespace org.neuroph.adapters.weka
{

	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;
	using Attribute = weka.core.Attribute;
	using DenseInstance = weka.core.DenseInstance;
	using FastVector = weka.core.FastVector;
	using Instance = weka.core.Instance;
	using Instances = weka.core.Instances;

	/// <summary>
	/// Provides methods to convert dataset instances from Neuroph to Weka 
	/// and from Weka to Neuroph.
	/// 
	/// @author Zoran Sevarac
	/// </summary>
	public class WekaDataSetConverter
	{

		/// <summary>
		/// Converts Weka data set to Neuroph data set </summary>
		/// <param name="wekaDataset"> Instances Weka data set </param>
		/// <param name="numInputs"> int Number of inputs </param>
		/// <param name="numOutputs"> int Number of outputs </param>
		/// <returns> Neuroph data set </returns>
		public static DataSet convertWekaToNeurophDataset(Instances wekaDataset, int numInputs, int numOutputs)
		{

			if (numInputs <= 0)
			{
				throw new System.ArgumentException("Number of inputs  in DataSet cannot be zero or negative!");
			}

			if (numOutputs < 0)
			{
				throw new System.ArgumentException("Number of outputs  in DataSet cannot be negative!");
			}

			if (numOutputs + numInputs < wekaDataset.numAttributes())
			{
				throw new System.ArgumentException("Number of outputs and inputs should be equal to number of attributes from data set!");
			}

			// create supervised or unsupervised data set that will be returned
			DataSet neurophDataset = null;

			if (numOutputs > 0)
			{
				neurophDataset = new DataSet(numInputs,numOutputs);
			}
			else
			{
				neurophDataset = new DataSet(numInputs);
			}

			List<double?> classValues = new List<double?>();

			// get all different class values (as ints) from weka dataset
			foreach (Instance inst in wekaDataset)
			{
				double? classDouble = inst.classValue();
				if (!classValues.Contains(classDouble))
				{
					classValues.Add(classDouble);
				}
			}

			System.Collections.IEnumerator en = wekaDataset.enumerateInstances();
			while (en.hasMoreElements()) // iterate all instances from dataset
			{
				Instance instance = (Instance) en.nextElement();
				double[] values = instance.toDoubleArray(); // get all the values from current instance
				if (numOutputs == 0) // add unsupervised row
				{
					neurophDataset.addRow(values);
				} // add supervised row
				else
				{
					double[] inputs = new double[numInputs];
					double[] outputs = new double[numOutputs];

					// set inputs 
					for (int k = 0; k < values.Length; k++)
					{
						if (k < numInputs)
						{
							inputs[k] = values[k];
						}
					}

					// set binary values for class outputs
					int k = 0;
					foreach (double? entry in classValues)
					{
						if ((double)entry == instance.classValue()) // if the
						{
							outputs[k] = 1;
						}
						else
						{
							outputs[k] = 0;
						}
						k++;
					}

					DataSetRow row = new DataSetRow(inputs, outputs);
					row.Label = instance.stringValue(instance.classIndex());
					neurophDataset.addRow(row);
				}
			}

			return neurophDataset;
		}


		/// <summary>
		/// Converts Neuroph data set to Weka data set </summary>
		/// <param name="neurophDataset"> DataSet Neuroph data set </param>
		/// <returns> instances Weka data set </returns>
		public static Instances convertNeurophToWekaDataset(DataSet neurophDataset)
		{

			IDictionary<double[], string> classValues = getClassValues(neurophDataset);

			Instances instances = createEmptyWekaDataSet(neurophDataset.InputSize, neurophDataset.size(), classValues);

			int numInputs = neurophDataset.InputSize;
	//        int numOutputs = neurophDataset.getOutputSize();
			int numOutputs = 1; // why is this, and the above line is commented? probably because weka

			instances.ClassIndex = numInputs;

			IEnumerator<DataSetRow> iterator = neurophDataset.GetEnumerator();
			while (iterator.MoveNext()) // iterate all dataset rows
			{
				DataSetRow row = iterator.Current;

				if (numOutputs > 0) // if it is supervised (has outputs)
				{
					Instance instance = new DenseInstance(numInputs + numOutputs);
					for (int i = 0; i < numInputs; i++)
					{
						instance.setValue(i, row.Input[i]);
					}

					instance.Dataset = instances;

					// set output attribute, as String and double value of class
					foreach (KeyValuePair<double[], string> entry in classValues)
					{
						if (entry.Value.Equals(row.Label))
						{
							instance.setValue(numInputs, entry.Value);
							double[] rowDouble = row.DesiredOutput;
							for (int i = 0; i < rowDouble.Length; i++)
							{
								if (rowDouble[i] == 1)
								{
									instance.setValue(numInputs, i);
								}
								break;
							}
							break;
						}
					}


					instances.add(instance);
				} // if it is unsupervised - has only inputs
				else
				{
					// create new instance
					Instance instance = new DenseInstance(numInputs);
					// set all input values
					for (int i = 0; i < numInputs; i++)
					{
						instance.setValue(i, row.Input[i]);
					}
					// and add instance to weka dataset
					instance.Dataset = instances;
					instances.add(instance);
				}
			}

			return instances;
		}

		/// <summary>
		/// Creates and returns empty weka data set </summary>
		/// <param name="numOfAttr"> int Number of attributes without class attribute </param>
		/// <param name="capacity"> int Capacity of sample </param>
		/// <returns> empty weka data set </returns>
		private static Instances createEmptyWekaDataSet(int numOfAttr, int capacity, IDictionary<double[], string> classValues)
		{
			//Vector for class attribute possible values
			FastVector fvClassVal = new FastVector();
			//Map double value for every possible class value
			Hashtable classVals = new Dictionary<string, double?>();
			//Map class label with double key value
			Hashtable classValsDoubleAsKey = new Dictionary<double?, string>();
			//ind represents double value for class attribute
			int ind = 0;

			//loop through possible class values
			foreach (KeyValuePair<double[], string> values in classValues)
			{

				//add value to vector
				fvClassVal.addElement(values.Value);

				//map double value for class value
				classVals[values.Value] = new double?(ind);
				//map class label for double key value
				classValsDoubleAsKey[new double?(ind)] = values.Value;

				ind++;
			}
			//Class attribute with possible values
			Attribute classAttribute = new Attribute("theClass", fvClassVal, classValues.Count);
			//Creating attribute vector for Instances class instance
			FastVector fvWekaAttributes = new FastVector(numOfAttr + 1);
			//Fill vector with simple attributes
			for (int i = 0; i < numOfAttr; i++)
			{
				fvWekaAttributes.addElement(new Attribute(i + "", i));
			}
			//Add class attribute to vector
			fvWekaAttributes.addElement(classAttribute);

			//newDataSet as Instances class instance
			Instances newDataSet = new Instances("newDataSet", fvWekaAttributes, capacity);
			return newDataSet;
		}

		/// <summary>
		/// Returns all posisible class values </summary>
		/// <param name="neurophDataset"> Neuroph data set </param>
		/// <returns> Map with all possible class values <classValue, className> </returns>
		private static IDictionary<double[], string> getClassValues(DataSet neurophDataset)
		{
			IDictionary<double[], string> classValues = new Dictionary<double[], string>();

			foreach (DataSetRow row in neurophDataset.Rows)
			{
				if (!classValues.ContainsValue(row.Label))
				{
					classValues[row.DesiredOutput] = row.Label;
				}
			}

			return classValues;
		}

	}
}