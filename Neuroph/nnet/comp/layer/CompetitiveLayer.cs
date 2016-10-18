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

namespace org.neuroph.nnet.comp.layer {

    using Layer = org.neuroph.core.Layer;
    using Neuron = org.neuroph.core.Neuron;
    using CompetitiveNeuron = org.neuroph.nnet.comp.neuron.CompetitiveNeuron;
    using NeuronProperties = org.neuroph.util.NeuronProperties;

    /// <summary>
    /// Represents layer of competitive neurons, and provides methods for competition.
    /// 
    /// TODO: competitive learning 3. training dw=n(i-w)
    /// 
    /// @author Zoran Sevarac <sevarac@gmail.com>
    /// </summary>
    public class CompetitiveLayer : Layer {

        /// <summary>
        /// The class fingerprint that is set to indicate serialization
        /// compatibility with a previous version of the class.
        /// </summary>
        private const long serialVersionUID = 1L;

        /// <summary>
        /// Max iterations for neurons to compete
        /// This is neccesery to limit, otherwise if there is no winner there will
        /// be endless loop.
        /// </summary>
        private int maxIterations = 100;

        /// <summary>
        /// The competition winner for this layer
        /// </summary>
        private CompetitiveNeuron winner;

        /// <summary>
        /// Create an instance of CompetitiveLayer with the specified number of
        /// neurons with neuron properties </summary>
        /// <param name="neuronNum"> neuron number in this layer </param>
        /// <param name="neuronProperties"> properties for the nurons in this layer </param>
        public CompetitiveLayer(int neuronNum, NeuronProperties neuronProperties) : base(neuronNum, neuronProperties) {
        }

        /// <summary>
        /// Performs calculaton for all neurons in this layer
        /// </summary>
        public override void calculate() {
            bool hasWinner = false;

            int iterationsCount = 0;

            while (!hasWinner) {
                int fireingNeurons = 0;
                foreach (Neuron neuron in this.Neurons) {
                    neuron.calculate();
                    if (neuron.Output > 0) {
                        fireingNeurons += 1;
                    }
                } // for

                if (iterationsCount > this.maxIterations) {
                    break;
                }

                if (fireingNeurons == 1) {
                    hasWinner = true;
                }
                iterationsCount++;

            } // while !done

            if (hasWinner) {
                // now set reference to winner
                double maxOutput = double.Epsilon;

                foreach (Neuron neuron in this.Neurons) {
                    CompetitiveNeuron cNeuron = (CompetitiveNeuron)neuron;
                    cNeuron.IsCompeting = false; // turn off competing mode
                    if (cNeuron.Output > maxOutput) {
                        maxOutput = cNeuron.Output;
                        this.winner = cNeuron;
                    }
                }
            }

        }

        /// <summary>
        /// Returns the winning neuron for this layer </summary>
        /// <returns> winning neuron for this layer </returns>
        public virtual CompetitiveNeuron Winner {
            get {
                return this.winner;
            }
        }

        /// <summary>
        /// Returns the maxIterations setting for this layer </summary>
        /// <returns> maxIterations setting for this layer </returns>
        public virtual int MaxIterations {
            get {
                return maxIterations;
            }
            set {
                this.maxIterations = value;
            }
        }




    }

}