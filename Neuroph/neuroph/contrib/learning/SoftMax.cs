using System;

namespace org.neuroph.contrib.learning
{


	using Layer = org.neuroph.core.Layer;
	using Neuron = org.neuroph.core.Neuron;
	using TransferFunction = org.neuroph.core.transfer.TransferFunction;

	/// <summary>
	/// Activation function which enforces that output neurons have probability distribution (sum of all outputs is one)
	/// </summary>
	public class SoftMax : TransferFunction
	{

		private Layer layer;
		private double totalLayerInput;

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public SoftMax(final org.neuroph.core.Layer layer)
		public SoftMax(Layer layer)
		{
			this.layer = layer;
		}


		public override double getOutput(double netInput)
		{
			totalLayerInput = 0;
			foreach (Neuron neuron in layer.Neurons)
			{
				totalLayerInput += Math.Exp(neuron.NetInput);
			}
			output = Math.Exp(netInput) / totalLayerInput;
			return output;
		}

		public override double getDerivative(double net)
		{
			return 1d * (1d - output);
		}
	}

}