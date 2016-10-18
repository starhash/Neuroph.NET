using System.Collections.Generic;

namespace org.neuroph.contrib.model.modelselection
{

    using Bootstrapping = org.neuroph.contrib.model.errorestimation.Bootstrapping;
    using CrossValidation = org.neuroph.contrib.model.errorestimation.CrossValidation;
    using ClassificationMetrics = org.neuroph.contrib.eval.classification.ClassificationMetrics;
    using org.neuroph.core;
    using DataSet = org.neuroph.core.data.DataSet;
    using LearningEvent = org.neuroph.core.events.LearningEvent;
    using LearningEventListener = org.neuroph.core.events.LearningEventListener;
    using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;
    using BackPropagation = org.neuroph.nnet.learning.BackPropagation;
    using Logger = org.slf4j.Logger;
    using LoggerFactory = org.slf4j.LoggerFactory;

    using ClassifierEvaluator = org.neuroph.contrib.eval.ClassifierEvaluator;
    using modelselection;

    /// @param <T> Type which defined which LearningRule will be used during model optimization </param>
    public class MultilayerPerceptronOptimazer<T> : NeurophModelOptimizer where T : org.neuroph.nnet.learning.BackPropagation
	{

		private static Logger LOG = LoggerFactory.getLogger(typeof(MultilayerPerceptronOptimazer<T>));

		/// 
		private HashSet<List<int>> allArchitectures = new HashSet<List<int>>();

		private List<int> optimalArchitecure;
		/// <summary>
		/// Optimal optimizer which will be selected during optimization process
		/// </summary>
		private NeuralNetwork optimalClassifier;
		/// <summary>
		/// Average metric scores for selected optimal classififer
		/// </summary>
		private ClassificationMetrics optimalResult;
		/// <summary>
		/// Method used for classifier error estimation (KFold, Bootstrap)
		/// </summary>
		private CrossValidation errorEstimationMethod;
		/// <summary>
		/// Learning rule used during classifier learning stage
		/// </summary>
		private BackPropagation learningRule;

		private int maxLayers = 1;
		private int minNeuronsPerLayer = 1;
		private int maxNeuronsPerLayer = 30;
		private int neuronIncrement = 1;

		/// <summary>
		/// If ErrorEstimationMethod is not provided use KFoldCrossValidation by default
		/// </summary>
		public MultilayerPerceptronOptimazer()
		{
		   // errorEstimationMethod = new KFoldCrossValidation(10);
		}

		public virtual MultilayerPerceptronOptimazer<T> withMaxLayers(int maxLayers)
		{
			this.maxLayers = maxLayers;
			return this;
		}

		public virtual MultilayerPerceptronOptimazer<T> withNeuronIncrement(int neuronIncrement)
		{
			this.neuronIncrement = neuronIncrement;
			return this;
		}

		public virtual MultilayerPerceptronOptimazer<T> withMaxNeurons(int maxNeurons)
		{
			this.maxNeuronsPerLayer = maxNeurons;
			return this;
		}

		public virtual MultilayerPerceptronOptimazer<T> withMinNeurons(int minNeurons)
		{
			this.minNeuronsPerLayer = minNeurons;
			return this;
		}


		public virtual MultilayerPerceptronOptimazer<T> withErrorEstimationMethod(CrossValidation errorEstimationMethod)
		{
			this.errorEstimationMethod = errorEstimationMethod;
			return this;
		}

		public virtual MultilayerPerceptronOptimazer<T> withLearningRule(BackPropagation learningRule)
		{
			this.learningRule = learningRule;
			return this;
		}


		/// <param name="dataSet"> training set used for error estimation </param>
		/// <returns> neural network model with optimized architecture for provided data set </returns>
		public virtual NeuralNetwork createOptimalModel(DataSet dataSet)
		{

			List<int> neurons = new List<int>();
			neurons.Add(minNeuronsPerLayer);
			findArchitectures(1, minNeuronsPerLayer, neurons);

			LOG.info("Total [{}] different network topologies found", allArchitectures.Count);

			foreach (List<int> architecture in allArchitectures)
			{
				architecture.Insert(0, dataSet.InputSize);
				architecture.Add(dataSet.OutputSize);

				LOG.info("Architecture: [{}]", architecture);

				MultiLayerPerceptron network = new MultiLayerPerceptron(architecture);
				LearningListener listener = new LearningListener(10, learningRule.MaxIterations);
				learningRule.addListener(listener);
				network.LearningRule = learningRule;

				errorEstimationMethod = new CrossValidation(network, dataSet, 10);
				errorEstimationMethod.run();
                // FIX
                var evaluator = errorEstimationMethod.getEvaluator<ClassifierEvaluator.MultiClass>(typeof(ClassifierEvaluator.MultiClass));

                ClassificationMetrics[] result = ClassificationMetrics.createFromMatrix(evaluator.Result);

				// nadji onaj sa najmanjim f measure
				if (optimalResult == null || optimalResult.FMeasure < result[0].FMeasure)
				{
					LOG.info("Architecture [{}] became optimal architecture  with metrics {}", architecture, result);
					optimalResult = result[0];
					optimalClassifier = network;
					optimalArchitecure = architecture;
				}

				LOG.info("#################################################################");
			}


			LOG.info("Optimal Architecture: {}", optimalArchitecure);
			return optimalClassifier;
		}

		private void findArchitectures(int currentLayer, int lastLayerNeuronCount, List<int> nerons)
		{
			allArchitectures.Add(new List<int>(nerons));

			if (lastLayerNeuronCount + neuronIncrement <= maxNeuronsPerLayer)
			{
				int indexOfLastElement = nerons.Count - 1;
				List<int> newList = new List<int>(nerons);
				newList[indexOfLastElement] = lastLayerNeuronCount + neuronIncrement;
				findArchitectures(currentLayer, lastLayerNeuronCount + neuronIncrement, newList);
			}
			if (currentLayer + 1 <= maxLayers)
			{
				List<int> newList = new List<int>(nerons);
				newList.Add(1);
				findArchitectures(currentLayer + 1, minNeuronsPerLayer, newList);
			}
		}


		internal class LearningListener : LearningEventListener
		{

			internal double[] foldErrors;
			internal int foldSize;

			public LearningListener(int foldSize, int maxIterations)
			{

				this.foldSize = foldSize;
				this.foldErrors = new double[maxIterations];
			}

			public virtual void handleLearningEvent(LearningEvent @event)
			{
				BackPropagation bp = (BackPropagation) @event.getSource();
				foldErrors[bp.CurrentIteration - 1] += bp.TotalNetworkError / foldSize;
			}
		}

	}

}