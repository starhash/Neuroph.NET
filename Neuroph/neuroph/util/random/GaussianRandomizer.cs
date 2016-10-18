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

namespace org.neuroph.util.random
{

	/// <summary>
	/// This class provides Gaussian randomization technique using Box Muller method.
	/// Based on GaussianRandomizer from Encog
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class GaussianRandomizer : WeightsRandomizer
	{

		internal double mean;
		internal double standardDeviation;

		/// <summary>
		/// The y2 value.
		/// </summary>
		private double y2;

		/// <summary>
		/// Should we use the last value.
		/// </summary>
		private bool useLast = false;

		public GaussianRandomizer(double mean, double standardDeviation)
		{
			this.mean = mean;
			this.standardDeviation = standardDeviation;
		}

		/// <summary>
		/// Compute a Gaussian random number.
		/// </summary>
		/// <param name="mean">
		///            The mean. </param>
		/// <param name="std">
		///            The standard deviation. </param>
		/// <returns> The random number. </returns>
		 private double boxMuller(double mean, double std)
		 {
			double x1, x2, w, y1;

			// use value from previous call
			if (this.useLast)
			{
				y1 = this.y2;
				this.useLast = false;
			}
			else
			{
				do
				{
					x1 = 2.0 * randomGenerator.NextDouble() - 1.0;
					x2 = 2.0 * randomGenerator.NextDouble() - 1.0;
					w = x1 * x1 + x2 * x2;
				} while (w >= 1.0);

				w = Math.Sqrt((-2.0 * Math.Log(w)) / w);
				y1 = x1 * w;
				this.y2 = x2 * w;
				this.useLast = true;
			}

			return (mean + y1 * std);
		 }

		protected internal override double nextRandomWeight()
		{
			return boxMuller(mean, standardDeviation);
		}
	}

}