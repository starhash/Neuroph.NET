namespace org.neuroph.nnet
{

	/// <summary>
	/// Auto Encoder Neural Network
	/// </summary>
	public class AutoencoderNetwork : MultiLayerPerceptron
	{

		public AutoencoderNetwork(int inputsCount, int hiddenCount) : base(inputsCount, hiddenCount, inputsCount)
		{
		}

	}

}