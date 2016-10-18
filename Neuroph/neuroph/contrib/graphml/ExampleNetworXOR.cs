namespace org.neuroph.contrib.graphml
{

	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;
	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;
	using TransferFunctionType = org.neuroph.util.TransferFunctionType;
	/// <summary>
	/// Approximate XOR gate using a multilayer percepton with 2 inputs, 3 hidden neurons and 1 output. 
	/// 
	/// The example is adapted from http://neuroph.sourceforge.net/tutorials/MultiLayerPerceptron.html
	/// 
	/// @author fernando carrillo fernando@carrillo.at 
	/// 
	/// </summary>
	public class ExampleNetworXOR
	{
		/// <summary>
		/// Training data with two input neurons. 
		/// Truth: 
		/// 0,0: 0
		/// 0,1: 1
		/// 1,0: 1 
		/// 1,1: 0 
		/// </summary>
		public static DataSet TrainingData
		{
			get
			{
				DataSet data = new DataSet(2, 1);
    
				data.addRow(new DataSetRow(new double[]{0, 0}, new double[]{0}));
				data.addRow(new DataSetRow(new double[]{0, 1}, new double[]{1}));
				data.addRow(new DataSetRow(new double[]{1, 0}, new double[]{1}));
				data.addRow(new DataSetRow(new double[]{1, 1}, new double[]{0}));
    
				return data;
			}
		}

		/// <summary>
		/// Returns multilayer percepton learned to approximate the XOR gate. 
		/// @return
		/// </summary>
		public static MultiLayerPerceptron Network
		{
			get
			{
    
				MultiLayerPerceptron mlPerceptron = new MultiLayerPerceptron(TransferFunctionType.TANH, 2, 3, 1);
    
    
				mlPerceptron.learn(TrainingData);
    
				return mlPerceptron;
			}
		}
	}

}