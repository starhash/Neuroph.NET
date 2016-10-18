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

namespace org.neuroph.contrib.matrixmlp
{

	using Connection = org.neuroph.core.Connection;
	using Layer = org.neuroph.core.Layer;
	using Neuron = org.neuroph.core.Neuron;
	using Tanh = org.neuroph.core.transfer.Tanh;
	using TransferFunction = org.neuroph.core.transfer.TransferFunction;
	using BiasNeuron = org.neuroph.nnet.comp.neuron.BiasNeuron;

	/// <summary>
	/// Matrix based layer optimized for backpropagation
	/// @author Zoran Sevarac
	/// </summary>
	public class MatrixMlpLayer : MatrixLayer
	{

		internal Layer sourceLayer;

		/// <summary>
		/// Number of neurons in this layer
		/// </summary>
		internal int neuronsCount = 0;

		/// <summary>
		/// Number of inputs from previous layer
		/// </summary>
		internal int inputsCount = 0;

		internal double[][] weights;
		internal double[][] deltaWeights;
		internal bool useBias;
	//    double[] biases;
	//    double[] deltaBiases;

		internal double[] inputs;
		internal double[] netInput;
		internal double[] outputs;
		internal double[] errors;

		internal TransferFunction transferFunction = new Tanh();
		internal MatrixLayer previousLayer = null;
		internal MatrixLayer nextLayer = null;


		// vidi konstruktore za MLayer
	//    public MatrixBackPropLayer(int inputSize, int outputSize, TransferFunction transferFunction) {
	//        inputs = new double[inputSize];
	//        outputs = new double[outputSize];
	//        weights = new double[outputSize][inputSize];
	//
	//        this.transferFunction = transferFunction;
	//    }



		  public MatrixMlpLayer(Layer sourceLayer, MatrixLayer previousLayer, TransferFunction transferFunction)
		  {
			  this.sourceLayer = sourceLayer;
			  this.previousLayer = previousLayer;
			  if (!(previousLayer is MatrixInputLayer))
			  {
				  ((MatrixMlpLayer)previousLayer).NextLayer = this;
			  }
			  this.transferFunction = transferFunction;

			  this.neuronsCount = sourceLayer.NeuronsCount;
	//          if (sourceLayer.getNeuronAt(neuronsCount-1) instanceof BiasNeuron) this.neuronsCount = this.neuronsCount -1;

			  this.inputsCount = previousLayer.Outputs.Length;

			  outputs = new double[neuronsCount];
	//          biases = new double[neuronsCount];
	//          deltaBiases = new double[neuronsCount];
			  inputs = new double[inputsCount];
			  netInput = new double[neuronsCount];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: weights = new double[neuronsCount][inputsCount];
			  weights = RectangularArrays.ReturnRectangularDoubleArray(neuronsCount, inputsCount);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: deltaWeights = new double[neuronsCount][inputsCount];
			  deltaWeights = RectangularArrays.ReturnRectangularDoubleArray(neuronsCount, inputsCount);

			  errors = new double[neuronsCount];

			  copyNeuronsToMatrices();
		  }


		public virtual MatrixLayer PreviousLayer
		{
			get
			{
				return previousLayer;
			}
			set
			{
				this.previousLayer = value;
			}
		}


		public virtual MatrixLayer NextLayer
		{
			get
			{
				return nextLayer;
			}
			set
			{
				this.nextLayer = value;
			}
		}




		// maybe omit the sourceLayer parmeter
		public virtual void copyNeuronsToMatrices()
		{

			int neuronIdx = 0, connIdx = 0;
			foreach (Neuron neuron in this.sourceLayer.Neurons)
			{
				if (neuron is BiasNeuron)
				{
					this.useBias = true;
				}
				outputs[neuronIdx] = neuron.Output;
				// should we copy net inputs also? weightedSums or netInputs = neuron.getNetInput()
				connIdx = 0;
				foreach (Connection conn in neuron.InputConnections)
				{
					weights[neuronIdx][connIdx] = conn.Weight.Value;
					connIdx++;
				}
				neuronIdx++;
			}
		}

		public virtual void copyMatricesToNeurons()
		{
			// assume full conectivity
		}

		public virtual double[] Inputs
		{
			get
			{
				return this.inputs;
			}
			set
			{
				this.inputs = value;
			}
		}

		public virtual double[] Outputs
		{
			get
			{
				return this.outputs;
			}
			set
			{
				this.outputs = value;
			}
		}



		public virtual double[][] Weights
		{
			get
			{
				return weights;
			}
		}

	//    public double[] getBiases() {
	//        return biases;
	//    }

	//    public void setBiases(double[] biases) {
	//        this.biases = biases;
	//    }


		public virtual void getInputsFromPreviousLayer()
		{
			this.inputs = this.previousLayer.Outputs;
		}


		public void calculate()
		{
			this.inputs = previousLayer.Outputs;
			   for (int i = 0; i < neuronsCount; i++)
			   {
				netInput[i] = 0;
				for (int j = 0; j < inputs.Length; j++)
				{
						netInput[i] += inputs[j] * weights[i][j];
				}
	//            netInput[i] = netInput[i] + biases[i];
				outputs[i] = transferFunction.getOutput(netInput[i]);
			   }

				if (useBias)
				{
					outputs[neuronsCount - 1] = 1; // this one is bias neuron
				}
		}

		public virtual int NeuronsCount
		{
			get
			{
				return outputs.Length;
			}
		}

	   public virtual double[] Errors
	   {
		   get
		   {
				return errors;
		   }
		   set
		   {
				this.errors = value;
		   }
	   }


		public virtual TransferFunction TransferFunction
		{
			get
			{
				return transferFunction;
			}
		}

		public virtual double[] NetInput
		{
			get
			{
				return netInput;
			}
		}

		public virtual void saveCurrentWeights()
		{
			Array.Copy(weights, 0, deltaWeights, 0, weights.Length);
	   //     System.arraycopy(biases, 0, deltaBiases, 0, biases.length);

		}

	//    public double[] getDeltaBiases() {
	//        return deltaBiases;
	//    }

		public virtual double[][] DeltaWeights
		{
			get
			{
				return deltaWeights;
			}
		}


		public virtual void sync()
		{
			// synchronize matrix and object structures
		}
	}

}