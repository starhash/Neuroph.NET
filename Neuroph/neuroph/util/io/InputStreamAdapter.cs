
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
	/// Implementation of InputAdapter interface for reading neural network inputs from input stream. </summary>
	/// <seealso cref= InputAdapter
	/// @author Zoran Sevarac <sevarac@gmail.com> </seealso>
	public class InputStreamAdapter : InputAdapter
	{

		protected internal BufferedReader bufferedReader;

		public InputStreamAdapter(InputStream inputStream)
		{
			bufferedReader = new BufferedReader(new InputStreamReader(inputStream));
		}

		public InputStreamAdapter(BufferedReader bufferedReader)
		{
			this.bufferedReader = bufferedReader;
		}

		public virtual double[] readInput()
		{
			try
			{
				string inputLine = bufferedReader.readLine();
				if (inputLine != null)
				{
				   double[] inputBuffer = VectorParser.parseDoubleArray(inputLine);
				   return inputBuffer;
				}
				return null;
			}
			catch (IOException ex)
			{
				 throw new NeurophInputException("Error reading input from stream!", ex);
			}
		}

		public virtual void close()
		{
			try
			{
				if (bufferedReader != null)
				{
					bufferedReader.close();
				}
			}
			catch (IOException ex)
			{
				throw new NeurophInputException("Error closing stream!", ex);
			}
		}

	}
}