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
	using Neuron = org.neuroph.core.Neuron;
	using DataSet = org.neuroph.core.data.DataSet;
	using UnsupervisedLearning = org.neuroph.core.learning.UnsupervisedLearning;

	/// <summary>
	/// Unsupervised hebbian learning rule.
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class UnsupervisedHebbianLearning : UnsupervisedLearning
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Creates new instance of UnsupervisedHebbianLearning algorithm
		/// </summary>
		public UnsupervisedHebbianLearning() : base()
		{
			this.LearningRate = 0.1d;
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
			stopLearning(); // stop learning ahter one learning epoch -- why ? - because we dont have any other stopping criteria for this - must limit the iterations
		}

		/// <summary>
		/// Adjusts weights for the output neurons
		/// </summary>
			protected internal override void updateNetworkWeights()
			{
			foreach (Neuron neuron in neuralNetwork.OutputNeurons)
			{
				this.updateNeuronWeights(neuron);
			}
			}

		/// <summary>
		/// This method implements weights update procedure for the single neuron
		/// </summary>
		/// <param name="neuron">
		///            neuron to update weights </param>
		protected internal virtual void updateNeuronWeights(Neuron neuron)
		{
			double output = neuron.Output;

			foreach (Connection connection in neuron.InputConnections)
			{
				double input = connection.Input;
				double deltaWeight = input * output * this.learningRate;
				connection.Weight.inc(deltaWeight);
			}
		}
	}

}