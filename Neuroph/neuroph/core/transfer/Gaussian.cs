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
	/// Gaussian neuron transfer function.
	///             -(x^2) / (2 * sigma^2)
	///  f(x) =    e
	/// </pre>
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	[Serializable]
	public class Gaussian : TransferFunction
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// The sigma parametetar of the gaussian function
		/// </summary>
		private double sigma = 0.5d;

		/// <summary>
		/// Creates an instance of Gaussian neuron transfer
		/// </summary>
		public Gaussian()
		{
		}

		/// <summary>
		/// Creates an instance of Gaussian neuron transfer function with the
		/// specified properties. </summary>
		/// <param name="properties"> properties of the Gaussian function </param>
		public Gaussian(Properties properties)
		{
			try
			{
				this.sigma = (double)properties.getProperty("transferFunction.sigma");
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

			public override double getOutput(double totalInput)
			{
				output = Math.Exp(-Math.Pow(totalInput, 2) / (2 * Math.Pow(sigma, 2)));
				  //  output = Math.exp(-0.5d * Math.pow(net, 2));
				return output;
			}

		public override double getDerivative(double net)
		{
			// TODO: check if this is correct
			double derivative = output * (-net / (sigma * sigma));
			return derivative;
		}

		/// <summary>
		/// Returns the sigma parametar of this function </summary>
		/// <returns>  sigma parametar of this function  </returns>
		public virtual double Sigma
		{
			get
			{
				return this.sigma;
			}
			set
			{
				this.sigma = value;
			}
		}


	}

}