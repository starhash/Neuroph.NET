using System.Collections.Generic;

namespace org.neuroph.samples.convolution.mnist
{


	using DataSet = org.neuroph.core.data.DataSet;
	using DataSetRow = org.neuroph.core.data.DataSetRow;

	/// <summary>
	/// Provides methods for loading MNIST dataset (training and test set)
	/// <p/>
	/// TODO: reorganizuj tako da imamo samo jedu metodu loadDataSet kojoj se
	/// prosledjuje naziv i koja ucitava i datas et i test set
	/// a ne da postoje dve posebne metode za to koje rade potpuno istu stvar
	/// 
	/// @author Boris Fulurija
	/// @author Zoran Sevarac
	/// </summary>
	public class MNISTDataSet
	{

		public const string TRAIN_LABEL_NAME = "data_sets/train-labels.idx1-ubyte";
		public const string TRAIN_IMAGE_NAME = "data_sets/train-images.idx3-ubyte";
		public const string TEST_LABEL_NAME = "data_sets/t10k-labels.idx1-ubyte";
		public const string TEST_IMAGE_NAME = "data_sets/t10k-images.idx3-ubyte";


//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static org.neuroph.core.data.DataSet createFromFile(String labelPath, String imagePath, int sampleCount) throws java.io.IOException
		public static DataSet createFromFile(string labelPath, string imagePath, int sampleCount)
		{
			List<MNISTImage> mnistImages = MNISTImage.loadDigitImages(labelPath, imagePath);
			DataSet dataSet = createDataSet(mnistImages, sampleCount);
			return dataSet;
		}

		// TODO; remove sampleCount param its ame as  list count
		private static DataSet createDataSet(List<MNISTImage> imageList, int sampleCount)
		{

			int pixelCount = imageList[0].Size;
			int totalSize = 1024;
			DataSet dataSet = new DataSet(totalSize, 10);

			for (int i = 0; i < sampleCount; i++)
			{
				MNISTImage dImage = imageList[i];
				double[] input = new double[totalSize];
				double[] output = new double[10];
				for (int j = 0; j < 10; j++)
				{
					output[j] = 0;
				}

				for (int j = 0; j < totalSize; j++)
				{
					input[j] = 0;
				}

				output[dImage.Label] = 1;
				sbyte[] imageData = dImage.Data;
				int k = 66;
				for (int j = 0; j < pixelCount; j++)
				{
					if ((imageData[j] & 0xff) > 0)
					{
						input[k++] = 255;
					}
					else
					{
						k++;
					}
					if (j % 28 == 27)
					{
						k += 4;
					}
				}
				DataSetRow row = new DataSetRow(input, output);
				dataSet.addRow(row);
			}
			dataSet.setColumnName(1024, "0");
			dataSet.setColumnName(1025, "1");
			dataSet.setColumnName(1026, "2");
			dataSet.setColumnName(1027, "3");
			dataSet.setColumnName(1028, "4");
			dataSet.setColumnName(1029, "5");
			dataSet.setColumnName(1030, "6");
			dataSet.setColumnName(1031, "7");
			dataSet.setColumnName(1032, "8");
			dataSet.setColumnName(1033, "9");

			return dataSet;
		}
	}

}