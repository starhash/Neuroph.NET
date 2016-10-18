using System.Collections;
using System.Collections.Generic;

namespace org.neuroph.adapters.jml
{

	using Classifier = net.sf.javaml.classification.Classifier;
	using Dataset = net.sf.javaml.core.Dataset;
	using Instance = net.sf.javaml.core.Instance;
	using org.neuroph.core;
	using Neuron = org.neuroph.core.Neuron;
	using DataSet = org.neuroph.core.data.DataSet;

	/// 
	/// <summary>
	/// @author zoran
	/// </summary>
	public class JMLNeurophClassifier : Classifier
	{

		/// <summary>
		/// NeuralNetwork
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
//ORIGINAL LINE: org.neuroph.core.NeuralNetwork<?> neuralNet;
		internal NeuralNetwork<?> neuralNet;

		/// <summary>
		/// Creates instance of NeurophJMLClassifier </summary>
		/// <param name="neuralNet"> NeuralNetwork </param>
		/// <param name="dataSet"> DataSet </param>
		public JMLNeurophClassifier(NeuralNetwork neuralNet)
		{
			this.neuralNet = neuralNet;
		}

		/// <summary>
		/// Neural network learns from Java-ML data set </summary>
		/// <param name="dataSetJML"> Dataset Java-ML data set </param>
		public override void buildClassifier(Dataset dataSetJML)
		{
			DataSet dataSet = JMLDataSetConverter.convertJMLToNeurophDataset(dataSetJML, neuralNet.InputsCount, neuralNet.OutputsCount);
			neuralNet.learn(dataSet);
		}

		/// <summary>
		/// Classifies instance as one of possible classes </summary>
		/// <param name="instnc"> Instance to classify </param>
		/// <returns> Object class as Object </returns>
		public override object classify(Instance instnc)
		{

			double[] item = convertInstanceToDoubleArray(instnc);

			// set neural network input
			neuralNet.Input = item;
			// calculate neural network output
			neuralNet.calculate();

			// find neuron with highest output
			Neuron maxNeuron = null;
			double maxOut = double.NegativeInfinity;
			foreach (Neuron neuron in neuralNet.OutputNeurons)
			{
				if (neuron.Output > maxOut)
				{
					maxNeuron = neuron;
					maxOut = neuron.Output;
				}
			}

			// and return its label
			return maxNeuron.Label;
		}

		/// <summary>
		/// Calculates predict values for every possible class that
		/// instance can be classified as that </summary>
		/// <param name="instnc"> Instance </param>
		/// <returns> Map<Object, Double> </returns>
		public override IDictionary<object, double?> classDistribution(Instance instnc)
		{

			// Convert instance to double array
			double[] item = convertInstanceToDoubleArray(instnc);

			// set neural network input
			neuralNet.Input = item;
			// calculate neural network output
			neuralNet.calculate();

			// find neuron with highest output
			IDictionary<object, double?> possibilities = new Dictionary<object, double?>();

			foreach (Neuron neuron in neuralNet.OutputNeurons)
			{
				possibilities[neuron.Label] = neuron.Output;
			}

			return possibilities;
		}

		/// <summary>
		/// Convert instance attribute values to double array values </summary>
		/// <param name="instnc"> Instance to convert </param>
		/// <returns> double[] </returns>
		private double[] convertInstanceToDoubleArray(Instance instnc)
		{
			IEnumerator attributeIterator = instnc.GetEnumerator();

			double[] item = new double[instnc.noAttributes()];
			int index = 0;

			while (attributeIterator.hasNext())
			{
				double? attrValue = (double?) attributeIterator.next();
				item[index] = (double)attrValue;
				index++;
			}

			return item;
		}
	}

}