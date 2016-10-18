using System;
using System.Collections.Generic;

/// <summary>
/// Copyright 2013 Neuroph Project http://neuroph.sourceforge.net
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

namespace org.neuroph.util.data.sample
{


	using DataSet = org.neuroph.core.data.DataSet;

	/// <summary>
	/// This class provides subsampling of a data set, and creates a specified number of subsets of a
	/// specified number of samples form given data set.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class SubSampling : Sampling
	{


		/// <summary>
		/// Number of sub sets
		/// </summary>
		private int subSetCount;

		/// <summary>
		/// Sizes of each subset
		/// </summary>
		private int[] subSetSizes;

		/// <summary>
		/// True if samples are allowed to repeat in different subsets
		/// </summary>
		private bool allowRepetition = false;


		/// <summary>
		/// Sampling will produce a specified number of subsets of equal sizes
		/// Handy for K Fold subsampling
		/// </summary>
		/// <param name="subSetCount"> number of subsets to produce </param>
		public SubSampling(int subSetCount) // without repetition
		{
			this.subSetCount = subSetCount;
			this.subSetSizes = null;
		}


		/// <summary>
		/// Sampling will produce subsets of specified sizes (in percents)
		/// </summary>
		/// <param name="subSetSizes"> size of subsets in percents </param>
		public SubSampling(params int[] subSetSizes) // without repetition
		{
			// sum of these must be 100%???
			 this.subSetSizes = subSetSizes;
			 this.subSetCount = subSetSizes.Length;
		}


		public virtual List<DataSet> sample(DataSet dataSet)
		{
			if (subSetSizes == null) // if number of subSetSizes is null calculate it based on subSetSount
			{
				int singleSubSetSize = dataSet.size() / subSetCount;
				subSetSizes = new int[subSetCount];
				for (int i = 0; i < subSetCount; i++)
				{
				   subSetSizes[i] = singleSubSetSize;
				}
			}

			List<DataSet> subSets = new List<DataSet>();

			// shuffle dataset in order to randomize rows that will be used to fill subsets
			dataSet.shuffle();

			int inputSize = dataSet.InputSize;
			int outputSize = dataSet.OutputSize;

			int idxCounter = 0;
			for (int s = 0; s < subSetSizes.Length; s++)
			{
				// create new sample subset
				DataSet newSubSet = new DataSet(inputSize, outputSize);
				// fill subset with rows

				if (!allowRepetition)
				{
					int itemCount = (int)((double)subSetSizes[s] / 100 * dataSet.size());
					for (int i = 0; i < itemCount; i++)
					{
						newSubSet.addRow(dataSet.getRowAt(idxCounter));
						idxCounter++;
					}
				}
				else
				{
					int randomIdx;
					Random rand = new Random();
					for (int i = 0; i < subSetSizes[s] / 100 * dataSet.size(); i++)
					{
						randomIdx = rand.Next(dataSet.size());
						newSubSet.addRow(dataSet.getRowAt(randomIdx));
						idxCounter++;
					}
				}
				// add subset to th elist of subsets to return
				subSets.Add(newSubSet);
			}

			return subSets;
		}

		/// <summary>
		/// Get flag which indicates if sample repetition is allowed in subsets </summary>
		/// <returns>  </returns>
		public virtual bool AllowRepetition
		{
			get
			{
				return allowRepetition;
			}
			set
			{
				this.allowRepetition = value;
			}
		}




	}

}