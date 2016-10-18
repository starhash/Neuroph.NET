using org.apache.commons.lang3.builder;
using System;
//using org.apache.commons.lang3.builder;

/// <summary>
/// Copyright 2010 Neuroph Project http://neuroph.sourceforge.net
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
/// 
///    http://www.apache.org/licenses/LICENSE-2.0
/// 
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// </summary>
namespace org.neuroph.core {


    /// <summary>
    /// Neuron connection weight.
    /// </summary>
    /// <seealso cref= Connection
    /// <remarks>author Zoran Sevarac <sevarac@gmail.com></remarks>
    /// </seealso>
    [Serializable]
    public class Weight : ICloneable {

        /// <summary>
        /// The class fingerprint that is set to indicate serialization compatibility
        /// with a previous version of the class
        /// </summary>
        private const long serialVersionUID = 2L;

        /// <summary>
        /// Weight value
        /// </summary>
        public double value;

        /// <summary>
        /// Weight change
        /// </summary>
        [NonSerialized]
        public double weightChange;

        /// <summary>
        /// Training data buffer holds various algorithm specific data which is used
        /// for adjusting this weight value during training
        /// </summary>
        [NonSerialized]
        private object trainingData;

        // maybe store deltaWeight and weight value history in transient fields...?
        /// <summary>
        /// Creates an instance of connection weight with random weight value in
        /// range [0..1]
        /// </summary>
        public Weight() {
            this.value = new Random(1).NextDouble() - 0.5d;
            this.weightChange = 0;
        }

        /// <summary>
        /// Creates an instance of connection weight with the specified weight value
        /// </summary>
        /// <param name="value"> weight value </param>
        public Weight(double value) {
            this.value = value;
        }

        /// <summary>
        /// Increases the weight for the specified amount
        /// </summary>
        /// <param name="amount"> amount to add to current weight value </param>
        public virtual void inc(double amount) {
            this.value += amount;
        }

        /// <summary>
        /// Decreases the weight for specified amount
        /// </summary>
        /// <param name="amount"> amount to subtract from the current weight value </param>
        public virtual void dec(double amount) {
            this.value -= amount;
        }

        /// <summary>
        /// Sets the weight value
        /// </summary>
        /// <param name="value"> weight value to set </param>
        public virtual double Value {
            set {
                this.value = value;
            }
            get {
                return this.value;
            }
        }


        /// <summary>
        /// Returns weight value as String
        /// </summary>
        public override string ToString() {
            return Convert.ToString(value);
        }

        /// <summary>
        /// Sets random weight value
        /// </summary>
        public virtual void randomize() {
            this.value = new Random(1).NextDouble() - 0.5d;
        }

        /// <summary>
        /// Sets random weight value within specified interval
        /// </summary>
        public virtual void randomize(double min, double max) {
            this.value = min + new Random(1).NextDouble() * (max - min);
        }

        public virtual void randomize(Random generator) {
            this.value = generator.NextDouble();
        }

        /// <summary>
        /// Returns training data buffer for this weight
        /// </summary>
        /// <returns> training data buffer for this weight </returns>
        public virtual object TrainingData {
            get {
                return trainingData;
            }
            set {
                this.trainingData = value;
            }
        }


        /// <summary>
        /// Returns cloned instance of this weight
        /// Important: trainingData will be lost in cloned instance </summary>
        /// <returns> cloned instance of this weight </returns>
        /// <exception cref="CloneNotSupportedException">  </exception>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: @Override public Object clone() throws CloneNotSupportedException
        public object Clone() {
            Weight cloned = (Weight)base.MemberwiseClone();
            cloned.TrainingData = null; // since we cannot call Object.clone() reset training data to nulll
            return cloned;
        }

        public override int GetHashCode() {
            return (new HashCodeBuilder(7, 17)).append(value).append(weightChange).append(trainingData).toHashCode();
        }

        public override bool Equals(object obj) {
            if (this == obj) {
                return true;
            }
            if (obj == null) {
                return false;
            }
            if (this.GetType() != obj.GetType()) {
                return false;
            }
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final Weight other = (Weight) obj;
            Weight other = (Weight)obj;
            if (BitConverter.DoubleToInt64Bits(this.value) != BitConverter.DoubleToInt64Bits(other.value)) {
                return false;
            }
            if (BitConverter.DoubleToInt64Bits(this.weightChange) != BitConverter.DoubleToInt64Bits(other.weightChange)) {
                return false;
            }
            if (!java.util.Objects.Equals(this.trainingData, other.trainingData)) {
                return false;
            }
            return true;
        }
    }

}