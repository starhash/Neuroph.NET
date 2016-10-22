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

namespace org.neuroph.util.random
{

	using Connection = org.neuroph.core.Connection;
	using Layer = org.neuroph.core.Layer;
	using Neuron = org.neuroph.core.Neuron;

	/// <summary>
	/// This class provides distort randomization technique, which distorts existing 
	/// weight values using specified distortion factor.
	/// Weights are distorted using following formula:
	/// newWeightValue = currentWeightValue + (distortionFactor - (random * distortionFactor * 2))
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class DistortRandomizer : WeightsRandomizer
	{

		/// <summary>
		/// Distrotion factor which determines the amount to distort existing weight values
		/// </summary>
		internal double distortionFactor;

		/// <summary>
		/// Create a new instance of DistortRandomizer with specified distortion factor </summary>
		/// <param name="distortionFactor"> amount to distort existing weights </param>
		public DistortRandomizer(double distortionFactor)
		{
			this.distortionFactor = distortionFactor;
		}

	//    /**
	//     * Iterate all layers, neurons and connection weight and apply distort randomization 
	//     * @param neuralNetwork 
	//     */
	//    @Override
	//    public void randomize(NeuralNetwork neuralNetwork) {
	//        for (Layer layer : neuralNetwork.getLayers()) {
	//            for (Neuron neuron : layer.getNeurons()) {
	//                for (Connection connection : neuron.getInputConnections()) {
	//                    double weight = connection.getWeight().getValue();
	//                    connection.getWeight().setValue(distort(weight));
	//                }
	//            }
	//        }
	//
	//    }

		/// <summary>
		/// Iterate all layers, neurons and connection weight and apply distort randomization </summary>
		/// <param name="neuron"> </param>
		public override void randomize(Neuron neuron)
		{
				foreach (Connection connection in neuron.InputConnections)
				{
						double weight = connection.Weight.Value;
						connection.Weight.Value = distort(weight);
				}
		}


		/// <summary>
		/// Returns distorted weight value </summary>
		/// <param name="weight"> current weight value </param>
		/// <returns> distorted weight value </returns>
		private double distort(double weight)
		{
			return weight + (this.distortionFactor - (randomGenerator.NextDouble() * this.distortionFactor * 2));
		}

	}

}