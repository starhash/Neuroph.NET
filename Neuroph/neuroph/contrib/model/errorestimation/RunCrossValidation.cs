using System;

namespace org.neuroph.contrib.model.errorestimation
{

	using ClassifierEvaluator = org.neuroph.contrib.eval.ClassifierEvaluator;
	using ErrorEvaluator = org.neuroph.contrib.eval.ErrorEvaluator;
	using org.neuroph.contrib.eval;
	using ClassificationMetrics = org.neuroph.contrib.eval.classification.ClassificationMetrics;
	using ConfusionMatrix = org.neuroph.contrib.eval.classification.ConfusionMatrix;
	using org.neuroph.core;
	using Neuron = org.neuroph.core.Neuron;
	using DataSet = org.neuroph.core.data.DataSet;

	/// 
	/// <summary>
	/// @author zoran
	/// </summary>
	public class RunCrossValidation
	{

		/// <param name="args"> the command line arguments </param>
		public static void Main(string[] args)
		{
			// test subsampling here too with some small dataset

//JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
//ORIGINAL LINE: org.neuroph.core.NeuralNetwork<?> nnet = org.neuroph.core.NeuralNetwork.createFromFile("MIcrNet1.nnet");
			NeuralNetwork nnet = NeuralNetwork.createFromFile("MIcrNet1.nnet");
			DataSet dataSet = DataSet.load("MICRData.tset");

			// get class labels from output neurons
			string[] classNames = new string[nnet.OutputsCount]; // = {"LeftHand", "RightHand", "Foot", "Rest"};
			int i = 0;
			foreach (Neuron n in nnet.OutputNeurons)
			{
				classNames[i] = n.Label;
				i++;
			}


			CrossValidation crossval = new CrossValidation(nnet, dataSet, 5);
			crossval.addEvaluator(new ClassifierEvaluator.MultiClass(classNames)); // add multi class here manualy to make it independent from data set
																					   // data set should hav ecolumn names when loading/creating , not hardcocd
			//   crossval.setSampling(null);

			crossval.run();
			CrossValidationResult results = crossval.Result;


			System.Console.WriteLine(results);

		   // razmisli kako da uzmes rezultate i kako da ih prikazes u Neuroph studio - vuci ih direktno iz evaluatora
		   // i kako da integrises ovo kroz training dialog -  samo dodati opciju KFold u trening dijalogu
			// tokom kfoldinga treba prikazivati gresku i desavanja tokom treninga - izlozi learning rule; napravi neki event listening za crossvalidaciju!!!
			// svaku istreniranu mrezu sacuvati, amozda negde i rezultate testiranja

			// potrebno je na kraju izracunati i srednju vrednost/statistikuu svih mera klasifikacije
			// takodje napravi boostraping - da radi ovo isto samo sa drugim sampling algoritmom


	//        System.out.println("MeanSquare Error: " + crossval.getEvaluator(ErrorEvaluator.class).getResult());
	//      
	//        ClassificationEvaluator evaluator = crossval.getEvaluator(ClassificationEvaluator.MultiClass.class);              
	//        ConfusionMatrix confusionMatrix = evaluator.getResult();        
	//        
	//        System.out.println("Confusion Matrix: \r\n"+confusionMatrix.toString());
	//                      
	//        System.out.println("Classification metrics: ");   
	//        
	//        ClassificationMetrics[] metrics = ClassificationMetrics.createFromMatrix(confusionMatrix);     // add all of these to result 
	//        // createaverage statisticss from ClassificationMetrics
	//        
	//        for(ClassificationMetrics cm : metrics)
	//            System.out.println(cm.toString());
	//          
	//        
		}

	}

}