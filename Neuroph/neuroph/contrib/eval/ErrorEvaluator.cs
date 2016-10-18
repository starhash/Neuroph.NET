namespace org.neuroph.contrib.eval
{

	using ErrorFunction = org.neuroph.core.learning.error.ErrorFunction;

	/// <summary>
	/// Calculates scalar result using ErrorFunction
	/// </summary>
	public class ErrorEvaluator : Evaluator<double>
	{

		private ErrorFunction errorFunction;

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public ErrorEvaluator(final org.neuroph.core.learning.error.ErrorFunction errorFunction)
		public ErrorEvaluator(ErrorFunction errorFunction)
		{
			this.errorFunction = errorFunction;
		}

		public override void processNetworkResult(double[] networkOutput, double[] desiredOutput)
		{
			errorFunction.calculatePatternError(networkOutput, desiredOutput);
		}

		public double Result
		{
			get
			{
				return errorFunction.TotalError;
			}
		}

		public override void reset()
		{
			errorFunction.reset();
		}
	}


}