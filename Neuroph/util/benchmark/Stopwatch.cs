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
	/// A class to help benchmark code, it simulates a real stop watch. </summary>
	/// <seealso cref= <a href="http://java.sun.com/docs/books/performance/1st_edition/html/JPMeasurement.fm.html#17818">http://java.sun.com/docs/books/performance/1st_edition/html/JPMeasurement.fm.html#17818</a> </seealso>
	public class Stopwatch
	{

		private System.DateTime startTime = new System.DateTime(-1);
		private System.DateTime stopTime = new System.DateTime(-1);
		private bool running = false;


		public Stopwatch()
		{
		}


		/// <summary>
		/// Starts measuring time
		/// </summary>
		public virtual void start()
		{
			startTime = System.DateTime.Now;
			running = true;
		}

		/// <summary>
		/// Stops measuring time
		/// </summary>
		public virtual void stop()
		{
            stopTime = System.DateTime.Now;
			running = false;
		}

		/// <summary>
		/// Returns elapsed time in milliseconds between calls to start and stop methods
		/// If the watch has never been started, returns zero
		/// </summary>
		public virtual System.TimeSpan ElapsedTime
		{
			get
			{
				if (startTime == new System.DateTime(-1))
				{
					return new System.TimeSpan(0);
				}
				if (running)
				{
                    return System.DateTime.Now.Subtract(startTime);
				}
				else
				{
					return stopTime.Subtract(startTime);
				}
			}
		}

		/// <summary>
		/// Resets the stopwatch (clears start and stop time)
		/// </summary>
		public virtual void reset()
		{
            startTime = new System.DateTime(-1);
			stopTime = new System.DateTime(-1);
			running = false;
		}
	}
}