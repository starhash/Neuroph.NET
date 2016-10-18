using System;
using System.Collections;
using System.Collections.Generic;

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

namespace org.neuroph.core.learning
{

	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;
	using LearningEvent = org.neuroph.core.events.LearningEvent;
	using LearningEventListener = org.neuroph.core.events.LearningEventListener;
	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	/// <summary>
	/// Base class for all neural network learning algorithms. It provides the
	/// general principles for training neural network.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	[Serializable]
	public abstract class LearningRule
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization compatibility
		/// with a previous version of the class
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Neural network to train
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
//ORIGINAL LINE: protected org.neuroph.core.NeuralNetwork<?> neuralNetwork;
		protected internal NeuralNetwork neuralNetwork { get; set; }

		/// <summary>
		/// Training data set
		/// </summary>
		[NonSerialized]
		private DataSet trainingSet;

		/// <summary>
		/// Flag to stop learning
		/// </summary>
		[NonSerialized]
		private volatile bool stopLearning_Renamed = false;

        /// <summary>
        /// List of learning rule listeners
        /// </summary>
        [NonSerialized]
        protected internal List<LearningEventListener> listeners = new List<LearningEventListener>();


		private readonly Logger LOGGER = LoggerFactory.getLogger(typeof(LearningRule));

		/// <summary>
		/// Creates new instance of learning rule
		/// </summary>
		public LearningRule()
		{
		}

		/// <summary>
		/// Sets training set for this learning rule
		/// </summary>
		/// <param name="trainingSet"> training set for this learning rule </param>
		public virtual DataSet TrainingSet
		{
			set
			{
				this.trainingSet = value;
			}
			get
			{
				return trainingSet;
			}
		}


		/// <summary>
		/// Gets neural network
		/// </summary>
		/// <returns> neural network </returns>
		public virtual NeuralNetwork NeuralNetwork
		{
			get
			{
				return neuralNetwork;
			}
			set
			{
				this.neuralNetwork = value;
			}
		}


		/// <summary>
		/// Prepares the learning rule to run by setting stop flag to false
		/// If you override this method make sure you call parent method first
		/// </summary>
		protected internal virtual void onStart()
		{
			this.stopLearning_Renamed = false;
	//        LOGGER.info("Learning Started");
		}

		/// <summary>
		/// Invoked after the learning has stopped
		/// </summary>
		protected internal virtual void onStop()
		{
	//        LOGGER.info("Learning Stoped");
		}

		/// <summary>
		/// Stops learning
		/// </summary>
		public virtual void stopLearning()
		{
			lock (this)
			{
				// note: as long as all this method does is assign stopLearning, it doesn't need to be synchronized if stopLearning is a VOLATILE field. - Jon Tait 6-19-2010
				this.stopLearning_Renamed = true;
			}
		}

		/// <summary>
		/// Returns true if learning has stopped, false otherwise
		/// </summary>
		/// <returns> true if learning has stopped, false otherwise </returns>
		public virtual bool Stopped
		{
			get
			{
				lock (this)
				{
					// note: as long as all this method does is return stopLearning, it doesn't need to be synchronized if stopLearning is a VOLATILE field. - Jon Tait 6-19-2010
					return this.stopLearning_Renamed;
				}
			}
		}

		// This methods allows classes to register for LearningEvents
		public virtual void addListener(LearningEventListener listener)
		{
			lock (this)
			{
				if (listener == null)
				{
					throw new System.ArgumentException("listener is null!");
				}
        
				if (!listeners.Contains(listener))
				{
					listeners.Add(listener);
				}
			}
		}

		// This methods allows classes to unregister for LearningEvents
		public virtual void removeListener(LearningEventListener listener)
		{
			lock (this)
			{
				if (listener == null)
				{
					throw new System.ArgumentException("listener is null!");
				}
        
				listeners.Remove(listener);
			}
		}

		// This private class is used to fire LearningEvents
		protected internal virtual void fireLearningEvent(LearningEvent evt)
		{
			lock (this)
			{
				foreach (LearningEventListener listener in listeners)
				{
				  listener.handleLearningEvent(evt);
				}
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private void readObject(java.io.ObjectInputStream in) throws java.io.IOException, ClassNotFoundException
		private void readObject(java.io.ObjectInputStream @in)
		{
			@in.defaultReadObject();
            listeners = new List<LearningEventListener>();
		}

		/// <summary>
		/// Override this method to implement specific learning procedures
		/// </summary>
		/// <param name="trainingSet"> training set </param>
		public abstract void learn(DataSet trainingSet);
	}
}