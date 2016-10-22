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
	/// A variant of Hebbian learning called Generalized Hebbian learning.
	/// Weight change is calculated using formula
	///      deltaWeight = (input - netInput) * output * learningRate
	/// @author Zoran Sevarac <sevarac@gmail.com> 
	/// </summary>
	public class GeneralizedHebbianLearning : UnsupervisedHebbianLearning
	{

		/// <summary>
		/// This method implements weights update procedure for the single neuron
		/// </summary>
		/// <param name="neuron">
		///            neuron to update weights </param>
		protected internal override void updateNeuronWeights(Neuron neuron)
		{
			double output = neuron.Output;
			foreach (Connection connection in neuron.InputConnections)
			{
				double input = connection.Input;
							double netInput = neuron.NetInput;
				double deltaWeight = (input - netInput) * output * this.learningRate; // is it right to use netInput here?
				connection.Weight.inc(deltaWeight);
			}
		}

	}

}