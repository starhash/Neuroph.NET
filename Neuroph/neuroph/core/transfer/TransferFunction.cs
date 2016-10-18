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

namespace org.neuroph.core.transfer
{

	/// <summary>
	/// Abstract base class for all neuron tranfer functions.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com> </summary>
	/// <seealso cref= org.neuroph.core.Neuron </seealso>
	[Serializable]
	public abstract class TransferFunction
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

			/// <summary>
			/// Output result
			/// </summary>
			protected internal double output; // cached output value to avoid double calculation for derivative

		/// <summary>
		/// Returns the ouput of this function.
		/// </summary>
		/// <param name="totalInput">
		///            total input  </param>
		public abstract double getOutput(double totalInput);

		/// <summary>
		/// Returns the first derivative of this function.
		/// Note: should this method should be abstract? </summary>
		/// <param name="totalInput">
		///            total  input </param>
		public virtual double getDerivative(double totalInput)
		{
			return 1d;
		}

	}

}