using System;

namespace org.neuroph.contrib.learning
{


	using ErrorFunction = org.neuroph.core.learning.error.ErrorFunction;

	/// <summary>
	/// Special error function which is recommended to be used in classification models
	/// </summary>
	[Serializable]
	public class CrossEntropyError : ErrorFunction
	{

		private double[] errorDerivative;
		[NonSerialized]
		private double totalError;
		[NonSerialized]
		private double n;

		public virtual double TotalError
		{
			get
			{
				return -totalError / n;
			}
		}

		public virtual void reset()
		{
			totalError = 0;
			n = 0;
		}

		public virtual double[] calculatePatternError(double[] predictedOutput, double[] targetOutput)
		{
			double[] error = new double[targetOutput.Length];

			if (predictedOutput.Length != targetOutput.Length)
			{
				throw new System.ArgumentException("Output array length and desired output array length must be the same size!");
			}

			for (int i = 0; i < predictedOutput.Length; i++)
			{
				errorDerivative[i] = targetOutput[i] - predictedOutput[i];
				totalError += targetOutput[i] * Math.Log(predictedOutput[i]);

			}
			n++;

			return error;
		}

	}

}