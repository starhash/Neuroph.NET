using System;

namespace org.org.neuroph.contrib.eval.classification
{


	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;

	/// <summary>
	/// Class used to test if there is statistical significant difference between two classifiers
	/// </summary>
	public class McNemarTest
	{

        //JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
        //ORIGINAL LINE: private int[,] contigencyMatrix = new int[2,2];
        private int[,] contigencyMatrix = new int[2, 2];

		/// <param name="network1"> first trained neurl netowrk </param>
		/// <param name="network2"> second trained neural network </param>
		/// <param name="dataSet">  data set used for performance evaluation </param>
		/// <returns> if there exists significant difference between two classification models </returns>
		public virtual bool evaluateNetworks(NeuralNetwork network1, NeuralNetwork network2, DataSet dataSet)
		{
			foreach (DataSetRow dataRow in dataSet.Rows)
			{
				forwardPass(network1, dataRow);
				forwardPass(network2, dataRow);

				double[] networkOutput1 = network1.Output;
				double[] networkOutput2 = network2.Output;

				int maxNeuronIdx1 = Utils.maxIdx(networkOutput1);
				int maxNeuronIdx2 = Utils.maxIdx(networkOutput2);

				ClassificationResult output1 = new ClassificationResult(maxNeuronIdx1, networkOutput1[maxNeuronIdx1]);
				ClassificationResult output2 = new ClassificationResult(maxNeuronIdx2, networkOutput2[maxNeuronIdx2]);

	//            ClassificationResult output1 = ClassificationResult.fromMaxOutput(network1.getOutput());
	//            ClassificationResult output2 = ClassificationResult.fromMaxOutput(network2.getOutput());

				//are their results different
				if (output1.ClassIdx != output2.ClassIdx)
				{
					//if first one is correct and second incorrect
					if (output1.ClassIdx == getDesiredClass(dataRow.DesiredOutput))
					{
						contigencyMatrix[1,0]++;
						//if first is incorrect and second is correct
					}
					else
					{
						contigencyMatrix[0,1]++;
					}
				}
				else
				{
					//if both are correct
					if (output1.ClassIdx == getDesiredClass(dataRow.DesiredOutput))
					{
						contigencyMatrix[1,1]++;
						//if both are incorrect
					}
					else
					{
						contigencyMatrix[0,0]++;
					}
				}
			}

			printContingencyMatrix();

			double a = Math.Abs(contigencyMatrix[0,1] - contigencyMatrix[1,0]) - 1;
			double hiSquare = (a * a) / (contigencyMatrix[0,1] + contigencyMatrix[1,0]);


			System.Console.WriteLine(hiSquare);
			return hiSquare > 3.841;
		}

		private void printContingencyMatrix()
		{
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					Console.Write(contigencyMatrix[i,j] + " ");
				}
				System.Console.WriteLine();
			}
		}

		private int getDesiredClass(double[] output)
		{

			for (int i = 0; i < output.Length; i++)
			{
				if (output[i] == 1)
				{
					return i;
				}
			}
			return -1;
		}

		private void forwardPass(NeuralNetwork neuralNetwork, DataSetRow dataRow)
		{
			neuralNetwork.Input = dataRow.Input;
			neuralNetwork.calculate();
		}

	}

}