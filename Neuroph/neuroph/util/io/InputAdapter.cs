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

namespace org.neuroph.util.io
{

	/// <summary>
	/// Interface for reading neural network inputs from various data sources.
	/// Provides methods to read from and close data source.
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public interface InputAdapter
	{

		/// <summary>
		/// Reads input from data source and returns input for neural network as array of doubles. </summary>
		/// <returns> neural network input as array of doubles </returns>
		double[] readInput();

		/// <summary>
		/// Close data source after reading is finnished.
		/// Free resources and perform cleanup.
		/// </summary>
		void close();
	}

}