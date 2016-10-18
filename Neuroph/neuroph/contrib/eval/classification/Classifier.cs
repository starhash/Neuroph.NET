/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
namespace org.neuroph.contrib.eval.classification
{

	using org.neuroph.core;
	using Neuron = org.neuroph.core.Neuron;
	using PluginBase = org.neuroph.util.plugins.PluginBase;

	/// <summary>
	/// Classifier plugin for neurla networks
	/// @author zoran
	/// </summary>
	public class Classifier : PluginBase
	{

		internal double threshold = 0.5;

		public virtual string classify(double[] pattern)
		{
//JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
//ORIGINAL LINE: org.neuroph.core.NeuralNetwork<?> nnet = getParentNetwork();
			NeuralNetwork nnet = ParentNetwork;
			nnet.Input = pattern;
			nnet.calculate();

			Neuron maxNeuron = null;
			double maxOutput = double.Epsilon;

			foreach (Neuron neuron in nnet.OutputNeurons)
			{
				if (neuron.Output > maxOutput)
				{
					maxOutput = neuron.Output;
					maxNeuron = neuron;
				}
			}

			if (maxOutput > threshold)
			{
					return maxNeuron.Label;
			}
			else
			{
				return null;
			}
		}

	}

}