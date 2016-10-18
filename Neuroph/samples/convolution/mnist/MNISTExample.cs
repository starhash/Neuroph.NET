namespace org.neuroph.samples.convolution.mnist
{

	using DataSet = org.neuroph.core.data.DataSet;
	using LearningEvent = org.neuroph.core.events.LearningEvent;
	using LearningEventListener = org.neuroph.core.events.LearningEventListener;
	using MeanSquaredError = org.neuroph.core.learning.error.MeanSquaredError;
	using ConvolutionalNetwork = org.neuroph.nnet.ConvolutionalNetwork;
	using BackPropagation = org.neuroph.nnet.learning.BackPropagation;
	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	using CrossValidation = org.neuroph.contrib.model.errorestimation.CrossValidation;
	using Layer = org.neuroph.core.Layer;
	using Neuron = org.neuroph.core.Neuron;
	using EuclideanRBF = org.neuroph.core.input.EuclideanRBF;
	using Gaussian = org.neuroph.core.transfer.Gaussian;
	using Linear = org.neuroph.core.transfer.Linear;
	using Sigmoid = org.neuroph.core.transfer.Sigmoid;
	using Dimension2D = org.neuroph.nnet.comp.Dimension2D;
	using ConvolutionalBackpropagation = org.neuroph.nnet.learning.ConvolutionalBackpropagation;
	using NeuronProperties = org.neuroph.util.NeuronProperties;

	/// <summary>
	/// Konvolucioni parametri
	/// <p/>
	/// Globalna arhitektura: Konvolucioni i pooling lejeri - naizmenicno (samo konvolucioni ili naizmenicno konvolccioni pooling)
	/// Za svaki lajer da ima svoj kernel (mogu svi konvolucioni da imaju isti kernel, ili svi pooling isti kernel)
	/// da mogu da zadaju neuron properties (transfer funkciju) za konvolucioni i pooling lejer(input)
	/// Konektovanje lejera? - po defaultu full connect (ostaviti api za custom konekcije)
	/// <p/>
	/// addFeatureMaps...
	/// connectFeatureMaps
	/// <p/>
	/// Helper utility klasa...
	/// <p/>
	/// Osnovni kriterijumi:
	/// 1. Jednostavno kreiranje default neuronske mreze
	/// 2. Laka customizacija i kreiranje custom arhitektura: konvolucionih i pooling lejera i transfer/input funkcija
	/// Napraviti prvo API i ond aprilagodti kod
	/// <p/>
	/// ------------------------
	/// <p/>
	/// promeniti nacin kreiranja i dodavanja feature maps layera
	/// resiti InputMaps Layer, overridovana metoda koja baca unsupported exception ukazuje da nesto nije u redu sa dizajnom
	/// Da li imamo potrebe za klasom kernel  - to je isto kao i dimension?
	/// <p/>
	/// zasto je public abstract void connectMaps apstraktna? (u klasi FeatureMapsLayer)
	/// <p/>
	/// InputMapsLayer konstruktoru superklase prosledjuje null...
	/// <p/>
	/// fullConnectMapLayers
	/// 
	/// 
	/// Same as CNNMNIST just with hardoced params
	/// 
	/// @author zoran
	/// </summary>


	public class MNISTExample
	{

		private static Logger LOG = LoggerFactory.getLogger(typeof(MNISTExample));

		public static void Main(string[] args)
		{
			try
			{

				DataSet trainSet = MNISTDataSet.createFromFile(MNISTDataSet.TRAIN_LABEL_NAME, MNISTDataSet.TRAIN_IMAGE_NAME, 60);
				DataSet testSet = MNISTDataSet.createFromFile(MNISTDataSet.TEST_LABEL_NAME, MNISTDataSet.TEST_IMAGE_NAME, 10);

				ConvolutionalNetwork convolutionNetwork = (new ConvolutionalNetwork.Builder()).withInputLayer(32, 32, 1).withConvolutionLayer(5, 5, 6).withPoolingLayer(2, 2).withConvolutionLayer(5, 5, 16).withPoolingLayer(2, 2).withConvolutionLayer(5, 5, 120).withFullConnectedLayer(84).withFullConnectedLayer(10).build(); // add transfer function and its properties



				// we need Output RBF euclidean layer - implement original LeNet5 - and make sure it works
		   //     + kreiraj RBF Euclidean i dodaj u output layer: EuclideanRBF
			//    - amplitude for tanh - dodaj parametar
			// trenutno konvolucioni sloj koristi RectifiedLinear.class - trebalo bi svi tanh
			// zasto dva puta okida event za learning? loguje dvaput?

				ConvolutionalBackpropagation backPropagation = new ConvolutionalBackpropagation();
				backPropagation.LearningRate = 0.001;
				backPropagation.MaxError = 0.01;
				//backPropagation.setMaxIterations(1000);
				backPropagation.ErrorFunction = new MeanSquaredError();

				convolutionNetwork.LearningRule = backPropagation;
				backPropagation.addListener(new LearningListener());

			 //   System.out.println("Started training...");

				convolutionNetwork.learn(trainSet);

			   // System.out.println("Done training!");

	//            CrossValidation crossValidation = new CrossValidation(convolutionNetwork, trainSet, 6);
	//            crossValidation.run();

	//           ClassificationMetrics validationResult = crossValidation.computeErrorEstimate(convolutionNetwork, trainSet);
			   // Evaluation.runFullEvaluation(convolutionNetwork, testSet);

				convolutionNetwork.save("mnist.nnet");

		//        System.out.println(crossValidation.getResult());

			}
			catch (IOException e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
			}
		}

		internal class LearningListener : LearningEventListener
		{


			internal long start = DateTimeHelperClass.CurrentUnixTimeMillis();

			public virtual void handleLearningEvent(LearningEvent @event)
			{
				BackPropagation bp = (BackPropagation) @event.Source;
				LOG.info("Current iteration: " + bp.CurrentIteration);
				LOG.info("Error: " + bp.TotalNetworkError);
				LOG.info("Calculation time: " + (DateTimeHelperClass.CurrentUnixTimeMillis() - start) / 1000.0);
			 //   neuralNetwork.save(bp.getCurrentIteration() + "CNN_MNIST" + bp.getCurrentIteration() + ".nnet");
				start = DateTimeHelperClass.CurrentUnixTimeMillis();
	//            NeuralNetworkEvaluationService.completeEvaluation(neuralNetwork, testSet);
			}

		}


	}

}