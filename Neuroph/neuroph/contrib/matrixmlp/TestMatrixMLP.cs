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

	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;
	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;
	using TransferFunctionType = org.neuroph.util.TransferFunctionType;

	/// <summary>
	/// Test class for matrix based MLP implementation.
	/// @author Zoran Sevarac
	/// </summary>
	public class TestMatrixMLP
	{

		/// <summary>
		/// Create and run MLP with XOR training set
		/// </summary>
		public static void Main(string[] args)
		{
			// create training set (logical XOR function)
			DataSet trainingSet = new DataSet(2, 1);
			trainingSet.addRow(new DataSetRow(new double[]{0, 0}, new double[]{0}));
			trainingSet.addRow(new DataSetRow(new double[]{0, 1}, new double[]{1}));
			trainingSet.addRow(new DataSetRow(new double[]{1, 0}, new double[]{1}));
			trainingSet.addRow(new DataSetRow(new double[]{1, 1}, new double[]{0}));

			MultiLayerPerceptron nnet = new MultiLayerPerceptron(TransferFunctionType.TANH,2, 3, 1);
			MatrixMultiLayerPerceptron mnet = new MatrixMultiLayerPerceptron(nnet);

			Console.WriteLine("Training network...");

			mnet.learn(trainingSet);

			Console.WriteLine("Done training network.");
		}


	}

}