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

namespace org.neuroph.core.input {


    /// <summary>
    /// Performs logic OR operation on input vector.
    /// 
    /// @author Zoran Sevarac <sevarac@gmail.com>
    /// </summary>
    [Serializable]
    public class Or : InputFunction {

        /// <summary>
        /// The class fingerprint that is set to indicate serialization
        /// compatibility with a previous version of the class.
        /// </summary>
        private const long serialVersionUID = 1L;

        /// <param name="inputVector"> Input values >= 0.5d are considered true, otherwise false. </param>
        public override double getOutput(List<Connection> inputConnections) {
            if (inputConnections.Count == 0) {
                return 0d;
            }

            bool output = false;

            foreach (Connection connection in inputConnections) {
                output = output || (connection.Input >= 0.5d);
            }

            return output ? 1d : 0d;
        }

    }

}