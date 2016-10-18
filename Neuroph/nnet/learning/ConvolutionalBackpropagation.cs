using System.Collections.Generic;

namespace org.neuroph.nnet.learning
{

	using Connection = org.neuroph.core.Connection;
	using ConvolutionalLayer = org.neuroph.nnet.comp.layer.ConvolutionalLayer;
	using FeatureMapLayer = org.neuroph.nnet.comp.layer.FeatureMapLayer;
	using Layer = org.neuroph.core.Layer;
	using Neuron = org.neuroph.core.Neuron;
	using TransferFunction = org.neuroph.core.transfer.TransferFunction;

	public class ConvolutionalBackpropagation : MomentumBackpropagation
	{

		private const long serialVersionUID = -7134947805154423695L;

			protected internal override void calculateErrorAndUpdateHiddenNeurons()
			{
			List<Layer> layers = neuralNetwork.Layers;
			for (int layerIdx = layers.Count - 2; layerIdx > 0; layerIdx--)
			{
				foreach (Neuron neuron in layers[layerIdx].Neurons)
				{
					double neuronError = this.calculateHiddenNeuronError(neuron);
					neuron.Error = neuronError;
					if (layers[layerIdx] is ConvolutionalLayer) // if it is convolutional layer c=adapt weughts, dont touch pooling. Pooling just propagate the error
					{
						this.updateNeuronWeights(neuron);
					}
				} // for
			} // for
			}


			// ova mora da se overriduje jer glavna uzima izvod //  ali ova treba samo za pooling sloj 
		protected internal override double calculateHiddenNeuronError(Neuron neuron)
		{

			// for convolutional layers use standard backprop formula
			if (neuron.ParentLayer is ConvolutionalLayer)
			{
				return base.calculateHiddenNeuronError(neuron);
			}

			// for pooling layer just transfer error without using tranfer function derivative
			double deltaSum = 0d;
			foreach (Connection connection in neuron.OutConnections)
			{
				double delta = connection.ToNeuron.Error * connection.Weight.value;
				deltaSum += delta; // weighted delta sum from the next layer
			} // for

		   return deltaSum;
		}

	//	@Override
	//	protected double calculateHiddenNeuronError(Neuron neuron) {
	//		double totalError = super.calculateHiddenNeuronError(neuron);
	//
	//        if (neuron.getParentLayer() instanceof  Layer2D) {
	//            Layer2D parentLayer = (Layer2D) neuron.getParentLayer();
	//            double weight = parentLayer.getHeight() * parentLayer.getWidth();
	//            return totalError / weight;
	//        }
	//        return totalError;
	//	}

	}

}