using System;
using System.Collections;
using System.Collections.Generic;
using org.neuroph.core;
using org.neuroph.core.data;
using org.neuroph.core.events;
using org.neuroph.core.exceptions;
using org.neuroph.core.learning;
using org.neuroph.util;
using org.neuroph.util.plugins;
using org.neuroph.util.random;
using org.slf4j;
using java.io;
using java.lang;

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
    /// Base class for artificial neural networks. It provides generic structure and functionality
    /// for the neural networks. Neural network contains a collection of neuron layers and learning rule.
    /// Custom neural networks are created by deriving from this class, creating layers of interconnected network specific neurons,
    /// and setting network specific learning rule.
    /// </pre>
    /// 
    /// @author Zoran Sevarac <sevarac@gmail.com> </summary>
    /// <seealso cref= Layer </seealso>
    /// <seealso cref= LearningRule </seealso>
    [Serializable]
    public class NeuralNetwork {


        /// <summary>
        /// The class fingerprint that is set to indicate serialization compatibility
        /// with a previous version of the class.
        /// </summary>
        private const long serialVersionUID = 7L;

        /// <summary>
        /// Network type id (see neuroph.util.NeuralNetworkType).
        /// </summary>
        private NeuralNetworkType type;

        /// <summary>
        /// Neural network layers
        /// </summary>
        private List<Layer> layers;

        /// <summary>
        /// Learning rule for this network
        /// </summary>
        private LearningRule learningRule;

        /// <summary>
        /// Neural network output buffer
        /// </summary>
        protected internal double[] outputBuffer;

        /// <summary>
        /// List of network input neurons.
        /// These neurons are used to set external input to network.
        /// </summary>
        private List<Neuron> inputNeurons;

        /// <summary>
        /// List of network output neurons.
        /// These neurons are used to read network's output.
        /// </summary>
        private List<Neuron> outputNeurons;

        /// <summary>
        /// Plugins collection
        /// </summary>
        private IDictionary<Class, PluginBase> plugins;

        /// <summary>
        /// Network label
        /// </summary>
        private string label = "";

        /// <summary>
        /// List of neural network listeners
        /// </summary>
        [NonSerialized]
        private List<NeuralNetworkEventListener> listeners = new List<NeuralNetworkEventListener>();

        /// <summary>
        /// Neural network logger
        /// </summary>
        private readonly Logger LOGGER = LoggerFactory.getLogger(typeof(NeuralNetwork));

        /// <summary>
        /// Creates an instance of empty neural network.
        /// </summary>
        public NeuralNetwork() {
            this.layers = new List<Layer>();
            this.inputNeurons = new List<Neuron>();
            this.outputNeurons = new List<Neuron>();
            this.plugins = new Dictionary<Class, util.plugins.PluginBase>();
        }

        /// <summary>
        /// Adds layer to neural network
        /// </summary>
        /// <param name="layer"> layer to add </param>
        public virtual void addLayer(Layer layer) {

            // In case of null throw exception to prevent adding null layers
            if (layer == null) {
                throw new ArgumentException("Layer cant be null!");
            }

            // set parent network for added layer
            layer.ParentNetwork = this;

            // add layer to layers collection
            layers.Add(layer);

            // notify listeners that layer has been added
            fireNetworkEvent(new events.NeuralNetworkEvent(layer, events.NeuralNetworkEvent.NeuralNetworkEventType.LAYER_ADDED));
        }

        /// <summary>
        /// Adds layer to specified index position in network
        /// </summary>
        /// <param name="index"> index position to add layer </param>
        /// <param name="layer"> layer to add </param>
        public virtual void addLayer(int index, Layer layer) {

            // in case of null value throw exception to prevent adding null layers
            if (layer == null) {
                throw new ArgumentException("Layer cant be null!");
            }

            // if layer position is negative also throw exception
            if (index < 0) {
                throw new ArgumentException("Layer index cannot be negative: " + index);
            }

            // set parent network for added layer
            layer.ParentNetwork = this;

            // add layer to layers collection at specified position        
            layers.Insert(index, layer);

            // notify listeners that layer has been added
            fireNetworkEvent(new NeuralNetworkEvent(layer, NeuralNetworkEvent.NeuralNetworkEventType.LAYER_ADDED));
        }

        /// <summary>
        /// Removes specified layer from network
        /// </summary>
        /// <param name="layer"> layer to remove </param>
        /// <exception cref="Exception"> </exception>
        public virtual void removeLayer(Layer layer) {

            if (!layers.Remove(layer)) {
                throw new java.lang.Exception("Layer not in Neural n/w");
            }

            // notify listeners that layer has been removed
            fireNetworkEvent(new NeuralNetworkEvent(layer, NeuralNetworkEvent.NeuralNetworkEventType.LAYER_REMOVED));
        }

        /// <summary>
        /// Removes layer at specified index position from net
        /// </summary>
        /// <param name="index"> int value represents index postion of layer which should be
        ///              removed </param>
        public virtual void removeLayerAt(int index) {
            Layer layer = layers[index];
            layers.RemoveAt(index);

            // notify listeners that layer has been removed
            fireNetworkEvent(new NeuralNetworkEvent(layer, NeuralNetworkEvent.NeuralNetworkEventType.LAYER_REMOVED));
        }

        /// <summary>
        /// Returns layers array
        /// </summary>
        /// <returns> array of layers </returns>
        public virtual List<Layer> Layers {
            get {
                return this.layers;
            }
        }

        /// <summary>
        /// Returns layer at specified index
        /// </summary>
        /// <param name="index"> layer index position </param>
        /// <returns> layer at specified index position </returns>
        public virtual Layer getLayerAt(int index) {
            return layers[index];
        }

        /// <summary>
        /// Returns index position of the specified layer
        /// </summary>
        /// <param name="layer"> requested Layer object </param>
        /// <returns> layer position index </returns>
        public virtual int IndexOf(Layer layer) {
            return layers.IndexOf(layer);
        }

        /// <summary>
        /// Returns number of layers in network
        /// </summary>
        /// <returns> number of layes in net </returns>
        public virtual int LayersCount {
            get {
                return layers.Count;
            }
        }

        /// <summary>
        /// Sets network input. Input is an array of double values.
        /// </summary>
        /// <param name="inputVector"> network input as double array </param>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public void setInput(double... inputVector) throws org.neuroph.core.exceptions.VectorSizeMismatchException
        public virtual double[] Input {
            set {
                if (value.Length != inputNeurons.Count) {
                    throw new VectorSizeMismatchException("Input vector size does not match network input dimension!");
                }

                // TODO: Make this more elegant
                int i = 0;
                foreach (Neuron neuron in this.inputNeurons) {
                    neuron.Input = value[i]; // set input to the corresponding neuron
                    i++;
                }
            }
        }


        /// <summary>
        /// Returns network output vector. Output vector is an array  collection of Double
        /// values.
        /// </summary>
        /// <returns> network output vector </returns>
        public virtual double[] Output {
            get {
                // TODO: Make this more elegant
                int i = 0;
                foreach (Neuron c in outputNeurons) {
                    outputBuffer[i] = c.Output;
                    i++;
                }

                return outputBuffer;
            }
        }

        //    This can be used to provid difefrent ways for leyer calculation
        //    public static interface Calculator {
        //        // default calculator
        //        public void calculate();
        //    }
        //    
        //    // default calculator is sequential using foreach loop
        //    transient Calculator calculator = new Calculator() {
        //        @Override
        //        public void calculate() {
        //            for (Layer layer : getLayers()) {
        //                layer.calculate();
        //            }
        //        }
        //    };
        //
        //    public Calculator getCalculator() {
        //        return calculator;
        //    }
        //
        //    public void setCalculator(Calculator calculator) {
        //        this.calculator = calculator;
        //    }



        /// <summary>
        /// Performs calculation on whole network
        /// </summary>
        public virtual void calculate() {
            foreach (Layer layer in this.layers) {
                layer.calculate();
            }


            fireNetworkEvent(new NeuralNetworkEvent(this, NeuralNetworkEvent.NeuralNetworkEventType.CALCULATED));
        }

        /// <summary>
        /// Resets the activation levels for whole network
        /// </summary>
        public virtual void reset() {
            foreach (Layer layer in this.layers) {
                layer.reset();
            }
        }

        /// <summary>
        /// Learn the specified training set
        /// </summary>
        /// <param name="trainingSet"> set of training elements to learn </param>
        public virtual void learn(DataSet trainingSet) {
            if (trainingSet == null) {
                throw new ArgumentException("Training set is null!");
            }

            learningRule.learn(trainingSet);
        }

        /// <summary>
        /// Learn the specified training set, using specified learning rule
        /// </summary>
        /// <param name="trainingSet">  set of training elements to learn </param>
        /// <param name="learningRule"> instance of learning rule to use for learning </param>
        public virtual void learn(DataSet trainingSet, LearningRule learningRule) {
            LearningRule = learningRule;
            learningRule.learn(trainingSet);
        }


        /// <summary>
        /// Stops learning
        /// </summary>
        public virtual void stopLearning() {
            learningRule.stopLearning();
        }

        /// <summary>
        /// Pause the learning - puts learning thread in ca state. Makes sense only
        /// wen learning is done in new thread with learnInNewThread() method
        /// </summary>
        public virtual void pauseLearning() {
            if (learningRule is IterativeLearning) {
                ((IterativeLearning)(object)learningRule).pause();
            }
        }

        /// <summary>
        /// Resumes paused learning - notifies the learning rule to continue
        /// </summary>
        public virtual void resumeLearning() {
            if (learningRule is IterativeLearning) {
                ((IterativeLearning)(object)learningRule).resume();
            }
        }

        /// <summary>
        /// Randomizes connection weights for the whole network
        /// </summary>
        public virtual void randomizeWeights() {
            randomizeWeights(new WeightsRandomizer());
        }

        /// <summary>
        /// Randomizes connection weights for the whole network within specified
        /// value range
        /// </summary>
        public virtual void randomizeWeights(double minWeight, double maxWeight) {
            randomizeWeights(new RangeRandomizer(minWeight, maxWeight));
        }

        /// <summary>
        /// Randomizes connection weights for the whole network using specified
        /// random generator
        /// </summary>
        public virtual void randomizeWeights(Random random) {
            randomizeWeights(new WeightsRandomizer(random));
        }

        /// <summary>
        /// Randomizes connection weights for the whole network using specified
        /// randomizer
        /// </summary>
        /// <param name="randomizer"> random weight generator to use </param>
        public virtual void randomizeWeights(WeightsRandomizer randomizer) {
            randomizer.randomize(this);
        }

        /// <summary>
        /// Returns type of this network
        /// </summary>
        /// <returns> network type </returns>
        public virtual NeuralNetworkType NetworkType {
            get {
                return type;
            }
            set {
                this.type = value;
            }
        }


        /// <summary>
        /// Returns input neurons
        /// </summary>
        /// <returns> input neurons </returns>
        public virtual List<Neuron> InputNeurons {
            get {
                return this.inputNeurons;
            }
            set {
                foreach (Neuron neuron in value) {
                    this.inputNeurons.Add(neuron);
                }
            }
        }

        /// <summary>
        /// Gets number of input neurons
        /// </summary>
        /// <returns> number of input neurons </returns>
        public virtual int InputsCount {
            get {
                return this.inputNeurons.Count;
            }
        }


        /// <summary>
        /// Returns output neurons
        /// </summary>
        /// <returns> list of output neurons </returns>
        public virtual List<Neuron> OutputNeurons {
            get {
                return this.outputNeurons;
            }
            set {
                foreach (Neuron neuron in value) {
                    this.outputNeurons.Add(neuron);
                }
                this.outputBuffer = new double[value.Count];
            }
        }

        public virtual int OutputsCount {
            get {
                return this.outputNeurons.Count;
            }
        }


        /// <summary>
        /// Sets labels for output neurons
        /// </summary>
        /// <param name="labels"> labels for output neurons </param>
        public virtual string[] OutputLabels {
            set {
                for (int i = 0; i < outputNeurons.Count; i++) {
                    outputNeurons[i].Label = value[i];
                }
            }
        }

        /// <summary>
        /// Returns the learning algorithm of this network
        /// </summary>
        /// <returns> algorithm for network training </returns>
        public virtual LearningRule LearningRule {
            get {
                return this.learningRule;
            }
            set {
                if (value == null) {
                    throw new ArgumentException("Learning rule can't be null!");
                }
                
                value.NeuralNetwork = this;
                this.learningRule = value;
            }
        }



        /// <summary>
        /// Returns all network weights as an double array
        /// </summary>
        /// <returns> network weights as an double array </returns>
        public virtual double[] getWeights() {
            List<double> weights = new List<double>();
            foreach (Layer layer in layers) {
                foreach (Neuron neuron in layer.Neurons) {
                    foreach (Connection conn in neuron.InputConnections) {
                        weights.Add(conn.Weight.Value);
                    }
                }
            }

            return weights.ToArray();
        }

        /// <summary>
        /// Sets network weights from the specified double array
        /// </summary>
        /// <param name="weights"> array of weights to set </param>
        public virtual void setWeights(double[] weights) {
            int i = 0;
            foreach (Layer layer in layers) {
                foreach (Neuron neuron in layer.Neurons) {
                    foreach (Connection conn in neuron.InputConnections) {
                        conn.Weight.Value = weights[i];
                        i++;
                    }
                }
            }
        }

        public virtual bool Empty {
            get {
                return layers.Count == 0;
            }
        }

        /// <summary>
        /// Creates connection with specified weight value between specified neurons
        /// </summary>
        /// <param name="fromNeuron"> neuron to connect </param>
        /// <param name="toNeuron">   neuron to connect to </param>
        /// <param name="weightVal">  connection weight value </param>
        public virtual void createConnection(Neuron fromNeuron, Neuron toNeuron, double weightVal) {
            //  Connection connection = new Connection(fromNeuron, toNeuron, weightVal);
            toNeuron.addInputConnection(fromNeuron, weightVal);
        }

        public override string ToString() {
            if (label != null) {
                return label;
            }

            return base.ToString();
        }

        /// <summary>
        /// Saves neural network into the specified file.
        /// </summary>
        /// <param name="filePath"> file path to save network into </param>
        public virtual void save(string filePath) {
            ObjectOutputStream @out = null;
            try {
                File file = new File(filePath);
                @out = new ObjectOutputStream(new BufferedOutputStream(new FileOutputStream(file)));
                @out.writeObject(this);
                @out.flush();
            } catch (IOException ioe) {
                throw new NeurophException("Could not write neural network to file!", ioe);
            } finally {
                if (@out != null) {
                    try {
                        @out.close();
                    } catch (IOException) {
                    }
                }
            }
        }

        /// <summary>
        /// Loads neural network from the specified file.
        /// </summary>
        /// <param name="filePath"> file path to load network from </param>
        /// <returns> loaded neural network as NeuralNetwork object </returns>
        /// @deprecated Use createFromFile method instead 
        public static NeuralNetwork load(string filePath) {
            ObjectInputStream oistream = null;

            try {
                File file = new File(filePath);
                if (!file.exists()) {
                    throw new FileNotFoundException("Cannot find file: " + filePath);
                }

                oistream = new ObjectInputStream(new BufferedInputStream(new FileInputStream(filePath)));
                NeuralNetwork nnet = (NeuralNetwork)oistream.readObject();
                return nnet;

            } catch (IOException ioe) {
                throw new NeurophException("Could not read neural network file!", ioe);
            } catch (ClassNotFoundException cnfe) {
                throw new NeurophException("Class not found while trying to read neural network from file!", cnfe);
            } finally {
                if (oistream != null) {
                    try {
                        oistream.close();
                    } catch (IOException) {
                    }
                }
            }
        }

        /// <summary>
        /// Loads neural network from the specified InputStream.
        /// </summary>
        /// <param name="inputStream"> input stream to load network from </param>
        /// <returns> loaded neural network as NeuralNetwork object </returns>
        public static NeuralNetwork load(InputStream inputStream) {
            ObjectInputStream oistream = null;

            try {
                oistream = new ObjectInputStream(new BufferedInputStream(inputStream));
                NeuralNetwork nnet = (NeuralNetwork)oistream.readObject();

                return nnet;

            } catch (IOException ioe) {
                throw new NeurophException("Could not read neural network file!", ioe);
            } catch (ClassNotFoundException cnfe) {
                throw new NeurophException("Class not found while trying to read neural network from file!", cnfe);
            } finally {
                if (oistream != null) {
                    try {
                        oistream.close();
                    } catch (IOException) {
                    }
                }
            }
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: private void readObject(java.io.ObjectInputStream in) throws IOException, ClassNotFoundException
        private void readObject(java.io.ObjectInputStream @in) {
            @in.defaultReadObject();
            listeners = new List<events.NeuralNetworkEventListener>();
        }

        /// <summary>
        /// Loads and return s neural network instance from specified file
        /// </summary>
        /// <param name="file"> neural network file </param>
        /// <returns> neural network instance </returns>
        public static NeuralNetwork createFromFile(File file) {
            ObjectInputStream oistream = null;

            try {
                if (!file.exists()) {
                    throw new FileNotFoundException("Cannot find file: " + file);
                }

                oistream = new ObjectInputStream(new BufferedInputStream(new FileInputStream(file)));
                NeuralNetwork nnet = (NeuralNetwork)oistream.readObject();
                return nnet;

            } catch (IOException ioe) {
                throw new NeurophException("Could not read neural network file!", ioe);
            } catch (ClassNotFoundException cnfe) {
                throw new NeurophException("Class not found while trying to read neural network from file!", cnfe);
            } finally {
                if (oistream != null) {
                    try {
                        oistream.close();
                    } catch (IOException) {
                    }
                }
            }
        }

        public static NeuralNetwork createFromFile(string filePath) {
            File file = new File(filePath);
            return NeuralNetwork.createFromFile(file);
        }

        /// <summary>
        /// Adds plugin to neural network
        /// </summary>
        /// <param name="plugin"> neural network plugin to add </param>
        public virtual void addPlugin(PluginBase plugin) {
            plugin.ParentNetwork = this;
            this.plugins[plugin.GetType()] = plugin;
        }

        /// <summary>
        /// Returns the requested plugin
        /// </summary>
        /// <param name="pluginClass"> class of the plugin to get </param>
        /// <returns> instance of specified plugin class </returns>
        public virtual T getPlugin<T>(Class pluginClass) where T : org.neuroph.util.plugins.PluginBase {
            return (T)pluginClass.cast(plugins[pluginClass]);
        }

        /// <summary>
        /// Removes the plugin with specified name
        /// </summary>
        /// <param name="pluginClass"> class of the plugin to remove </param>
        public virtual void removePlugin(Type pluginClass) {
            this.plugins.Remove(pluginClass);
        }

        /// <summary>
        /// Get network label
        /// </summary>
        /// <returns> network label </returns>
        public virtual string Label {
            get {
                return label;
            }
            set {
                this.label = value;
            }
        }


        // This methods allows classes to register for LearningEvents
        public virtual void addListener(NeuralNetworkEventListener listener) {
            lock (this) {
                if (listener == null) {
                    throw new System.ArgumentException("listener is null!");
                }

                listeners.Add(listener);
            }
        }

        // This methods allows classes to unregister for LearningEvents
        public virtual void removeListener(NeuralNetworkEventListener listener) {
            lock (this) {
                if (listener == null) {
                    throw new System.ArgumentException("listener is null!");
                }

                listeners.Remove(listener);
            }
        }

        // This method is used to fire NeuralNetworkEvents
        public virtual void fireNetworkEvent(NeuralNetworkEvent evt) {
            lock (this) {
                foreach (NeuralNetworkEventListener listener in listeners) {
                    listener.handleNeuralNetworkEvent(evt);
                }
            }
        }
    }
}