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

namespace org.neuroph.contrib.samples.stockmarket
{

	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;

	/// <summary>
	/// Creates training set from the given data.
	/// See http://neuroph.sourceforge.net/tutorials/StockMarketPredictionTutorial.html
	/// @author Dr.V.Steinhauer
	/// </summary>
	public class TrainingData
	{

		private string[] valuesRow;
		private DataSet trainingSet = new DataSet(4, 1);
		private double normalizer = 10000.0D;
		private double minlevel = 0.0D;

		public TrainingData()
		{
		}

		public TrainingData(string[] valuesRow)
		{
			this.ValuesRow = valuesRow;
		}


		public virtual string[] ValuesRow
		{
			get
			{
				return valuesRow;
			}
			set
			{
				this.valuesRow = value;
			}
		}


		public virtual double Normalizer
		{
			get
			{
				return normalizer;
			}
			set
			{
				this.normalizer = value;
			}
		}



		public virtual DataSet TrainingSet
		{
			get
			{
				int length = valuesRow.Length;
				if (length < 5)
				{
					System.Console.WriteLine("valuesRow.length < 5");
					return null;
				}
				try
				{
					for (int i = 0; i + 4 < valuesRow.Length; i++)
					{
						string[] s1 = valuesRow[i].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
						string[] s2 = valuesRow[i + 1].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
						string[] s3 = valuesRow[i + 2].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
						string[] s4 = valuesRow[i + 3].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
						string[] s5 = valuesRow[i + 4].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
						double d1 = (Convert.ToDouble(s1[1]) - minlevel) / normalizer;
						double d2 = (Convert.ToDouble(s2[1]) - minlevel) / normalizer;
						double d3 = (Convert.ToDouble(s3[1]) - minlevel) / normalizer;
						double d4 = (Convert.ToDouble(s4[1]) - minlevel) / normalizer;
						double d5 = (Convert.ToDouble(s5[1]) - minlevel) / normalizer;
						System.Console.WriteLine(i + " " + d1 + " " + d2 + " " + d3 + " " + d4 + " ->" + d5);
						trainingSet.addRow(new DataSetRow(new double[]{d1, d2, d3, d4}, new double[]{d5}));
					}
				}
				catch (Exception e)
				{
					System.Console.WriteLine(e.Message);
					return null;
				}
				return trainingSet;
			}
			set
			{
				this.trainingSet = value;
			}
		}



	}

}