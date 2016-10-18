﻿using System;

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


	using TransferFunctionType = org.neuroph.util.TransferFunctionType;

	/// <summary>
	/// Sgn neuron transfer function.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	[Serializable]
	public class Sgn : TransferFunction
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		///  y = 1, x > 0  
		///  y = -1, x <= 0
		/// </summary>

		public override double getOutput(double net)
		{
			if (net > 0d)
			{
				return 1d;
			}
			else
			{
				return -1d;
			}
		}

		/// <summary>
		/// Returns the properties of this function </summary>
		/// <returns> properties of this function </returns>
		public virtual java.util.Properties Properties
		{
			get
			{
                java.util.Properties properties = new java.util.Properties();
				properties.setProperty("transferFunction", TransferFunctionType.SGN.ToString());
				return properties;
			}
		}

	}

}