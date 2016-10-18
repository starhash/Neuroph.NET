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

namespace org.neuroph.nnet.comp
{

	using DelayedNeuron = org.neuroph.nnet.comp.neuron.DelayedNeuron;
	using Connection = org.neuroph.core.Connection;
	using Neuron = org.neuroph.core.Neuron;

	/// <summary>
	/// Represents the connection between neurons which can delay signal.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class DelayedConnection : Connection
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Delay factor for this conection
		/// </summary>
		private int delay = 0;

		/// <summary>
		/// Creates an instance of delayed connection to cpecified neuron and
		/// with specified weight </summary>
		/// <param name="fromNeuron"> neuron to connect (source neuron) </param>
		/// <param name="toNeuron"> neuron to connect to (destination neuron) </param>
		/// <param name="weightVal"> weight value for the connection </param>
		/// <param name="delay"> delay for the connection </param>
		public DelayedConnection(Neuron fromNeuron, Neuron toNeuron, double weightVal, int delay) : base(fromNeuron, toNeuron, weightVal)
		{
			this.delay = delay;
		}

		/// <summary>
		/// Returns delay value for this connection </summary>
		/// <returns> delay value for this connection </returns>
		public virtual int Delay
		{
			get
			{
				return this.delay;
			}
			set
			{
				this.delay = value;
			}
		}


		/// <summary>
		/// Gets delayed input through this connection </summary>
		/// <returns> delayed output from connected neuron </returns>
		public override double Input
		{
			get
			{
				if (this.fromNeuron is DelayedNeuron)
				{
					return ((DelayedNeuron) this.fromNeuron).getOutput(delay);
				}
				else
				{
					return this.fromNeuron.Output;
				}
			}
		}

	}

}