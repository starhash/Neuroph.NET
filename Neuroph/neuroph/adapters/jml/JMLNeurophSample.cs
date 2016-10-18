using System;
using System.Collections;
using System.Collections.Generic;

namespace org.neuroph.adapters.jml
{

	using Dataset = net.sf.javaml.core.Dataset;
	using DenseInstance = net.sf.javaml.core.DenseInstance;
	using Instance = net.sf.javaml.core.Instance;
	using NormalizeMidrange = net.sf.javaml.filter.normalize.NormalizeMidrange;
	using FileHandler = net.sf.javaml.tools.data.FileHandler;
	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;
	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;

	/// <summary>
	/// Example how to use adapters for Java ML http://java-ml.sourceforge.net/
	/// 
	/// @author Zoran Sevarac
	/// </summary>
	public class JMLNeurophSample
	{

		public static void Main(string[] args)
		{
			try
			{
				//create jml dataset
				Dataset jmlDataset = FileHandler.loadDataset(new File("datasets/iris.data"), 4, ",");

				// normalize dataset
				NormalizeMidrange nmr = new NormalizeMidrange(0,1);
				nmr.build(jmlDataset);
				nmr.filter(jmlDataset);

				//print data as read from file
				Console.WriteLine(jmlDataset);

				//convert jml dataset to neuroph
				DataSet neurophDataset = JMLDataSetConverter.convertJMLToNeurophDataset(jmlDataset, 4, 3);

				//convert neuroph dataset to jml
				Dataset jml = JMLDataSetConverter.convertNeurophToJMLDataset(neurophDataset);

				//print out both to compare them
				Console.WriteLine("Java-ML data set read from file");
				printDataset(jmlDataset);
				Console.WriteLine("Neuroph data set converted from Java-ML data set");
				printDataset(neurophDataset);
				Console.WriteLine("Java-ML data set reconverted from Neuroph data set");
				printDataset(jml);

				Console.WriteLine("JMLNeuroph classifier test");
				//test NeurophJMLClassifier
				testJMLNeurophClassifier(jmlDataset);

			}
			catch (Exception ex)
			{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
				Logger.getLogger(typeof(JMLNeurophSample).FullName).log(Level.SEVERE, null, ex);
			}

		}

		/// <summary>
		/// Prints Java-ML data set
		/// </summary>
		/// <param name="jmlDataset"> Dataset Java-ML data set </param>
		public static void printDataset(Dataset jmlDataset)
		{
			Console.WriteLine("JML dataset");
			IEnumerator iterator = jmlDataset.GetEnumerator();

			while (iterator.hasNext())
			{
				Instance instance = (Instance) iterator.next();
				Console.WriteLine("inputs");
				Console.WriteLine(instance.values());
				Console.WriteLine(instance.classValue());
			}
		}

		/// <summary>
		/// Prints Neuroph data set
		/// </summary>
		/// <param name="neurophDataset"> Dataset Neuroph data set </param>
		public static void printDataset(DataSet neurophDataset)
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
				}
			}
		}


		/// <summary>
		/// Converts Java-ML data set to Map
		/// </summary>
		/// <param name="jmlDataset"> Dataset Java-ML data set </param>
		/// <returns> Map converted from Java-ML data set </returns>
		private static IDictionary<double[], string> convertJMLDatasetToMap(Dataset jmlDataset)
		{

			//number of attributes without class attribute
			int numOfAttributes = jmlDataset.noAttributes();

			//initialize map
			IDictionary<double[], string> itemClassMap = new Dictionary<double[], string>();

			//iterate through jml dataset
			foreach (Instance dataRow in jmlDataset)
			{

				//initialize double array for values from dataset
				double[] values = new double[numOfAttributes];
				int ind = 0;

				//iterate through values in dataset instance an adding them in double array
				foreach (double? val in dataRow)
				{
					values[ind] = val;
					ind++;
				}

				//put attribute values and class value in map
				itemClassMap[values] = dataRow.classValue().ToString();
			}
			return itemClassMap;
		}

		/// <summary>
		/// Test JMLNeurophClassifier
		/// </summary>
		/// <param name="jmlDataset"> Dataset Java-ML data set </param>
		private static void testJMLNeurophClassifier(Dataset jmlDataset)
		{
			MultiLayerPerceptron neuralNet = new MultiLayerPerceptron(4, 16, 3);

			// set labels for output neurons
			neuralNet.OutputNeurons[0].Label = "Setosa";
			neuralNet.OutputNeurons[1].Label = "Versicolor";
			neuralNet.OutputNeurons[2].Label = "Virginica";

			// initialize NeurophJMLClassifier
			JMLNeurophClassifier jmlnClassifier = new JMLNeurophClassifier(neuralNet);

			// Process Java-ML data set
			jmlnClassifier.buildClassifier(jmlDataset);

			// test item
			//double[] item = {5.1, 3.5, 1.4, 0.2}; // normalized item is below
			double[] item = new double[] {-0.27777777777777773, 0.1249999999999999, -0.4322033898305085, -0.45833333333333337};

			// Java-ML instance out of test item
			Instance instance = new DenseInstance(item);

			// why are these not normalised?
			Console.WriteLine("NeurophJMLClassifier - classify of {0.22222222222222213, 0.6249999999999999, 0.06779661016949151, 0.04166666666666667}");
			Console.WriteLine(jmlnClassifier.classify(instance));
			Console.WriteLine("NeurophJMLClassifier - classDistribution of {0.22222222222222213, 0.6249999999999999, 0.06779661016949151, 0.04166666666666667}");
			Console.WriteLine(jmlnClassifier.classDistribution(instance));
		}
	}

}