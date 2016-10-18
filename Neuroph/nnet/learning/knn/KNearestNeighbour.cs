using System.Collections.Generic;

namespace org.neuroph.nnet.learning.knn {

    using KVector = org.neuroph.nnet.learning.kmeans.KVector;

    /// <summary>
    /// for given vector
    /// calculate distances to all vectors from list 
    /// and find minimum vector
    /// or sort vectors by distance and seelct
    /// 
    /// @author zoran
    /// </summary>
    public class KNearestNeighbour {

        private List<KVector> dataSet;

        /// <summary>
        /// http://en.wikipedia.org/wiki/Selection_algorithm </summary>
        /// <param name="vector"> </param>
        /// <param name="k"> </param>
        /// <returns>  </returns>
        public virtual KVector[] getKNearestNeighbours(KVector vector, int k) {
            KVector[] nearestNeighbours = new KVector[k];

            // calculate distances for entire dataset
            foreach (KVector otherVector in dataSet) {
                double distance = vector.distanceFrom(otherVector);
                otherVector.Distance = distance;
            }

            for (int i = 0; i < k; i++) {
                int minIndex = i;
                KVector minVector = dataSet[i];
                double minDistance = minVector.Distance;

                for (int j = i + 1; j < dataSet.Count; j++) {
                    if (dataSet[j].Distance <= minDistance) {
                        minVector = dataSet[j];
                        minDistance = minVector.Distance;
                        minIndex = j;
                    }
                }

                // swap list[i] and list[minIndex]
                KVector temp = dataSet[i];
                dataSet[i] = dataSet[minIndex];
                dataSet[minIndex] = temp;

                nearestNeighbours[i] = dataSet[i];
            }


            //            function select(list[1..n], k)
            //                for i from 1 to k
            //                    minIndex = i
            //                    minValue = list[i]
            //                    for j from i+1 to n
            //                        if list[j] < minValue
            //                            minIndex = j
            //                            minValue = list[j]
            //                    swap list[i] and list[minIndex]
            //                return list[k]

            return nearestNeighbours;
        }

        public virtual List<KVector> DataSet {
            get {
                return dataSet;
            }
            set {
                this.dataSet = value;
            }
        }




    }

}