using System;
using System.Collections.Generic;
using org.neuroph.core.events;
using org.neuroph.util;

/// <summary>
/// Copyright 2014 Neuroph Project http://neuroph.sourceforge.net
/// 
/// Licensed under the Apache License, Version 2.0 (the "License"); you may not
/// use this file except in compliance with the License. You may obtain a copy of
/// the License at
/// 
/// http://www.apache.org/licenses/LICENSE-2.0
/// 
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
/// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
/// License for the specific language governing permissions and limitations under
/// the License.
/// </summary>
namespace org.neuroph.core {
    /// <summary>
    /// <pre>
    /// Layer of neurons in a neural network. The Layer is basic neuron container (a collection of neurons),
    /// and it provides methods for manipulating neurons (add, remove, get, set, calculate, ...).
    /// </pre>
    /// </summary>
    /// <seealso cref= Neuron
    /// @author Zoran Sevarac <sevarac@gmail.com> </seealso>
    [Serializable]
    public class Layer {


        /// <summary>
        /// The class fingerprint that is set to indicate serialization compatibility
        /// with a previous version of the class
        /// </summary>
        private const long serialVersionUID = 4L;

        /// <summary>
        /// Parent neural network - to which this layer belongs
        /// </summary>
        private core.NeuralNetwork parentNetwork;

        /// <summary>
        /// Collection of neurons in this layer
        /// </summary>
        protected internal List<Neuron> neurons;

        /// <summary>
        /// Label for this layer
        /// </summary>
        private string label;

        /// <summary>
        /// Creates an instance of empty Layer
        /// </summary>
        public Layer() {
            neurons = new List<Neuron>();
        }

        /// <summary>
        /// Creates an instance of empty Layer for specified number of neurons </summary>
        /// <param name="neuronsCount"> number of neurons in this layer </param>
        public Layer(int neuronsCount) {
            neurons = new List<Neuron>(neuronsCount);
        }

        /// <summary>
        /// Creates an instance of Layer with the specified number of neurons with
        /// specified neuron properties
        /// </summary>
        /// <param name="neuronsCount"> number of neurons in layer </param>
        /// <param name="neuronProperties"> properties of neurons in layer </param>
        public Layer(int neuronsCount, NeuronProperties neuronProperties) : this(neuronsCount) {

            for (int i = 0; i < neuronsCount; i++) {
                Neuron neuron = NeuronFactory.createNeuron(neuronProperties);
                this.addNeuron(neuron);
            }
        }

        /// <summary>
        /// Sets reference on parent network
        /// </summary>
        /// <param name="parent"> parent network </param>
        public NeuralNetwork ParentNetwork {
            set {
                this.parentNetwork = value;
            }
            get {
                return this.parentNetwork;
            }
        }


        /// <summary>
        /// Returns array neurons in this layer as array
        /// </summary>
        /// <returns> array of neurons in this layer </returns>
        public List<Neuron> Neurons {
            get {
                // return Collections.unmodifiableList(neurons);
                return neurons;
            }
        }

        /// <summary>
        /// Adds specified neuron to this layer
        /// </summary>
        /// <param name="neuron"> neuron to add </param>
        public void addNeuron(Neuron neuron) {
            // prevent adding null neurons
            if (neuron == null) {
                throw new System.ArgumentException("Neuron cant be null!");
            }

            // set neuron's parent layer to this layer 
            neuron.ParentLayer = this;

            // add new neuron at the end of the array
            neurons.Add(neuron);

            // notify network listeners that neuron has been added
            if (parentNetwork != null) {
                parentNetwork.fireNetworkEvent(new NeuralNetworkEvent(neuron, NeuralNetworkEvent.NeuralNetworkEventType.NEURON_ADDED));
            }
        }

        /// <summary>
        /// Adds specified neuron to this layer,at specified index position
        /// 
        /// Throws IllegalArgumentException if neuron is null, or index is
        /// illegal value (index<0 or index>neuronsCount)      
        /// </summary>
        /// <param name="neuron"> neuron to add </param>
        /// <param name="index"> index position at which neuron should be added </param>
        public void addNeuron(int index, Neuron neuron) {
            // prevent adding null neurons
            if (neuron == null) {
                throw new System.ArgumentException("Neuron cant be null!");
            }

            // add neuron to this layer
            neurons.Insert(index, neuron);

            // set neuron's parent layer to this layer
            neuron.ParentLayer = this;

            // notify network listeners that neuron has been added
            if (parentNetwork != null) {
                parentNetwork.fireNetworkEvent(new NeuralNetworkEvent(neuron, NeuralNetworkEvent.NeuralNetworkEventType.NEURON_ADDED));
            }
        }

        /// <summary>
        /// Sets (replace) the neuron at specified position in layer
        /// </summary>
        /// <param name="index"> index position to set/replace </param>
        /// <param name="neuron"> new Neuron object to set </param>
        public void setNeuron(int index, Neuron neuron) {
            // make sure that neuron is not null
            if (neuron == null) {
                throw new System.ArgumentException("Neuron cant be null!");
            }

            // new neuron at specified index position        
            neurons[index] = neuron;

            // set neuron's parent layer to this layer                        
            neuron.ParentLayer = this;

            // notify network listeners that neuron has been added
            if (parentNetwork != null) {
                parentNetwork.fireNetworkEvent(new NeuralNetworkEvent(neuron, NeuralNetworkEvent.NeuralNetworkEventType.NEURON_ADDED));
            }

        }

        /// <summary>
        /// Removes neuron from layer
        /// </summary>
        /// <param name="neuron"> neuron to remove </param>
        public void removeNeuron(Neuron neuron) {
            int index = IndexOf(neuron);
            removeNeuronAt(index);
        }

        /// <summary>
        /// Removes neuron at specified index position in this layer
        /// </summary>
        /// <param name="index"> index position of neuron to remove </param>
        public void removeNeuronAt(int index) {
            Neuron neuron = neurons[index];
            neuron.ParentLayer = null;
            neuron.removeAllConnections(); // why we're doing this here? maybe we shouldnt
            neurons.RemoveAt(index);

            // notify listeners that neuron has been removed
            if (parentNetwork != null) {
                parentNetwork.fireNetworkEvent(new NeuralNetworkEvent(this, NeuralNetworkEvent.NeuralNetworkEventType.NEURON_REMOVED));
            }
        }

        public void removeAllNeurons() {
            neurons.Clear();

            // notify listeners that neurons has been removed
            if (parentNetwork != null) {
                parentNetwork.fireNetworkEvent(new NeuralNetworkEvent(this, NeuralNetworkEvent.NeuralNetworkEventType.NEURON_REMOVED));
            }
        }

        /// <summary>
        /// Returns neuron at specified index position in this layer
        /// </summary>
        /// <param name="index"> neuron index position </param>
        /// <returns> neuron at specified index position </returns>
        public virtual Neuron getNeuronAt(int index) {
            return neurons[index];
        }

        /// <summary>
        /// Returns the index position in layer for the specified neuron
        /// </summary>
        /// <param name="neuron"> neuron object </param>
        /// <returns> index position of specified neuron </returns>
        public virtual int IndexOf(Neuron neuron) {
            return neurons.IndexOf(neuron);
        }

        /// <summary>
        /// Returns number of neurons in this layer
        /// </summary>
        /// <returns> number of neurons in this layer </returns>
        public virtual int NeuronsCount {
            get {
                return neurons.Count;
            }
        }

        // static final ForkJoinPool mainPool = new ForkJoinPool(Runtime.getRuntime().availableProcessors());

        /// <summary>
        /// Performs calculaton for all neurons in this layer
        /// </summary>
        public virtual void calculate() {

            //        for (Neuron neuron : this.neurons) { // use directly underlying array since its faster
            //            neuron.calculate();
            //        }
            //JAVA TO C# CONVERTER TODO TASK: Java lambdas satisfy functional interfaces, while .NET lambdas satisfy delegates - change the appropriate interface to a delegate:
            foreach (Neuron n in neurons) {
                n.calculate();
            }

            //        mainPool.invokeAll(Arrays.asList(neurons.asArray()));
        }

        /// <summary>
        /// Resets the activation and input levels for all neurons in this layer
        /// </summary>
        public virtual void reset() {
            foreach (Neuron neuron in this.neurons) {
                neuron.reset();
            }
        }

        /// <summary>
        /// Initialize connection weights for the whole layer to to specified value
        /// </summary>
        /// <param name="value"> the weight value </param>
        public virtual void initializeWeights(double value) {
            foreach (Neuron neuron in this.neurons) {
                neuron.initializeWeights(value);
            }
        }

        /// <summary>
        /// Get layer label
        /// </summary>
        /// <returns> layer label </returns>
        public virtual string Label {
            get {
                return label;
            }
            set {
                this.label = value;
            }
        }


        public virtual bool Empty {
            get {
                return neurons.Count == 0;
            }
        }

    }
}