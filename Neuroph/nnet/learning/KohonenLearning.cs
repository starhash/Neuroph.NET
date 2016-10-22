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

namespace org.neuroph.nnet.learning
{

	using Connection = org.neuroph.core.Connection;
	using Layer = org.neuroph.core.Layer;
	using org.neuroph.core;
	using Neuron = org.neuroph.core.Neuron;
	using LearningEvent = org.neuroph.core.events.LearningEvent;
	using LearningRule = org.neuroph.core.learning.LearningRule;
	using DataSetRow = org.neuroph.core.data.DataSetRow;
	using DataSet = org.neuroph.core.data.DataSet;

	/// <summary>
	/// Learning algorithm for Kohonen network.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class KohonenLearning : LearningRule
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		internal double learningRate = 0.9d;
		internal int[] iterations = new int[] {100, 0};
		internal double[] decStep = new double[2];
		internal int mapSize = 0;
		internal int[] nR = new int[] {1, 1}; // neighborhood radius
		internal int currentIteration;


		public KohonenLearning() : base()
		{
		}

			public override void learn(DataSet trainingSet)
			{

			for (int phase = 0; phase < 2; phase++)
			{
				for (int k = 0; k < iterations[phase]; k++)
				{
					IEnumerator<DataSetRow> iterator = trainingSet.iterator();
					while (iterator.MoveNext() && !Stopped)
					{
						DataSetRow trainingSetRow = iterator.Current;
						learnPattern(trainingSetRow, nR[phase]);
					} // while
					currentIteration = k;
									fireLearningEvent(new LearningEvent(this, LearningEvent.Type.EPOCH_ENDED));
					if (Stopped)
					{
						return;
					}
				} // for k
				learningRate = learningRate * 0.5;
			} // for phase
			}

		private void learnPattern(DataSetRow dataSetRow, int neighborhood)
		{
			neuralNetwork.Input = dataSetRow.Input;
			neuralNetwork.calculate();
			Neuron winner = ClosestNeuron;
			if (winner.Output == 0)
			{
				return; // ako je vec istrenirana jedna celija, izadji
			}

			Layer mapLayer = neuralNetwork.getLayerAt(1);
			int winnerIdx = mapLayer.IndexOf(winner);
			adjustCellWeights(winner, 0);

			int cellNum = mapLayer.NeuronsCount;
			for (int p = 0; p < cellNum; p++)
			{
				if (p == winnerIdx)
				{
					continue;
				}
				if (isNeighbor(winnerIdx, p, neighborhood))
				{
					Neuron cell = mapLayer.getNeuronAt(p);
					adjustCellWeights(cell, 1);
				} // if
			} // for

		}

		// get unit with closetst weight vector
		private Neuron ClosestNeuron
		{
			get
			{
				Neuron winner = new Neuron();
				double minOutput = 100;
						foreach (Neuron n in this.neuralNetwork.getLayerAt(1).Neurons)
						{
					double @out = n.Output;
					if (@out < minOutput)
					{
						minOutput = @out;
						winner = n;
					} // if
						} // while
				return winner;
			}
		}

		private void adjustCellWeights(Neuron cell, int r)
		{
					foreach (Connection conn in cell.InputConnections)
					{
				double dWeight = (learningRate / (r + 1)) * (conn.Input - conn.Weight.Value);
				conn.Weight.inc(dWeight);
					}
		}

		private bool isNeighbor(int i, int j, int n)
		{
			// i - centralna celija
			// n - velicina susedstva
			// j - celija za proveru
			n = 1;
			int d = mapSize;

			// if (j<(i-n*d-n)||(j>(i+n*d+n))) return false;

			int rt = n; // broj celija ka gore
			while ((i - rt * d) < 0)
			{
				rt--;
			}

			int rb = n; // broj celija ka dole
			while ((i + rb * d) > (d * d - 1))
			{
				rb--;
			}

			for (int g = -rt; g <= rb; g++)
			{
				int rl = n; // broj celija u levu stranu
				int rlMod = (i - rl) % d;
				int i_mod = i % d;
				while (rlMod > i_mod)
				{
					rl--;
					rlMod = (i - rl) % d;
				}

				int rd = n; // broj celija u desnu stranu
				int rdMod = (i + rd) % d;
				while (rdMod < i_mod)
				{
					rd--;
					rdMod = (i + rd) % d;
				}

				if ((j >= (i + g * d - rl)) && (j <= (i + g * d + rd)))
				{
					return true;
				}
				// else if (j<(i+g*d-rl)) return false;
			} // for
			return false;
		}

		public virtual double LearningRate
		{
			get
			{
				return learningRate;
			}
			set
			{
				this.learningRate = value;
			}
		}


		public virtual void setIterations(int Iphase, int IIphase)
		{
			this.iterations[0] = Iphase;
			this.iterations[1] = IIphase;
		}

		public virtual int Iteration
		{
			get
			{
				return currentIteration;
			}
		}

		public virtual int MapSize
		{
			get
			{
				return mapSize;
			}
		}

			public override NeuralNetwork NeuralNetwork
			{
				set
				{
					base.NeuralNetwork = value;
					int neuronsNum = value.getLayerAt(1).NeuronsCount;
					mapSize = (int) Math.Sqrt(neuronsNum);
				}
			}

	}

}