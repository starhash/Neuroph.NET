namespace org.neuroph.samples.evaluation
{

	using Evaluation = org.neuroph.contrib.eval.Evaluation;
	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;
	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;
	using TransferFunctionType = org.neuroph.util.TransferFunctionType;

	/// <summary>
	/// Simple example which shows how to use EvaluationService on Binary classification problem (XOR problem)
	/// </summary>
	public class TestBinaryClass
	{

		public static void Main(string[] args)
		{

			DataSet trainingSet = new DataSet(2, 1);
			trainingSet.addRow(new DataSetRow(new double[]{0, 0}, new double[]{0}));
			trainingSet.addRow(new DataSetRow(new double[]{0, 1}, new double[]{1}));
			trainingSet.addRow(new DataSetRow(new double[]{1, 0}, new double[]{1}));
			trainingSet.addRow(new DataSetRow(new double[]{1, 1}, new double[]{0}));

			MultiLayerPerceptron neuralNet = new MultiLayerPerceptron(TransferFunctionType.TANH, 2, 3, 1);
			neuralNet.learn(trainingSet);

			Evaluation.runFullEvaluation(neuralNet, trainingSet);
		}

	}

}