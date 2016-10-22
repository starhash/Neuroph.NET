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

    /// <summary>
    /// Contains neural network types and labels.
    /// </summary>
    [System.Serializable]
    public sealed class NeuralNetworkType
	{
		public static readonly NeuralNetworkType ADALINE = new NeuralNetworkType("ADALINE", InnerEnum.ADALINE, "Adaline");
		public static readonly NeuralNetworkType PERCEPTRON = new NeuralNetworkType("PERCEPTRON", InnerEnum.PERCEPTRON, "Perceptron");
		public static readonly NeuralNetworkType MULTI_LAYER_PERCEPTRON = new NeuralNetworkType("MULTI_LAYER_PERCEPTRON", InnerEnum.MULTI_LAYER_PERCEPTRON, "Multi Layer Perceptron");
		public static readonly NeuralNetworkType HOPFIELD = new NeuralNetworkType("HOPFIELD", InnerEnum.HOPFIELD, "Hopfield");
		public static readonly NeuralNetworkType KOHONEN = new NeuralNetworkType("KOHONEN", InnerEnum.KOHONEN, "Kohonen");
		public static readonly NeuralNetworkType NEURO_FUZZY_REASONER = new NeuralNetworkType("NEURO_FUZZY_REASONER", InnerEnum.NEURO_FUZZY_REASONER, "Neuro Fuzzy Reasoner");
		public static readonly NeuralNetworkType SUPERVISED_HEBBIAN_NET = new NeuralNetworkType("SUPERVISED_HEBBIAN_NET", InnerEnum.SUPERVISED_HEBBIAN_NET, "Supervised Hebbian network");
		public static readonly NeuralNetworkType UNSUPERVISED_HEBBIAN_NET = new NeuralNetworkType("UNSUPERVISED_HEBBIAN_NET", InnerEnum.UNSUPERVISED_HEBBIAN_NET, "Unsupervised Hebbian network");
		public static readonly NeuralNetworkType COMPETITIVE = new NeuralNetworkType("COMPETITIVE", InnerEnum.COMPETITIVE, "Competitive");
		public static readonly NeuralNetworkType MAXNET = new NeuralNetworkType("MAXNET", InnerEnum.MAXNET, "Maxnet");
		public static readonly NeuralNetworkType INSTAR = new NeuralNetworkType("INSTAR", InnerEnum.INSTAR, "Instar");
		public static readonly NeuralNetworkType OUTSTAR = new NeuralNetworkType("OUTSTAR", InnerEnum.OUTSTAR, "Outstar");
		public static readonly NeuralNetworkType RBF_NETWORK = new NeuralNetworkType("RBF_NETWORK", InnerEnum.RBF_NETWORK, "RBF Network");
		public static readonly NeuralNetworkType BAM = new NeuralNetworkType("BAM", InnerEnum.BAM, "BAM");
			public static readonly NeuralNetworkType BOLTZMAN = new NeuralNetworkType("BOLTZMAN", InnerEnum.BOLTZMAN, "Boltzman");
			public static readonly NeuralNetworkType COUNTERPROPAGATION = new NeuralNetworkType("COUNTERPROPAGATION", InnerEnum.COUNTERPROPAGATION, "CounterPropagation");
			public static readonly NeuralNetworkType INSTAR_OUTSTAR = new NeuralNetworkType("INSTAR_OUTSTAR", InnerEnum.INSTAR_OUTSTAR, "InstarOutstar");
			public static readonly NeuralNetworkType PCA_NETWORK = new NeuralNetworkType("PCA_NETWORK", InnerEnum.PCA_NETWORK, "PCANetwork");
		public static readonly NeuralNetworkType RECOMMENDER = new NeuralNetworkType("RECOMMENDER", InnerEnum.RECOMMENDER, "Recommender");

		private static readonly List<NeuralNetworkType> valueList = new List<NeuralNetworkType>();

		static NeuralNetworkType()
		{
			valueList.Add(ADALINE);
			valueList.Add(PERCEPTRON);
			valueList.Add(MULTI_LAYER_PERCEPTRON);
			valueList.Add(HOPFIELD);
			valueList.Add(KOHONEN);
			valueList.Add(NEURO_FUZZY_REASONER);
			valueList.Add(SUPERVISED_HEBBIAN_NET);
			valueList.Add(UNSUPERVISED_HEBBIAN_NET);
			valueList.Add(COMPETITIVE);
			valueList.Add(MAXNET);
			valueList.Add(INSTAR);
			valueList.Add(OUTSTAR);
			valueList.Add(RBF_NETWORK);
			valueList.Add(BAM);
			valueList.Add(BOLTZMAN);
			valueList.Add(COUNTERPROPAGATION);
			valueList.Add(INSTAR_OUTSTAR);
			valueList.Add(PCA_NETWORK);
			valueList.Add(RECOMMENDER);
		}

		public enum InnerEnum
		{
			ADALINE,
			PERCEPTRON,
			MULTI_LAYER_PERCEPTRON,
			HOPFIELD,
			KOHONEN,
			NEURO_FUZZY_REASONER,
			SUPERVISED_HEBBIAN_NET,
			UNSUPERVISED_HEBBIAN_NET,
			COMPETITIVE,
			MAXNET,
			INSTAR,
			OUTSTAR,
			RBF_NETWORK,
			BAM,
			BOLTZMAN,
			COUNTERPROPAGATION,
			INSTAR_OUTSTAR,
			PCA_NETWORK,
			RECOMMENDER
		}

		private readonly string nameValue;
		private readonly int ordinalValue;
		private readonly InnerEnum innerEnumValue;
		private static int nextOrdinal = 0;

		private string typeLabel;

		private NeuralNetworkType(string name, InnerEnum innerEnum, string typeLabel)
		{
			this.typeLabel = typeLabel;

			nameValue = name;
			ordinalValue = nextOrdinal++;
			innerEnumValue = innerEnum;
		}

		public string TypeLabel
		{
			get
			{
				return typeLabel;
			}
		}

		public static List<NeuralNetworkType> values()
		{
			return valueList;
		}

		public InnerEnum InnerEnumValue()
		{
			return innerEnumValue;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public override string ToString()
		{
			return nameValue;
		}

		public static NeuralNetworkType valueOf(string name)
		{
			foreach (NeuralNetworkType enumInstance in NeuralNetworkType.values())
			{
				if (enumInstance.nameValue == name)
				{
					return enumInstance;
				}
			}
			throw new System.ArgumentException(name);
		}
	}

}