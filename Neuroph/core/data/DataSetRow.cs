using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Copyright 2010 Neuroph Project http://neuroph.sourceforge.net
/// 
/// Licensed under the Apache License, Version 2.0 (the "License"); you may not
/// use this file except in compliance with the License. You may obtain a copy of
/// the License at
/// 
/// http://www.apache.org/licenses/LICENSE-2.0
/// 
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
/// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
/// License for the specific language governing permissions and limitations under
/// the License.
/// </summary>
namespace org.neuroph.core.data {

    using VectorParser = org.neuroph.util.VectorParser;

    /// <summary>
    /// This class represents single data row in a data set. It has input and desired output
    /// for supervised learning rules. It can also be used only with input for unsupervised learning rules.
    /// 
    /// @author Zoran Sevarac <sevarac@gmail.com>
    /// </summary>
    [Serializable]
    public class DataSetRow {

        /// <summary>
        /// The class fingerprint that is set to indicate serialization compatibility
        /// with a previous version of the class
        /// </summary>
        private const long serialVersionUID = 1L;
        /// <summary>
        /// Input vector for this training element
        /// </summary>
        protected internal double[] input;

        /// <summary>
        /// Desired output for this training element
        /// </summary>
        private double[] desiredOutput;

        /// <summary>
        /// Label for this training element
        /// </summary>
        protected internal string label;

        /// <summary>
        /// Creates new training element with specified input and desired output
        /// vectors specifed as strings
        /// </summary>
        /// <param name="input"> input vector as space separated string </param>
        /// <param name="desiredOutput"> desired output vector as space separated string </param>
        public DataSetRow(string input, string desiredOutput) {
            this.input = VectorParser.parseDoubleArray(input);
            this.desiredOutput = VectorParser.parseDoubleArray(desiredOutput);
        }

        /// <summary>
        /// Creates new training element with specified input and desired output
        /// vectors
        /// </summary>
        /// <param name="input"> input array </param>
        /// <param name="desiredOutput"> desired output array </param>
        public DataSetRow(double[] input, double[] desiredOutput) {
            this.input = input;
            this.desiredOutput = desiredOutput;
        }

        /// <summary>
        /// Creates new training element with input array
        /// </summary>
        /// <param name="input"> input array </param>
        public DataSetRow(params double[] input) {
            this.input = input;
        }

        /// <summary>
        /// Creates new training element with specified input and desired output
        /// vectors
        /// </summary>
        /// <param name="input">
        ///            input vector </param>
        /// <param name="desiredOutput">
        ///            desired output vector </param>
        public DataSetRow(List<double> input, List<double> desiredOutput) {
            this.input = VectorParser.toDoubleArray(input);
            this.desiredOutput = VectorParser.toDoubleArray(desiredOutput);
        }


        public DataSetRow(List<double> input) {
            this.input = VectorParser.toDoubleArray(input);
        }

        /// <summary>
        /// Returns input vector
        /// </summary>
        /// <returns> input vector </returns>
        public virtual double[] Input {
            get {
                return this.input;
            }
            set {
                this.input = value;
            }
        }


        public virtual double[] DesiredOutput {
            get {
                return desiredOutput;
            }
            set {
                this.desiredOutput = value;
            }
        }


        /// <summary>
        /// Get training element label
        /// </summary>
        /// <returns> training element label </returns>
        public virtual string Label {
            get {
                return label;
            }
            set {
                this.label = value;
            }
        }


        public virtual bool Supervised {
            get {
                return (desiredOutput != null);
            }
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();

            sb.Append("Input: ");
            foreach (double @in in input) {
                sb.Append(@in).Append(", ");
            }
            sb.Remove(sb.Length - 2, sb.Length - 1 - sb.Length + 2);

            if (Supervised) {
                sb.Append(" Desired output: ");
                foreach (double @out in desiredOutput) {
                    sb.Append(@out).Append(", ");
                }
                sb.Remove(sb.Length - 2, sb.Length - 1 - sb.Length + 2);
            }

            return sb.ToString();
        }


        public virtual string toCSV() {
            StringBuilder sb = new StringBuilder();

            foreach (double @in in input) {
                sb.Append(@in).Append(", ");
            }

            if (Supervised) {
                foreach (double @out in desiredOutput) {
                    sb.Append(@out).Append(", ");
                }
            }

            sb.Remove(sb.Length - 2, sb.Length - 1 - sb.Length - 2);

            return sb.ToString();
        }
    }
}