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

namespace org.neuroph.contrib.matrixmlp
{

	/// <summary>
	/// Input matrix layer
	/// @author Zoran Sevarac
	/// </summary>
	public class MatrixInputLayer : MatrixLayer
	{
		internal double[] inputs;

		public MatrixInputLayer(int neuronsCount)
		{
			this.inputs = new double[neuronsCount];
		}

		public virtual double[] Inputs
		{
			set
			{
				Array.Copy(value, 0, this.inputs, 0, value.Length);
				this.inputs[this.inputs.Length - 1] = 1;
				//this.inputs = value;
				// dodaj i bias output
			}
			get
			{
				return inputs;
			}
		}


		public virtual double[] Outputs
		{
			set
			{
				this.inputs = value;
			}
			get
			{
				return inputs;
			}
		}


		public virtual void calculate()
		{
		}


	}

}