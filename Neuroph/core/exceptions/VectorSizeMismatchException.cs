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

namespace org.neuroph.core.exceptions {

    /// <summary>
    /// Thrown to indicate that vector size does not match the network input or training element size.
    /// 
    /// @author Jon Tain
    /// @author Zoran Sevarac
    /// </summary>
    public class VectorSizeMismatchException : NeurophException {

        private const long serialVersionUID = 2L;

        /// <summary>
        /// Constructs an VectorSizeMismatchException with no detail message.
        /// </summary>
        public VectorSizeMismatchException() : base() {
        }

        /// <summary>
        /// Constructs an VectorSizeMismatchException with the specified detail message. </summary>
        /// <param name="message"> the detail message. </param>
        public VectorSizeMismatchException(string message) : base(message) {
        }

        /// <summary>
        /// Constructs a VectorSizeMismatchException with the specified detail message and specified cause. </summary>
        /// <param name="message"> the detail message. </param>
        /// <param name="cause"> the cause for exception </param>
        public VectorSizeMismatchException(string message, Exception cause) : base(message, cause) {
        }

        /// <summary>
        /// Constructs a new runtime exception with the specified cause </summary>
        /// <param name="cause"> the cause for exception </param>
        public VectorSizeMismatchException(Exception cause) : base(cause) {
        }
    }

}