using java.io;
using System;
using System.Collections;

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

namespace org.neuroph.contrib.samples.stockmarket
{


	/// <summary>
	/// Provides method to reads stock data from the file.
	/// See http://neuroph.sourceforge.net/tutorials/StockMarketPredictionTutorial.html
	/// @author Dr.V.Steinhauer
	/// </summary>
	public class StockFileReader
	{

		private int maxCounter;
		private string[] valuesRow;

		public StockFileReader()
		{
			this.MaxCounter = 100;
		}

		public StockFileReader(int maxCounter)
		{
			this.MaxCounter = maxCounter;
		}

		public virtual string[] ValuesRow
		{
			get
			{
				return valuesRow;
			}
			set
			{
				this.valuesRow = value;
			}
		}


		public virtual int MaxCounter
		{
			get
			{
				return maxCounter;
			}
			set
			{
				this.maxCounter = value;
			}
		}


//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("static-access") public void read(String fileName)
		public virtual void read(string fileName)
		{
			Hashtable hm = new Hashtable();
			File file = new File(fileName);
			System.Console.WriteLine("file = " + fileName + ". It will be filtered the values for the moment of the market opened");
			int counter = 0;
			try
			{
				FileInputStream fis = new FileInputStream(file);
				BufferedReader dis = new BufferedReader(new InputStreamReader(fis));
				string s;
				while ((s = dis.readLine()) != null)
				{
					//System.out.println(s);
					string[] s1 = s.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
					string s00 = s1[0].Replace('\"', ' ').Trim();
					string s01 = s1[1].Replace('\"', ' ').Trim();
					hm[s00] = s.Replace('\"', ' ').Trim();
					//System.out.println(s00 + " " + s01);
					counter = counter + 1;
				}
				fis.close();
			}
			catch (IOException ioe)
			{
				System.Console.WriteLine("Oops- an IOException happened.");
				System.Console.WriteLine(ioe.ToString());
				System.Console.Write(ioe.StackTrace);
				Environment.Exit(1);
			}
			System.Console.WriteLine("full number of values = " + counter);
			ICollection sk = hm.Keys;
			IEnumerator i = sk.GetEnumerator();
			valuesRow = new string[this.MaxCounter];
			int n = 0;
			while (i.MoveNext())
			{
				string key = (string) i.Current;
				string value = (string) hm[key];
				//System.out.println(key + "->" + value);
				n = n + 1;
				if (counter - n < this.MaxCounter)
				{
					valuesRow[counter - n] = value;
					System.Console.WriteLine(counter + " " + n + " " + valuesRow[counter - n] + " " + (counter - n));
				}
			}
			System.Console.WriteLine("valuesRow.length=" + valuesRow.Length);
		}
	}

}