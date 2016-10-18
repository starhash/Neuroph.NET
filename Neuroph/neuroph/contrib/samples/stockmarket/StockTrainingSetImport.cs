using System.Collections.Generic;

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

namespace org.neuroph.contrib.samples.stockmarket
{

	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;
	using TrainingSetImport = org.neuroph.util.TrainingSetImport;

	/// <summary>
	/// The part of simple stock market components, easy to use
	/// stock market interface for neural network. Provides method to import stock training set froman array
	/// 
	/// @author Valentin Steinhauer <valentin.steinhauer@t-online.de>
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class StockTrainingSetImport : TrainingSetImport
	{

		/// <summary>
		/// Creates and returns training set for stock market prediction using the provided data from array </summary>
		/// <param name="values"> an array containing stock data </param>
		/// <param name="inputsCount"> training element (neural net) inputs count </param>
		/// <param name="outputsCount"> training element (neural net) ouputs count </param>
		/// <returns> training set with stock data </returns>
		public static DataSet importFromArray(double[] values, int inputsCount, int outputsCount)
		{
			DataSet trainingSet = new DataSet(inputsCount, outputsCount);
			for (int i = 0; i < values.Length - inputsCount; i++)
			{
				List<double> inputs = new List<double>();
				for (int j = i; j < i + inputsCount; j++)
				{
					inputs.Add(values[j]);
				}
				List<double> outputs = new List<double>();
				if (outputsCount > 0 && i + inputsCount + outputsCount <= values.Length)
				{
					for (int j = i + inputsCount; j < i + inputsCount + outputsCount; j++)
					{
						outputs.Add(values[j]);
					}
					if (outputsCount > 0)
					{
						trainingSet.addRow(new DataSetRow(inputs, outputs));
					}
					else
					{
						trainingSet.addRow(new DataSetRow(inputs));
					}
				}
			}
			return trainingSet;
		}
	}

}