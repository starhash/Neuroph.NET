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
	using Neuron = org.neuroph.core.Neuron;
	using DataSetRow = org.neuroph.core.data.DataSetRow;

	/// <summary>
	/// Supervised hebbian learning rule.
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class SupervisedHebbianLearning : LMS
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Creates new instance of SupervisedHebbianLearning algorithm
		/// </summary>
		public SupervisedHebbianLearning() : base()
		{
		}


		/// <summary>
		/// Learn method override without network error and iteration limit
		/// Implements just one pass through the training set Used for testing -
		/// debugging algorithm
		/// 
		/// public void learn(TrainingSet trainingSet) { Iterator
		/// iterator=trainingSet.iterator(); while(iterator.hasNext()) {
		/// SupervisedTrainingElement trainingElement =
		/// (SupervisedTrainingElement)iterator.next();
		/// this.learnPattern(trainingElement); } }
		/// </summary>


		//TODO --- Check if this breaks something...??????
	//	/**
	//	 * Trains network with the pattern from the specified training element
	//	 *
	//	 * @param trainingSetRow
	//	 *            a single  data set row to learn which contains input and desired output patterns (arrays)
	//	 */
	//	@Override
	//	protected void learnPattern(DataSetRow trainingSetRow) {
	//                double[] input = trainingSetRow.getInput();
	//                this.neuralNetwork.setInput(input); // set network input
	//                this.neuralNetwork.calculate(); // calculate the network
	//                double[] output = this.neuralNetwork.getOutput(); // get actual network output
	//                double[] desiredOutput = trainingSetRow.getDesiredOutput();
	//
	//                double[] outputError = this.calculateOutputError(desiredOutput, output); // calculate error as difference between desired and actual
	//                this.addToSquaredErrorSum(outputError); // add error to total MSE
	//
	//                this.updateNetworkWeights(desiredOutput); // apply the weights update procedure
	//	}

		/// <summary>
		/// This method implements weight update procedure for the whole network for
		/// this learning rule
		/// </summary>
		/// <param name="desiredOutput">
		///            desired network output </param>
		protected internal override void updateNetworkWeights(double[] desiredOutput)
		{
			int i = 0;
			foreach (Neuron neuron in neuralNetwork.OutputNeurons)
			{
				this.updateNeuronWeights(neuron, desiredOutput[i]);
				i++;
			}

		}

		/// <summary>
		/// This method implements weights update procedure for the single neuron
		/// </summary>
		/// <param name="neuron">
		///            neuron to update weights
		///        desiredOutput
		///	      desired output of the neuron </param>
		protected internal virtual void updateNeuronWeights(Neuron neuron, double desiredOutput)
		{
			foreach (Connection connection in neuron.InputConnections)
			{
				double input = connection.Input;
				double deltaWeight = input * desiredOutput * this.learningRate;
				connection.Weight.inc(deltaWeight);
			}
		}
	}

}