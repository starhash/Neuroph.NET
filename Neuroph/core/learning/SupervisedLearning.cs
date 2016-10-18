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
    using ErrorFunction = org.neuroph.core.learning.error.ErrorFunction;
    using MeanSquaredError = org.neuroph.core.learning.error.MeanSquaredError;
    using MaxErrorStop = org.neuroph.core.learning.stop.MaxErrorStop;
    using StopCondition = org.neuroph.core.learning.stop.StopCondition;

    // TODO:  random pattern order

    /// <summary>
    /// Base class for all supervised learning algorithms.
    /// It extends IterativeLearning, and provides general supervised learning principles.
    /// 
    /// @author Zoran Sevarac <sevarac@gmail.com>
    /// </summary>
    [Serializable]
    public abstract class SupervisedLearning : IterativeLearning {

        /// <summary>
        /// The class fingerprint that is set to indicate serialization
        /// compatibility with a previous version of the class
        /// </summary>
        private const long serialVersionUID = 3L;
        /// <summary>
        /// Total network error in previous epoch
        /// </summary>
        [NonSerialized]
        protected internal double previousEpochError;
        /// <summary>
        /// Max allowed network error (condition to stop learning)
        /// </summary>
        protected internal double maxError = 0.01d;
        /// <summary>
        /// Stopping condition: training stops if total network error change is smaller than minErrorChange
        /// for minErrorChangeIterationsLimit number of iterations
        /// </summary>
        private double minErrorChange = double.PositiveInfinity;
        /// <summary>
        /// Stopping condition: training stops if total network error change is smaller than minErrorChange
        /// for minErrorChangeStopIterations number of iterations
        /// </summary>
        private int minErrorChangeIterationsLimit = int.MaxValue;
        /// <summary>
        /// Count iterations where error change is smaller then minErrorChange
        /// </summary>
        [NonSerialized]
        private int minErrorChangeIterationsCount;
        /// <summary>
        /// Setting to determine if learning (weights update) is in batch mode
        /// False by default.
        /// </summary>
        private bool batchMode = false;

        private ErrorFunction errorFunction;

        /// <summary>
        /// Creates new supervised learning rule
        /// </summary>
        public SupervisedLearning() : base() {
            this.errorFunction = new MeanSquaredError();
            // create stop condition structure based on settings               
            this.stopConditions.Add(new MaxErrorStop(this));
        }

        /// <summary>
        /// Trains network for the specified training set and maxError
        /// </summary>
        /// <param name="trainingSet"> training set to learn </param>
        /// <param name="maxError">    learning stop condition. If maxError is reached learning stops </param>
        public virtual void learn(DataSet trainingSet, double maxError) {
            this.maxError = maxError;
            this.learn(trainingSet);
        }

        /// <summary>
        /// Trains network for the specified training set, maxError and number of iterations
        /// </summary>
        /// <param name="trainingSet">   training set to learn </param>
        /// <param name="maxError">      learning stop condition. if maxError is reached learning stops </param>
        /// <param name="maxIterations"> maximum number of learning iterations </param>
        public virtual void learn(DataSet trainingSet, double maxError, int maxIterations) {
            this.maxError = maxError;
            this.MaxIterations = maxIterations;
            this.learn(trainingSet);
        }

        protected internal override void onStart() {
            base.onStart(); // reset iteration counter
            this.minErrorChangeIterationsCount = 0;
            this.previousEpochError = 0d;

            // this is now done in constructor
            //        this.errorFunction = new MeanSquaredError();
            //        // create stop condition structure based on settings               
            //        this.stopConditions.add(new MaxErrorStop(this));
        }

        protected internal override void beforeEpoch() {
            this.previousEpochError = errorFunction.TotalError;
            this.errorFunction.reset();
        }

        protected internal override void afterEpoch() {

            // calculate abs error change and count iterations if its below specified min error change (used for stop condition)
            double absErrorChange = Math.Abs(previousEpochError - errorFunction.TotalError);
            if (absErrorChange <= this.minErrorChange) {
                this.minErrorChangeIterationsCount++;
            } else {
                this.minErrorChangeIterationsCount = 0;
            }

            // if learning is performed in batch mode, apply accumulated weight changes from this epoch        
            if (this.batchMode == true) {
                doBatchWeightsUpdate();
            }
        }

        /// <summary>
        /// This method implements basic logic for one learning epoch for the
        /// supervised learning algorithms. Epoch is the one pass through the
        /// training set. This method  iterates through the training set
        /// and trains network for each element. It also sets flag if conditions
        /// to stop learning has been reached: network error below some allowed
        /// value, or maximum iteration count
        /// </summary>
        /// <param name="trainingSet"> training set for training network </param>
        public override void doLearningEpoch(DataSet trainingSet) {

            // feed network with all elements from training set
            IEnumerator<DataSetRow> iterator = trainingSet.iterator();
            while (iterator.MoveNext() && !Stopped) {
                DataSetRow dataSetRow = iterator.Current;
                // learn current input/output pattern defined by SupervisedTrainingElement
                this.learnPattern(dataSetRow);
            }

            // calculate total network error as MSE. Use MSE so network does not grow with bigger training sets
            //this.totalNetworkError = errorFunction.getTotalError();

            // moved stopping condition to separate method hasReachedStopCondition() so it can be overriden / customized in subclasses
            // this condition is allready checked in IterativeLearning.learn(DataSet trainingSet)
            //        if (hasReachedStopCondition()) {
            //            stopLearning();
            //        }
        }

        /// <summary>
        /// Trains network with the input and desired output pattern from the specified training element
        /// </summary>
        /// <param name="trainingElement"> supervised training element which contains input and desired output </param>
        protected internal virtual void learnPattern(DataSetRow trainingElement) {
            double[] input = trainingElement.Input;
            this.neuralNetwork.Input = input;
            this.neuralNetwork.calculate();
            double[] output = this.neuralNetwork.Output;
            double[] desiredOutput = trainingElement.DesiredOutput;
            double[] patternError = errorFunction.calculatePatternError(output, desiredOutput);
            this.updateNetworkWeights(patternError);
        }

        /// <summary>
        /// This method updates network weights in batch mode - use accumulated weights change stored in Weight.deltaWeight
        /// It is executed after each learning epoch, only if learning is done in batch mode.
        /// </summary>
        /// <seealso cref= SupervisedLearning#doLearningEpoch(org.neuroph.core.data.DataSet) </seealso>
        protected internal virtual void doBatchWeightsUpdate() {
            // iterate layers from output to input
            List<Layer> layers = neuralNetwork.Layers;
            for (int i = neuralNetwork.LayersCount - 1; i > 0; i--) {
                // iterate neurons at each layer
                foreach (Neuron neuron in layers[i].Neurons) {
                    // iterate connections/weights for each neuron
                    foreach (Connection connection in neuron.InputConnections) {
                        // for each connection weight apply accumulated weight change
                        Weight weight = connection.Weight;
                        weight.value += weight.weightChange; // apply delta weight which is the sum of delta weights in batch mode
                        weight.weightChange = 0; // reset deltaWeight
                    }
                }
            }
        }


        /// <summary>
        /// Returns true if stop condition has been reached, false otherwise.
        /// Override this method in derived classes to implement custom stop criteria.
        /// </summary>
        /// <returns> true if stop condition is reached, false otherwise </returns>
        //    protected boolean hasReachedStopCondition() {
        //        // da li ovd etreba staviti da proverava i da li se koristi ovaj uslov??? ili staviti da uslov bude automatski samo s ajaako malom vrednoscu za errorChange Doule.minvalue
        //        return (this.totalNetworkError < this.maxError) || this.errorChangeStalled();
        //      //  return stopConditions.isReached();
        //    }

        /// <summary>
        /// Returns true if absolute error change is sufficently small (<=minErrorChange) for minErrorChangeStopIterations number of iterations </summary>
        /// <returns> true if absolute error change is stalled (error is sufficently small for some number of iterations) </returns>
        //    protected boolean errorChangeStalled() {
        //        double absErrorChange = Math.abs(previousEpochError - totalNetworkError);
        //
        //        if (absErrorChange <= this.minErrorChange) {
        //            this.minErrorChangeIterationsCount++;
        //
        //            if (this.minErrorChangeIterationsCount >= this.minErrorChangeIterationsLimit) {
        //                return true;
        //            }
        //        } else {
        //            this.minErrorChangeIterationsCount = 0;
        //        }
        //
        //        return false;
        //    }


        //    public void addStopCondition(StopCondition stopCondition) {        
        //        stopConditions.add(stopCondition);
        //    }

        /// <summary>
        /// Returns true if learning is performed in batch mode, false otherwise
        /// </summary>
        /// <returns> true if learning is performed in batch mode, false otherwise </returns>
        public virtual bool InBatchMode {
            get {
                return batchMode;
            }
        }

        /// <summary>
        /// Sets batch mode on/off (true/false)
        /// </summary>
        /// <param name="batchMode"> batch mode setting </param>
        public virtual bool BatchMode {
            set {
                this.batchMode = value;
            }
        }

        /// <summary>
        /// Sets allowed network error, which indicates when to stopLearning training
        /// </summary>
        /// <param name="maxError"> network error </param>
        public virtual double MaxError {
            set {
                this.maxError = value;
            }
            get {
                return maxError;
            }
        }


        /// <summary>
        /// Returns total network error in previous learning epoch
        /// </summary>
        /// <returns> total network error in previous learning epoch </returns>
        public virtual double PreviousEpochError {
            get {
                return previousEpochError;
            }
        }

        /// <summary>
        /// Returns min error change stopping criteria
        /// </summary>
        /// <returns> min error change stopping criteria </returns>
        public virtual double MinErrorChange {
            get {
                return minErrorChange;
            }
            set {
                this.minErrorChange = value;
            }
        }


        /// <summary>
        /// Returns number of iterations for min error change stopping criteria
        /// </summary>
        /// <returns> number of iterations for min error change stopping criteria </returns>
        public virtual int MinErrorChangeIterationsLimit {
            get {
                return minErrorChangeIterationsLimit;
            }
            set {
                this.minErrorChangeIterationsLimit = value;
            }
        }


        /// <summary>
        /// Returns number of iterations count for for min error change stopping criteria
        /// </summary>
        /// <returns> number of iterations count for for min error change stopping criteria </returns>
        public virtual int MinErrorChangeIterationsCount {
            get {
                return minErrorChangeIterationsCount;
            }
        }

        public virtual ErrorFunction ErrorFunction {
            get {
                return errorFunction;
            }
            set {
                this.errorFunction = value;
            }
        }



        public virtual double TotalNetworkError {
            get {
                return errorFunction.TotalError;
            }
        }

        /// <summary>
        /// This method should implement the weights update procedure for the whole network
        /// for the given output error vector.
        /// </summary>
        /// <param name="outputError"> output error vector for some network input (aka. patternError, network error)
        ///                    usually the difference between desired and actual output </param>
        protected internal abstract void updateNetworkWeights(double[] outputError);
    }

}