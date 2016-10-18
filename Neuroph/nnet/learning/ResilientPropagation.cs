using System;
using System.Collections.Generic;

namespace org.neuroph.nnet.learning
{

	using Connection = org.neuroph.core.Connection;
	using Layer = org.neuroph.core.Layer;
	using Neuron = org.neuroph.core.Neuron;
	using Weight = org.neuroph.core.Weight;

	/// <summary>
	/// Resilient Propagation learning rule used for Multi Layer Perceptron neural networks.
	/// Its one of the most efficent learning rules for this type of networks, and it does not require 
	/// setting of learning rule parameter.
	/// @author Borislav Markov
	/// @author Zoran Sevarac
	/// </summary>
	public class ResilientPropagation : BackPropagation
	{

		private double decreaseFactor = 0.5;
		private double increaseFactor = 1.2;
		private double initialDelta = 0.1;
		private double maxDelta = 1;
		private double minDelta = 1e-6;
		private const double ZERO_TOLERANCE = 1e-27; // the lowest limit when something is considered to be zero

		public ResilientPropagation() : base()
		{
			base.BatchMode = true;
		}

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: private int sign(final double value)
		private int sign(double value)
		{
			if (Math.Abs(value) < ZERO_TOLERANCE)
			{
				return 0;
			}
			else if (value > 0)
			{
				return 1;
			}
			else
			{
				return -1;
			}
		}

		protected internal override void onStart()
		{
			base.onStart(); // init all stuff from superclasses

			// create ResilientWeightTrainingtData objects that will hold additional data (resilient specific) during the training 
			foreach (Layer layer in this.neuralNetwork.Layers)
			{
				foreach (Neuron neuron in layer.Neurons)
				{
					foreach (Connection connection in neuron.InputConnections)
					{
						connection.Weight.TrainingData = new ResilientWeightTrainingtData(this);
					}
				}
			}
		}

		/// <summary>
		/// Calculate and sum gradients for each neuron's weight, the actual weight update is done in batch mode </summary>
		/// <seealso cref= ResilientPropagation#resillientWeightUpdate(org.neuroph.core.Weight)  </seealso>
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
				// get the current connection's weight
				Weight weight = connection.Weight;
				// ... and get the object that stores reislient training data for that weight
				ResilientWeightTrainingtData weightData = (ResilientWeightTrainingtData) weight.TrainingData;

				// calculate the weight gradient (and sum gradients since learning is done in batch mode)
				weightData.gradient += neuronError * input;
			}
		}

		protected internal override void doBatchWeightsUpdate()
		{
			// iterate layers from output to input
			List<Layer> layers = neuralNetwork.Layers;
			for (int i = neuralNetwork.LayersCount - 1; i > 0; i--)
			{
				// iterate neurons at each layer
				foreach (Neuron neuron in layers[i].Neurons)
				{
					// iterate connections/weights for each neuron
					foreach (Connection connection in neuron.InputConnections)
					{
						// for each connection weight apply following changes
						Weight weight = connection.Weight;
						resillientWeightUpdate(weight);
					}
				}
			}
		}

		/// <summary>
		/// Weight update by done by ResilientPropagation  learning rule
		/// Executed at the end of epoch (in batch mode) </summary>
		/// <param name="weight">  </param>
		protected internal virtual void resillientWeightUpdate(Weight weight)
		{
			// get resilient training data for the current weight
			ResilientWeightTrainingtData weightData = (ResilientWeightTrainingtData) weight.TrainingData;

			// multiply the current and previous gradient, and take the sign. 
			// We want to see if the gradient has changed its sign.            
			int gradientSignChange = sign(weightData.previousGradient * weightData.gradient);

			double weightChange = 0; // weight change to apply (delta weight)
			double delta; //  adaptation factor

			if (gradientSignChange > 0)
			{
				// if the gradient has retained its sign, then we increase delta (adaptation factor) so that it will converge faster
				delta = Math.Min(weightData.previousDelta * increaseFactor, maxDelta);
				//  weightChange = -sign(weightData.gradient) * delta; // if error is increasing (gradient is positive) then subtract delta, if error is decreasing (gradient negative) then add delta
				// note that our gradient has different sign eg. -dE_dw so we omit the minus here
				weightChange = sign(weightData.gradient) * delta;
				weightData.previousDelta = delta;
			}
			else if (gradientSignChange < 0)
			{
				// if gradientSignChange<0, then the sign has changed, and the last weight change was too big                
				delta = Math.Max(weightData.previousDelta * decreaseFactor, minDelta);
				// weightChange = - weightData.previousDelta;// 0;// -delta  - weightData.previousDelta; // ovo je problematicno treba da bude weightChange          
				weightChange = -weightData.previousWeightChange; // if it skipped min in previous step go back
				// avoid double punishment
				weightData.gradient = 0;
				weightData.previousGradient = 0;

				//move values in the past
				weightData.previousDelta = delta;
			}
			else if (gradientSignChange == 0)
			{
				// if gradientSignChange==0 then there is no change to the delta
				delta = weightData.previousDelta;
				//delta = weightData.previousGradient; // note that encog does this
				weightChange = sign(weightData.gradient) * delta;
			}

			weight.value += weightChange;
			weightData.previousWeightChange = weightChange;
			weightData.previousGradient = weightData.gradient; // as in moveNowValuesToPreviousEpochValues
			weightData.gradient = 0;
		}

		public virtual double DecreaseFactor
		{
			get
			{
				return decreaseFactor;
			}
			set
			{
				this.decreaseFactor = value;
			}
		}


		public virtual double IncreaseFactor
		{
			get
			{
				return increaseFactor;
			}
			set
			{
				this.increaseFactor = value;
			}
		}


		public virtual double InitialDelta
		{
			get
			{
				return initialDelta;
			}
			set
			{
				this.initialDelta = value;
			}
		}


		public virtual double MaxDelta
		{
			get
			{
				return maxDelta;
			}
			set
			{
				this.maxDelta = value;
			}
		}


		public virtual double MinDelta
		{
			get
			{
				return minDelta;
			}
			set
			{
				this.minDelta = value;
			}
		}


		public class ResilientWeightTrainingtData
		{
			internal bool InstanceFieldsInitialized = false;

			internal virtual void InitializeInstanceFields()
			{
				previousDelta = outerInstance.initialDelta;
			}

			private readonly ResilientPropagation outerInstance;

			public ResilientWeightTrainingtData(ResilientPropagation outerInstance)
			{
				this.outerInstance = outerInstance;

				if (!InstanceFieldsInitialized)
				{
					InitializeInstanceFields();
					InstanceFieldsInitialized = true;
				}
			}

			public double gradient;
			public double previousGradient;
			public double previousWeightChange;
			public double previousDelta;
		}
	}

}