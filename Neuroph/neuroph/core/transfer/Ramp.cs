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
	/// Ramp neuron transfer function.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	[Serializable]
	public class Ramp : TransferFunction
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// The slope parametetar of the ramp function
		/// </summary>
		private double slope = 1d;

		/// <summary>
		/// Threshold for the high output level
		/// </summary>
		private double xHigh = 1d;

		/// <summary>
		/// Threshold for the low output level
		/// </summary>
		private double xLow = 0d;

		/// <summary>
		/// Output value for the high output level
		/// </summary>
		private double yHigh = 1d;

		/// <summary>
		/// Output value for the low output level
		/// </summary>
		private double yLow = 0d;

		/// <summary>
		/// Creates an instance of Ramp transfer function with default settings
		/// </summary>
		public Ramp()
		{
		}

		/// <summary>
		/// Creates an instance of Ramp transfer function with specified settings
		/// </summary>
		public Ramp(double slope, double xLow, double xHigh, double yLow, double yHigh)
		{
			this.slope = slope;
			this.xLow = xLow;
			this.xHigh = xHigh;
			this.yLow = yLow;
			this.yHigh = yHigh;
		}

		/// <summary>
		/// Creates an instance of Ramp transfer function with specified properties.
		/// </summary>
		public Ramp(Properties properties)
		{
			try
			{
				this.slope = (double)properties.getProperty("transferFunction.slope");
				this.yHigh = (double)properties.getProperty("transferFunction.yHigh");
				this.yLow = (double)properties.getProperty("transferFunction.yLow");
				this.xHigh = (double)properties.getProperty("transferFunction.xHigh");
				this.xLow = (double)properties.getProperty("transferFunction.xLow");
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
			if (net < this.xLow)
			{
				return this.yLow;
			}
			else if (net > this.xHigh)
			{
				return this.yHigh;
			}
			else
			{
				return (double)(slope * net);
			}
			}

		/// <summary>
		/// Returns threshold value for the low output level </summary>
		/// <returns> threshold value for the low output level  </returns>
		public virtual double XLow
		{
			get
			{
				return this.xLow;
			}
			set
			{
				this.xLow = value;
			}
		}


		/// <summary>
		/// Returns threshold value for the high output level </summary>
		/// <returns> threshold value for the high output level  </returns>
		public virtual double XHigh
		{
			get
			{
				return this.xHigh;
			}
			set
			{
				this.xHigh = value;
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


	}
}