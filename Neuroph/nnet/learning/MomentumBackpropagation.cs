/// <summary>
/// Copyright 2010 Neuroph Project http://neuroph.sourceforge.net
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
namespace org.neuroph.nnet.learning
{

	using Connection = org.neuroph.core.Connection;
	using Layer = org.neuroph.core.Layer;
	using Neuron = org.neuroph.core.Neuron;
	using Weight = org.neuroph.core.Weight;

    /// <summary>
    /// Backpropagation learning rule with momentum.
    /// 
    /// @author Zoran Sevarac <sevarac@gmail.com>
    /// </summary>
    [System.Serializable]
    public class MomentumBackpropagation : BackPropagation
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization compatibility
		/// with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;
		/// <summary>
		/// Momentum factor
		/// </summary>
		protected internal double momentum = 0.25d;

		/// <summary>
		/// Creates new instance of MomentumBackpropagation learning
		/// </summary>
		public MomentumBackpropagation() : base()
		{
		}

		/// <summary>
		/// This method implements weights update procedure for the single neuron for
		/// the back propagation with momentum factor
		/// </summary>
		/// <param name="neuron"> neuron to update weights </param>
		public override void updateNeuronWeights(Neuron neuron)
		{
			foreach (Connection connection in neuron.InputConnections)
			{
				double input = connection.Input;
				if (input == 0)
				{
					continue;
				}

				// get the error for specified neuron,
				double neuronError = neuron.Error;

				// tanh can be used to minimise the impact of big error values, which can cause network instability
				// suggested at https://sourceforge.net/tracker/?func=detail&atid=1107579&aid=3130561&group_id=238532
				// double neuronError = Math.tanh(neuron.getError());

				Weight weight = connection.Weight;
				MomentumWeightTrainingData weightTrainingData = (MomentumWeightTrainingData) weight.TrainingData;

				//double currentWeightValue = weight.getValue();
				double previousWeightValue = weightTrainingData.previousValue;
				double weightChange = this.learningRate * neuronError * input + momentum * (weight.value - previousWeightValue);
				// save previous weight value
				//weight.getTrainingData().set(TrainingData.PREVIOUS_WEIGHT, currentWeightValue);
				weightTrainingData.previousValue = weight.value;


				// if the learning is in batch mode apply the weight change immediately
				if (this.InBatchMode == false)
				{
					weight.weightChange = weightChange;
					weight.value += weightChange;
				} // otherwise, sum the weight changes and apply them after at the end of epoch
				else
				{
					weight.weightChange += weightChange;
				}
			}
		}

		/// <summary>
		/// Returns the momentum factor
		/// </summary>
		/// <returns> momentum factor </returns>
		public virtual double Momentum
		{
			get
			{
				return momentum;
			}
			set
			{
				this.momentum = value;
			}
		}


		public class MomentumWeightTrainingData
		{

			public double previousValue;
		}

		protected internal override void onStart()
		{
			base.onStart();
			// create MomentumWeightTrainingData objects that will be used during the training to store previous weight value
			foreach (Layer layer in neuralNetwork.Layers)
			{
				foreach (Neuron neuron in layer.Neurons)
				{
					foreach (Connection connection in neuron.InputConnections)
					{
						connection.Weight.TrainingData = new MomentumWeightTrainingData();
					}
				} // for
			} // for
		}
	}
}