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

namespace org.neuroph.util
{

	using Layer = org.neuroph.core.Layer;
	using org.neuroph.core;
	using Neuron = org.neuroph.core.Neuron;
	using Adaline = org.neuroph.nnet.Adaline;
	using BAM = org.neuroph.nnet.BAM;
	using CompetitiveNetwork = org.neuroph.nnet.CompetitiveNetwork;
	using Hopfield = org.neuroph.nnet.Hopfield;
	using Instar = org.neuroph.nnet.Instar;
	using Kohonen = org.neuroph.nnet.Kohonen;
	using MaxNet = org.neuroph.nnet.MaxNet;
	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;
	using Outstar = org.neuroph.nnet.Outstar;
	using Perceptron = org.neuroph.nnet.Perceptron;
	using RBFNetwork = org.neuroph.nnet.RBFNetwork;
	using SupervisedHebbianNetwork = org.neuroph.nnet.SupervisedHebbianNetwork;
	using UnsupervisedHebbianNetwork = org.neuroph.nnet.UnsupervisedHebbianNetwork;
	using BiasNeuron = org.neuroph.nnet.comp.neuron.BiasNeuron;
	using BackPropagation = org.neuroph.nnet.learning.BackPropagation;
	using BinaryDeltaRule = org.neuroph.nnet.learning.BinaryDeltaRule;
	using DynamicBackPropagation = org.neuroph.nnet.learning.DynamicBackPropagation;
	using MomentumBackpropagation = org.neuroph.nnet.learning.MomentumBackpropagation;
	using PerceptronLearning = org.neuroph.nnet.learning.PerceptronLearning;
	using ResilientPropagation = org.neuroph.nnet.learning.ResilientPropagation;

	/// <summary>
	/// Provides methods to create various neural networks.
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class NeuralNetworkFactory
	{

		/// <summary>
		/// Creates and returns a new instance of Adaline network </summary>
		/// <param name="inputsCount"> number of inputs of Adaline network </param>
		/// <returns> instance of Adaline network </returns>
		public static Adaline createAdaline(int inputsCount)
		{
			Adaline nnet = new Adaline(inputsCount);
			return nnet;
		}


		/// <summary>
		/// Creates  and returns a new instance of Perceptron network </summary>
		/// <param name="inputNeuronsCount"> number of neurons in input layer </param>
		/// <param name="outputNeuronsCount"> number of neurons in output layer </param>
		/// <param name="transferFunctionType"> type of transfer function to use </param>
		/// <returns> instance of Perceptron network </returns>
		public static Perceptron createPerceptron(int inputNeuronsCount, int outputNeuronsCount, TransferFunctionType transferFunctionType)
		{
			Perceptron nnet = new Perceptron(inputNeuronsCount, outputNeuronsCount, transferFunctionType);
			return nnet;
		}

		/// <summary>
		/// Creates  and returns a new instance of Perceptron network </summary>
		/// <param name="inputNeuronsCount"> number of neurons in input layer </param>
		/// <param name="outputNeuronsCount"> number of neurons in output layer </param>
		/// <param name="transferFunctionType"> type of transfer function to use </param>
		/// <param name="learningRule"> learning rule class </param>
		/// <returns> instance of Perceptron network </returns>
		public static Perceptron createPerceptron(int inputNeuronsCount, int outputNeuronsCount, TransferFunctionType transferFunctionType, Type learningRule)
		{
			Perceptron nnet = new Perceptron(inputNeuronsCount, outputNeuronsCount, transferFunctionType);

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
					if (learningRule.FullName.Equals(typeof(PerceptronLearning).FullName))
					{
						nnet.LearningRule = new PerceptronLearning();
					}
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
					else if (learningRule.FullName.Equals(typeof(BinaryDeltaRule).FullName))
					{
						nnet.LearningRule = new BinaryDeltaRule();
					}

			return nnet;
		}

		/// <summary>
		/// Creates and returns a new instance of Multi Layer Perceptron </summary>
		/// <param name="layersStr"> space separated number of neurons in layers </param>
		/// <param name="transferFunctionType"> transfer function type for neurons </param>
		/// <returns> instance of Multi Layer Perceptron </returns>
		public static MultiLayerPerceptron createMLPerceptron(string layersStr, TransferFunctionType transferFunctionType)
		{
			List<int> layerSizes = VectorParser.parseInteger(layersStr);
			MultiLayerPerceptron nnet = new MultiLayerPerceptron(layerSizes, transferFunctionType);
			return nnet;
		}

		/// <summary>
		/// Creates and returns a new instance of Multi Layer Perceptron </summary>
		/// <param name="layersStr"> space separated number of neurons in layers </param>
		/// <param name="transferFunctionType"> transfer function type for neurons </param>
		/// <returns> instance of Multi Layer Perceptron </returns>
		public static MultiLayerPerceptron createMLPerceptron(string layersStr, TransferFunctionType transferFunctionType, Type learningRule, bool useBias, bool connectIO)
		{
			List<int> layerSizes = VectorParser.parseInteger(layersStr);
					NeuronProperties neuronProperties = new NeuronProperties(transferFunctionType, useBias);
			MultiLayerPerceptron nnet = new MultiLayerPerceptron(layerSizes, neuronProperties);

					// set learning rule - TODO: use reflection here
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
					if (learningRule.FullName.Equals(typeof(BackPropagation).FullName))
					{
						nnet.LearningRule = new BackPropagation();
					}
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
					else if (learningRule.FullName.Equals(typeof(MomentumBackpropagation).FullName))
					{
						nnet.LearningRule = new MomentumBackpropagation();
					}
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
					else if (learningRule.FullName.Equals(typeof(DynamicBackPropagation).FullName))
					{
						nnet.LearningRule = new DynamicBackPropagation();
					}
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
					else if (learningRule.FullName.Equals(typeof(ResilientPropagation).FullName))
					{
						nnet.LearningRule = new ResilientPropagation();
					}

					// connect io
					if (connectIO)
					{
						nnet.connectInputsToOutputs();
					}

			return nnet;
		}

		/// <summary>
		/// Creates and returns a new instance of Hopfield network </summary>
		/// <param name="neuronsCount"> number of neurons in Hopfield network </param>
		/// <returns> instance of Hopfield network </returns>
		public static Hopfield createHopfield(int neuronsCount)
		{
			Hopfield nnet = new Hopfield(neuronsCount);
			return nnet;
		}

		/// <summary>
		/// Creates and returns a new instance of BAM network </summary>
		/// <param name="inputNeuronsCount"> number of input neurons </param>
		/// <param name="outputNeuronsCount"> number of output neurons </param>
		/// <returns> instance of BAM network </returns>
		public static BAM createBam(int inputNeuronsCount, int outputNeuronsCount)
		{
			BAM nnet = new BAM(inputNeuronsCount, outputNeuronsCount);
			return nnet;
		}

		/// <summary>
		/// Creates and returns a new instance of Kohonen network </summary>
		/// <param name="inputNeuronsCount"> number of input neurons </param>
		/// <param name="outputNeuronsCount"> number of output neurons </param>
		/// <returns> instance of Kohonen network </returns>
		public static Kohonen createKohonen(int inputNeuronsCount, int outputNeuronsCount)
		{
			Kohonen nnet = new Kohonen(inputNeuronsCount, outputNeuronsCount);
			return nnet;
		}

		/// <summary>
		/// Creates and returns a new instance of Hebbian network </summary>
		/// <param name="inputNeuronsCount"> number of neurons in input layer </param>
		/// <param name="outputNeuronsCount"> number of neurons in output layer </param>
		/// <param name="transferFunctionType"> neuron's transfer function type </param>
		/// <returns> instance of Hebbian network </returns>
		public static SupervisedHebbianNetwork createSupervisedHebbian(int inputNeuronsCount, int outputNeuronsCount, TransferFunctionType transferFunctionType)
		{
			SupervisedHebbianNetwork nnet = new SupervisedHebbianNetwork(inputNeuronsCount, outputNeuronsCount, transferFunctionType);
			return nnet;
		}

		/// <summary>
		/// Creates and returns a new instance of Unsupervised Hebbian Network </summary>
		/// <param name="inputNeuronsCount"> number of neurons in input layer </param>
		/// <param name="outputNeuronsCount"> number of neurons in output layer </param>
		/// <param name="transferFunctionType"> neuron's transfer function type </param>
		/// <returns> instance of Unsupervised Hebbian Network </returns>
		public static UnsupervisedHebbianNetwork createUnsupervisedHebbian(int inputNeuronsCount, int outputNeuronsCount, TransferFunctionType transferFunctionType)
		{
			UnsupervisedHebbianNetwork nnet = new UnsupervisedHebbianNetwork(inputNeuronsCount, outputNeuronsCount, transferFunctionType);
			return nnet;
		}

		/// <summary>
		/// Creates and returns a new instance of Max Net network </summary>
		/// <param name="neuronsCount"> number of neurons (same num in input and output layer) </param>
		/// <returns> instance of Max Net network </returns>
		public static MaxNet createMaxNet(int neuronsCount)
		{
			MaxNet nnet = new MaxNet(neuronsCount);
			return nnet;
		}

		/// <summary>
		/// Creates and returns a new instance of Instar network </summary>
		/// <param name="inputNeuronsCount"> umber of input neurons </param>
		/// <returns> instance of Instar network </returns>
		public static Instar createInstar(int inputNeuronsCount)
		{
			Instar nnet = new Instar(inputNeuronsCount);
			return nnet;
		}

		/// <summary>
		/// Creates and returns a new instance of Outstar network </summary>
		/// <param name="outputNeuronsCount"> number of output neurons </param>
		/// <returns> instance of Outstar network </returns>
		public static Outstar createOutstar(int outputNeuronsCount)
		{
			Outstar nnet = new Outstar(outputNeuronsCount);
			return nnet;
		}

		/// <summary>
		/// Creates and returns a new instance of competitive network </summary>
		/// <param name="inputNeuronsCount"> number of neurons in input layer </param>
		/// <param name="outputNeuronsCount"> number of neurons in output layer </param>
		/// <returns> instance of CompetitiveNetwork </returns>
		public static CompetitiveNetwork createCompetitiveNetwork(int inputNeuronsCount, int outputNeuronsCount)
		{
			CompetitiveNetwork nnet = new CompetitiveNetwork(inputNeuronsCount, outputNeuronsCount);
			return nnet;
		}

		/// <summary>
		/// Creates and returns a new instance of RBF network </summary>
		/// <param name="inputNeuronsCount"> number of neurons in input layer </param>
		/// <param name="rbfNeuronsCount"> number of neurons in RBF layer </param>
		/// <param name="outputNeuronsCount"> number of neurons in output layer </param>
		/// <returns> instance of RBF network </returns>
		public static RBFNetwork createRbfNetwork(int inputNeuronsCount, int rbfNeuronsCount, int outputNeuronsCount)
		{
			RBFNetwork nnet = new RBFNetwork(inputNeuronsCount, rbfNeuronsCount, outputNeuronsCount);
			return nnet;
		}

		/// <summary>
		/// Sets default input and output neurons for network (first layer as input,
		/// last as output)
		/// </summary>
		public static NeuralNetwork DefaultIO
		{
			set
			{
					   List<Neuron> inputNeuronsList = new List<Neuron>();
						Layer firstLayer = value.getLayerAt(0);
						foreach (Neuron neuron in firstLayer.Neurons)
						{
							if (!(neuron is BiasNeuron)) // dont set input to bias neurons
							{
								inputNeuronsList.Add(neuron);
							}
						}
    
				List<Neuron> outputNeurons = ((Layer) value.getLayerAt(value.LayersCount - 1)).Neurons;
    
				value.InputNeurons = inputNeuronsList;
				value.OutputNeurons = outputNeurons;
			}
		}

	}

}