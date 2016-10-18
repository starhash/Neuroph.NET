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

	using Properties = org.neuroph.util.Properties;

	/// <summary>
	/// <pre>
	/// Sigmoid neuron transfer function.
	/// 
	/// output = 1/(1+ e^(-slope*input))
	/// </pre>
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	[Serializable]
	public class Sigmoid : TransferFunction
	{
		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 2L;

		/// <summary>
		/// The slope parametetar of the sigmoid function
		/// </summary>
		private double slope = 1d;

		/// <summary>
		/// Creates an instance of Sigmoid neuron transfer function with default
		/// slope=1.
		/// </summary>
		public Sigmoid()
		{
		}

		/// <summary>
		/// Creates an instance of Sigmoid neuron transfer function with specified
		/// value for slope parametar. </summary>
		/// <param name="slope"> the slope parametar for the sigmoid function </param>
		public Sigmoid(double slope)
		{
			this.slope = slope;
		}

		/// <summary>
		/// Creates an instance of Sigmoid neuron transfer function with the
		/// specified properties. </summary>
		/// <param name="properties"> properties of the sigmoid function </param>
		public Sigmoid(Properties properties)
		{
			try
			{
				this.slope = (double)properties.getProperty("transferFunction.slope");
			}
			catch (System.NullReferenceException)
			{
				// if properties are not set just leave default values
			}
			catch (java.lang.NumberFormatException)
			{
				Console.Error.WriteLine("Invalid transfer function properties! Using default values.");
			}
		}

		/// <summary>
		/// Returns the slope parametar of this function </summary>
		/// <returns>  slope parametar of this function  </returns>
		public virtual double Slope
		{
			get
			{
				return this.slope;
			}
			set
			{
				this.slope = value;
			}
		}


		public override double getOutput(double net)
		{
					// conditional logic helps to avoid NaN
					if (net > 100)
					{
						return 1.0;
					}
					else if (net < -100)
					{
						return 0.0;
					}

			double den = 1d + Math.Exp(-this.slope * net);
					this.output = (1d / den);

			return this.output;
		}

		public override double getDerivative(double net) // remove net parameter? maybe we dont need it since we use cached output value
		{
					// +0.1 is fix for flat spot see http://www.heatonresearch.com/wiki/Flat_Spot
			double derivative = this.slope * this.output * (1d - this.output) + 0.1;
			return derivative;
		}

	}

}