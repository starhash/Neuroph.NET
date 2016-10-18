﻿using System;

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

namespace org.neuroph.contrib.samples.timeseries
{

	/// <summary>
	/// Very simple class which just wraps up the generation of a sine wave at one degree steps.
	/// Just call getNextSample each time you need a new sample. Class internally tracks the angle.
	/// See http://neuroph.sourceforge.net/TimeSeriesPredictionTutorial.html
	/// 
	/// @author Laura Ellen Carter-Greaves
	/// </summary>

	public class GenerateSineWave
	{

		private double frequency = 1;
		private double amplitude = 1;
		private double angle = 0;
		private static double oneDegree = Math.PI / 180;

			/// <summary>
			/// Constructor for sine wave generator with specified frequency and amplitude </summary>
			/// <param name="frequency"> sine wave frequency </param>
			/// <param name="amplitude"> sine wave amplitude </param>
		internal GenerateSineWave(double frequency, double amplitude)
		{
			this.frequency = frequency;
			this.amplitude = amplitude;
		}
        

        public void GenerateNextSample() {
            double d = NextSample;
        }
			/// <summary>
			/// Gets next sampling value for the sine wave </summary>
			/// <returns> next sampling value for the sine wave </returns>
		public virtual double NextSample
		{
			get
			{
				angle += oneDegree;
				return Math.Sin(frequency * angle) * amplitude;
			}
		}
	}

}