﻿using System;
using System.Collections;
using System.Collections.Generic;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
namespace org.neuroph.imgrec.samples
{

	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;
	using Dimension = org.neuroph.imgrec.image.Dimension;
	using MomentumBackpropagation = org.neuroph.nnet.learning.MomentumBackpropagation;
	using TransferFunctionType = org.neuroph.util.TransferFunctionType;

	/// 
	/// <summary>
	/// @author Zoran Sevarac
	/// </summary>
	public class RGBImageRecognitionTrainingSample
	{

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws java.io.IOException
		public static void Main(string[] args)
		{

			// path to image directory
			string imageDir = "/home/zoran/Downloads/MihailoHSLTest/trening";

			// image names - used for output neuron labels
			List<string> imageLabels = new ArrayList();
			imageLabels.Add("bird");
			imageLabels.Add("cat");
			imageLabels.Add("dog");


			// create dataset
			IDictionary<string, FractionRgbData> map = ImageRecognitionHelper.getFractionRgbDataForDirectory(new File(imageDir), new Dimension(20, 20));
			DataSet dataSet = ImageRecognitionHelper.createRGBTrainingSet(imageLabels, map);

			// create neural network
			List<int?> hiddenLayers = new List<int?>();
			hiddenLayers.Add(12);
			NeuralNetwork nnet = ImageRecognitionHelper.createNewNeuralNetwork("someNetworkName", new Dimension(20,20), ColorMode.COLOR_RGB, imageLabels, hiddenLayers, TransferFunctionType.SIGMOID);

			// set learning rule parameters
			MomentumBackpropagation mb = (MomentumBackpropagation)nnet.LearningRule;
			mb.LearningRate = 0.2;
			mb.MaxError = 0.9;
			mb.Momentum = 1;

			// traiin network
			Console.WriteLine("NNet start learning...");
			nnet.learn(dataSet);
			Console.WriteLine("NNet learned");

		}

	}

}