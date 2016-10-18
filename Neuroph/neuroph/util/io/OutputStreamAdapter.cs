using java.io;
using System.Text;

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
	/// Implementation of OutputAdapter interface for writing neural network outputs to output stream. </summary>
	/// <seealso cref= OutputAdapter
	/// @author Zoran Sevarac <sevarac@gmail.com> </seealso>
	public class OutputStreamAdapter : OutputAdapter
	{

		protected internal BufferedWriter bufferedWriter;

		/// <summary>
		/// Creates a new OutputStreamAdapter for specified output stream.
		/// </summary>
		public OutputStreamAdapter(OutputStream outputStream)
		{
			bufferedWriter = new BufferedWriter(new OutputStreamWriter(outputStream));
		}

		/// <summary>
		/// Creates a new OutputStreamAdapter for specified BufferedWriter.
		/// </summary>
		public OutputStreamAdapter(BufferedWriter bufferedWriter)
		{
			this.bufferedWriter = bufferedWriter;
		}

		/// <summary>
		/// Writes specified output to output stream </summary>
		/// <param name="output"> output vector to write </param>
		public virtual void writeOutput(double[] output)
		{
			try
			{
				StringBuilder outputLine = new StringBuilder();
				for (int i = 0; i < output.Length; i++)
				{
					outputLine.Append(output[i]).Append(' ').Append(outputLine);
				}
				outputLine.Append(java.lang.System.lineSeparator());

				bufferedWriter.write(outputLine.ToString());
			}
			catch (IOException ex)
			{
				throw new NeurophOutputException("Error writing output to stream!", ex);
			}
		}

		/// <summary>
		/// Closes output stream.
		/// </summary>
		public virtual void close()
		{
			try
			{
				bufferedWriter.close();
			}
			catch (IOException ex)
			{
				throw new NeurophOutputException("Error closing output stream!", ex);
			}
		}
	}

}