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

namespace org.neuroph.util.benchmark
{

	/// <summary>
	/// This class holds benchmarking results, elapsed times for all iterations and 
	/// various statistics min, max, avg times and standard deviation
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class BenchmarkTaskResults
	{

		internal int testIterations;
		internal long[] elapsedTimes;
		internal int timesCounter;
		internal double averageTestTime;
		internal double standardDeviation;
		internal double minTestTime;
		internal double maxTestTime;

		public BenchmarkTaskResults(int testIterations)
		{
			this.testIterations = testIterations;
			this.elapsedTimes = new long[testIterations];
			this.timesCounter = 0;
		}

		public virtual double AverageTestTime
		{
			get
			{
				return averageTestTime;
			}
		}

		public virtual long[] ElapsedTimes
		{
			get
			{
				return elapsedTimes;
			}
		}

		public virtual double MaxTestTime
		{
			get
			{
				return maxTestTime;
			}
		}

		public virtual double MinTestTime
		{
			get
			{
				return minTestTime;
			}
		}

		public virtual double StandardDeviation
		{
			get
			{
				return standardDeviation;
			}
		}

		public virtual int TestIterations
		{
			get
			{
				return testIterations;
			}
		}

		public virtual void addElapsedTime(long time)
		{
			this.elapsedTimes[timesCounter++] = time;
		}

		public virtual void calculateStatistics()
		{

			this.minTestTime = elapsedTimes[0];
			this.maxTestTime = elapsedTimes[0];
			long sum = 0;

			for (int i = 0; i < timesCounter; i++)
			{
				sum += elapsedTimes[i];
				if (elapsedTimes[i] < minTestTime)
				{
					minTestTime = elapsedTimes[i];
				}
				if (elapsedTimes[i] > maxTestTime)
				{
					maxTestTime = elapsedTimes[i];
				}
			}

			this.averageTestTime = sum / (double) timesCounter;

			//  std. deviation
			long sqrSum = 0;

			for (int i = 0; i < timesCounter; i++)
			{
				sqrSum += (long)((elapsedTimes[i] - averageTestTime) * (elapsedTimes[i] - averageTestTime));
			}

			this.standardDeviation = Math.Sqrt(sqrSum / (double)timesCounter);
		}

		public override string ToString()
		{
			string results = "Test iterations: " + testIterations + "\n" + "Min time: " + minTestTime + " ms\n" + "Max time: " + maxTestTime + " ms\n" + "Average time: " + averageTestTime + " ms\n" + "Std. deviation: " + standardDeviation + "\n";

			results += "Test times:\n";

			for (int i = 0; i < timesCounter; i++)
			{
				results += (i + 1) + ". iteration: " + elapsedTimes[i] + "ms\n";
			}

			return results;
		}
	}

}