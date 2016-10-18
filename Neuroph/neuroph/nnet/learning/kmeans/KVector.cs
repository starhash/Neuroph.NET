using System;
using System.Text;

namespace org.neuroph.nnet.learning.kmeans {

    /// <summary>
    /// Represents feature vector used in k-means clustering algorithm
    /// @author Zoran Sevarac
    /// @author Uros Stojkic
    /// </summary>
    public class KVector {

        /// <summary>
        /// Values of feature vector
        /// </summary>
        private double[] values;

        /// <summary>
        /// Cluster to which this vector is assigned during clustering
        /// </summary>
        private Cluster cluster;


        /// <summary>
        /// Distance fro other vector (used by K nearest neighbour)
        /// </summary>
        private double distance;


        public KVector(int size) {
            this.values = new double[size];
        }

        public KVector(double[] values) {
            this.values = values;
        }


        public virtual void setValueAt(int idx, double value) {
            this.values[idx] = value;
        }

        public virtual double getValueAt(int idx) {
            return values[idx];
        }

        public virtual double[] Values {
            get {
                return values;
            }
            set {
                this.values = value;
            }
        }



        public virtual Cluster Cluster {
            get {
                return cluster;
            }
            set {
                // remove this from old value and assign it to new
                if (this.cluster != null) {
                    this.cluster.removePoint(this);
                }
                this.cluster = value;
            }
        }


        /// <summary>
        /// Calculates and returns intensity of this vector </summary>
        /// <returns> intensity of this vector </returns>
        public virtual double Intensity {
            get {
                double intensity = 0;

                foreach (double value in values) {
                    intensity += value * value;
                }

                intensity = Math.Sqrt(intensity);

                return intensity;
            }
        }

        /// <summary>
        /// Calculates and returns euclidean distance of this vector from the given cluster </summary>
        /// <param name="cluster"> </param>
        /// <returns> euclidean distance of this vector from given cluster </returns>
        public virtual double distanceFrom(KVector otherVector) {
            // get values and do this in loop
            double[] otherValues = otherVector.Values;

            double distance = 0;

            for (int i = 0; i < values.Length; i++) {
                distance += Math.Pow(otherValues[i] - values[i], 2);
            }

            distance = Math.Sqrt(distance);

            return distance;
        }

        public virtual int size() {
            return values.Length;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append("KMeansVector{");
            for (int i = 0; i < values.Length; i++) {
                sb.Append("[" + i + "]=" + values[i] + ",");
            }

            sb.Append('}');

            return sb.ToString();
        }

        public virtual double Distance {
            get {
                return distance;
            }
            set {
                this.distance = value;
            }
        }
    }
}