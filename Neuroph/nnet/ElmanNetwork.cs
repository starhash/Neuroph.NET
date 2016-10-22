namespace org.neuroph.nnet
{

	using Layer = org.neuroph.core.Layer;
	using org.neuroph.core;
	using InputLayer = org.neuroph.nnet.comp.layer.InputLayer;
	using BiasNeuron = org.neuroph.nnet.comp.neuron.BiasNeuron;
	using BackPropagation = org.neuroph.nnet.learning.BackPropagation;
	using ConnectionFactory = org.neuroph.util.ConnectionFactory;
	using NeuralNetworkFactory = org.neuroph.util.NeuralNetworkFactory;
	using NeuronProperties = org.neuroph.util.NeuronProperties;
	using TransferFunctionType = org.neuroph.util.TransferFunctionType;

	/// <summary>
	/// Under development: Learning rule BackProp Through Time required
	/// @author zoran
	/// </summary>
	public class ElmanNetwork : NeuralNetwork
	{

		public ElmanNetwork(int inputNeuronsCount, int hiddenNeuronsCount, int contextNeuronsCount, int outputNeuronsCount)
		{
			createNetwork(inputNeuronsCount, hiddenNeuronsCount, contextNeuronsCount, outputNeuronsCount);
		}
		// three layers: input, hidden, output
		// as mlp add context layer
		// elman connect output of hidden layer to input of context layer
		// output of context to input of hidden layer 





		private void createNetwork(int inputNeuronsCount, int hiddenNeuronsCount, int contextNeuronsCount, int outputNeuronsCount)
		{

					// create input layer
					InputLayer inputLayer = new InputLayer(inputNeuronsCount);
					inputLayer.addNeuron(new BiasNeuron());
					addLayer(inputLayer);

			NeuronProperties neuronProperties = new NeuronProperties();
				   // neuronProperties.setProperty("useBias", true);
			neuronProperties.setProperty("transferFunction", TransferFunctionType.Sigmoid.ToString());

					Layer hiddenLayer = new Layer(hiddenNeuronsCount, neuronProperties);
					hiddenLayer.addNeuron(new BiasNeuron());
					addLayer(hiddenLayer);

					ConnectionFactory.fullConnect(inputLayer, hiddenLayer);

					Layer contextLayer = new Layer(contextNeuronsCount, neuronProperties);
					addLayer(contextLayer); // we might also need bias for context neurons?

					Layer outputLayer = new Layer(outputNeuronsCount, neuronProperties);
					addLayer(outputLayer);

					ConnectionFactory.fullConnect(hiddenLayer, outputLayer);

					ConnectionFactory.forwardConnect(hiddenLayer, contextLayer); // forward or full connectivity?
					ConnectionFactory.fullConnect(contextLayer, hiddenLayer);


			// set input and output cells for network
					  NeuralNetworkFactory.DefaultIO = this;

					  // set learnng rule
			this.LearningRule = new BackPropagation();

		}

	}

}