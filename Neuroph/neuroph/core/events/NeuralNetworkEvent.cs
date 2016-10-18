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

namespace org.neuroph.core.events {

    using org.neuroph.core;

    /// <summary>
    /// This class holds information about the source and type of some neural network event.
    /// @author Zoran Sevarac <sevarac@gmail.com>
    /// </summary>
    public class NeuralNetworkEvent : java.util.EventObject {

        internal NeuralNetworkEventType eventType;

        public NeuralNetworkEvent(object source, NeuralNetworkEventType eventType) : base(source) {
            this.eventType = eventType;
        }

        public NeuralNetworkEvent(Layer source, NeuralNetworkEventType eventType) : base(source) {
            this.eventType = eventType;
        }

        public NeuralNetworkEvent(Neuron source, NeuralNetworkEventType eventType) : base(source) {
            this.eventType = eventType;
        }

        public virtual NeuralNetworkEventType EventType {
            get {
                return eventType;
            }
        }

        /// <summary>
        /// Types of neural network events
        /// </summary>
        public enum NeuralNetworkEventType {
            CALCULATED,
            LAYER_ADDED,
            LAYER_REMOVED,
            NEURON_ADDED,
            NEURON_REMOVED,
            CONNECTION_ADDED,
            CONNECTION_REMOVED
        }
    }
}