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

	using Layer = org.neuroph.core.Layer;
	using org.neuroph.core;
	using Tanh = org.neuroph.core.transfer.Tanh;
	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;

	/// <summary>
	/// Matrix based implementation of Multi LAyer Perceptron
	/// @author Zoran Sevarac
	/// </summary>
	public class MatrixMultiLayerPerceptron : NeuralNetwork
	{

		internal MultiLayerPerceptron sourceNetwork;
		internal MatrixLayer[] matrixLayers;

		public MatrixMultiLayerPerceptron(MultiLayerPerceptron sourceNetwork)
		{
			this.sourceNetwork = sourceNetwork;
			// copy layers, input and output neurons

			createMatrixLayers();
			this.LearningRule = new MatrixMomentumBackpropagation();
		}

		public virtual MatrixLayer[] MatrixLayers
		{
			get
			{
				return matrixLayers;
			}
		}



		private void createMatrixLayers()
		{
			matrixLayers = new MatrixLayer[sourceNetwork.LayersCount];
			matrixLayers[0] = new MatrixInputLayer(sourceNetwork.getLayerAt(0).NeuronsCount);

			MatrixLayer prevLayer = matrixLayers[0];

			 for (int i = 1; i < sourceNetwork.LayersCount; i++)
			 {
				Layer layer = sourceNetwork.getLayerAt(i);
				MatrixMlpLayer newBpLayer = new MatrixMlpLayer(layer, prevLayer, new Tanh());
				matrixLayers[i] = newBpLayer;
				prevLayer = newBpLayer;
			 }
		}

			public override void calculate()
			{
				 for (int i = 1; i < matrixLayers.Length; i++)
				 {
					 matrixLayers[i].calculate();
				 }
			}

			 public override double[] Input
			 {
				 set
				 {
					 matrixLayers[0].Inputs = value;
				 }
			 }

			 public override double[] Output
			 {
				 get
				 {
					return matrixLayers[matrixLayers.Length - 1].Outputs;
				 }
			 }

			 public override int LayersCount
			 {
				 get
				 {
					 return matrixLayers.Length;
				 }
			 }


	}




}