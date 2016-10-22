using System.Collections.Generic;

namespace org.neuroph.samples.convolution.mnist
{



	/// <summary>
	/// Represents one Image from MNIST dataset
	/// </summary>
	public class MNISTImage
	{


		private int label;
			private sbyte[] imageData;

		/// <summary>
		/// the following constants are defined as per the values described at
		/// http://yann.lecun.com/exdb/mnist/
		/// 
		/// </summary>

		private const int MAGIC_OFFSET = 0; // position of the magic number - its allways first in file
		private const int OFFSET_SIZE = 4; // in bytes ???

		private const int LABEL_MAGIC = 2049; // magic number for labels?
		private const int IMAGE_MAGIC = 2051; // magic number for images?

		private const int NUMBER_ITEMS_OFFSET = 4; // number of images or labels in dataset
		private const int ITEMS_SIZE = 4; // size of rows number 32-bit integer = 4 bytes


		public const int ROWS = 28; // number of image rows
			private const int NUMBER_OF_ROWS_OFFSET = 8; // position of the number of rows
		private const int ROWS_SIZE = 4; // size of rows number 32-bit integer = 4 bytes

		public const int COLUMNS = 28; // number of image columns
		private const int NUMBER_OF_COLUMNS_OFFSET = 12; // position of the number of columns
		private const int COLUMNS_SIZE = 4; // size of columns number 32-bit integer = 4 bytes

		private const int IMAGE_OFFSET = 16; // position where image data starts
		private static readonly int IMAGE_SIZE = ROWS * COLUMNS; // size of the image


		public MNISTImage(int label, sbyte[] imageData)
		{
			this.imageData = imageData;
			this.label = label;
		}

		public virtual int Label
		{
			get
			{
				return label;
			}
		}

		public virtual sbyte[] Data
		{
			get
			{
				return imageData;
			}
		}

		public virtual int Size
		{
			get
			{
				return imageData.Length;
			}
		}


//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static java.util.List<MNISTImage> loadDigitImages(String labelFileName, String imageFileName) throws java.io.IOException
		public static List<MNISTImage> loadDigitImages(string labelFileName, string imageFileName)
		{
			List<MNISTImage> images = new List<MNISTImage>();

			ByteArrayOutputStream labelBuffer = new ByteArrayOutputStream();
			ByteArrayOutputStream imageBuffer = new ByteArrayOutputStream();
					InputStream labelInputStream = new FileInputStream(labelFileName);
			InputStream imageInputStream = new FileInputStream(imageFileName);
		/*	InputStream labelInputStream = MNISTDataSet.class.getResourceAsStream(labelFileName);
			InputStream imageInputStream = MNISTDataSet.class.getResourceAsStream(imageFileName);*/

			int read;
			sbyte[] buffer = new sbyte[16384];

			while ((read = labelInputStream.read(buffer, 0, buffer.Length)) != -1)
			{
				labelBuffer.write(buffer, 0, read);
			}

			labelBuffer.flush();

			while ((read = imageInputStream.read(buffer, 0, buffer.Length)) != -1)
			{
				imageBuffer.write(buffer, 0, read);
			}

			imageBuffer.flush();

			sbyte[] labelBytes = labelBuffer.toByteArray();
			sbyte[] imageBytes = imageBuffer.toByteArray();

			sbyte[] labelMagic = Arrays.copyOfRange(labelBytes, 0, OFFSET_SIZE);
			sbyte[] imageMagic = Arrays.copyOfRange(imageBytes, 0, OFFSET_SIZE);

			if (ByteBuffer.wrap(labelMagic).Int != LABEL_MAGIC)
			{
				throw new IOException("Bad magic number in label file!");
			}

			if (ByteBuffer.wrap(imageMagic).Int != IMAGE_MAGIC)
			{
				throw new IOException("Bad magic number in image file!");
			}

			int numberOfLabels = ByteBuffer.wrap(Arrays.copyOfRange(labelBytes, NUMBER_ITEMS_OFFSET, NUMBER_ITEMS_OFFSET + ITEMS_SIZE)).Int;
			int numberOfImages = ByteBuffer.wrap(Arrays.copyOfRange(imageBytes, NUMBER_ITEMS_OFFSET, NUMBER_ITEMS_OFFSET + ITEMS_SIZE)).Int;

			if (numberOfImages != numberOfLabels)
			{
				throw new IOException("The number of labels and images do not match!");
			}

			int numRows = ByteBuffer.wrap(Arrays.copyOfRange(imageBytes, NUMBER_OF_ROWS_OFFSET, NUMBER_OF_ROWS_OFFSET + ROWS_SIZE)).Int;
			int numCols = ByteBuffer.wrap(Arrays.copyOfRange(imageBytes, NUMBER_OF_COLUMNS_OFFSET, NUMBER_OF_COLUMNS_OFFSET + COLUMNS_SIZE)).Int;

			if (numRows != ROWS && numCols != COLUMNS)
			{
				throw new IOException("Bad image. Rows and columns do not equal " + ROWS + "x" + COLUMNS);
			}

			for (int i = 0; i < numberOfLabels; i++)
			{
				int label = labelBytes[OFFSET_SIZE + ITEMS_SIZE + i];
				sbyte[] imageData = Arrays.copyOfRange(imageBytes, (i * IMAGE_SIZE) + IMAGE_OFFSET, (i * IMAGE_SIZE) + IMAGE_OFFSET + IMAGE_SIZE);

				images.Add(new MNISTImage(label, imageData));
			}

			return images;
		}

	}

}