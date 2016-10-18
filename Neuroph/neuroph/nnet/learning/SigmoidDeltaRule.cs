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

	using Neuron = org.neuroph.core.Neuron;
	using TransferFunction = org.neuroph.core.transfer.TransferFunction;

	/// <summary>
	/// Delta rule learning algorithm for perceptrons with sigmoid (or any other diferentiable continuous) functions.
	/// 
	/// TODO: Rename to DeltaRuleContinuous (ContinuousDeltaRule) or something like that, but that will break backward compatibility,
	/// posibly with backpropagation which is the most used
	/// </summary>
	/// <seealso cref= LMS
	/// @author Zoran Sevarac <sevarac@gmail.com> </seealso>
	public class SigmoidDeltaRule : LMS
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Creates new SigmoidDeltaRule
		/// </summary>
		public SigmoidDeltaRule() : base()
		{
		}

		/// <summary>
		/// This method implements weight update procedure for the whole network for
		/// this learning rule
		/// </summary>
		/// <param name="outputError">
		///            output error vector </param>
		protected internal override void updateNetworkWeights(double[] outputError)
		{
			int i = 0;
					// for all output neurons
			foreach (Neuron neuron in neuralNetwork.OutputNeurons)
			{
							// if error is zero, just set zero error and continue to next neuron
				if (outputError[i] == 0)
				{
					neuron.Error = 0;
									i++;
					continue;
				}

							// otherwise calculate and set error/delta for the current neuron
				TransferFunction transferFunction = neuron.TransferFunction;
				double neuronInput = neuron.NetInput;
				double delta = outputError[i] * transferFunction.getDerivative(neuronInput); // delta = (d-y)*df(net)
				neuron.Error = delta;

							// and update weights of the current neuron
				this.updateNeuronWeights(neuron);
				i++;
			} // for
		}

	}

}