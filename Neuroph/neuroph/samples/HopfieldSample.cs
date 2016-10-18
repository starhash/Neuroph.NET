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
	using Hopfield = org.neuroph.nnet.Hopfield;

	/// <summary>
	/// This sample shows how to create and train Hopfield neural network
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class HopfieldSample
	{

		/// <summary>
		/// Runs this sample
		/// </summary>
		public static void Main(string[] args)
		{

			// create training set (H and T letter in 3x3 grid)
			DataSet trainingSet = new DataSet(9);
			trainingSet.addRow(new DataSetRow(new double[]{1, 0, 1, 1, 1, 1, 1, 0, 1})); // H letter

			trainingSet.addRow(new DataSetRow(new double[]{1, 1, 1, 0, 1, 0, 0, 1, 0})); // T letter

			// create hopfield network
			Hopfield myHopfield = new Hopfield(9);
			// learn the training set
			myHopfield.learn(trainingSet);

			// test hopfield network
			Console.WriteLine("Testing network");

			// add one more 'incomplete' H pattern for testing - it will be recognized as H
			trainingSet.addRow(new DataSetRow(new double[]{1, 0, 0, 1, 0, 1, 1, 0, 1}));


			// print network output for the each element from the specified training set.
			foreach (DataSetRow trainingSetRow in trainingSet.Rows)
			{
				myHopfield.Input = trainingSetRow.Input;
				myHopfield.calculate();
				myHopfield.calculate();
				double[] networkOutput = myHopfield.Output;

				Console.Write("Input: " + Arrays.ToString(trainingSetRow.Input));
				Console.WriteLine(" Output: " + Arrays.ToString(networkOutput));
			}

		}

	}
}