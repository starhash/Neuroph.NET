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

namespace org.neuroph.core.learning.error {

    /// <summary>
    /// Commonly used mean squared error
    /// 
    /// @author Zoran Sevarac <sevarac@gmail.com>
    /// </summary>
    [Serializable]
    public class MeanSquaredError : ErrorFunction {

        private const long serialVersionUID = 1L;

        [NonSerialized]
        private double totalError;

        /// <summary>
        /// Number of patterns - n 
        /// </summary>
        [NonSerialized]
        private double patternCount;

        public MeanSquaredError() {
            reset();
        }


        public virtual void reset() {
            totalError = 0d;
            patternCount = 0;
        }


        public virtual double TotalError {
            get {
                return totalError / (2 * patternCount);
            }
        }

        public virtual double[] calculatePatternError(double[] predictedOutput, double[] targetOutput) {
            double[] patternError = new double[targetOutput.Length];

            for (int i = 0; i < predictedOutput.Length; i++) {
                patternError[i] = targetOutput[i] - predictedOutput[i];
                totalError += patternError[i] * patternError[i];
            }
            patternCount++;
            return patternError;
        }

    }

}