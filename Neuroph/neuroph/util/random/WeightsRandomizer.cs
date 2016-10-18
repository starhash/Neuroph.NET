using System;

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
	using LearningRule = org.neuroph.core.learning.LearningRule;
	using FeatureMapsLayer = org.neuroph.nnet.comp.layer.FeatureMapsLayer;
	using FeatureMapLayer = org.neuroph.nnet.comp.layer.FeatureMapLayer;

	/// <summary>
	/// Basic weights randomizer, iterates and randomizes all connection weights in network.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class WeightsRandomizer
	{

		/// <summary>
		/// Random number genarator used by randomizers
		/// </summary>
		protected internal Random randomGenerator;

		/// <summary>
		/// Create a new instance of WeightsRandomizer
		/// </summary>
		public WeightsRandomizer()
		{
			this.randomGenerator = new Random();
		}

		/// <summary>
		/// Create a new instance of WeightsRandomizer with specified random generator
		/// If you use the same random generators, you'll get the same random sequences
		/// </summary>
		/// <param name="randomGenerator"> random geneartor to use for randomizing weights </param>
		public WeightsRandomizer(Random randomGenerator)
		{
			this.randomGenerator = randomGenerator;
		}

		/// <summary>
		/// Gets random generator used to generate random values
		/// </summary>
		/// <returns> random generator used to generate random values </returns>
		public virtual Random RandomGenerator
		{
			get
			{
				return randomGenerator;
			}
			set
			{
				this.randomGenerator = value;
			}
		}



		/// <summary>
		/// Iterates and randomizes all layers in specified network
		/// </summary>
		/// <param name="neuralNetwork"> neural network to randomize </param>
		public virtual void randomize(NeuralNetwork neuralNetwork)
		{
		//    List<Layer> layers = neuralNetwork.getLayers();
			foreach (Layer layer in neuralNetwork.Layers)
			{
					this.randomize(layer);
			}
		}

		/// <summary>
		/// Iterate and randomizes all neurons in specified layer
		/// </summary>
		/// <param name="layer"> layer to randomize </param>
		public virtual void randomize(Layer layer)
		{
			foreach (Neuron neuron in layer.Neurons)
			{
				randomize(neuron);
			}
		}

		/// <summary>
		/// Iterates and randomizes all connection weights in specified neuron
		/// </summary>
		/// <param name="neuron"> neuron to randomize </param>
		public virtual void randomize(Neuron neuron)
		{
			int numberOfInputConnections = neuron.InputConnections.Count;
			double coefficient = 1d / Math.Sqrt(numberOfInputConnections);
			coefficient = coefficient == 0 ? 1 : coefficient;
			foreach (Connection connection in neuron.InputConnections)
			{
	//            connection.getWeight().setValue(coefficient * nextRandomWeight());
				connection.Weight.Value = nextRandomWeight();

			}
		}

		/// <summary>
		/// Returns next random value from random generator, that will be used to initialize weight
		/// </summary>
		/// <returns> next random value fro random generator </returns>
		protected internal virtual double nextRandomWeight()
		{
			return randomGenerator.NextDouble();
		}
	}

}