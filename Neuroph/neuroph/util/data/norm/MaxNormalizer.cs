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
	/// Max normalization method, which normalize data in regard to max element in training set (by columns)
	/// Normalization is done according to formula:
	/// normalizedVector[i] = vector[i] / abs(max[i])
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class MaxNormalizer : Normalizer
	{
		internal double[] maxIn, maxOut; // these contain max values for in and out columns


		public virtual void normalize(DataSet dataSet)
		{

			findMaxVectors(dataSet);

			foreach (DataSetRow row in dataSet.Rows)
			{
				double[] normalizedInput = normalizeMax(row.Input, maxIn);
				row.Input = normalizedInput;

				if (dataSet.Supervised)
				{
					double[] normalizedOutput = normalizeMax(row.DesiredOutput, maxOut);
					row.DesiredOutput = normalizedOutput;
				}
			}

		}


	   /// <summary>
	   /// Finds max values for columns in input and output vector for given data set </summary>
	   /// <param name="dataSet">   </param>
		private void findMaxVectors(DataSet dataSet)
		{
			int inputSize = dataSet.InputSize;
			int outputSize = dataSet.OutputSize;

			maxIn = new double[inputSize];
			for (int i = 0; i < inputSize; i++)
			{
				maxIn[i] = double.Epsilon;
			}

			maxOut = new double[outputSize];
			for (int i = 0; i < outputSize; i++)
			{
				maxOut[i] = double.Epsilon;
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
				}

				double[] output = dataSetRow.DesiredOutput;
				for (int i = 0; i < outputSize; i++)
				{
					if (output[i] > maxOut[i])
					{
						maxOut[i] = output[i];
					}
				}

			}
		}


		public virtual double[] normalizeMax(double[] vector, double[] max)
		{
			double[] normalizedVector = new double[vector.Length];

			for (int i = 0; i < vector.Length; i++)
			{
					normalizedVector[i] = vector[i] / max[i];
			}

			return normalizedVector;
		}

	}

}