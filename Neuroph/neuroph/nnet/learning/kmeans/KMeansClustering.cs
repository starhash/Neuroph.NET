using System;
using System.Collections.Generic;
using System.Text;

namespace org.neuroph.nnet.learning.kmeans {

    using DataSet = org.neuroph.core.data.DataSet;
    using DataSetRow = org.neuroph.core.data.DataSetRow;

    /// 
    /// <summary>
    ///   1. Pick an initial set of K centroids (this can be random or any other means)
    ///   2. For each data point, assign it to the member of the closest centroid according to the given distance function
    ///   3. Adjust the centroid position as the mean of all its assigned member data points. Go back to (2) until the membership isn't change and centroid position is stable.
    /// 
    /// @author Zoran Sevarac
    /// @author Uros Stojkic
    /// </summary>
    public class KMeansClustering {

        /// <summary>
        /// Data/points to cluster
        /// </summary>
        private DataSet dataSet;

        private KVector[] dataVectors;

        /// <summary>
        /// Total number of clusters
        /// </summary>
        private int numberOfClusters;

        /// <summary>
        /// Clusters
        /// </summary>
        private Cluster[] clusters;

        internal StringBuilder log = new StringBuilder();

        public KMeansClustering(DataSet dataSet) {
            this.dataSet = dataSet;
            this.dataVectors = new KVector[dataSet.size()];
            // iterate dataset and create dataVectors field
            this.dataVectors = new KVector[dataSet.size()];
            // iterate dataset and create dataVectors field
            int i = 0;
            foreach (DataSetRow row in dataSet.Rows) {
                KVector vector = new KVector(row.Input);
                this.dataVectors[i] = vector;
                i++;

            }
        }

        public KMeansClustering(DataSet dataSet, int numberOfClusters) {
            this.dataSet = dataSet;
            this.numberOfClusters = numberOfClusters;
            this.dataVectors = new KVector[dataSet.size()];
            // iterate dataset and create dataVectors field
            int i = 0;
            foreach (DataSetRow row in dataSet.Rows) {
                KVector vector = new KVector(row.Input);
                this.dataVectors[i] = vector;
                i++;

            }
        }

        private static Random rng = new Random();

        public static void Shuffle<T>(List<T> list) {
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
            }
        }

        private static int SHUFFLE_THRESHOLD = 5;

        public static void Shuffle<T>(List<T> list, Random rnd) {
            int size = list.Count;
            if (size < SHUFFLE_THRESHOLD) {
                for (int i = size; i > 1; i--) {
                    int k = rnd.Next(i);
                    T value = list[k];
                    list[k] = list[i - 1];
                    list[i - 1] = value;
                }
            }
        }

        // find initial values for centroids/clusters
        // dont need to return string this is just for debuging and output in dialog
        // forgy and random partitions
        // http://en.wikipedia.org/wiki/K-means_clustering
        public virtual void initClusters() {

            List<int> idxList = new List<int>();

            for (int i = 0; i < dataSet.size(); i++) {
                idxList.Add(i);
            }
            Shuffle(idxList);

            //   log.append("Clusters initialized at:\n\n");

            clusters = new Cluster[numberOfClusters];
            for (int i = 0; i < numberOfClusters; i++) {
                clusters[i] = new Cluster();
                int randomIdx = idxList[i];
                KVector randomVector = dataVectors[randomIdx];
                clusters[i].Centroid = randomVector;
                //log.append(randomVector.toString()+System.lineSeparator() );
            }
            //log.append(System.lineSeparator());

        }

        /// <summary>
        /// Find and return the nearest cluster for the specified vector </summary>
        /// <param name="vector"> </param>
        /// <returns>  </returns>
        private Cluster getNearestCluster(KVector vector) {
            Cluster nearestCluster = null;
            double minimumDistanceFromCluster = double.MaxValue;
            double distanceFromCluster = 0;

            foreach (Cluster cluster in clusters) {
                distanceFromCluster = vector.distanceFrom(cluster.Centroid);
                if (distanceFromCluster < minimumDistanceFromCluster) {
                    minimumDistanceFromCluster = distanceFromCluster;
                    nearestCluster = cluster;
                }
            }

            return nearestCluster;
        }

        // runs cluctering
        public virtual void doClustering() {
            // throw exception if number of clusters is 0 
            if (numberOfClusters <= 0) {
                throw new Exception("Error: Number of clusters must be greater then zero!");
            }

            // initialize clusters
            initClusters();

            // initial nearest cluster assignement
            foreach (KVector vector in dataVectors) // Iterate all dataSet
            {
                Cluster nearestCluster = getNearestCluster(vector);
                nearestCluster.assignVector(vector);
            }

            // this is the loop doing the main thing
            //  keep re-calculating centroids and assigning points until there is no change
            bool clustersChanged; // flag to indicate cluster change
            do {
                clustersChanged = false;
                recalculateCentroids();

                foreach (KVector vector in dataVectors) {
                    Cluster nearestCluster = getNearestCluster(vector);
                    if (!vector.Cluster.Equals(nearestCluster)) {
                        nearestCluster.assignVector(vector);
                        clustersChanged = true;
                    }
                }
            } while (clustersChanged);
        }

        /// <summary>
        /// Calculate new centroids as an average of all dataSet in cluster
        /// </summary>
        private void recalculateCentroids() {
            // for each cluster do the following
            foreach (Cluster cluster in clusters) // for each cluster
            {
                if (cluster.size() > 0) // that cointains data
                {
                    double[] avgSum = cluster.AvgSum; // calculate avg sum
                    cluster.Centroid.Values = avgSum; // and set new centroid to avg sum
                }
            }
        }

        public virtual DataSet DataSet {
            get {
                return dataSet;
            }
            set {
                this.dataSet = value;
            }
        }


        public virtual int NumberOfClusters {
            set {
                this.numberOfClusters = value;
            }
        }

        public virtual Cluster[] Clusters {
            get {
                return clusters;
            }
        }

        public virtual string Log {
            get {
                return log.ToString();
            }
        }
    }
}