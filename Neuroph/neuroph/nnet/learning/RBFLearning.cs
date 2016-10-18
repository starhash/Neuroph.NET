using System;
using System.Collections.Generic;

namespace org.neuroph.nnet.learning
{

	using Connection = org.neuroph.core.Connection;
	using Layer = org.neuroph.core.Layer;
	using Neuron = org.neuroph.core.Neuron;
	using Gaussian = org.neuroph.core.transfer.Gaussian;
	using Cluster = org.neuroph.nnet.learning.kmeans.Cluster;
	using KMeansClustering = org.neuroph.nnet.learning.kmeans.KMeansClustering;
	using KVector = org.neuroph.nnet.learning.kmeans.KVector;
	using KNearestNeighbour = org.neuroph.nnet.learning.knn.KNearestNeighbour;

	/// <summary>
	/// Learning rule for Radial Basis Function networks.
	/// Use K-Means to determine centroids for hidden units, K-NearestNeighbour to set widths,
	/// and LMS to tweak output neurons.
	/// 
	/// @author Zoran Sevarac sevarac@gmail.com
	/// </summary>
	public class RBFLearning : LMS
	{

		/// <summary>
		/// how many nearest neighbours to use when determining width (sigma) for
		/// gaussian functions
		/// </summary>
		internal int k = 2;

		protected internal override void onStart()
		{
			base.onStart();

			// set weights between input and rbf layer using kmeans
			KMeansClustering kmeans = new KMeansClustering(TrainingSet);
			kmeans.NumberOfClusters = neuralNetwork.getLayerAt(1).NeuronsCount; // set number of clusters as number of rbf neurons
			kmeans.doClustering();

			// get clusters (centroids)
			Cluster[] clusters = kmeans.Clusters;

			// assign each rbf neuron to one cluster
			// and use centroid vectors to initialize neuron's input weights
			Layer rbfLayer = neuralNetwork.getLayerAt(1);
			int i = 0;
			foreach (Neuron neuron in rbfLayer.Neurons)
			{
				KVector centroid = clusters[i].Centroid;
				double[] weightValues = centroid.Values;
				int c = 0;
				foreach (Connection conn in neuron.InputConnections)
				{
					conn.Weight.Value = weightValues[c];
					c++;
				}
				i++;
			}

			// get cluster centroids as list
			List<KVector> centroids = new List<KVector>();
			foreach (Cluster cluster in clusters)
			{
				centroids.Add(cluster.Centroid);
			}

			// use KNN to calculate sigma param - gausssian function width for each neuron
			KNearestNeighbour knn = new KNearestNeighbour();
			knn.DataSet = centroids;

			int n = 0;
			foreach (KVector centroid in centroids)
			{
			// calculate and set sigma for each neuron in rbf layer
				KVector[] nearestNeighbours = knn.getKNearestNeighbours(centroid, k);
				double sigma = calculateSigma(centroid, nearestNeighbours); // calculate in method
				Neuron neuron = rbfLayer.getNeuronAt(n);
				((Gaussian)neuron.TransferFunction).Sigma = sigma;
				i++;

			}


		}

		/// <summary>
		/// Calculates and returns  width of a gaussian function </summary>
		/// <param name="centroid"> </param>
		/// <param name="nearestNeighbours"> </param>
		/// <returns>  </returns>
		private double calculateSigma(KVector centroid, KVector[] nearestNeighbours)
		{
		   double sigma = 0;

		   foreach (KVector nn in nearestNeighbours)
		   {
			   sigma += Math.Pow(centroid.distanceFrom(nn), 2);
		   }

		   sigma = Math.Sqrt(1 / ((double)nearestNeighbours.Length) * sigma);

		   return sigma;
		}





	}

}