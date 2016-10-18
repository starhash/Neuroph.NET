using System;
using System.Collections.Generic;

/// <summary>
/// Copyright 2013 Neuroph Project http://neuroph.sourceforge.net
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
namespace org.neuroph.samples.forestCover
{

	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;
	using MaxNormalizer = org.neuroph.util.data.norm.MaxNormalizer;
	using Normalizer = org.neuroph.util.data.norm.Normalizer;

	public class GenerateData
	{

		private Config config;

		public GenerateData(Config config)
		{
			this.config = config;
		}

		public virtual void createTrainingAndTestSet()
		{
			//Creating data set from file
			DataSet dataSet = createDataSet();
			dataSet.shuffle();

			//Splitting main data set to training set (75%) and test set (25%)
			DataSet[] trainingAndTestSet = dataSet.createTrainingAndTestSubsets(75, 25);

			//Saving training set to file
			DataSet trainingSet = trainingAndTestSet[0];
			Console.WriteLine("Saving training set to file...");
			trainingSet.save(config.TrainingFileName);

			Console.WriteLine("Training set successfully saved!");

			//Normalizing test set
			DataSet testSet = trainingAndTestSet[1];
			Console.WriteLine("Normalizing test set...");

			Normalizer nor = new MaxNormalizer();
			nor.normalize(testSet);

			Console.WriteLine("Saving normalized test set to file...");
			testSet.shuffle();
			testSet.save(config.TestFileName);
			Console.WriteLine("Normalized test set successfully saved!");
			Console.WriteLine("Training set size: " + trainingSet.Rows.Count + " rows. ");
			Console.WriteLine("Test set size: " + testSet.Rows.Count + " rows. ");
			Console.WriteLine("-----------------------------------------------");

			double percentTraining = (double) trainingSet.Rows.Count * 100.0 / (double) dataSet.Rows.Count;
			double percentTest = (double) testSet.Rows.Count * 100.0 / (double) dataSet.Rows.Count;
			Console.WriteLine("Training set takes " + formatDecimalNumber(percentTraining) + "% of main data set. ");
			Console.WriteLine("Test set takes " + formatDecimalNumber(percentTest) + "% of main data set. ");

		}

		//Create data set from file
		private DataSet createDataSet()
		{
			DataSet dataSet = DataSet.createFromFile(config.DataFilePath, 54, 7, ",");
			Console.WriteLine("Main data set size: " + dataSet.Rows.Count + " rows. ");

			return dataSet;
		}

		//Formating decimal number to have 3 decimal places
		private string formatDecimalNumber(double number)
		{
			return (new decimal(number)).setScale(3, RoundingMode.HALF_UP).ToString();
		}

		//Creating balanced training set with defined maximum sample of each type od tree 
		public virtual void createBalancedTrainingSet(int count)
		{
			//Creating empety data set
			DataSet balanced = new DataSet(54, 7);
			//Declare counter for all seven type of tree
			int firstType = 0;
			int secondType = 0;
			int thirdType = 0;
			int fourthType = 0;
			int fifthType = 0;
			int sixthType = 0;
			int seventhType = 0;

			DataSet trainingSet = DataSet.load(config.TrainingFileName);
			List<DataSetRow> rows = trainingSet.Rows;
			Console.WriteLine("Test set size: " + rows.Count + " rows. ");

			foreach (DataSetRow row in rows)
			{
				//Taking desired output vector from loaded file
				double[] DesiredOutput = row.DesiredOutput;
				int index = -1;
				//Find index of number one in output vector. 
				for (int i = 0; i < DesiredOutput.Length; i++)
				{
					if (DesiredOutput[i] == 1.0)
					{
						index = i;
						break;
					}
				}
				//Add row to balanced data set if number of that type of tree is less than maximum
				switch (index + 1)
				{
					case 1:
						if (firstType < count)
						{
							balanced.addRow(row);
							firstType++;
						}
						break;
					case 2:
						if (secondType < count)
						{
							balanced.addRow(row);
							secondType++;
						}
						break;
					case 3:
						if (thirdType < count)
						{
							balanced.addRow(row);
							thirdType++;
						}
						break;
					case 4:
						if (fourthType < count)
						{
							balanced.addRow(row);
							fourthType++;
						}
						break;
					case 5:
						if (fifthType < count)
						{
							balanced.addRow(row);
							fifthType++;
						}
						break;
					case 6:
						if (sixthType < count)
						{
							balanced.addRow(row);
							sixthType++;
						}
						break;
					case 7:
						if (seventhType < count)
						{
							balanced.addRow(row);
							seventhType++;
						}
						break;
					default:
						Console.WriteLine("Error with output vector size! ");
					break;
				}
			}
			Console.WriteLine("Balanced test set size: " + balanced.Rows.Count + " rows. ");
			Console.WriteLine("Samples per tree: ");
			Console.WriteLine("First type: " + firstType + " samples. ");
			Console.WriteLine("Second type: " + secondType + " samples. ");
			Console.WriteLine("Third type: " + thirdType + " samples. ");
			Console.WriteLine("Fourth type: " + fourthType + " samples. ");
			Console.WriteLine("Fifth type: " + fifthType + " samples. ");
			Console.WriteLine("Sixth type: " + sixthType + " samples. ");
			Console.WriteLine("Seventh type: " + seventhType + " samples. ");

			balanced.save(config.BalancedFileName);
		}

		public virtual void normalizeBalancedTrainingSet()
		{
			//Normalizing balanced training set with MaxNormalizer
			DataSet dataSet = DataSet.load(config.BalancedFileName);
			Normalizer normalizer = new MaxNormalizer();
			normalizer.normalize(dataSet);

			Console.WriteLine("Saving normalized training data set to file... ");
			dataSet.shuffle();
			dataSet.shuffle();
			dataSet.save(config.NormalizedBalancedFileName);
			Console.WriteLine("Normalized training data set successfully saved!");
		}
	}

}