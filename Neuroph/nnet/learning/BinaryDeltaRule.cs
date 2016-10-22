using System;
using System.Collections.Generic;

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

	using Neuron = org.neuroph.core.Neuron;
	using ThresholdNeuron = org.neuroph.nnet.comp.neuron.ThresholdNeuron;

	/// <summary>
	/// Delta rule learning algorithm for perceptrons with step functions.
	/// 
	/// The difference to Perceptronlearning is that Delta Rule calculates error
	/// before the non-lnear step transfer function
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class BinaryDeltaRule : PerceptronLearning
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// The errorCorrection parametar of this learning algorithm
		/// </summary>
		private double errorCorrection = 0.1;

		/// <summary>
		/// Creates new BinaryDeltaRule learning
		/// </summary>
		public BinaryDeltaRule() : base()
		{
		}


		/// <summary>
		/// This method implements weight update procedure for the whole network for
		/// this learning rule
		/// </summary>
		/// <param name="patternError">
		///            single pattern error vector
		///        
		/// if the output is 0 and required value is 1, increase rthe weights
		/// if the output is 1 and required value is 0, decrease the weights
		/// otherwice leave weights unchanged
		///         </param>
		protected internal override void updateNetworkWeights(double[] patternError)
		{
			int i = 0;
					List<Neuron> outputNeurons = neuralNetwork.OutputNeurons;
					foreach (Neuron outputNeuron in outputNeurons)
					{
				ThresholdNeuron neuron = (ThresholdNeuron)outputNeuron;
				double outErr = patternError[i];
				double thresh = neuron.Thresh;
				double netInput = neuron.NetInput;
				double threshError = thresh - netInput; // distance from zero
							// use output error to decide weathet to inrease, decrase or leave unchanged weights
							// add errorCorrection to threshError to move above or below zero
							double neuronError = outErr * (Math.Abs(threshError) + errorCorrection);

							// use same adjustment principle as PerceptronLearning,
							// just with different neuronError
							neuron.Error = neuronError;
				updateNeuronWeights(neuron);

				i++;
					} // for
		}

			/// <summary>
			/// Gets the errorCorrection parametar
			/// </summary>
			/// <returns> errorCorrection parametar </returns>
		public virtual double ErrorCorrection
		{
			get
			{
				return this.errorCorrection;
			}
			set
			{
				this.errorCorrection = value;
			}
		}


	}
}