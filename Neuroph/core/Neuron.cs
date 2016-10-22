using System;
using System.Collections.Generic;
using org.apache.commons.lang3;
using org.neuroph.core.input;
using org.neuroph.core.transfer;

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

namespace org.neuroph.core {
    /// <summary>
    /// <pre>
    /// Basic general neuron model according to McCulloch-Pitts neuron model.
    /// Different neuron models can be created by using different input and transfer functions for instances of this class,
    /// or by deriving from this class. The neuron is basic processing element of neural network.
    /// This class implements the following behaviour:
    /// 
    /// output = transferFunction( inputFunction(inputConnections) )
    /// </pre>
    /// 
    /// @author Zoran Sevarac <sevarac@gmail.com> </summary>
    /// <seealso cref= InputFunction </seealso>
    /// <seealso cref= TransferFunction </seealso>

    [Serializable]
    public class Neuron : ICloneable //, Callable<Void>
    {

        //    @Override
        //    public Void call() throws Exception {
        //        calculate();
        //        return null;
        //    }


        /// <summary>
        /// The class fingerprint that is set to indicate serialization
        /// compatibility with a previous version of the class
        /// </summary>
        private const long serialVersionUID = 4L;

        /// <summary>
        /// Parent layer for this neuron
        /// </summary>
        protected internal Layer parentLayer;

        /// <summary>
        /// Collection of neuron's input connections (connections to this neuron)
        /// </summary>
        protected internal List<Connection> inputConnections;

        /// <summary>
        /// Collection of neuron's output connections (connections from this to other
        /// neurons)
        /// </summary>
        protected internal List<Connection> outConnections;

        /// <summary>
        /// Total net input for this neuron. Represents total input for this neuron
        /// received from input function.
        /// </summary>
        protected internal double totalInput = 0;

        /// <summary>
        /// Neuron output
        /// </summary>
        protected internal double output = 0;

        /// <summary>
        /// Local error for this neuron
        /// </summary>
        protected internal double error = 0;

        /// <summary>
        /// Input function for this neuron
        /// </summary>
        protected internal InputFunction inputFunction;

        /// <summary>
        /// Transfer function for this neuron
        /// </summary>
        protected internal TransferFunction transferFunction;

        /// <summary>
        /// Neuron's label
        /// </summary>
        private string label;

        /// <summary>
        /// Creates an instance of Neuron with default settings: weighted sum input function
        /// and Step transfer function. This is the basic McCulloch-Pitts neuron model.
        /// </summary>
        public Neuron() {
            this.inputFunction = new WeightedSum();
            this.transferFunction = new Step();
            this.inputConnections = new List<Connection>();
            this.outConnections = new List<Connection>();
        }

        /// <summary>
        /// Creates an instance of Neuron with the specified input and transfer functions.
        /// </summary>
        /// <param name="inputFunction">    input function for this neuron </param>
        /// <param name="transferFunction"> transfer function for this neuron </param>
        public Neuron(InputFunction inputFunction, TransferFunction transferFunction) {
            if (inputFunction == null) {
                throw new System.ArgumentException("Input function cannot be null!");
            }

            if (transferFunction == null) {
                throw new System.ArgumentException("Transfer function cannot be null!");
            }

            this.inputFunction = inputFunction;
            this.transferFunction = transferFunction;
            this.inputConnections = new List<Connection>();
            this.outConnections = new List<Connection>();
        }

        /// <summary>
        /// Calculates neuron's output
        /// </summary>
        public virtual void calculate() {
            this.totalInput = inputFunction.getOutput(inputConnections);
            this.output = transferFunction.getOutput(totalInput);
        }

        /// <summary>
        /// Sets input and output activation levels to zero
        /// </summary>
        public virtual void reset() {
            this.Input = 0d;
            this.Output = 0d;
        }

        /// <summary>
        /// Sets neuron's input
        /// </summary>
        /// <param name="input"> input value to set </param>
        public virtual double Input {
            set {
                this.totalInput = value;
            }
        }

        /// <summary>
        /// Returns total net input
        /// </summary>
        /// <returns> total net input </returns>
        public virtual double NetInput {
            get {
                return this.totalInput;
            }
        }

        /// <summary>
        /// Returns neuron's output
        /// </summary>
        /// <returns> neuron output </returns>
        public virtual double Output {
            get {
                return this.output;
            }
            set {
                this.output = value;
            }
        }

        /// <summary>
        /// Returns true if there are input connections for this neuron, false
        /// otherwise
        /// </summary>
        /// <returns> true if there is input connection, false otherwise </returns>
        public virtual bool hasInputConnections() {
            return (this.inputConnections.Count > 0);
        }

        public virtual bool hasOutputConnectionTo(Neuron neuron) {
            foreach (Connection connection in outConnections) {
                if (connection.ToNeuron == neuron) {
                    return true;
                }
            }
            return false;
        }

        public virtual bool hasInputConnectionFrom(Neuron neuron) {
            foreach (Connection connection in inputConnections) {
                if (connection.FromNeuron == neuron) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds the specified input connection
        /// </summary>
        /// <param name="connection"> input connection to add </param>
        public virtual void addInputConnection(Connection connection) {
            // check whether connection is  null
            if (connection == null) {
                throw new System.ArgumentException("Attempt to add null connection to neuron!");
            }

            // make sure that connection instance is pointing to this neuron
            if (connection.ToNeuron != this) {
                throw new System.ArgumentException("Cannot add input connection - bad toNeuron specified!");
            }

            // if it already has connection from same neuron do nothing
            if (this.hasInputConnectionFrom(connection.FromNeuron)) {
                return;
            }

            this.inputConnections.Add(connection);

            Neuron fromNeuron = connection.FromNeuron;
            fromNeuron.addOutputConnection(connection);
        }

        /// <summary>
        /// Adds input connection from specified neuron.
        /// </summary>
        /// <param name="fromNeuron"> neuron to connect from </param>
        public virtual void addInputConnection(Neuron fromNeuron) {
            Connection connection = new Connection(fromNeuron, this);
            this.addInputConnection(connection);
        }

        /// <summary>
        /// Adds input connection with the given weight, from given neuron
        /// </summary>
        /// <param name="fromNeuron"> neuron to connect from </param>
        /// <param name="weightVal">  connection weight value </param>
        public virtual void addInputConnection(Neuron fromNeuron, double weightVal) {
            Connection connection = new Connection(fromNeuron, this, weightVal);
            this.addInputConnection(connection);
        }

        /// <summary>
        /// Adds the specified output connection
        /// </summary>
        /// <param name="connection"> output connection to add </param>
        protected internal virtual void addOutputConnection(Connection connection) {
            // First do some checks
            // check whether connection is  null
            if (connection == null) {
                throw new System.ArgumentException("Attempt to add null connection to neuron!");
            }

            // make sure that connection instance is pointing to this neuron
            if (connection.FromNeuron != this) {
                throw new System.ArgumentException("Cannot add output connection - bad fromNeuron specified!");
            }

            // if this neuron is already connected to neuron specified in connection do nothing
            if (this.hasOutputConnectionTo(connection.ToNeuron)) {
                return;
            }

            // Now we can safely add new connection
            this.outConnections.Add(connection);
        }

        /// <summary>
        /// Returns input connections for this neuron
        /// </summary>
        /// <returns> input connections of this neuron </returns>
        public List<Connection> InputConnections {
            get {
                return inputConnections;
            }
        }

        /// <summary>
        /// Returns output connections from this neuron
        /// </summary>
        /// <returns> output connections from this neuron </returns>
        public List<Connection> OutConnections {
            get {
                return outConnections;
            }
        }

        protected internal virtual void removeInputConnection(Connection conn) {
            inputConnections.Remove(conn);
        }

        protected internal virtual void removeOutputConnection(Connection conn) {
            outConnections.Remove(conn);
        }

        /// <summary>
        /// Removes input connection which is connected to specified neuron
        /// </summary>
        /// <param name="fromNeuron"> neuron which is connected as input </param>
        public virtual void removeInputConnectionFrom(Neuron fromNeuron) {
            // run through all input connections
            foreach (Connection c in inputConnections) {
                // and look for specified fromNeuron
                if (c.FromNeuron == fromNeuron) {
                    fromNeuron.removeOutputConnection(c);
                    this.removeInputConnection(c);
                    break; // assumes that a pair of neurons can only be connected once
                }
            }
        }

        public virtual void removeOutputConnectionTo(Neuron toNeuron) {
            // run through all output connections
            foreach (Connection c in outConnections) {
                // and look for specified toNeuron
                if (c.ToNeuron == toNeuron) {
                    toNeuron.removeInputConnection(c);
                    this.removeOutputConnection(c);
                    break; // assumes that a pair of neurons can only be connected once
                }
            }
        }

        public virtual void removeAllInputConnections() {
            inputConnections.Clear();
        }

        public virtual void removeAllOutputConnections() {
            outConnections.Clear();
        }

        public virtual void removeAllConnections() {
            removeAllInputConnections();
            removeAllOutputConnections();
        }

        /// Gets input connection from the specified neuron * <param name="fromNeuron">
        /// neuron connected to this neuron as input </param>
        public virtual Connection getConnectionFrom(Neuron fromNeuron) {
            foreach (Connection connection in this.inputConnections) {
                if (connection.FromNeuron == fromNeuron) {
                    return connection;
                }
            }
            return null;
        }

        /// <summary>
        /// Sets input function
        /// </summary>
        /// <param name="inputFunction"> input function for this neuron </param>
        public virtual InputFunction InputFunction {
            set {
                this.inputFunction = value;
            }
            get {
                return this.inputFunction;
            }
        }

        /// <summary>
        /// Sets transfer function
        /// </summary>
        /// <param name="transferFunction"> transfer function for this neuron </param>
        public virtual TransferFunction TransferFunction {
            set {
                this.transferFunction = value;
            }
            get {
                return this.transferFunction;
            }
        }



        /// <summary>
        /// Sets reference to parent layer for this neuron (layer in which the neuron
        /// is located)
        /// </summary>
        /// <param name="parent"> reference on layer in which the cell is located </param>
        public virtual Layer ParentLayer {
            set {
                this.parentLayer = value;
            }
            get {
                return this.parentLayer;
            }
        }


        /// <summary>
        /// Returns weights vector of input connections
        /// </summary>
        /// <returns> weights vector of input connections </returns>
        public virtual Weight[] Weights {
            get {
                Weight[] weights = new Weight[inputConnections.Count];
                for (int i = 0; i < inputConnections.Count; i++) {
                    weights[i] = inputConnections[i].Weight;
                }
                return weights;
            }
        }

        /// <summary>
        /// Returns error for this neuron. This is used by supervised learing rules.
        /// </summary>
        /// <returns> error for this neuron which is set by learning rule </returns>
        public virtual double Error {
            get {
                return error;
            }
            set {
                this.error = value;
            }
        }




        /// <summary>
        /// Initialize weights for all input connections to specified value
        /// </summary>
        /// <param name="value"> the weight value </param>
        public virtual void initializeWeights(double value) {
            foreach (Connection connection in this.inputConnections) {
                connection.Weight.Value = value;
            }
        }

        /// <summary>
        /// Returns label for this neuron
        /// </summary>
        /// <returns> label for this neuron </returns>
        public virtual string Label {
            get {
                return label;
            }
            set {
                this.label = value;
            }
        }


        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: @Override public Object clone() throws CloneNotSupportedException
        public object Clone() {
            throw new System.NotSupportedException("Not yer implemented");
            // return super.clone(); //To change body of generated methods, choose Tools | Templates.
        }




    }

}