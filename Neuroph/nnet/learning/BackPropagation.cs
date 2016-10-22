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
	using Layer = org.neuroph.core.Layer;
	using Neuron = org.neuroph.core.Neuron;
	using TransferFunction = org.neuroph.core.transfer.TransferFunction;

    /// <summary>
    /// Back Propagation learning rule for Multi Layer Perceptron neural networks.
    /// 
    /// @author Zoran Sevarac <sevarac@gmail.com>
    /// </summary>
    [System.Serializable]
    public class BackPropagation : LMS
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Creates new instance of BackPropagation learning
		/// </summary>
		public BackPropagation() : base()
		{
		}


		/// <summary>
		/// This method implements weight update procedure for the whole network
		/// for the specified  output error vector
		/// </summary>
		/// <param name="outputError"> output error vector </param>
		protected internal override void updateNetworkWeights(double[] outputError)
		{
			this.calculateErrorAndUpdateOutputNeurons(outputError);
			this.calculateErrorAndUpdateHiddenNeurons();
		}


		/// <summary>
		/// This method implements weights update procedure for the output neurons
		/// Calculates delta/error and calls updateNeuronWeights to update neuron's weights
		/// for each output neuron
		/// </summary>
		/// <param name="outputError"> error vector for output neurons </param>
		protected internal virtual void calculateErrorAndUpdateOutputNeurons(double[] outputError)
		{
			int i = 0;

			// for all output neurons
			List<Neuron> outputNeurons = neuralNetwork.OutputNeurons;
			foreach (Neuron neuron in outputNeurons)
			{
				// if error is zero, just se;t zero error and continue to next neuron
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

		/// <summary>
		/// This method implements weights adjustment for the hidden layers
		/// </summary>
		protected internal virtual void calculateErrorAndUpdateHiddenNeurons()
		{
			List<Layer> layers = neuralNetwork.Layers;
			for (int layerIdx = layers.Count - 2; layerIdx > 0; layerIdx--)
			{
				foreach (Neuron neuron in layers[layerIdx].Neurons)
				{
					// calculate the neuron's error (delta)
					double neuronError = this.calculateHiddenNeuronError(neuron);
					neuron.Error = neuronError;
					this.updateNeuronWeights(neuron);
				} // for
			} // for
		}

		/// <summary>
		/// Calculates and returns the neuron's error (neuron's delta) for the given neuron param
		/// </summary>
		/// <param name="neuron"> neuron to calculate error for </param>
		/// <returns> neuron error (delta) for the specified neuron </returns>
		protected internal virtual double calculateHiddenNeuronError(Neuron neuron)
		{
			double deltaSum = 0d;
			foreach (Connection connection in neuron.OutConnections)
			{
				double delta = connection.ToNeuron.Error * connection.Weight.value;
				deltaSum += delta; // weighted delta sum from the next layer
			} // for

			TransferFunction transferFunction = neuron.TransferFunction;
			double netInput = neuron.NetInput; // should we use input of this or other neuron?
			double f1 = transferFunction.getDerivative(netInput);
			double neuronError = f1 * deltaSum;
			return neuronError;
		}

	}

}