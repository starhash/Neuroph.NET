namespace org.neuroph.contrib.eval.classification
{

	/// <summary>
	/// Ovu klasu definitivno izabciti
	/// KOristi se samo getMaxOutput koja uz to i potpuno nebulozna jer vraca  ClassificationResult
	/// vidi samo zasta nam treba u McNemar
	/// 
	/// a pri tom uvek se poziva getActual
	/// 
	/// Wrapper class used for ordering classification results
	/// </summary>
	public class ClassificationResult
	{

		private int classIdx; // order of the actual class (output neuron order)
		private double neuronOutput; // coresponding neuron output number
		private string label; // class label: should be obtained with neuron.getLabel()

		// TODO: add neuron label to constructor 

		public ClassificationResult(int classIdx, double neuronOutput)
		{
			this.classIdx = classIdx;
			this.neuronOutput = neuronOutput;
		}

		public virtual int ClassIdx
		{
			get
			{
				return classIdx;
			}
		}

		public virtual double NeuronOutput
		{
			get
			{
				return neuronOutput;
			}
		}

		public virtual string Label
		{
			get
			{
				return label;
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj == this)
			{
				return true;
			}
			if (!(obj is ClassificationResult))
			{
				return false;
			}
			ClassificationResult that = (ClassificationResult) obj;
			return this.ClassIdx == that.ClassIdx;
		}


		public override string ToString()
		{
			string @object = "ID: " + ClassIdx + ", Output: " + NeuronOutput;
			return @object;
		}



	//    /**
	//     *
	//     * @param results classification results computed by NeuralNetwork @see NeuralNetwork.getOutput
	//     * @return priority queue ordered by total results of each class in classifier
	//     */
	//    public static PriorityQueue<ClassificationResult> orderedOutput(final double[] results) {
	//        PriorityQueue<ClassificationResult> classificationOutput = new PriorityQueue<>();
	//
	//        for (int i = 0; i < results.length; i++) {
	//            classificationOutput.add(new ClassificationResult(i, results[i]));
	//        }
	//        return classificationOutput;
	//    }
	//
	//    public static ClassificationResult fromMaxOutput(final double[] results) {
	//        return orderedOutput(results).peek();
	//    }


	}
}