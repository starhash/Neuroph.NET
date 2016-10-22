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
	/// Fuzzy trapezoid neuron tranfer function.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	[Serializable]
	public class Trapezoid : TransferFunction
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		// these are the points of trapezoid function
		internal double leftLow, leftHigh, rightLow, rightHigh;


		/// <summary>
		/// Creates an instance of Trapezoid transfer function
		/// </summary>
		public Trapezoid()
		{
			this.leftLow = 0d;
			this.leftHigh = 1d;
			this.rightLow = 3d;
			this.rightHigh = 2d;
		}

		/// <summary>
		/// Creates an instance of Trapezoid transfer function with the specified
		/// setting.
		/// </summary>
		public Trapezoid(double leftLow, double leftHigh, double rightLow, double rightHigh)
		{
			this.leftLow = leftLow;
			this.leftHigh = leftHigh;
			this.rightLow = rightLow;
			this.rightHigh = rightHigh;
		}

		/// <summary>
		/// Creates an instance of Trapezoid transfer function with the specified
		/// properties.
		/// </summary>
		public Trapezoid(Properties properties)
		{
			try
			{
				this.leftLow = (double)properties.getProperty("transferFunction.leftLow");
				this.leftHigh = (double)properties.getProperty("transferFunction.leftHigh");
				this.rightLow = (double)properties.getProperty("transferFunction.rightLow");
				this.rightHigh = (double)properties.getProperty("transferFunction.rightHigh");
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
			if ((net >= leftHigh) && (net <= rightHigh))
			{
				return 1d;
			}
			else if ((net > leftLow) && (net < leftHigh))
			{
				return (net - leftLow) / (leftHigh - leftLow);
			}
			else if ((net > rightHigh) && (net < rightLow))
			{
				return (rightLow - net) / (rightLow - rightHigh);
			}

			return 0d;
			}

		/// <summary>
		/// Sets left low point of trapezoid function </summary>
		/// <param name="leftLow"> left low point of trapezoid function </param>
		public virtual double LeftLow
		{
			set
			{
				this.leftLow = value;
			}
			get
			{
				return leftLow;
			}
		}

		/// <summary>
		/// Sets left high point of trapezoid function </summary>
		/// <param name="leftHigh"> left high point of trapezoid function </param>
		public virtual double LeftHigh
		{
			set
			{
				this.leftHigh = value;
			}
			get
			{
				return leftHigh;
			}
		}

		/// <summary>
		/// Sets right low point of trapezoid function </summary>
		/// <param name="rightLow"> right low point of trapezoid function </param>
		public virtual double RightLow
		{
			set
			{
				this.rightLow = value;
			}
			get
			{
				return rightLow;
			}
		}

		/// <summary>
		/// Sets right high point of trapezoid function </summary>
		/// <param name="rightHigh"> right high point of trapezoid function </param>
		public virtual double RightHigh
		{
			set
			{
				this.rightHigh = value;
			}
			get
			{
				return rightHigh;
			}
		}





	}

}