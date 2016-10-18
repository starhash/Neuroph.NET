using System;
using System.Collections.Generic;
using System.Threading;

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
namespace org.neuroph.core.learning {

    using DataSet = org.neuroph.core.data.DataSet;
    using LearningEvent = org.neuroph.core.events.LearningEvent;
    using MaxIterationsStop = org.neuroph.core.learning.stop.MaxIterationsStop;
    using StopCondition = org.neuroph.core.learning.stop.StopCondition;

    /// <summary>
    /// Base class for all iterative learning algorithms. It provides the iterative
    /// learning procedure for all of its subclasses.
    /// 
    /// @author Zoran Sevarac <sevarac@gmail.com>
    /// </summary>
    [Serializable]
    public abstract class IterativeLearning : LearningRule {

        /// <summary>
        /// The class fingerprint that is set to indicate serialization compatibility
        /// with a previous version of the class
        /// </summary>
        private const long serialVersionUID = 1L;
        /// <summary>
        /// Learning rate parametar
        /// </summary>
        protected internal double learningRate = 0.1d;
        /// <summary>
        /// Current iteration counter
        /// </summary>
        protected internal int currentIteration = 0;
        /// <summary>
        /// Max training iterations (when to stopLearning training) TODO: this field
        /// should be private, to force use of setMaxIterations from derived classes,
        /// so iterationsLimited flag is also set at the sam etime.Wil that break
        /// backward compatibility with serialized networks?
        /// </summary>
        private int maxIterations = int.MaxValue;

        /// <summary>
        /// Flag for indicating if the training iteration number is limited
        /// </summary>
        private bool iterationsLimited = false;


        protected internal List<StopCondition> stopConditions;
        /// <summary>
        /// Flag for indicating if learning thread is paused
        /// </summary>
        [NonSerialized]
        private volatile bool pausedLearning = false;

        /// <summary>
        /// Creates new instance of IterativeLearning learning algorithm
        /// </summary>
        public IterativeLearning() : base() {
            this.stopConditions = new List<StopCondition>();
        }

        /// <summary>
        /// Returns learning rate for this algorithm
        /// </summary>
        /// <returns> learning rate for this algorithm </returns>
        public virtual double LearningRate {
            get {
                return this.learningRate;
            }
            set {
                this.learningRate = value;
            }
        }


        /// <summary>
        /// Sets iteration limit for this learning algorithm
        /// </summary>
        /// <param name="maxIterations"> iteration limit for this learning algorithm </param>
        public virtual int MaxIterations {
            set {
                if (value > 0) {
                    this.maxIterations = value;
                    this.iterationsLimited = true;
                }
            }
            get {
                return maxIterations;
            }
        }


        public virtual bool IterationsLimited {
            get {
                return iterationsLimited;
            }
        }

        /// <summary>
        /// Returns current iteration of this learning algorithm
        /// </summary>
        /// <returns> current iteration of this learning algorithm </returns>
        public virtual int CurrentIteration {
            get {
                return this.currentIteration; // why boxed integer here?
            }
        }

        /// <summary>
        /// Returns true if learning thread is paused, false otherwise
        /// </summary>
        /// <returns> true if learning thread is paused, false otherwise </returns>
        public virtual bool PausedLearning {
            get {
                return pausedLearning;
            }
        }

        /// <summary>
        /// Pause the learning
        /// </summary>
        public virtual void pause() {
            this.pausedLearning = true;
        }

        /// <summary>
        /// Resumes the paused learning
        /// </summary>
        public virtual void resume() {
            this.pausedLearning = false;
            lock (this) {
                Monitor.Pulse(this);
            }
        }

        /// <summary>
        /// This method is executed when learning starts, before the first epoch.
        /// Used for initialisation.
        /// </summary>
        protected internal override void onStart() {
            base.onStart();

            if (this.iterationsLimited) {
                this.stopConditions.Add(new MaxIterationsStop(this));
            }

            this.currentIteration = 0;
        }

        protected internal virtual void beforeEpoch() {
        }

        protected internal virtual void afterEpoch() {
        }

        public override sealed void learn(DataSet trainingSet) {
            TrainingSet = trainingSet; // set this field here su subclasses can access it
            onStart();

            while (!Stopped) {
                beforeEpoch();
                doLearningEpoch(trainingSet);
                this.currentIteration++;
                afterEpoch();

                // now check if stop condition is satisfied
                if (hasReachedStopCondition()) {
                    stopLearning();
                } else if (!iterationsLimited && (currentIteration == int.MaxValue)) {
                    // if counter has reached max value and iteration number is not limited restart iteration counter
                    this.currentIteration = 1;
                }

                // notify listeners that epoch has ended
                fireLearningEvent(new LearningEvent(this, LearningEvent.Type.EPOCH_ENDED));

                // Thread safe pause when learning is paused
                if (this.pausedLearning) {
                    lock (this) {
                        while (this.pausedLearning) {
                            try {
                                Monitor.Wait(this);
                            } catch (Exception) {
                            }
                        }
                    }
                }

            }
            onStop();
            fireLearningEvent(new LearningEvent(this, LearningEvent.Type.LEARNING_STOPPED));
        }

        protected internal virtual bool hasReachedStopCondition() {
            foreach (StopCondition stop in stopConditions) {
                if (stop.Reached) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Trains network for the specified training set and number of iterations
        /// </summary>
        /// <param name="trainingSet"> training set to learn </param>
        /// <param name="maxIterations"> maximum numberof iterations to learn
        ///  </param>
        public virtual void learn(DataSet trainingSet, int maxIterations) {
            this.MaxIterations = maxIterations;
            this.learn(trainingSet);
        }

        /// <summary>
        /// Runs one learning iteration for the specified training set and notfies
        /// observers. This method does the the doLearningEpoch() and in addtion
        /// notifes observrs when iteration is done.
        /// </summary>
        /// <param name="trainingSet"> training set to learn </param>
        public virtual void doOneLearningIteration(DataSet trainingSet) {
            beforeEpoch();
            doLearningEpoch(trainingSet);
            afterEpoch();
            // notify listeners        
            fireLearningEvent(new LearningEvent(this, LearningEvent.Type.LEARNING_STOPPED));
        }

        /// <summary>
        /// Override this method to implement specific learning epoch - one learning
        /// iteration, one pass through whole training set
        /// </summary>
        /// <param name="trainingSet"> training set </param>
        public abstract void doLearningEpoch(DataSet trainingSet);
    }
}