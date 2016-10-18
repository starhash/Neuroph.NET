using System;

namespace org.neuroph.samples
{

	using Connection = org.neuroph.core.Connection;
	using Layer = org.neuroph.core.Layer;
	using org.neuroph.core;
	using Neuron = org.neuroph.core.Neuron;
	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;
	using DistortRandomizer = org.neuroph.util.random.DistortRandomizer;
	using NguyenWidrowRandomizer = org.neuroph.util.random.NguyenWidrowRandomizer;

	/// <summary>
	/// This sample shows how to use various weight randomization techniques in Neuroph.
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class RandomizationSample
	{

		/// <summary>
		/// Runs this sample
		/// </summary>
		public static void Main(string[] args)
		{

			MultiLayerPerceptron neuralNet = new MultiLayerPerceptron(2, 3, 1);
			// neuralNet.randomizeWeights(new WeightsRandomizer());
			// neuralNet.randomizeWeights(new RangeRandomizer(0.1, 0.9));
			// neuralNet.randomizeWeights(new GaussianRandomizer(0.4, 0.3));
			neuralNet.randomizeWeights(new NguyenWidrowRandomizer(0.3, 0.7));
			printWeights(neuralNet);

			neuralNet.randomizeWeights(new DistortRandomizer(0.5));
			printWeights(neuralNet);
		}

		public static void printWeights<T1>(NeuralNetwork<T1> neuralNet)
		{
			foreach (Layer layer in neuralNet.Layers)
			{
				foreach (Neuron neuron in layer.Neurons)
				{
					foreach (Connection connection in neuron.InputConnections)
					{
						Console.Write(connection.Weight.value + " ");
					}
					Console.WriteLine();
				}
			}
		}
	}

}