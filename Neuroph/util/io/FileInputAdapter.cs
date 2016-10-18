
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
	/// Implementation of InputAdapter interface for reading neural network inputs from files. </summary>
	/// <seealso cref= InputAdapter
	/// @author Zoran Sevarac <sevarac@gmail.com> </seealso>
	public class FileInputAdapter : InputStreamAdapter
	{

		/// <summary>
		/// Creates a new FileInputAdapter by opening a connection to an actual file,
		/// specified by the file param </summary>
		/// <param name="file"> File object in the file system </param>
		/// <exception cref="FileNotFoundException"> if specified file was not found </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public FileInputAdapter(java.io.File file) throws java.io.FileNotFoundException
		public FileInputAdapter(File file) : base(new BufferedReader(new FileReader(file)))
		{
		}

		/// <summary>
		/// Creates a new FileInputAdapter by opening a connection to an actual file,
		/// specified by the fileName param </summary>
		/// <param name="fileName"> name of the file in file system </param>
		/// <exception cref="FileNotFoundException"> if specified file was not found </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public FileInputAdapter(String fileName) throws java.io.FileNotFoundException
		public FileInputAdapter(string fileName) : this(new File(fileName))
		{
		}

	}
}