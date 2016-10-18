using System;
using System.Collections.Generic;

/// <summary>
/// Copyright 2010 Neuroph Project http://neuroph.sourceforge.net
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
/// 
///    http://www.apache.org/licenses/LICENSE-2.0
/// 
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// </summary>

namespace org.neuroph.util.random
{

	using Connection = org.neuroph.core.Connection;
	using Layer = org.neuroph.core.Layer;
	using org.neuroph.core;
	using Neuron = org.neuroph.core.Neuron;

	/// <summary>
	/// This class provides NguyenWidrow randmization technique, which gives very good results
	/// for Multi Layer Perceptrons trained with back propagation family of learning rules.
	/// Based on NguyenWidrowRandomizer from Encog
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class NguyenWidrowRandomizer : RangeRandomizer
	{

		public NguyenWidrowRandomizer(double min, double max) : base(min, max)
		{
		}

		public override void randomize(NeuralNetwork neuralNetwork)
		{
			base.randomize(neuralNetwork);

			int inputNeuronsCount = neuralNetwork.InputNeurons.Count;
			int hiddenNeuronsCount = 0;

			for (int i = 1; i < neuralNetwork.LayersCount - 1; i++)
			{
				hiddenNeuronsCount += neuralNetwork.getLayerAt(i).NeuronsCount;
			}

			double beta = 0.7 * Math.Pow(hiddenNeuronsCount, 1.0 / inputNeuronsCount); // should we use the total number of hidden neurons or different norm for each layer

			List<Layer> layers = neuralNetwork.Layers;
			foreach (Layer layer in layers)
			{
				// Calculate the Euclidean Norm for the weights: norm += value * value - suma vadrata tezina u layeru
				double norm = 0.0;
				foreach (Neuron neuron in layer.Neurons)
				{
					foreach (Connection connection in neuron.InputConnections)
					{
						double weight = connection.Weight.Value;
						norm += weight * weight;
					}
				}
				norm = Math.Sqrt(norm);

				// Rescale the weights using beta and the norm: beta * value / norm            
				foreach (Neuron neuron in layer.Neurons)
				{
					foreach (Connection connection in neuron.InputConnections)
					{
						double weight = connection.Weight.Value;
						weight = beta * weight / norm;
						connection.Weight.Value = weight;
					}
				}
			}

		}
	}
}