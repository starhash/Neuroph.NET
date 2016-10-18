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
	/// Provides behaviour specific for neurons which act as input and the output
	/// neurons within the same layer. For example in Hopfield network and BAM.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class InputOutputNeuron : Neuron
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Flag which is set true if neuron external input is set
		/// </summary>
		private bool externalInputSet;

		/// <summary>
		/// Bias value for this neuron
		/// </summary>
		private double bias = 0;

		/// <summary>
		/// Creates an instance of neuron for Hopfield network
		/// </summary>
		public InputOutputNeuron() : base()
		{
		}

		/// <summary>
		/// Creates an instance of neuron for Hopfield network with specified input
		/// and transfer functions </summary>
		/// <param name="inFunc"> neuron input function </param>
		/// <param name="transFunc"> neuron transfer function </param>
		public InputOutputNeuron(InputFunction inFunc, TransferFunction transFunc) : base(inFunc, transFunc)
		{
		}

		/// <summary>
		/// Sets total net input for this cell
		/// </summary>
		/// <param name="input">
		///            input value </param>
		public override double Input
		{
			set
			{
				this.totalInput = value;
				this.externalInputSet = true;
			}
		}

		/// <summary>
		/// Returns bias value for this neuron </summary>
		/// <returns> bias value for this neuron </returns>
		public virtual double Bias
		{
			get
			{
				return bias;
			}
			set
			{
				this.bias = value;
			}
		}


		/// <summary>
		/// Calculates neuron output
		/// </summary>
		public override void calculate()
		{

			if (!externalInputSet) // ako ulaz nije setovan spolja
			{
				if (this.hasInputConnections()) // bias neuroni ne racunaju ulaz iz mreze jer
				{
										// nemaju ulaze
					totalInput = inputFunction.getOutput(this.inputConnections);
				}
			}

			// calculqate cell output
			this.output = transferFunction.getOutput(this.totalInput + bias); // izracunaj
																			// izlaz

			if (externalInputSet) // ulaz setovan 'spolja' vazi samo za jedno izracunavanje
			{
				externalInputSet = false;
				totalInput = 0;
			}
		}

	}

}