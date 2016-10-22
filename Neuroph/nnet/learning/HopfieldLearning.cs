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

namespace org.neuroph.nnet.learning
{

	using Connection = org.neuroph.core.Connection;
	using Layer = org.neuroph.core.Layer;
	using org.neuroph.core;
	using Neuron = org.neuroph.core.Neuron;
	using LearningRule = org.neuroph.core.learning.LearningRule;
	using DataSetRow = org.neuroph.core.data.DataSetRow;
	using DataSet = org.neuroph.core.data.DataSet;

	/// <summary>
	/// Learning algorithm for the Hopfield neural network.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class HopfieldLearning : LearningRule
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Creates new HopfieldLearning
		/// </summary>
		public HopfieldLearning() : base()
		{
		}


		/// <summary>
		/// Calculates weights for the hopfield net to learn the specified training
		/// set
		/// </summary>
		/// <param name="trainingSet">
		///            training set to learn </param>
		public override void learn(DataSet trainingSet)
		{
			int M = trainingSet.size();
			int N = neuralNetwork.getLayerAt(0).NeuronsCount;
			Layer hopfieldLayer = neuralNetwork.getLayerAt(0);

			for (int i = 0; i < N; i++)
			{
				for (int j = 0; j < N; j++)
				{
					if (j == i)
					{
						continue;
					}
					Neuron ni = hopfieldLayer.getNeuronAt(i);
					Neuron nj = hopfieldLayer.getNeuronAt(j);
					Connection cij = nj.getConnectionFrom(ni);
					Connection cji = ni.getConnectionFrom(nj);
					double w = 0;
					for (int k = 0; k < M; k++)
					{
						DataSetRow trainingSetRow = trainingSet.getRowAt(k);
						double pki = trainingSetRow.Input[i];
						double pkj = trainingSetRow.Input[j];
						w = w + pki * pkj;
					} // k
					cij.Weight.Value = w;
					cji.Weight.Value = w;
				} // j
			} // i

		}

	}

}