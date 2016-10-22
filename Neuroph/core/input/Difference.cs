using System;
using System.Collections.Generic;

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

namespace org.neuroph.core.input {


    /// <summary>
    /// Performs the vector difference operation on input and
    /// weight vector.
    /// 
    /// @author Zoran Sevarac <sevarac@gmail.com>
    /// </summary>
    [Serializable]
    public class Difference : InputFunction {

        /// <summary>
        /// The class fingerprint that is set to indicate serialization
        /// compatibility with a previous version of the class.
        /// </summary>
        private const long serialVersionUID = 21L;


        public override double getOutput(List<Connection> inputConnections) {
            double output = 0d;

            double sum = 0d;
            foreach (Connection connection in inputConnections) {
                Neuron neuron = connection.FromNeuron;
                Weight weight = connection.Weight;
                double diff = neuron.Output - weight.Value;
                sum += diff * diff;
            }

            output = Math.Sqrt(sum);

            return output;
        }

        public virtual double[] getOutput(double[] inputs, double[] weights) {
            double[] output = new double[inputs.Length];

            for (int i = 0; i < inputs.Length; i++) {
                output[i] = inputs[i] - weights[i];
            }

            return output;
        }

    }

}