
using java.io;
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
	/// Implementation of OutputAdapter interface for writing neural network outputs to URL. </summary>
	/// <seealso cref= OutputAdapter
	/// @author Zoran Sevarac <sevarac@gmail.com> </seealso>
	public class URLOutputAdapter : OutputStreamAdapter
	{

		/// <summary>
		/// Creates a new URLOutputAdapter by opening a connection to URL specified by the url input param </summary>
		/// <param name="url"> URL object to connect to. </param>
		/// <exception cref="IOException"> if connection  </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public URLOutputAdapter(java.net.URL url) throws java.io.IOException
		public URLOutputAdapter(java.net.URL url) : base(new BufferedWriter(new OutputStreamWriter(url.openConnection().getOutputStream())))
		{
		}

		/// <summary>
		/// Creates a new URLOutputAdapter by opening a connection to URL specified by the string url input param </summary>
		/// <param name="url"> URL to connect to as string. </param>
		/// <exception cref="IOException"> if connection  </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public URLOutputAdapter(String url) throws java.net.MalformedURLException, java.io.IOException
		public URLOutputAdapter(string url) : this(new java.net.URL(url))
		{
		}
	}
}