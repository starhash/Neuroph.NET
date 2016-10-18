using java.util;
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

namespace org.neuroph.util
{


	/// <summary>
	/// Provides methods to parse strings as Integer or Double vectors.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	// rename to ArrayUtils
	public class VectorParser
	{

		/// <summary>
		/// This method parses input String and returns Integer vector
		/// </summary>
		/// <param name="str">
		///            input String </param>
		/// <returns> Integer vector </returns>
		public static List<int> parseInteger(string str)
		{
			StringTokenizer tok = new StringTokenizer(str);
			List<int> ret = new List<int>();
			while (tok.hasMoreTokens())
			{
				int d = Convert.ToInt32(tok.nextToken());
				ret.Add(d);
			}
			return ret;
		}

		/// <summary>
		/// This method parses input String and returns double array
		/// </summary>
		/// <param name="inputStr">
		///            input String </param>
		/// <returns> double array </returns>
		public static double[] parseDoubleArray(string inputStr)
		{
			string[] inputsArrStr = inputStr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			double[] ret = new double[inputsArrStr.Length];
			for (int i = 0; i < inputsArrStr.Length; i++)
			{
				ret[i] = Convert.ToDouble(inputsArrStr[i]);
			}

			return ret;
		}

		public static double[] toDoubleArray(List<double> list)
		{
			double[] ret = new double[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				ret[i] = (double)list[i];
			}
			return ret;
		}

		public static List<double> convertToVector(double[] array)
		{
			List<double> vector = new List<double>(array.Length);

			foreach (double val in array)
			{
				vector.Add(val);
			}

			return vector;
		}

	}
}