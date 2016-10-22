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

namespace org.neuroph.nnet
{

	using Layer = org.neuroph.core.Layer;
	using org.neuroph.core;
	using Neuron = org.neuroph.core.Neuron;
	using Min = org.neuroph.core.input.Min;
	using WeightedSum = org.neuroph.core.input.WeightedSum;
	using Linear = org.neuroph.core.transfer.Linear;
	using Trapezoid = org.neuroph.core.transfer.Trapezoid;
	using LMS = org.neuroph.nnet.learning.LMS;
	using ConnectionFactory = org.neuroph.util.ConnectionFactory;
	using LayerFactory = org.neuroph.util.LayerFactory;
	using NeuralNetworkFactory = org.neuroph.util.NeuralNetworkFactory;
	using NeuralNetworkType = org.neuroph.util.NeuralNetworkType;
	using NeuronProperties = org.neuroph.util.NeuronProperties;
	using TransferFunctionType = org.neuroph.util.TransferFunctionType;

	/// <summary>
	/// The NeuroFuzzyReasoner class represents Neuro Fuzzy Reasoner architecture.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class NeuroFuzzyPerceptron : NeuralNetwork
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		public NeuroFuzzyPerceptron(double[][] pointsSets, double[][] timeSets)
		{
			List<int> inputSets = new List<int>();
			inputSets.Add(Convert.ToInt32(4));
			inputSets.Add(Convert.ToInt32(3));

			this.createStudentNFR(2, inputSets, 4, pointsSets, timeSets);
		}

		public NeuroFuzzyPerceptron(int inputNum, List<int> inputSets, int outNum)
		{
			this.createNetwork(inputNum, inputSets, outNum);
		}

		// build example network for student classification
		private void createStudentNFR(int inputNum, List<int> inputSets, int outNum, double[][] pointsSets, double[][] timeSets)
		{

			// set network type
			this.NetworkType = NeuralNetworkType.NEURO_FUZZY_REASONER;

			// createLayer input layer
			NeuronProperties neuronProperties = new NeuronProperties();
			Layer inLayer = LayerFactory.createLayer(inputNum, neuronProperties);
			this.addLayer(inLayer);

			// createLayer fuzzy set layer
			neuronProperties.setProperty("transferFunction", TransferFunctionType.Trapezoid.ToString());
			IEnumerator<int> e = inputSets.GetEnumerator();
			int fuzzySetsNum = 0;
			while (e.MoveNext())
			{
				int i = e.Current;
				fuzzySetsNum = fuzzySetsNum + (int)i;
			}
			Layer setLayer = LayerFactory.createLayer(fuzzySetsNum, neuronProperties);
			this.addLayer(setLayer);

			// TODO: postavi parametre funkcija pripadnosti
			// nizove sa trning elementima iznesi van klase i prosledjuj ih kao
			// parametre
	//		Iterator<Neuron> ii = setLayer.getNeuronsIterator();
			IEnumerator<int> en; // =setLayer.neurons();
			int c = 0;
					foreach (Neuron cell in setLayer.Neurons)
					{
	//		while (ii.hasNext()) {
	//			Neuron cell = ii.next();
				Trapezoid tf = (Trapezoid) cell.TransferFunction;

				if (c <= 3)
				{
					tf.LeftLow = pointsSets[c][0];
					tf.LeftHigh = pointsSets[c][1];
					tf.RightLow = pointsSets[c][3];
					tf.RightHigh = pointsSets[c][2];
				}
				else
				{
					tf.LeftLow = timeSets[c - 4][0];
					tf.LeftHigh = timeSets[c - 4][1];
					tf.RightLow = timeSets[c - 4][3];
					tf.RightHigh = timeSets[c - 4][2];
				}
				c++;
					}

			// povezi prvi i drugi sloj
			int s = 0; // brojac celija sloja skupova (fazifikacije)
			for (int i = 0; i < inputNum; i++) // brojac ulaznih celija
			{
				Neuron from = inLayer.getNeuronAt(i);
				int jmax = (int)inputSets[i];
				for (int j = 0; j < jmax; j++)
				{
					Neuron to = setLayer.getNeuronAt(s);
					ConnectionFactory.createConnection(from, to, 1);
					s++;
				}
			}

			// ----------------------------------------------------------

			// createLayer rules layer
			NeuronProperties ruleNeuronProperties = new NeuronProperties(typeof(Neuron), typeof(WeightedSum), typeof(Linear));
			en = inputSets.GetEnumerator();
			int fuzzyAntNum = 1;
			while (en.MoveNext())
			{
				int i = en.Current;
				fuzzyAntNum = fuzzyAntNum * (int)i;
			}
			Layer ruleLayer = LayerFactory.createLayer(fuzzyAntNum, ruleNeuronProperties);
			this.addLayer(ruleLayer);

			int scIdx = 0; // set cell index

			for (int i = 0; i < inputNum; i++) // brojac ulaza (grupa fuzzy
			{
													// skupova)
				int setsNum = (int)inputSets[i];

				for (int si = 0; si < setsNum; si++) // brojac celija fuzzy
				{
														// skupova
					if (i == 0)
					{
						Neuron from = setLayer.getNeuronAt(si);
						int connPerCell = fuzzyAntNum / setsNum;
						scIdx = si;

						for (int k = 0; k < connPerCell; k++) // brojac celija
						{
																// hipoteza
							Neuron to = ruleLayer.getNeuronAt(si * connPerCell + k);
							ConnectionFactory.createConnection(from, to, 1);
						} // for
					} // if
					else
					{
						scIdx++;
						Neuron from = setLayer.getNeuronAt(scIdx);
						int connPerCell = fuzzyAntNum / setsNum;

						for (int k = 0; k < connPerCell; k++) // brojac celija
						{
																// hipoteza
							int toIdx = si + k * setsNum;
							Neuron to = ruleLayer.getNeuronAt(toIdx);
							ConnectionFactory.createConnection(from, to, 1);
						} // for k
					} // else
				} // for si
			} // for i

			// kreiraj izlazni sloj
			neuronProperties = new NeuronProperties();
			neuronProperties.setProperty("transferFunction", TransferFunctionType.Step.ToString());
			Layer outLayer = LayerFactory.createLayer(outNum, neuronProperties);
			this.addLayer(outLayer);

			ConnectionFactory.fullConnect(ruleLayer, outLayer);

			// inicijalizuj ulazne i izlazne celije
			NeuralNetworkFactory.DefaultIO = this;

			this.LearningRule = new LMS();
		}

		/// <summary>
		/// Creates custom NFR architecture
		/// </summary>
		/// <param name="inputNum">
		///            number of getInputsIterator </param>
		/// <param name="inputSets">
		///            input fuzzy sets </param>
		/// <param name="outNum">
		///            number of outputs </param>
		private void createNetwork(int inputNum, List<int> inputSets, int outNum)
		{

			// set network type
			this.NetworkType = NeuralNetworkType.NEURO_FUZZY_REASONER;

			// CREATE INPUT LAYER
			NeuronProperties neuronProperties = new NeuronProperties();
			Layer inLayer = LayerFactory.createLayer(inputNum, neuronProperties);
			this.addLayer(inLayer);

			// CREATE FUZZY SET LAYER
			neuronProperties.setProperty("transferFunction", TransferFunctionType.Trapezoid.ToString());
			IEnumerator<int> e = inputSets.GetEnumerator();
			int fuzzySetsNum = 0;
			while (e.MoveNext())
			{
				int i = e.Current;
				fuzzySetsNum = fuzzySetsNum + (int)i;
			}
			Layer setLayer = LayerFactory.createLayer(fuzzySetsNum, neuronProperties);
			this.addLayer(setLayer);

			// TODO: postavi parametre funkcija pripadnosti
			// nizove sa trning elementima iznesi van klase i prosledjuj ih kao
			// parametre
	//		Iterator<Neuron> ii = setLayer.getNeuronsIterator();
			IEnumerator<int> en; // =setLayer.neurons();
			int c = 0;
					foreach (Neuron cell in setLayer.Neurons)
					{
	//		while (ii.hasNext()) {
	//			Neuron cell = ii.next();
				Trapezoid tf = (Trapezoid) cell.TransferFunction;
				/*
				 * if (c<=3) { tf.setLeftLow(pointsSets[c][0]);
				 * tf.setLeftHigh(pointsSets[c][1]); tf.setRightLow(pointsSets[c][3]);
				 * tf.setRightHigh(pointsSets[c][2]); } else { tf.setLeftLow(timeSets[c-4][0]);
				 * tf.setLeftHigh(timeSets[c-4][1]); tf.setRightLow(timeSets[c-4][3]);
				 * tf.setRightHigh(timeSets[c-4][2]); } c++;
				 */
					}

			// createLayer connections between input and fuzzy set getLayersIterator
			int s = 0; // brojac celija sloja skupova (fazifikacije)
			for (int i = 0; i < inputNum; i++) // brojac ulaznih celija
			{
				Neuron from = inLayer.getNeuronAt(i);
				int jmax = (int)inputSets[i];
				for (int j = 0; j < jmax; j++)
				{
					Neuron to = setLayer.getNeuronAt(s);
					ConnectionFactory.createConnection(from, to, 1);
					s++;
				}
			}

			// ----------------------------------------------------------

			// kreiraj sloj pravila
			neuronProperties.setProperty("inputFunction", typeof(Min));
			neuronProperties.setProperty("transferFunction", util.TransferFunctionType.Linear.ToString());
			en = inputSets.GetEnumerator();
			int fuzzyAntNum = 1;
			while (en.MoveNext())
			{
				int i = en.Current;
				fuzzyAntNum = fuzzyAntNum * (int)i;
			}
			Layer ruleLayer = LayerFactory.createLayer(fuzzyAntNum, neuronProperties);
			this.addLayer(ruleLayer);

			// povezi set i rule layer

			int scIdx = 0; // set cell index

			for (int i = 0; i < inputNum; i++) // brojac ulaza (grupa fuzzy
			{
													// skupova)
				int setsNum = (int)inputSets[i];

				for (int si = 0; si < setsNum; si++) // brojac celija fuzzy
				{
														// skupova
					if (i == 0)
					{
						Neuron from = setLayer.getNeuronAt(si);
						int connPerCell = fuzzyAntNum / setsNum;
						scIdx = si;

						for (int k = 0; k < connPerCell; k++) // brojac celija
						{
																// hipoteza
							Neuron to = ruleLayer.getNeuronAt(si * connPerCell + k);
							ConnectionFactory.createConnection(from, to, 1);
						} // for
					} // if
					else
					{
						scIdx++;
						Neuron from = setLayer.getNeuronAt(scIdx);
						int connPerCell = fuzzyAntNum / setsNum;

						for (int k = 0; k < connPerCell; k++) // brojac celija
						{
																// hipoteza
							int toIdx = si + k * setsNum;
							Neuron to = ruleLayer.getNeuronAt(toIdx);
							ConnectionFactory.createConnection(from, to, 1);
						} // for k
					} // else
				} // for si
			} // for i

			// set input and output cells for this network
			neuronProperties = new NeuronProperties();
			neuronProperties.setProperty("transferFunction", TransferFunctionType.Step.ToString());
			Layer outLayer = LayerFactory.createLayer(outNum, neuronProperties);
			this.addLayer(outLayer);

			ConnectionFactory.fullConnect(ruleLayer, outLayer);

			// set input and output cells for this network
			NeuralNetworkFactory.DefaultIO = this;

			this.LearningRule = new LMS();
		}

	}

}