/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
namespace org.neuroph.imgrec.filter
{

	/// <summary>
	/// Contains image and name of applied filter.
	/// 
	/// @author Aleksandar
	/// </summary>
	public class FilteredImage
	{

		private BufferedImage image;
		private string filterName;

		public FilteredImage(BufferedImage image, string filterName)
		{
			this.image = image;
			this.filterName = filterName;
		}

		public virtual BufferedImage Image
		{
			get
			{
				return image;
			}
			set
			{
				this.image = value;
			}
		}


		public virtual string FilterName
		{
			get
			{
				return filterName;
			}
			set
			{
				this.filterName = value;
			}
		}


	}

}