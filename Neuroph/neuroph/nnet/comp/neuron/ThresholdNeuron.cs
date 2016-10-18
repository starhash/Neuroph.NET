using System;

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
	using InputFunction = org.neuroph.core.input.InputFunction;
	using TransferFunction = org.neuroph.core.transfer.TransferFunction;

	/// <summary>
	/// Provides behaviour for neurons with threshold.
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class ThresholdNeuron : Neuron
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Threshold value for this neuron
		/// </summary>
		protected internal double thresh = 0;

		/// <summary>
		/// Creates a neuron with threshold behaviour, and with the specified input
		/// and transfer functions.
		/// </summary>
		/// <param name="inputFunction">
		///            input function for this neuron </param>
		/// <param name="transferFunction">
		///            transfer function for this neuron </param>
		public ThresholdNeuron(InputFunction inputFunction, TransferFunction transferFunction)
		{
			this.inputFunction = inputFunction;
			this.transferFunction = transferFunction;
					this.thresh = new Random(1).NextDouble();
		}

		/// <summary>
		/// Calculates neuron's output
		/// </summary>
		public override void calculate()
		{
			this.totalInput = this.inputFunction.getOutput(this.inputConnections);
					this.output = this.transferFunction.getOutput(this.totalInput - this.thresh);
		}

		/// <summary>
		/// Returns threshold value for this neuron </summary>
		/// <returns> threshold value for this neuron </returns>
		public virtual double Thresh
		{
			get
			{
				return thresh;
			}
			set
			{
				this.thresh = value;
			}
		}


	}

}