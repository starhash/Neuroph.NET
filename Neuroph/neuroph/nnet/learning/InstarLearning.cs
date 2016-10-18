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

	/// <summary>
	/// Hebbian-like learning rule for Instar network.
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class InstarLearning : UnsupervisedHebbianLearning
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Creates new instance of InstarLearning algorithm  
		/// </summary>
		public InstarLearning() : base()
		{
			this.LearningRate = 0.1;
		}


		/// <summary>
		/// This method implements weights update procedure for the single neuron
		/// </summary>
		/// <param name="neuron">
		///            neuron to update weights for </param>
		protected internal override void updateNeuronWeights(Neuron neuron)
		{
			double output = neuron.Output;
			foreach (Connection connection in neuron.InputConnections)
			{
				double input = connection.Input;
				double weight = connection.Weight.Value;
				double deltaWeight = this.learningRate * output * (input - weight);
				connection.Weight.inc(deltaWeight);
			}
		}

	}

}