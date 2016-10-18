using System;

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

namespace org.neuroph.imgrec
{

	/// <summary>
	/// This exception is thrown when image vector size is different than
	/// input vector size which is accepted by neural network
	/// @author Zoran Sevarac
	/// </summary>
	public class ImageSizeMismatchException : Exception
	{

		private const long serialVersionUID = 1L;

		public ImageSizeMismatchException() : base()
		{
		}

		public ImageSizeMismatchException(string message) : base(message)
		{
		}

		public ImageSizeMismatchException(string message, Exception arg1) : base(message, arg1)
		{
		}

		public ImageSizeMismatchException(Exception arg0) : base(arg0)
		{
		}
	}


}