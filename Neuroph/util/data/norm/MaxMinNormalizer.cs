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

namespace org.neuroph.util.data.norm
{

	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;

	/// <summary>
	/// MaxMin normalization method, which normalize data in regard to min and max elements in training set (by columns)
	/// Normalization is done according to formula:
	/// normalizedVector[i] = (vector[i] - min[i]) / (max[i] - min[i])
	/// 
	/// This class works fine if  max and min are both positive and we want to normalize to  [0,1]
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class MaxMinNormalizer : Normalizer
	{
		internal double[] maxIn, maxOut; // contains max values for in and out columns
		internal double[] minIn, minOut; // contains min values for in and out columns


		public virtual void normalize(DataSet dataSet)
		{
			// find min i max vectors
			findMaxAndMinVectors(dataSet);

			foreach (DataSetRow row in dataSet.Rows)
			{
			   double[] normalizedInput = normalizeMaxMin(row.Input, minIn, maxIn);
			   row.Input = normalizedInput;

			   if (dataSet.Supervised)
			   {
					double[] normalizedOutput = normalizeMaxMin(row.DesiredOutput, minOut, maxOut);
					row.DesiredOutput = normalizedOutput;
			   }
			}

		}

	  private void findMaxAndMinVectors(DataSet dataSet)
	  {
			int inputSize = dataSet.InputSize;
			int outputSize = dataSet.OutputSize;

			maxIn = new double[inputSize];
			minIn = new double[inputSize];

			for (int i = 0; i < inputSize; i++)
			{
				maxIn[i] = double.Epsilon;
				minIn[i] = double.MaxValue;
			}

			maxOut = new double[outputSize];
			minOut = new double[outputSize];

			for (int i = 0; i < outputSize; i++)
			{
				maxOut[i] = double.Epsilon;
				minOut[i] = double.MaxValue;
			}

			foreach (DataSetRow dataSetRow in dataSet.Rows)
			{
				double[] input = dataSetRow.Input;
				for (int i = 0; i < inputSize; i++)
				{
					if (input[i] > maxIn[i])
					{
						maxIn[i] = input[i];
					}
					if (input[i] < minIn[i])
					{
						minIn[i] = input[i];
					}
				}

				double[] output = dataSetRow.DesiredOutput;
				for (int i = 0; i < outputSize; i++)
				{
					if (output[i] > maxOut[i])
					{
						maxOut[i] = output[i];
					}
					if (output[i] < minOut[i])
					{
						minOut[i] = output[i];
					}
				}

			}
	  }


		private double[] normalizeMaxMin(double[] vector, double[] min, double[] max)
		{
			double[] normalizedVector = new double[vector.Length];

			for (int i = 0; i < vector.Length; i++)
			{
				normalizedVector[i] = (vector[i] - min[i]) / (max[i] - min[i]);
			}

			return normalizedVector;
		}

	}

}