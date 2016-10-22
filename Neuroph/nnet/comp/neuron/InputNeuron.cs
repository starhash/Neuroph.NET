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

namespace org.neuroph.nnet.comp.neuron
{

    using Neuron = org.neuroph.core.Neuron;
    using WeightedSum = org.neuroph.core.input.WeightedSum;
    using Linear = org.neuroph.core.transfer.Linear;
    using core.input;
    using core.transfer;

    /// <summary>
    /// Provides input neuron behaviour - neuron with input extranaly set, which just
    /// transfer that input to output without change. Its purporse is to distribute its input
    /// to all neurons it is connected to. It has no input connections
    /// @author Zoran Sevarac <sevarac@gmail.com>
    /// </summary>
    [System.Serializable]
    public class InputNeuron : Neuron
	{

		private const long serialVersionUID = 1L;

		/// <summary>
		/// Creates a new instance of InputNeuron with linear transfer function
		/// </summary>
		public InputNeuron() : base(new WeightedSum(), new Linear())
		{
		}

        public InputNeuron(InputFunction input, TransferFunction transfer) : base(input, transfer) { }

		/// <summary>
		/// Calculate method of this type of neuron just transfers its externaly set 
		/// input (with setNetInput) to its output
		/// </summary>
		public override void calculate()
		{
			this.output = this.totalInput;
		}
	}

}