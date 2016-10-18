/// <summary>
/// Copyright 2013 Neuroph Project http://neuroph.sourceforge.net
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
namespace org.neuroph.samples.forestCover
{

	public class Config
	{

		//File name where is saved main data set
		private string dataFilePath;
		//File name where we will save training data set
		private string trainingFileName;
		//File name where we will save test data set
		private string testFileName;
		//File name where we will save balanced data set (3000 of 
		//every type of tree created from training file)    
		private string balancedFileName;
		//File name where we will save normalized balanced data set
		private string normalizedBalancedFileName;
		//File name where we will save trained neural network
		private string trainedNetworkFileName;

		private int inputCount;
		private int firstHiddenLayerCount;
		private int secondHiddenLayerCount;
		private int outputCount;

		public Config()
		{

			dataFilePath = "data_sets/cover type.txt";
			trainingFileName = "data_sets/cover_type_data/training.txt";
			testFileName = "data_sets/cover_type_data/test.txt";
			balancedFileName = "data_sets/cover_type_data/balanceTraining.txt";
			normalizedBalancedFileName = "data_sets/cover_type_data/normalizedBalanceTraining.txt";
			trainedNetworkFileName = "data_sets/cover_type_data/trainedNetwork.txt";

			inputCount = 54;
			firstHiddenLayerCount = 40;
			secondHiddenLayerCount = 20;
			outputCount = 7;

		}

		public virtual string DataFilePath
		{
			get
			{
				return dataFilePath;
			}
			set
			{
				this.dataFilePath = value;
			}
		}


		public virtual string TrainingFileName
		{
			get
			{
				return trainingFileName;
			}
			set
			{
				this.trainingFileName = value;
			}
		}


		public virtual string TestFileName
		{
			get
			{
				return testFileName;
			}
			set
			{
				this.testFileName = value;
			}
		}


		public virtual string BalancedFileName
		{
			get
			{
				return balancedFileName;
			}
			set
			{
				this.balancedFileName = value;
			}
		}


		public virtual string NormalizedBalancedFileName
		{
			get
			{
				return normalizedBalancedFileName;
			}
			set
			{
				this.normalizedBalancedFileName = value;
			}
		}


		public virtual string TrainedNetworkFileName
		{
			get
			{
				return trainedNetworkFileName;
			}
			set
			{
				this.trainedNetworkFileName = value;
			}
		}


		public virtual int InputCount
		{
			get
			{
				return inputCount;
			}
			set
			{
				this.inputCount = value;
			}
		}


		public virtual int FirstHiddenLayerCount
		{
			get
			{
				return firstHiddenLayerCount;
			}
			set
			{
				this.firstHiddenLayerCount = value;
			}
		}


		public virtual int SecondHiddenLayerCount
		{
			get
			{
				return secondHiddenLayerCount;
			}
			set
			{
				this.secondHiddenLayerCount = value;
			}
		}


		public virtual int OutputCount
		{
			get
			{
				return outputCount;
			}
			set
			{
				this.outputCount = value;
			}
		}

	}

}