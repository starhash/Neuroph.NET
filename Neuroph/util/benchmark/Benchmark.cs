using System;
using System.Collections.Generic;

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
	/// This class is main benchmark driver. It holds collection of benchmarking tasks and provides benchmarking workflow.
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class Benchmark
	{
		internal List<BenchmarkTask> tasks;

		/// <summary>
		/// Creates a new Benchmark instance
		/// </summary>
		public Benchmark()
		{
			tasks = new List<BenchmarkTask>();
		}

		/// <summary>
		/// Adds specified benchmark task </summary>
		/// <param name="task"> benchmark task </param>
		public virtual void addTask(BenchmarkTask task)
		{
			tasks.Add(task);
		}

		/// <summary>
		/// Runs specified benchmark tasks, the basic benchmarking workflow.
		/// Prepares benchmark, run warming up iterations, measures the execution
		/// time for specified number of benchmarking iterations, and gets the benchmarking results </summary>
		/// <param name="task">  </param>
		public static void runTask(BenchmarkTask task)
		{
			System.Console.WriteLine("Preparing task " + task.Name);
			task.prepareTest();

			System.Console.WriteLine("Warming up " + task.Name);
			for (int i = 0; i < task.WarmupIterations; i++)
			{
				task.runTest();
			}

			System.Console.WriteLine("Runing " + task.Name);
			//task.prepare(); // ovde mozda poziv nekoj reset ili init metodi koja bi randomizovala mrezu, u osnovnoj klasi da neradi nista tako da moze da se redefinise i ne mora

			Stopwatch timer = new Stopwatch();
			BenchmarkTaskResults results = new BenchmarkTaskResults(task.TestIterations);

			for (int i = 0; i < task.TestIterations; i++)
			{
				timer.reset();

				timer.start();
				task.runTest();
				timer.stop();

                results.addElapsedTime((long)Math.Round(timer.ElapsedTime.TotalMilliseconds));
			}

			results.calculateStatistics();
			System.Console.WriteLine(task.Name + " results");
			System.Console.WriteLine(results); // could be sent to file
		}

		/// <summary>
		/// Runs all benchmark tasks
		/// </summary>
		public virtual void run()
		{
			for (int i = 0; i < tasks.Count; i++)
			{
				runTask(tasks[i]);
			}
		}

	}

}