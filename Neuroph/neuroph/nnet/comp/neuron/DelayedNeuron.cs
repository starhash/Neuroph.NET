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

namespace org.neuroph.nnet.comp.neuron
{

	using Neuron = org.neuroph.core.Neuron;
	using InputFunction = org.neuroph.core.input.InputFunction;
	using TransferFunction = org.neuroph.core.transfer.TransferFunction;

	/// <summary>
	/// Provides behaviour for neurons with delayed output.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class DelayedNeuron : Neuron
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Output history for this neuron
		/// </summary>
		[NonSerialized]
		protected internal List<double> outputHistory;

		/// <summary>
		/// Creates an instance of neuron which can delay output </summary>
		/// <param name="inputFunction"> neuron input function </param>
		/// <param name="transferFunction"> neuron transfer function </param>
		public DelayedNeuron(InputFunction inputFunction, TransferFunction transferFunction) : base(inputFunction, transferFunction)
		{
			outputHistory = new List<double>(5); // default delay buffer size is 5
			outputHistory.Add(0.0d);
		}

		public override void calculate()
		{
			base.calculate();
			outputHistory.Insert(0, this.output);
			if (outputHistory.Count > 5)
			{
				outputHistory.RemoveAt(5);
			}
		}

		/// <summary>
		/// Returns neuron output with the specified delay </summary>
		/// <param name="delay"> output delay </param>
		/// <returns> neuron output at (t-delay) moment </returns>
		public virtual double getOutput(int delay)
		{
			return (double)outputHistory[delay];
		}


//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private void readObject(java.io.ObjectInputStream in) throws java.io.IOException, ClassNotFoundException
			private void readObject(java.io.ObjectInputStream @in)
			{
				@in.defaultReadObject();
				outputHistory = new List<double>(5);
			}

	}

}