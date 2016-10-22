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
	/// Tanh neuron transfer function.
	/// 
	/// output = ( e^(2*input)-1) / ( e^(2*input)+1 )
	/// </pre>
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	[Serializable]
	public class Tanh : TransferFunction
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 2L;

		/// <summary>
		/// The slope parametetar of the Tanh function
		/// </summary>
		private double slope = 2d;

		/// <summary>
		/// The amplitude parameter
		/// </summary>
		private double amplitude = 1.7159d;

		/// <summary>
		/// Creates an instance of Tanh neuron transfer function with default
		/// slope=1.
		/// </summary>
		public Tanh()
		{
		}

		/// <summary>
		/// Creates an instance of Tanh neuron transfer function with specified
		/// value for slope parametar.
		/// </summary>
		/// <param name="slope"> the slope parametar for the Tanh function </param>
		public Tanh(double slope)
		{
			this.slope = slope;
		}

		/// <summary>
		/// Creates an instance of Tanh neuron transfer function with the
		/// specified properties.
		/// </summary>
		/// <param name="properties"> properties of the Tanh function </param>
		public Tanh(Properties properties)
		{
			try
			{
				this.slope = (double) properties.getProperty("transferFunction.slope");
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

		public override sealed double getOutput(double input)
		{
			// conditional logic helps to avoid NaN
			if (input > 100)
			{
				return 1.0;
			}
			else if (input < -100)
			{
				return -1.0;
			}

			double E_x = Math.Exp(this.slope * input);
			this.output = amplitude * ((E_x - 1d) / (E_x + 1d));
	//        this.output =  Math.tanh(2.0d/3.0*net) ;
	//        this.output = Math.tanh(net);

			return this.output;
		}

		public override sealed double getDerivative(double net)
		{
			return (1d - output * output);
		}

		/// <summary>
		/// Returns the slope parametar of this function
		/// </summary>
		/// <returns> slope parametar of this function </returns>
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


		public virtual double Amplitude
		{
			get
			{
				return amplitude;
			}
			set
			{
				this.amplitude = value;
			}
		}





	}

}