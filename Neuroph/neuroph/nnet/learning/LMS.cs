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
namespace org.neuroph.nnet.learning
{

	using Connection = org.neuroph.core.Connection;
	using Neuron = org.neuroph.core.Neuron;
	using Weight = org.neuroph.core.Weight;
	using SupervisedLearning = org.neuroph.core.learning.SupervisedLearning;

	/// <summary>
	/// LMS learning rule for neural networks.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	[Serializable]
	public class LMS : SupervisedLearning
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 2L;


		/// <summary>
		/// Creates a new LMS learning rule
		/// This learning rule is used to train Adaline neural network, 
		/// and this class is base for all LMS based learning rules like 
		/// PerceptronLearning, DeltaRule, SigmoidDeltaRule, Backpropagation etc.
		/// </summary>
		public LMS()
		{

		}


		/// <summary>
		/// This method implements the weights update procedure for the whole network
		/// for the given output error vector.
		/// </summary>
		/// <param name="outputError">
		///            output error vector for some network input- the difference between desired and actual output </param>
		/// <seealso cref= SupervisedLearning#learnPattern(org.neuroph.core.data.DataSetRow)  learnPattern </seealso>
		protected internal override void updateNetworkWeights(double[] outputError)
		{
			int i = 0;
			// for each neuron in output layer
			List<Neuron> outputNeurons = neuralNetwork.OutputNeurons;
			foreach (Neuron neuron in outputNeurons)
			{
				neuron.Error = outputError[i]; // set the neuron error, as difference between desired and actual output
				this.updateNeuronWeights(neuron); // and update neuron weights
				i++;
			}
		}

		/// <summary>
		/// This method implements weights update procedure for the single neuron
		/// It iterates through all neuron's input connections, and calculates/set weight change for each weight
		/// using formula 
		///      deltaWeight = learningRate * neuronError * input
		/// 
		/// where neuronError is difference between desired and actual output for specific neuron
		///      neuronError = desiredOutput[i] - actualOutput[i] (see method SuprevisedLearning.calculateOutputError)
		/// </summary>
		/// <param name="neuron">
		///            neuron to update weights
		/// </param>
		/// <seealso cref= LMS#updateNetworkWeights(double[])  </seealso>
		public virtual void updateNeuronWeights(Neuron neuron)
		{
			// get the error(delta) for specified neuron,
			double neuronError = neuron.Error;

			// tanh can be used to minimise the impact of big error values, which can cause network instability
			// suggested at https://sourceforge.net/tracker/?func=detail&atid=1107579&aid=3130561&group_id=238532
			// double neuronError = Math.tanh(neuron.getError());

			// iterate through all neuron's input connections
			foreach (Connection connection in neuron.InputConnections)
			{
				// get the input from current connection
				double input = connection.Input;
				// calculate the weight change
				double weightChange = this.learningRate * neuronError * input;

				// get the connection weight
				Weight weight = connection.Weight;
				// if the learning is in online mode (not batch) apply the weight change immediately
				if (!this.InBatchMode)
				{
					weight.weightChange = weightChange;
					weight.value += weightChange;
				} // otherwise its in batch mode, so sum the weight changes and apply them later, after the current epoch (see SupervisedLearning.doLearningEpoch method)
				else
				{
					weight.weightChange += weightChange;
				}
			}
		}

	}
}