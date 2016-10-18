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

namespace org.neuroph.util.plugins {

    using org.neuroph.core;

    /// <summary>
    /// Base class for all neural network plugins.
    /// 
    /// @author Zoran Sevarac <sevarac@gmail.com>
    /// </summary>
    [Serializable]
    public class PluginBase {

        /// <summary>
        /// The class fingerprint that is set to indicate serialization
        /// compatibility with a previous version of the class.
        /// </summary>
        private const long serialVersionUID = 1L;

        /// <summary>
        /// Name for this plugin
        /// </summary>
        private string name;

        /// <summary>
        /// Reference to parent neural network
        /// </summary>
        //JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
        //ORIGINAL LINE: private org.neuroph.core.NeuralNetwork<?> parentNetwork;
        private NeuralNetwork parentNetwork;


        public PluginBase() {
        }


        /// <summary>
        /// Creates an instance of plugin for neural network
        /// </summary>
        public PluginBase(string name) {
            this.name = name;
        }

        /// <summary>
        /// Returns the name of this plugin </summary>
        /// <returns> name of this plugin </returns>
        public virtual string Name {
            get {
                return this.name;
            }
        }

        /// <summary>
        /// Returns the parent network for this plugin </summary>
        /// <returns> parent network for this plugin </returns>
        //JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
        //ORIGINAL LINE: public org.neuroph.core.NeuralNetwork<?> getParentNetwork()
        public virtual NeuralNetwork ParentNetwork {
            get {
                return parentNetwork;
            }
            set {
                this.parentNetwork = value;
            }
        }


    }

}