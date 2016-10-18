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

namespace org.neuroph.samples
{

	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;
	using MaxMinNormalizer = org.neuroph.util.data.norm.MaxMinNormalizer;
	using Normalizer = org.neuroph.util.data.norm.Normalizer;

	/// <summary>
	/// This sample shows how to do data normalization in Neuroph.
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class NormalizationSample
	{

		/// <summary>
		/// Runs this sample
		/// </summary>
		public static void Main(string[] args)
		{

			// create data set to normalize
			DataSet dataSet = new DataSet(2, 1);
			dataSet.addRow(new DataSetRow(new double[]{10, 12}, new double[]{0}));
			dataSet.addRow(new DataSetRow(new double[]{23, 19}, new double[]{0}));
			dataSet.addRow(new DataSetRow(new double[]{47, 76}, new double[]{0}));
			dataSet.addRow(new DataSetRow(new double[]{98, 123}, new double[]{1}));

			Normalizer norm = new MaxMinNormalizer();
			norm.normalize(dataSet);

			// print out normalized training set
			foreach (DataSetRow dataSetRow in dataSet.Rows)
			{
				Console.Write("Input: " + Arrays.ToString(dataSetRow.Input));
				Console.Write("Output: " + Arrays.ToString(dataSetRow.DesiredOutput));
			}
		}
	}

}