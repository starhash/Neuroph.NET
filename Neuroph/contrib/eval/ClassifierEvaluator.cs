namespace org.neuroph.contrib.eval
{
    using System;
    using ConfusionMatrix = org.neuroph.contrib.eval.classification.ConfusionMatrix;
    using Utils = org.neuroph.contrib.eval.classification.Utils;

    public abstract class ClassifierEvaluator : Evaluator<ConfusionMatrix>
	{

		private string[] classLabels;
		internal ConfusionMatrix confusionMatrix;
		private double threshold; // not used at the moment


		private ClassifierEvaluator(string[] labels)
		{
			this.classLabels = labels;
			confusionMatrix = new ConfusionMatrix(labels);
		}

		public ConfusionMatrix Result
		{
			get
			{
				return confusionMatrix;
			}
		}


		public virtual double Threshold
		{
			get
			{
				return threshold;
			}
			set
			{
				this.threshold = value;
			}
		}

        public override void reset() {
            confusionMatrix = new ConfusionMatrix(classLabels);
        }

        public override void processNetworkResult(double[] networkOutput, double[] desiredOutput) {
            
        }



        //    public static ClassificationEvaluator createForDataSet(final DataSet dataSet) {
        //        if (dataSet.getOutputSize() == 1) {
        //            //TODO how can we handle different thresholds??? - use thresholds for both binary and multiclass
        //            return new Binary(0.5);
        //        } else {
        //            return new MultiClass(dataSet);
        //        }
        //    }


        /// <summary>
        /// Binary evaluator used for computation of metrics in case when data has only one output result (one output neuron)
        /// </summary>
        public class Binary : ClassifierEvaluator
		{

			public static readonly string[] BINARY_CLASS_LABELS = new string[]{"False", "True"};
			public const int TRUE = 1;
			public const int FALSE = 0;



			public Binary(double threshold) : base(BINARY_CLASS_LABELS)
			{
				Threshold = threshold;

			}

			public override void processNetworkResult(double[] networkOutput, double[] desiredOutput)
			{
				int actualClass = classForValueOf(desiredOutput[0]);
				int predictedClass = classForValueOf(networkOutput[0]);

				confusionMatrix.incrementElement(actualClass, predictedClass);
			}

			internal virtual int classForValueOf(double classResult)
			{
				int classValue = FALSE;
				if (classResult >= Threshold)
				{
					classValue = TRUE;
				}
				return classValue;
			}

		}

		/// <summary>
		/// Evaluator used for computation of metrics in case when data has
		/// multiple classes - one vs many classification
		/// </summary>
		public class MultiClass : ClassifierEvaluator
		{

			// TODO: use column labels here
			public MultiClass(string[] classLabels) : base(classLabels)
			{
				// dataSet.getColumnNames()
			}

			public override void processNetworkResult(double[] predictedOutput, double[] actualOutput)
			{
				// just get max index
				int actualClassIdx = Utils.maxIdx(actualOutput);
				int predictedClassIdx = Utils.maxIdx(predictedOutput);

				confusionMatrix.incrementElement(actualClassIdx, predictedClassIdx);
			}
		}

	}

}