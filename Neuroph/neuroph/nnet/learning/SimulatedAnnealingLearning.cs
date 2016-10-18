using System;
using System.Collections.Generic;

namespace org.neuroph.nnet.learning
{

	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;
	using SupervisedLearning = org.neuroph.core.learning.SupervisedLearning;
	using NeuralNetworkCODEC = org.neuroph.util.NeuralNetworkCODEC;

	/// <summary>
	/// This class implements a simulated annealing learning rule for supervised
	/// neural networks. It is based on the generic SimulatedAnnealing class. It is
	/// used in the same manner as any other training class that implements the
	/// SupervisedLearning interface.
	/// <p/>
	/// Simulated annealing is a common training method. It is often used in
	/// conjunction with a propagation training method. Simulated annealing can be
	/// very good when propagation training has reached a local minimum.
	/// <p/>
	/// The name and inspiration come from annealing in metallurgy, a technique
	/// involving heating and controlled cooling of a material to increase the size
	/// of its crystals and reduce their defects. The heat causes the atoms to become
	/// unstuck from their initial positions (a local minimum of the internal energy)
	/// and wander randomly through states of higher energy; the slow cooling gives
	/// them more chances of finding configurations with lower internal energy than
	/// the initial one.
	/// 
	/// @author Jeff Heaton (http://www.jeffheaton.com)
	/// </summary>
	public class SimulatedAnnealingLearning : SupervisedLearning
	{

		/// <summary>
		/// The serial id.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// The starting temperature.
		/// </summary>
		private double startTemperature;

		/// <summary>
		/// The ending temperature.
		/// </summary>
		private double stopTemperature;

		/// <summary>
		/// The number of cycles that will be used.
		/// </summary>
		private int cycles;

		/// <summary>
		/// The current temperature.
		/// </summary>
		protected internal double temperature;

		/// <summary>
		/// Current weights from the neural network.
		/// </summary>
		private double[] weights;

		/// <summary>
		/// Best weights so far.
		/// </summary>
		private double[] bestWeights;

		/// <summary>
		/// Construct a simulated annleaing trainer for a feedforward neural network.
		/// </summary>
		/// <param name="network">   The neural network to be trained. </param>
		/// <param name="startTemp"> The starting temperature. </param>
		/// <param name="stopTemp">  The ending temperature. </param>
		/// <param name="cycles">    The number of cycles in a training iteration. </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public SimulatedAnnealingLearning(final org.neuroph.core.NeuralNetwork network, final double startTemp, final double stopTemp, final int cycles)
		public SimulatedAnnealingLearning(NeuralNetwork network, double startTemp, double stopTemp, int cycles)
		{
			NeuralNetwork = network;
			this.temperature = startTemp;
			this.startTemperature = startTemp;
			this.stopTemperature = stopTemp;
			this.cycles = cycles;

			this.weights = new double[NeuralNetworkCODEC.determineArraySize(network)];
			this.bestWeights = new double[NeuralNetworkCODEC.determineArraySize(network)];

			NeuralNetworkCODEC.network2array(network, this.weights);
			NeuralNetworkCODEC.network2array(network, this.bestWeights);
		}

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public SimulatedAnnealingLearning(final org.neuroph.core.NeuralNetwork network)
		public SimulatedAnnealingLearning(NeuralNetwork network) : this(network, 10, 2, 1000)
		{
		}

		/// <summary>
		/// Get the best network from the training.
		/// </summary>
		/// <returns> The best network. </returns>
		public virtual NeuralNetwork Network
		{
			get
			{
				return NeuralNetwork;
			}
		}

		/// <summary>
		/// Randomize the weights and thresholds. This function does most of the work
		/// of the class. Each call to this class will randomize the data according
		/// to the current temperature. The higher the temperature the more
		/// randomness. </summary>
		/// <param name="randomChance">  </param>
		public virtual void randomize(double randomChance)
		{

			for (int i = 0; i < this.weights.Length; i++)
			{
			  if (new Random(1).NextDouble() < randomChance)
			  {
				double add = 0.5 - (new Random(2).NextDouble());
				add /= this.startTemperature;
				add *= this.temperature;
				this.weights[i] = this.weights[i] + add;
			  }
			}

			NeuralNetworkCODEC.array2network(this.weights, Network);
		}

		/// <summary>
		/// Used internally to calculate the error for a training set.
		/// </summary>
		/// <param name="trainingSet"> The training set to calculate for. </param>
		/// <returns> The error value. </returns>
		private double determineError(DataSet trainingSet)
		{
			double result = 0d;

			IEnumerator<DataSetRow> iterator = trainingSet.iterator();
			while (iterator.MoveNext() && !Stopped)
			{
				DataSetRow trainingSetRow = iterator.Current;
				double[] input = trainingSetRow.Input;
				Network.Input = input;
				Network.calculate();
				double[] output = Network.Output;
				double[] desiredOutput = trainingSetRow.DesiredOutput;

				double[] patternError = ErrorFunction.calculatePatternError(desiredOutput, output);
				double sqrErrorSum = 0;
				foreach (double error in patternError)
				{
					sqrErrorSum += (error * error);
				}
				result += sqrErrorSum / (2 * patternError.Length);

			}

			return result;
		}

		/// <summary>
		/// Perform one simulated annealing epoch.
		/// </summary>
		public override void doLearningEpoch(DataSet trainingSet)
		{
		  doLearningEpoch(trainingSet, 0.5);
		}

		public virtual void doLearningEpoch(DataSet trainingSet, double randomChance)
		{
			Array.Copy(this.weights, 0, this.bestWeights, 0, this.weights.Length);

			double bestError = determineError(trainingSet);

			this.temperature = this.startTemperature;

			for (int i = 0; i < this.cycles; i++)
			{

				randomize(randomChance);
				double currentError = determineError(trainingSet);

				if (currentError < bestError)
				{
					Array.Copy(this.weights, 0, this.bestWeights, 0, this.weights.Length);
					bestError = currentError;
				}
				else
				{
					Array.Copy(this.bestWeights, 0, this.weights, 0, this.weights.Length);
				}

				NeuralNetworkCODEC.array2network(this.bestWeights, Network);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ratio = Math.exp(Math.log(this.stopTemperature / this.startTemperature) / (this.cycles - 1));
				double ratio = Math.Exp(Math.Log(this.stopTemperature / this.startTemperature) / (this.cycles - 1));
				this.temperature *= ratio;
			}

			// the following line is probably wrong (when is reset() called?), but the result might not be used for anything
			this.previousEpochError = ErrorFunction.TotalError;

			// moved stopping condition to separate method hasReachedStopCondition()
			// so it can be overriden / customized in subclasses
			if (hasReachedStopCondition())
			{
				stopLearning();
			}
		}


		/// <summary>
		/// Not used.
		/// </summary>
		protected internal override void updateNetworkWeights(double[] patternError)
		{

		}

	}

}