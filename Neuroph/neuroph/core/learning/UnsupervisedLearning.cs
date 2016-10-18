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

namespace org.neuroph.core.learning {

    using DataSet = org.neuroph.core.data.DataSet;
    using DataSetRow = org.neuroph.core.data.DataSetRow;


    /// <summary>
    /// Base class for all unsupervised learning algorithms.
    /// 
    /// @author Zoran Sevarac <sevarac@gmail.com>
    /// </summary>
    [Serializable]
    public abstract class UnsupervisedLearning : IterativeLearning {

        /// <summary>
        /// The class fingerprint that is set to indicate serialization 
        /// compatibility with a previous version of the class
        /// </summary>
        private const long serialVersionUID = 1L;

        /// <summary>
        /// Creates new unsupervised learning rule
        /// </summary>
        public UnsupervisedLearning() : base() {
        }


        /// <summary>
        /// This method does one learning epoch for the unsupervised learning rules.
        /// It iterates through the training set and trains network weights for each
        /// element
        /// </summary>
        /// <param name="trainingSet">
        ///            training set for training network </param>
        public override void doLearningEpoch(DataSet trainingSet) {
            IEnumerator<DataSetRow> iterator = trainingSet.iterator();
            while (iterator.MoveNext() && !Stopped) {
                DataSetRow trainingSetRow = iterator.Current;
                learnPattern(trainingSetRow);
            }
        }

        /// <summary>
        /// Trains network with the pattern from the specified training element
        /// </summary>
        /// <param name="DataSetItem">
        ///            unsupervised training element which contains network input </param>
        protected internal virtual void learnPattern(DataSetRow trainingElement) {
            double[] input = trainingElement.Input;
            this.neuralNetwork.Input = input;
            this.neuralNetwork.calculate();
            this.updateNetworkWeights();
        }



        /// <summary>
        /// This method implements the weight adjustment
        /// </summary>
        protected internal abstract void updateNetworkWeights();

    }
}