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
	/// Step neuron transfer function.
	/// y = yHigh, x > 0
	/// y = yLow, x <= 0
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	[Serializable]
	public class Step : TransferFunction
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Output value for high output level
		/// </summary>
		private double yHigh = 1d;

		/// <summary>
		/// Output value for low output level
		/// </summary>
		private double yLow = 0d;

		/// <summary>
		/// Creates an instance of Step transfer function
		/// </summary>
		public Step()
		{
		}

		/// <summary>
		/// Creates an instance of Step transfer function with specified properties
		/// </summary>
		public Step(Properties properties)
		{
			try
			{
				this.yHigh = (double)properties.getProperty("transferFunction.yHigh");
				this.yLow = (double)properties.getProperty("transferFunction.yLow");
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

			public override double getOutput(double net)
			{
			if (net > 0d)
			{
				return yHigh;
			}
			else
			{
				return yLow;
			}
			}

		/// <summary>
		/// Returns output value for high output level </summary>
		/// <returns> output value for high output level  </returns>
		public virtual double YHigh
		{
			get
			{
				return this.yHigh;
			}
			set
			{
				this.yHigh = value;
			}
		}


		/// <summary>
		/// Returns output value for low output level </summary>
		/// <returns> output value for low output level  </returns>
		public virtual double YLow
		{
			get
			{
				return this.yLow;
			}
			set
			{
				this.yLow = value;
			}
		}


		/// <summary>
		/// Returns the properties of this function </summary>
		/// <returns> properties of this function </returns>
		public virtual Properties Properties
		{
			get
			{
				Properties properties = new Properties();
				properties.setProperty("transferFunction.yHigh", Convert.ToString(yHigh));
				properties.setProperty("transferFunction.yLow", Convert.ToString(yLow));
				return properties;
			}
		}

	}

}