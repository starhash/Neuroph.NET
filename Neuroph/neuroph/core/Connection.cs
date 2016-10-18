using System;

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
    /// Weighted connection to another neuron.
    /// </summary>
    /// <seealso cref= Weight </seealso>
    /// <seealso cref= Neuron
    /// @author Zoran Sevarac <sevarac@gmail.com> </seealso>
    [Serializable]
    public class Connection {

        /// <summary>
        /// The class fingerprint that is set to indicate serialization
        /// compatibility with a previous version of the class
        /// </summary>
        private const long serialVersionUID = 1L;

        /// <summary>
        /// From neuron for this connection (source neuron).
        /// This connection is output connection for from neuron.
        /// </summary>
        protected internal Neuron fromNeuron;

        /// <summary>
        /// To neuron for this connection (target, destination neuron)
        /// This connection is input connection for to neuron.
        /// </summary>
        protected internal Neuron toNeuron;

        /// <summary>
        /// Weight for this connection
        /// </summary>
        protected internal Weight weight;

        /// <summary>
        /// Creates a new connection between specified neurons with random weight
        /// </summary>
        /// <param name="fromNeuron"> neuron to connect from </param>
        /// <param name="toNeuron"> neuron to connect to </param>
        public Connection(Neuron fromNeuron, Neuron toNeuron) {

            if (fromNeuron == null) {
                throw new System.ArgumentException("From neuron in connection cant be null !");
            } else {
                this.fromNeuron = fromNeuron;
            }

            if (toNeuron == null) {
                throw new System.ArgumentException("To neuron in connection cant be null!");
            } else {
                this.toNeuron = toNeuron;
            }

            this.weight = new Weight();
        }

        /// <summary>
        /// Creates a new connection to specified neuron with specified weight object
        /// </summary>
        /// <param name="fromNeuron"> neuron to connect from </param>
        /// <param name="toNeuron"> neuron to connect to </param>
        /// <param name="weight">
        ///            weight for this connection </param>
        public Connection(Neuron fromNeuron, Neuron toNeuron, Weight weight) : this(fromNeuron, toNeuron) {

            if (weight == null) {
                throw new System.ArgumentException("Connection Weight cant be null!");
            } else {
                this.weight = weight;
            }

        }

        /// <summary>
        /// Creates a new connection to specified neuron with specified weight value
        /// </summary>
        /// <param name="fromNeuron"> neuron to connect from </param>
        /// <param name="toNeuron"> neuron to connect to </param>
        /// <param name="weightVal">
        ///            weight value for this connection </param>
        public Connection(Neuron fromNeuron, Neuron toNeuron, double weightVal) : this(fromNeuron, toNeuron, new Weight(weightVal)) {
        }

        /// <summary>
        /// Returns weight for this connection
        /// </summary>
        /// <returns> weight for this connection </returns>
        public virtual Weight Weight {
            get {
                return this.weight;
            }
            set {
                if (value == null) {
                    throw new System.ArgumentException("Connection Weight cant be null!");
                } else {
                    this.weight = value;
                }
            }
        }


        /// <summary>
        /// Returns input received through this connection - the activation that
        /// comes from the output of the cell on the other end of connection
        /// </summary>
        /// <returns> input received through this connection </returns>
        public virtual double Input {
            get {
                return this.fromNeuron.Output;
            }
        }

        /// <summary>
        /// Returns the weighted input received through this connection
        /// </summary>
        /// <returns> weighted input received through this connection </returns>
        public virtual double WeightedInput {
            get {
                return this.fromNeuron.Output * weight.value;
            }
        }

        /// <summary>
        /// Gets from neuron for this connection </summary>
        /// <returns> from neuron for this connection </returns>
        public virtual Neuron FromNeuron {
            get {
                return fromNeuron;
            }
        }

        /// <summary>
        /// Gets to neuron for this connection </summary>
        /// <returns> neuron to set as to neuron </returns>
        public virtual Neuron ToNeuron {
            get {
                return toNeuron;
            }
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: @Override public Object clone() throws CloneNotSupportedException
        public object Clone() {
            Connection cloned = (Connection)base.MemberwiseClone();
            cloned.Weight = (Weight)weight.Clone();
            cloned.toNeuron = (Neuron)toNeuron.Clone();
            cloned.fromNeuron = (Neuron)fromNeuron.Clone();

            return cloned;
        }

        public override int GetHashCode() {
            int hash = 7;
            //hash = 67 * hash + java.util.Objects.hashCode(this.fromNeuron);
            //hash = 67 * hash + java.util.Objects.hashCode(this.toNeuron);
            //hash = 67 * hash + java.util.Objects.hashCode(this.weight);
            hash = 67 * hash + this.fromNeuron.GetHashCode();
            hash = 67 * hash + this.toNeuron.GetHashCode();
            hash = 67 * hash + this.weight.GetHashCode();
            return hash;
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
            //ORIGINAL LINE: final Connection other = (Connection) obj;
            Connection other = (Connection)obj;
            if (!java.util.Objects.Equals(this.fromNeuron, other.fromNeuron)) {
                return false;
            }
            if (!java.util.Objects.Equals(this.toNeuron, other.toNeuron)) {
                return false;
            }
            if (!java.util.Objects.Equals(this.weight, other.weight)) {
                return false;
            }
            return true;
        }

        public override string ToString() {
            return "Connection{" + "fromNeuron=" + fromNeuron + ", toNeuron=" + toNeuron + ", weight=" + weight + '}';
        }
    }
}