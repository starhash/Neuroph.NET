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

	using org.neuroph.core;
	using TransferFunction = org.neuroph.core.transfer.TransferFunction;
	using MomentumBackpropagation = org.neuroph.nnet.learning.MomentumBackpropagation;

	/// <summary>
	/// Momentum Backpropagation for matrix based MLP
	/// @author Zoran Sevarac
	/// </summary>
	public class MatrixMomentumBackpropagation : MomentumBackpropagation
	{

			private MatrixMultiLayerPerceptron matrixMlp;
			private MatrixLayer[] matrixLayers;

			public override NeuralNetwork NeuralNetwork
			{
				set
				{
					base.NeuralNetwork = value;
					this.matrixMlp = (MatrixMultiLayerPerceptron)this.NeuralNetwork;
					matrixLayers = matrixMlp.MatrixLayers;
				}
			}

			/// <summary>
			/// This method implements weights update procedure for the output neurons
			/// </summary>
			/// <param name="patternError">
			///            single pattern error vector </param>
			protected internal override void calculateErrorAndUpdateOutputNeurons(double[] patternError)
			{

					// get output layer
					MatrixMlpLayer outputLayer = (MatrixMlpLayer)matrixLayers[matrixLayers.Length - 1];
					TransferFunction transferFunction = outputLayer.TransferFunction;

					// get output vector
					double[] outputs = outputLayer.Outputs;
					double[] netInputs = outputLayer.NetInput;
					double[] neuronErrors = outputLayer.Errors; // these will hold  -should be set from here!!!!

					// calculate errors(deltas) for all output neurons
					for (int i = 0; i < outputs.Length; i++)
					{
						neuronErrors[i] = patternError[i] * transferFunction.getDerivative(netInputs[i]); // ovde mi treba weighted sum, da ne bi morao ponovo da racunam
					}

					// update weights
					this.updateLayerWeights(outputLayer, neuronErrors);
					Console.WriteLine("MSE:" + ErrorFunction.TotalError);
			}


			// http://netbeans.org/kb/docs/java/profiler-profilingpoints.html
			protected internal virtual void updateLayerWeights(MatrixMlpLayer layer, double[] errors)
			{

				double[] inputs = layer.Inputs;
				double[][] weights = layer.Weights;
				double[][] deltaWeights = layer.DeltaWeights;

					for (int neuronIdx = 0; neuronIdx < layer.NeuronsCount; neuronIdx++) // iterate neurons
					{
					  for (int weightIdx = 0; weightIdx < weights[neuronIdx].Length; weightIdx++) // iterate weights
					  {
						  // calculate weight change
						  double deltaWeight = this.learningRate * errors[neuronIdx] * inputs[weightIdx] + momentum * (deltaWeights[neuronIdx][weightIdx]);

						  deltaWeights[neuronIdx][weightIdx] = deltaWeight; // save weight change to calculate momentum
						  weights[neuronIdx][weightIdx] += deltaWeight; // apply weight change

					  }
					}
			}


			/// <summary>
			/// backpropogate errors through all hidden layers and update conneciion weights
			/// for those layers.
			/// </summary>
			protected internal override void calculateErrorAndUpdateHiddenNeurons()
			{
				int layersCount = matrixMlp.LayersCount;

				for (int layerIdx = layersCount - 2 ; layerIdx > 0 ; layerIdx--)
				{
				   MatrixMlpLayer currentLayer = (MatrixMlpLayer)matrixLayers[layerIdx];

				   TransferFunction transferFunction = currentLayer.TransferFunction;
				   int neuronsCount = currentLayer.NeuronsCount;

				   double[] neuronErrors = currentLayer.Errors;
				   double[] netInputs = currentLayer.NetInput;

				   MatrixMlpLayer nextLayer = (MatrixMlpLayer)currentLayer.NextLayer;
				   double[] nextLayerErrors = nextLayer.Errors;
				   double[][] nextLayerWeights = nextLayer.Weights;

				   // calculate error for each neuron in current layer
				   for (int neuronIdx = 0; neuronIdx < neuronsCount; neuronIdx++)
				   {
					   // calculate weighted sum of errors of all neuron it is attached to - calculate how much this neuron is contributing to errors in next layer
					   double weightedErrorsSum = 0;

					   for (int nextLayerNeuronIdx = 0; nextLayerNeuronIdx < nextLayer.NeuronsCount; nextLayerNeuronIdx++)
					   {
						 weightedErrorsSum += nextLayerErrors[nextLayerNeuronIdx] * nextLayerWeights[nextLayerNeuronIdx][neuronIdx];
					   }

					   // calculate the error for this neuron
					   neuronErrors[neuronIdx] = transferFunction.getDerivative(netInputs[neuronIdx]) * weightedErrorsSum;
				   } // neuron iterator

				   this.updateLayerWeights(currentLayer, neuronErrors);

				} // layer iterator
			}
	}
}