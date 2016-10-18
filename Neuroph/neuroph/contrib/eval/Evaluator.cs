namespace org.neuroph.contrib.eval
{

	/// <summary>
	/// Interface for all Evaluators
	/// </summary>
	/// @param <T> Generic type used to define the return type of final evaluation result </param>
	public interface IEvaluator
	{

		/// <summary>
		/// This method should handle processing of a single network output within an evaluation procedure
		/// </summary>
		/// <param name="networkOutput"> </param>
		/// <param name="desiredOutput">  </param>
		void processNetworkResult(double[] networkOutput, double[] desiredOutput);

		/// <summary>
		/// This method should return final evaluation result </summary>
		/// <returns>  </returns>



		void reset();

	}

    public abstract class Evaluator<T> : IEvaluator {
        T Result { get; }

        public abstract void processNetworkResult(double[] networkOutput, double[] desiredOutput);
        public abstract void reset();
    }
}