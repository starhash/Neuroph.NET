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
	/// This class provides ranged weights randomizer, which randomize weights in specified [min, max] range.
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class RangeRandomizer : WeightsRandomizer
	{
		/// <summary>
		/// Lower range limit
		/// </summary>
		protected internal double min;

		/// <summary>
		/// Upper range limit
		/// </summary>
		protected internal double max;

		/// <summary>
		/// Creates a new instance of RangeRandomizer within specified .
		/// The random values are generated according to formula:
		/// newValue = min + random * (max - min) </summary>
		/// <param name="min"> min weight value </param>
		/// <param name="max"> max weight value </param>
		public RangeRandomizer(double min, double max)
		{
			this.max = max;
			this.min = min;
		}

		/// <summary>
		/// Generates next random value within [min, max] range determined by the settings in this randomizer </summary>
		/// <returns> next weight random value </returns>
		protected internal override double nextRandomWeight()
		{
			return min + randomGenerator.NextDouble() * (max - min);
		}
	}

}