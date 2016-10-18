/// <summary>
/// Copyright 2010 Neuroph Project http://neuroph.sourceforge.net
/// 
/// Licensed under the Apache License, Version 2.0 (the "License"); you may not
/// use this file except in compliance with the License. You may obtain a copy of
/// the License at
/// 
/// http://www.apache.org/licenses/LICENSE-2.0
/// 
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
/// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
/// License for the specific language governing permissions and limitations under
/// the License.
/// </summary>
namespace org.neuroph.util.data.norm
{

	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;

	/// <summary>
	/// Decimal scaling normalization method, which normalize data by moving decimal
	/// point in regard to max element in training set (by columns) Normalization is
	/// done according to formula: normalizedVector[i] = vector[i] / scaleFactor[i]
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class DecimalScaleNormalizer : Normalizer
	{

		private double[] maxIn, maxOut; // contains max values for all columns
		private double[] scaleFactorIn, scaleFactorOut; // holds scaling values for all columns

		public virtual void normalize(DataSet dataSet)
		{
			findMaxVectors(dataSet);
			findScaleVectors();

			foreach (DataSetRow dataSetRow in dataSet.Rows)
			{
				double[] normalizedInput = normalizeScale(dataSetRow.Input, scaleFactorIn);
				dataSetRow.Input = normalizedInput;

				if (dataSet.Supervised)
				{
					double[] normalizedOutput = normalizeScale(dataSetRow.DesiredOutput, scaleFactorOut);
					dataSetRow.DesiredOutput = normalizedOutput;
				}
			}
		}

		/// <summary>
		/// Finds max values for all columns in dataset (inputs and outputs)
		/// Sets max column values to maxIn and maxOut fields </summary>
		/// <param name="dataSet">  </param>
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

		public virtual void findScaleVectors()
		{
			scaleFactorIn = new double[maxIn.Length];
			for (int i = 0; i < scaleFactorIn.Length; i++)
			{
				scaleFactorIn[i] = 1;
			}

			for (int i = 0; i < maxIn.Length; i++)
			{
				while (maxIn[i] > 1)
				{
					maxIn[i] = maxIn[i] / 10.0;
					scaleFactorIn[i] = scaleFactorIn[i] * 10;
				}
			}

			scaleFactorOut = new double[maxOut.Length];
			for (int i = 0; i < scaleFactorOut.Length; i++)
			{
				scaleFactorOut[i] = 1;
			}

			for (int i = 0; i < maxOut.Length; i++)
			{
				while (maxOut[i] > 1)
				{
					maxOut[i] = maxOut[i] / 10.0;
					scaleFactorOut[i] = scaleFactorOut[i] * 10;
				}
			}


		}

		private double[] normalizeScale(double[] vector, double[] scaleFactor)
		{
			double[] normalizedVector = new double[vector.Length];
			for (int i = 0; i < vector.Length; i++)
			{
				normalizedVector[i] = vector[i] / scaleFactor[i];
			}
			return normalizedVector;
		}
	}

}