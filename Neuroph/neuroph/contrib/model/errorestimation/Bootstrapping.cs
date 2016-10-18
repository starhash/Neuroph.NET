using System.Collections.Generic;

namespace org.neuroph.contrib.model.errorestimation
{

	using RandomSamplingWithRepetition = org.neuroph.contrib.model.sampling.RandomSamplingWithRepetition;
	using DataSet = org.neuroph.core.data.DataSet;

	/// <summary>
	/// Error estimation method which uses sampling with repetition. Each sample contains N elements which forms
	/// so called E632 Bootstrap
	/// 
	/// U odnosu na KFold nema nikakvu dodatnu logiku, samo drugi sampling i izbor datasetova
	/// tako da mogu kfold da spojim sa error estimation i da ovaj dobijam kroz setere ili parametrizaciju
	/// 
	/// 
	/// Vidi konacni kakva j erazlika izmedju ova dva
	/// ako je samo smpling metod, onda se moze koristiti korsvalifacija a drugim smpling metodom 
	/// 
	/// moze da se izvuce ErrorEstimation method kao osnovna klasa
	/// 
	/// Boostraraping koristi sabsamples ne subsets
	/// http://www.faqs.org/faqs/ai-faq/neural-nets/part3/section-12.html
	/// 
	/// mozes da koristis 632+ botstraping
	///  50 - 2000 subsamples
	/// 
	/// mogao bih da napravim sve to sa jednom klasom; KFold samo sa drugim sampling algoritmom
	/// 
	/// 
	/// </summary>
	public class Bootstrapping // extends KFoldCrossValidation
	{

		/// <summary>
		/// Default constructor for creating BootstrapEstimationMethod error estimation
		/// </summary>
		/// <param name="numberOfSamples"> defines number of bootstrap samples </param>
		public Bootstrapping(int numberOfSamples)
		{
		   // super(numberOfSamples);
		   // setSampling(new RandomSamplingWithRepetition(numberOfSamples));
		}

	//    @Override
	//    protected DataSet createTrainingSet(List<DataSet> samples, int selectedSampleIndex) {
	//        return samples.get(selectedSampleIndex);
	//    }

	   // @Override
		protected internal virtual DataSet createTestSet(List<DataSet> samples, int selectedSampleIndex)
		{
			return samples[(selectedSampleIndex + 1) % samples.Count];
		}


	}

}