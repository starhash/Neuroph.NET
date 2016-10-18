namespace org.neuroph.contrib.model.modelselection
{

	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;

	/// <summary>
	/// Contract specification which all concrete optimizers should implement
	/// </summary>
	public interface NeurophModelOptimizer
	{

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: org.neuroph.core.NeuralNetwork createOptimalModel(final org.neuroph.core.data.DataSet trainSet);
		NeuralNetwork createOptimalModel(DataSet trainSet);

	}

}