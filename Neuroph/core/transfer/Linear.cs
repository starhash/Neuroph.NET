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
	/// Linear neuron transfer function.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	[Serializable]
	public class Linear : TransferFunction
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// The slope parametetar of the linear function
		/// </summary>
		private double slope = 1d;

		/// <summary>
		/// Creates an instance of Linear transfer function
		/// </summary>
		public Linear()
		{
		}

		/// <summary>
		/// Creates an instance of Linear transfer function with specified value
		/// for getSlope parametar.
		/// </summary>
		public Linear(double slope)
		{
			this.slope = slope;
		}

		/// <summary>
		/// Creates an instance of Linear transfer function with specified properties
		/// </summary>
		public Linear(Properties properties)
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
			return slope * net;
			}

		public override double getDerivative(double net)
		{
			return this.slope;
		}
	}

}