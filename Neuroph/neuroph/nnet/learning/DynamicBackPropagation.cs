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

namespace org.neuroph.nnet.learning
{

	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;


	/// <summary>
	/// Backpropagation learning rule with dynamic learning rate and momentum
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class DynamicBackPropagation : MomentumBackpropagation
	{
		private const long serialVersionUID = 1L;

		private double maxLearningRate = 0.9d;
		private double minLearningRate = 0.1d;
		private double learningRateChange = 0.99926d;
		private bool useDynamicLearningRate = true;

		private double maxMomentum = 0.9d;
		private double minMomentum = 0.1d;
		private double momentumChange = 0.99926d;
		private bool useDynamicMomentum = true;

	   // private double previousNetworkError;


			public DynamicBackPropagation() : base()
			{
			}


			// Adjusting learning rate dynamically
			/* If network error of current epoch is higher than the network error of the previous
			 * epoch the learning rate is adjusted by minus 1 per cent of current learning rate.
			 * Otherwise the learning rate is adjusted by plus 1 per cent of current learning
			 * rate. So, learning rate increases faster than decreasing does. But if learning rate
			 * reaches 0.9 it switches back to 0.5 to avoid endless training. The lowest learning
			 * rate is 0.5 also to avoid endless training.
			 */
			protected internal virtual void adjustLearningRate()
			{

				// 1. First approach - probably the best
				// bigger error -> smaller learning rate; minimize the error growth
				// smaller error -> bigger learning rate; converege faster
				// the amount of earning rate change is proportional to error change - by using errorChange

				double errorChange = this.previousEpochError - ErrorFunction.TotalError;
				this.learningRate = this.learningRate + (errorChange * learningRateChange);

				if (this.learningRate > this.maxLearningRate)
				{
				   this.learningRate = this.maxLearningRate;
				}

				if (this.learningRate < this.minLearningRate)
				{
				   this.learningRate = this.minLearningRate;
				}


	//            System.out.println("Learning rate: "+this.learningRate);

				// 2. Second approach
				// doing this lineary for each epoch considering network error behaviour
				// probbaly the worst one
	/*
	            if (this.totalNetworkError >= this.totalNetworkErrorInPreviousEpoch) {
	                this.learningRate = this.learningRate * this.learningRateChange;
	
	                if (this.learningRate < this.minLearningRate)
	                    this.learningRate = this.minLearningRate;
	
	            } else {
	                this.learningRate = this.learningRate * (1 + (1 - this.learningRateChange)); // *1.01
	
	                if (this.learningRate > this.maxLearningRate)
	                    this.learningRate = this.maxLearningRate;
	
	            }
	*/
	// third approach used by sharky nn
	// By default It starts with ni = 0,9, and after each epoch ni is changed by: 0,99977 ^ N
	//    where N is number of points, and ^ is power.
	// ni = ni * 0,99977 ^ N
	// this one drops the learning rate too fast
	//           this.learningRate = this.learningRate * Math.pow(learningRateChange, this.getTrainingSet().size());
	//            if (this.learningRate > this.maxLearningRate)
	//               this.learningRate = this.maxLearningRate;
	//
	//            if (this.learningRate < this.minLearningRate)
	//               this.learningRate = this.minLearningRate;

	  //          System.out.println("Iteration: "+currentIteration + " Learning rate: "+ this.learningRate);

				// one more approach suggested at https://sourceforge.net/tracker/?func=detail&atid=1107579&aid=3130561&group_id=238532
	//            if (this.totalNetworkError >= this.previousEpochError) {
	//            // If going wrong way, drop to minimum learning and work our way back up.
	//            // This way we accelerate as we improve.
	//            learningRate=minLearningRate;
	//            } else {
	//            this.learningRate = this.learningRate * (1 + (1 - this.learningRateChange)); // *1.01
	//
	//            if (this.learningRate > this.maxLearningRate)
	//            this.learningRate = this.maxLearningRate;
	//
	//            }

			}

			protected internal virtual void adjustMomentum()
			{
				double errorChange = this.previousEpochError - ErrorFunction.TotalError;
				this.momentum = this.momentum + (errorChange * momentumChange);

				if (this.momentum > this.maxMomentum)
				{
				   this.momentum = this.maxMomentum;
				}

				if (this.momentum < this.minMomentum)
				{
				   this.momentum = this.minMomentum;
				}

				// one more approach suggested at https://sourceforge.net/tracker/?func=detail&atid=1107579&aid=3130561&group_id=238532
				// Probably want to drop momentum to minimum value.
	//            if (this.totalNetworkError >= this.previousEpochError) {
	//                momentum = momentum * momentumChange;
	//                if (momentum < minMomentum) momentum = minMomentum;
	//            } else {
	//                momentum = momentum * (1 + (1 - momentumChange)); // *1.01
	//                if (momentum > maxMomentum) momentum = maxMomentum;
	//            }

			}

			public override void doLearningEpoch(DataSet trainingSet)
			{
			   base.doLearningEpoch(trainingSet);

			   if (currentIteration > 0)
			   {
						if (useDynamicLearningRate)
						{
							adjustLearningRate();
						}
						if (useDynamicMomentum)
						{
							adjustMomentum();
						}
			   }


			}

		public virtual double LearningRateChange
		{
			get
			{
				return learningRateChange;
			}
			set
			{
				this.learningRateChange = value;
			}
		}


		public virtual double MaxLearningRate
		{
			get
			{
				return maxLearningRate;
			}
			set
			{
				this.maxLearningRate = value;
			}
		}


		public virtual double MaxMomentum
		{
			get
			{
				return maxMomentum;
			}
			set
			{
				this.maxMomentum = value;
			}
		}


		public virtual double MinLearningRate
		{
			get
			{
				return minLearningRate;
			}
			set
			{
				this.minLearningRate = value;
			}
		}


		public virtual double MinMomentum
		{
			get
			{
				return minMomentum;
			}
			set
			{
				this.minMomentum = value;
			}
		}


		public virtual double MomentumChange
		{
			get
			{
				return momentumChange;
			}
			set
			{
				this.momentumChange = value;
			}
		}


		public virtual bool UseDynamicLearningRate
		{
			get
			{
				return useDynamicLearningRate;
			}
			set
			{
				this.useDynamicLearningRate = value;
			}
		}


		public virtual bool UseDynamicMomentum
		{
			get
			{
				return useDynamicMomentum;
			}
			set
			{
				this.useDynamicMomentum = value;
			}
		}






	}

}