using System.Collections.Generic;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
namespace org.neuroph.ocr.util
{

	using Histogram = org.neuroph.ocr.util.histogram.Histogram;

	/// 
	/// <summary>
	/// @author Mihailo Stupar
	/// </summary>
	public class Letter
	{

		private int cropWidth;
		private int cropHeight;
		private int smallestSizeLetter;
		private int letterSize;
		private int trashSize;
		private int spaceGap;

		private int scanQuality;
		private int fontSize;
		private BufferedImage image;
		private int[] heightHistogram;
		private int[] gradient;

	//    public Letter(int scanQuality, int fontSize) {
	//        this.scanQuality = scanQuality;
	//        this.fontSize = fontSize;
	//
	//        calculateDimensions();
	//        calculateSmallestSizeLetter();
	//        calculateLetterSize();
	//        calculateTrashsize();
	//        calculateSpaceGap();
	//    }

		public Letter(int scanQuality, BufferedImage image)
		{
			this.scanQuality = scanQuality;
			this.image = image;
			heightHistogram = Histogram.heightHistogram(image);
			gradient = Histogram.gradient(heightHistogram);
			calculateSmallestSizeLetter();
			List<int?> rowHeights = OCRUtilities.rowHeights(gradient, smallestSizeLetter);
			int meanHeight = (int) caluclateMean(rowHeights);
			calculateDimensions(meanHeight);
			calculateLetterSize(meanHeight);
			calculateSpaceGap(meanHeight);


		}


		private void calculateDimensions(int meanHeight)
		{
			int offset = (int)(0.1 * meanHeight);
			cropWidth = meanHeight + offset;
			cropHeight = meanHeight + offset;
		}

		private void calculateSmallestSizeLetter()
		{
			if (scanQuality == 300)
			{
				smallestSizeLetter = 9;
			}
			if (scanQuality == 600)
			{
				smallestSizeLetter = 18;
			}
			if (scanQuality == 1200)
			{
				smallestSizeLetter = 36;
			}
		}






		private void calculateLetterSize(int meanHeight)
		{
			letterSize = meanHeight;
		}

		private void calculateTrashsize(int meanHeight)
		{
			int offset = (int)(0.1 * meanHeight);
			trashSize = meanHeight - offset;
		}

		private void calculateSpaceGap(int meanHeight)
		{
			spaceGap = (int)(0.3 * meanHeight);
		}

	//    /**
	//     * If you want to recognize small characters as dots and comas. Otherwise
	//     * the algorithm will ignore them.
	//     */
	//    public void recognizeDots() {
	//        trashSize = 9;
	//    }

		/// <returns> height of the image with single character. </returns>
		public virtual int CropHeight
		{
			get
			{
				return cropHeight;
			}
			set
			{
				this.cropHeight = value;
			}
		}

		/// <returns> width of the image with single character. </returns>
		public virtual int CropWidth
		{
			get
			{
				return cropWidth;
			}
			set
			{
				this.cropWidth = value;
			}
		}

		/// 
		/// <returns> predicted letter size (height) based on the scanQuality and
		/// fontSize </returns>
		public virtual int LetterSize
		{
			get
			{
				return letterSize;
			}
			set
			{
				this.letterSize = value;
			}
		}

		/// <summary>
		/// Used for finding rows in text. Smaller value is probably dot or coma
		/// which is not recognized as row. Size is actually height of letter
		/// </summary>
		/// <returns> predicted smallest size of letter. </returns>
		public virtual int SmallestSizeLetter
		{
			get
			{
				return smallestSizeLetter;
			}
			set
			{
				this.smallestSizeLetter = value;
			}
		}

		/// <summary>
		/// Avoid to recognize the trash. size is actually number of pixels
		/// </summary>
		/// <returns> predicted smallest size of trash </returns>
		public virtual int TrashSize
		{
			get
			{
				return trashSize;
			}
			set
			{
				this.trashSize = value;
			}
		}

		/// 
		/// <returns> the space(measured in pixels) that should represent the space
		/// typed on keyboard </returns>
		public virtual int SpaceGap
		{
			get
			{
				return spaceGap;
			}
			set
			{
				this.spaceGap = value;
			}
		}

		/// 
		/// <returns> scan quality measured in dpi  </returns>
		public virtual int ScanQuality
		{
			get
			{
				return scanQuality;
			}
		}

		/// 
		/// <returns> font size measured in pt </returns>
		public virtual int FontSize
		{
			get
			{
				return fontSize;
			}
			set
			{
				this.fontSize = value;
			}
		}








		private double caluclateMean(List<int?> list)
		{
			double sum = 0;
			foreach (int? element in list)
			{
				sum += element;
			}
			return sum / list.Count;
		}





	}

}