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

namespace org.neuroph.nnet.learning
{


	using Connection = org.neuroph.core.Connection;
	using DataSet = org.neuroph.core.data.DataSet;
	using UnsupervisedLearning = org.neuroph.core.learning.UnsupervisedLearning;
	using CompetitiveLayer = org.neuroph.nnet.comp.layer.CompetitiveLayer;
	using CompetitiveNeuron = org.neuroph.nnet.comp.neuron.CompetitiveNeuron;


	/// <summary>
	/// Competitive learning rule.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class CompetitiveLearning : UnsupervisedLearning
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Creates new instance of CompetitiveLearning
		/// </summary>
		public CompetitiveLearning() : base()
		{
		}


		/// <summary>
		/// This method does one learning epoch for the unsupervised learning rules.
		/// It iterates through the training set and trains network weights for each
		/// element. Stops learning after one epoch.
		/// </summary>
		/// <param name="trainingSet">
		///            training set for training network </param>
		public override void doLearningEpoch(DataSet trainingSet)
		{
			base.doLearningEpoch(trainingSet);
			stopLearning(); // stop learning ahter one learning epoch - because we dont have any stopping criteria  for unsupervised...
		}

		/// <summary>
		/// Adjusts weights for the winning neuron
		/// </summary>
		protected internal override void updateNetworkWeights()
		{
			// find active neuron in output layer
			// TODO : change idx, in general case not 1
			CompetitiveNeuron winningNeuron = ((CompetitiveLayer) neuralNetwork.getLayerAt(1)).Winner;

			List<Connection> inputConnections = winningNeuron.ConnectionsFromOtherLayers;

			foreach (Connection connection in inputConnections)
			{
				double weight = connection.Weight.Value;
				double input = connection.Input;
				double deltaWeight = this.learningRate * (input - weight);
				connection.Weight.inc(deltaWeight);
			}
		}

	}

}