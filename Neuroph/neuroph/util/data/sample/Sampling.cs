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
	/// Interface for data set sampling  methods.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public interface Sampling
	{

		List<DataSet> sample(DataSet dataSet);

	}

}