using System.Collections.Generic;

namespace org.neuroph.adapters.weka
{

	using org.neuroph.core;
	using Neuron = org.neuroph.core.Neuron;
	using DataSet = org.neuroph.core.data.DataSet;
	using AbstractClassifier = weka.classifiers.AbstractClassifier;
	using Instance = weka.core.Instance;
	using Instances = weka.core.Instances;

	/// <summary>
	/// Weka classifier wrapper for Neuroph neural networks
	/// Classifier based on Neuroph which can be used inside weka
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class WekaNeurophClassifier : AbstractClassifier
	{

		/// <summary>
		/// NeuralNetwork
		/// </summary>
		private NeuralNetwork neuralNet;

		/// <summary>
		/// Creates instance of NeurophWekaClassifier using specified neural network </summary>
		/// <param name="neuralNet"> NeuralNetwork </param>
		public WekaNeurophClassifier(NeuralNetwork neuralNet)
		{
			this.neuralNet = neuralNet;
		}

		/// <summary>
		/// Builds classifier using specified data set
		/// (trains neural network using that data set)
		/// </summary>
		/// <param name="data"> Instance weka data set </param>
		/// <exception cref="Exception"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public void buildClassifier(weka.core.Instances data) throws Exception
		public override void buildClassifier(Instances data)
		{
			// convert weka dataset to neuroph dataset
			DataSet dataSet = WekaDataSetConverter.convertWekaToNeurophDataset(data, neuralNet.InputsCount, neuralNet.OutputsCount);
			// train neural network
			neuralNet.learn(dataSet);
		}

		/// <summary>
		/// Classifies instance as one of possible classes </summary>
		/// <param name="instance"> Instance to classify </param>
		/// <returns> double classes double value </returns>
		/// <exception cref="Exception"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public double classifyInstance(weka.core.Instance instance) throws Exception
		public override double classifyInstance(Instance instance)
		{
			double[] item = convertInstanceToDoubleArray(instance);

			// set neural network input
			neuralNet.Input = item;
			// calculate neural network output
			neuralNet.calculate();

			// find neuron with highest output
			List<Neuron> outputNeurons = neuralNet.OutputNeurons;
			Neuron maxNeuron = null;
			int maxIdx = 0;
			double maxOut = double.NegativeInfinity;
			for (int i = 0; i < outputNeurons.Count; i++)
			{
				if (outputNeurons[i].Output > maxOut)
				{
					maxOut = outputNeurons[i].Output;
					maxIdx = i;
				}
			}

			// and return its idx (class)
			return maxIdx;
		}

		/// <summary>
		/// Calculates predict values for every possible class that
		/// instance can be classified as that </summary>
		/// <param name="instance"> Instance to calculate values for </param>
		/// <returns> double[] array of predict values </returns>
		/// <exception cref="Exception"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public double[] distributionForInstance(weka.core.Instance instance) throws Exception
		public override double[] distributionForInstance(Instance instance)
		{
			// Convert instance to double array
			double[] item = convertInstanceToDoubleArray(instance);

			// set neural network input
			neuralNet.Input = item;
			// calculate neural network output
			neuralNet.calculate();

			return neuralNet.Output;
		}

		public virtual NeuralNetwork NeuralNetwork
		{
			get
			{
				return neuralNet;
			}
		}


		private double[] convertInstanceToDoubleArray(Instance instance)
		{
			//initialize double array for values
			double[] item = new double[instance.numAttributes() - 1];

			//fill double array with values from instances
			for (int i = 0; i < instance.numAttributes() - 1; i++)
			{
				item[i] = instance.value(i);
			}

			return item;
		}
	}
}