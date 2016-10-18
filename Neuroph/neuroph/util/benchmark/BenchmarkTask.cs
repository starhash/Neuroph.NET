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
	/// This class is an abstract base class for specific microbenchmarking tasks
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public abstract class BenchmarkTask
	{

		private string name;
		private int warmupIterations = 1;
		private int testIterations = 1;

		/// <summary>
		/// Creates a new instance of BenchmarkTask with specified name </summary>
		/// <param name="name"> benchmark task name </param>
		public BenchmarkTask(string name)
		{
			this.name = name;
		}

		/// <summary>
		/// Gets task name </summary>
		/// <returns> task name </returns>
		public virtual string Name
		{
			get
			{
				return name;
			}
			set
			{
				this.name = value;
			}
		}


		/// <summary>
		/// Gets number of test (benchmarking) iterations </summary>
		/// <returns> number of test iterations </returns>
		public virtual int TestIterations
		{
			get
			{
				return testIterations;
			}
			set
			{
				this.testIterations = value;
			}
		}


		/// <summary>
		/// Gets number of warmup iterations.
		/// Warmup iterations are used to run test for some time to stabilize JVM (compiling, optimizations, gc) </summary>
		/// <returns> number of warmup iterations  </returns>
		public virtual int WarmupIterations
		{
			get
			{
				return warmupIterations;
			}
			set
			{
				this.warmupIterations = value;
			}
		}



		/// <summary>
		/// Any initialization before running performance test (benchmark) goes here
		/// </summary>
		public abstract void prepareTest();

		/// <summary>
		/// This method should hold the code to benchmark
		/// </summary>
		public abstract void runTest();
	}
}