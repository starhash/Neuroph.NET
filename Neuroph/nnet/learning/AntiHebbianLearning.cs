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
	/// A variant of Hebbian learning called Anti-Hebbian learning.
	/// The only difference is that it subbstracts weight change (Hebbian learning adds)
	/// @author Zoran Sevarac <sevarac@gmail.com> 
	/// </summary>
	public class AntiHebbianLearning : UnsupervisedHebbianLearning
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
				double deltaWeight = input * output * this.learningRate;
				connection.Weight.dec(deltaWeight); // the only difference to UnsupervisedHebbianLearning is this substraction instead addition
			}
			 }

	}

}