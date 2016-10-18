using java.io;
using System;
using System.Threading;

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
	/// Provides method to reads stock data from the socket.
	/// See http://neuroph.sourceforge.net/tutorials/StockMarketPredictionTutorial.html
	/// @author Dr.V.Steinhauer
	/// </summary>
	public class StockSocketReader : java.lang.Runnable {

		private int maxCounter;
		private int tsleep = 5000;
		private string[] valuesRow;

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


		public virtual int Tsleep
		{
			get
			{
				return tsleep;
			}
			set
			{
				this.tsleep = value;
			}
		}


		public StockSocketReader()
		{
			//this.setTsleep(10000);
			this.MaxCounter = 100;
		}

		public StockSocketReader(int maxCounter)
		{
			//this.setTsleep(10000);
			this.MaxCounter = maxCounter;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("static-access") public void run()
		public virtual void run()
		{
			valuesRow = new string[this.MaxCounter];
			for (int i = 0; i < this.MaxCounter; i++)
			{
				InputStream @is = null;
				try
				{
					string surl = "http://download.finance.yahoo.com/d/quotes.csv?s=^GDAXI&f=sl1d1t1c1ohgv&e=.csv";
                    java.net.URL url = new java.net.URL(surl);
					@is = url.openStream();
					BufferedReader dis = new BufferedReader(new InputStreamReader(@is));
					string s = dis.readLine();
                    System.Console.WriteLine(s);
					valuesRow[i] = s;
					@is.close();
				}
				catch (java.net.MalformedURLException mue)
				{
                    System.Console.WriteLine("Ouch - a MalformedURLException happened.");
                    System.Console.WriteLine(mue.ToString());
                    System.Console.Write(mue.StackTrace);
					Environment.Exit(1);
				}
				catch (IOException ioe)
				{
                    System.Console.WriteLine("Oops- an IOException happened.");
                    System.Console.WriteLine(ioe.ToString());
                    System.Console.Write(ioe.StackTrace);
					Environment.Exit(1);
				}
				try
				{
					Thread.Sleep(Tsleep);
				}
				catch (java.lang.InterruptedException)
				{
				}
			} //end of for
			System.Console.WriteLine("valuesRow.length=" + valuesRow.Length);

		}
	}

}