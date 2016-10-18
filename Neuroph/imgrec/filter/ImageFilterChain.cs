using System;
using System.Collections.Generic;

namespace org.neuroph.imgrec.filter
{


	/// <summary>
	/// Process images by applying all filters in chain 
	/// @author Sanja
	/// </summary>
	[Serializable]
	public class ImageFilterChain : ImageFilter
	{

		private List<ImageFilter> filters = new List<ImageFilter>();
		private string chainName;
		/// <summary>
		/// Add filter to chain </summary>
		/// <param name="filter"> filter to be added </param>
		public virtual void addFilter(ImageFilter filter)
		{
			filters.Add(filter);
		}
		/// <summary>
		/// Remove filter from chain </summary>
		/// <param name="filter"> filter to be removed </param>
		/// <returns> true if filter is removed  </returns>
		public virtual bool removeFilter(ImageFilter filter)
		{
			return filters.Remove(filter);
		}

		/// <summary>
		/// Apply all filters from a chain on image </summary>
		/// <param name="image"> image to process </param>
		/// <returns> processed image </returns>
		public virtual BufferedImage processImage(BufferedImage image)
		{

			BufferedImage tempImage = image;
			foreach (ImageFilter filter in filters)
			{
				BufferedImage filteredImage = filter.processImage(tempImage);
				tempImage = filteredImage;
			}

			return tempImage;

		}
		/// <summary>
		/// Returns images of all stages in processing
		/// Used for testing </summary>
		/// <param name="image"> </param>
		/// <returns>  </returns>
		public virtual List<FilteredImage> processImageTest(BufferedImage image)
		{
			List<FilteredImage> list = new List<FilteredImage>();
			BufferedImage tempImage = image;
			foreach (ImageFilter filter in filters)
			{
				BufferedImage processedImage = filter.processImage(tempImage);
				string filterName = filter.ToString();
				FilteredImage filteredImage = new FilteredImage(processedImage,filterName);
				list.Add(filteredImage);
				tempImage = processedImage;
			}

			return list;
		}
		/// <summary>
		/// Get filters from chain </summary>
		/// <returns>  </returns>
		public virtual List<ImageFilter> Filters
		{
			get
			{
				return filters;
			}
			set
			{
				this.filters = value;
			}
		}


		public virtual string ChainName
		{
			get
			{
				return chainName;
			}
			set
			{
				this.chainName = value;
			}
		}


		public override string ToString()
		{
			return chainName;
		}

	}

}