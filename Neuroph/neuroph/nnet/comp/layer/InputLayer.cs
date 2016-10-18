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

namespace org.neuroph.nnet.comp.layer
{

	using InputNeuron = org.neuroph.nnet.comp.neuron.InputNeuron;
	using Layer = org.neuroph.core.Layer;
	using Neuron = org.neuroph.core.Neuron;
	using Linear = org.neuroph.core.transfer.Linear;
	using NeuronFactory = org.neuroph.util.NeuronFactory;
	using NeuronProperties = org.neuroph.util.NeuronProperties;

	/// <summary>
	/// Represents a layer of input neurons - a typical neural network input layer 
	/// @author Zoran Sevarac <sevarac@gmail.com> </summary>
	/// <seealso cref= InputNeuron </seealso>
	public class InputLayer : Layer
	{

		/// <summary>
		/// Creates a new instance of InputLayer with specified number of input neurons </summary>
		/// <param name="neuronsCount"> input neurons count for this layer </param>
		public InputLayer(int neuronsCount)
		{
			NeuronProperties inputNeuronProperties = new NeuronProperties(typeof(InputNeuron), typeof(Linear));

			for (int i = 0; i < neuronsCount; i++)
			{
				Neuron neuron = NeuronFactory.createNeuron(inputNeuronProperties);
				this.addNeuron(neuron);
			}
		}
	}

}