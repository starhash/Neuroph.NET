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

namespace org.neuroph.nnet.comp.neuron {

    using Connection = org.neuroph.core.Connection;
    using Neuron = org.neuroph.core.Neuron;

    /// <summary>
    /// Neuron with constant high output (1), used as bias
    /// </summary>
    /// <seealso cref= Neuron
    /// @author Zoran Sevarac <sevarac@gmail.com> </seealso>
    public class BiasNeuron : Neuron {

        /// <summary>
        /// The class fingerprint that is set to indicate serialization 
        /// compatibility with a previous version of the class
        /// </summary>
        private const long serialVersionUID = 1L;

        /// <summary>
        /// Creates an instance of BiasedNeuron.
        /// </summary>
        public BiasNeuron() : base() {
        }

        public override double Output {
            get {
                return 1;
            }
        }

        public override void addInputConnection(Connection connection) {

        }

        public override void addInputConnection(Neuron fromNeuron, double weightVal) {

        }

        public override void addInputConnection(Neuron fromNeuron) {

        }



    }

}