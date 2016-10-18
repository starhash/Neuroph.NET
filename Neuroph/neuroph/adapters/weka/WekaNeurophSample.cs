using System;
using System.Collections;

namespace org.neuroph.adapters.weka
{

	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;
	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;
	using DenseInstance = weka.core.DenseInstance;
	using Instance = weka.core.Instance;
	using Instances = weka.core.Instances;
	using DataSource = weka.core.converters.ConverterUtils.DataSource;
	using Filter = weka.filters.Filter;
	using Normalize = weka.filters.unsupervised.attribute.Normalize;

	/// <summary>
	/// Example usage of Neuroph Weka adapters
	/// @author Zoran Sevarac
	/// </summary>
	public class WekaNeurophSample
	{

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws Exception
		public static void Main(string[] args)
		{

			// create weka dataset from file
			DataSource dataSource = new DataSource("datasets/iris.arff");
			Instances wekaDataset = dataSource.DataSet;
			wekaDataset.setClassIndex(4);

			// normalize dataset
			Normalize filter = new Normalize();
			filter.InputFormat = wekaDataset;
			wekaDataset = Filter.useFilter(wekaDataset, filter);

			// convert weka dataset to neuroph dataset
			DataSet neurophDataset = WekaDataSetConverter.convertWekaToNeurophDataset(wekaDataset, 4, 3);

			// convert back neuroph dataset to weka dataset
			Instances testWekaDataset = WekaDataSetConverter.convertNeurophToWekaDataset(neurophDataset);

			// print out all to compare
			Console.WriteLine("Weka data set from file");
			printDataSet(wekaDataset);

			Console.WriteLine("Neuroph data set converted from Weka data set");
			printDataSet(neurophDataset);

			Console.WriteLine("Weka data set reconverted from Neuroph data set");
			printDataSet(testWekaDataset);

			Console.WriteLine("Testing WekaNeurophClassifier");
			testNeurophWekaClassifier(wekaDataset);
		}

		/// <summary>
		/// Prints Neuroph data set
		/// </summary>
		/// <param name="neurophDataset"> Dataset Neuroph data set </param>
		public static void printDataSet(DataSet neurophDataset)
		{
			Console.WriteLine("Neuroph dataset");
			IEnumerator iterator = neurophDataset.GetEnumerator();

			while (iterator.hasNext())
			{
				DataSetRow row = (DataSetRow) iterator.next();
				Console.WriteLine("inputs");
				Console.WriteLine(Arrays.ToString(row.Input));
				if (row.DesiredOutput.Length > 0)
				{
					Console.WriteLine("outputs");
					Console.WriteLine(Arrays.ToString(row.DesiredOutput));
					Console.WriteLine(row.Label);
				}
			}
		}

		/// <summary>
		/// Prints Weka data set
		/// </summary>
		/// <param name="wekaDataset"> Instances Weka data set </param>
		private static void printDataSet(Instances wekaDataset)
		{
			Console.WriteLine("Weka dataset");
			System.Collections.IEnumerator en = wekaDataset.enumerateInstances();
			while (en.hasMoreElements())
			{
				Instance instance = (Instance) en.nextElement();
				double[] values = instance.toDoubleArray();
				Console.WriteLine(Arrays.ToString(values));
				Console.WriteLine(instance.stringValue(instance.classIndex()));
			}
		}

		/// <summary>
		/// Test NeurophWekaClassifier
		/// </summary>
		/// <param name="wekaDataset"> Instances Weka data set </param>
		private static void testNeurophWekaClassifier(Instances wekaDataset)
		{
			try
			{
				MultiLayerPerceptron neuralNet = new MultiLayerPerceptron(4, 16, 3);

				// set labels manualy
				neuralNet.OutputNeurons[0].Label = "Setosa";
				neuralNet.OutputNeurons[1].Label = "Versicolor";
				neuralNet.OutputNeurons[2].Label = "Virginica";

				// initialize NeurophWekaClassifier
				WekaNeurophClassifier neurophWekaClassifier = new WekaNeurophClassifier(neuralNet);
				// set class index on data set
				wekaDataset.setClassIndex(4);

				// process data set
				neurophWekaClassifier.buildClassifier(wekaDataset);

				// test item
				//double[] item = {5.1, 3.5, 1.4, 0.2, 0.0}; // normalized item is below
				double[] item = new double[] {0.22222222222222213, 0.6249999999999999, 0.06779661016949151, 0.04166666666666667, 0};

				// create weka instance for test item
				Instance instance = new DenseInstance(1, item);

				// test classification
				Console.WriteLine("NeurophWekaClassifier - classifyInstance for {5.1, 3.5, 1.4, 0.2}");
				Console.WriteLine("Class idx: " + neurophWekaClassifier.classifyInstance(instance));
				Console.WriteLine("NeurophWekaClassifier - distributionForInstance for {5.1, 3.5, 1.4, 0.2}");
				double[] dist = neurophWekaClassifier.distributionForInstance(instance);
				for (int i = 0; i < dist.Length; i++)
				{
					Console.WriteLine("Class " + i + ": " + dist[i]);
				}

			}
			catch (Exception ex)
			{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
				Logger.getLogger(typeof(WekaNeurophSample).FullName).log(Level.SEVERE, null, ex);
			}

		}
	}

}