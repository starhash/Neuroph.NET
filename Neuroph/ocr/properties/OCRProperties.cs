/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
namespace org.neuroph.ocr.properties
{

	using Letter = org.neuroph.ocr.util.Letter;
	using Text = org.neuroph.ocr.util.Text;

	/// 
	/// <summary>
	/// @author Mihailo Stupar
	/// </summary>
	public abstract class OCRProperties
	{

		protected internal BufferedImage image;
		protected internal int scanQuality;
		protected internal int fontSize;

		protected internal Letter letterInformation;
		protected internal Text textInformation;


		public OCRProperties(Letter letterInformation, Text textInformation)
		{
			this.letterInformation = letterInformation;
			this.textInformation = textInformation;
			this.image = textInformation.Image;
			this.scanQuality = letterInformation.ScanQuality;
			this.fontSize = letterInformation.ScanQuality;
		}

		/// <summary>
		/// dimensions of letter, of spacing, of cropped image... </summary>
		/// <returns> Information about letter  </returns>
		public virtual Letter LetterInformation
		{
			get
			{
				return letterInformation;
			}
		}

		/// <summary>
		/// informations about line positions, word positions... </summary>
		/// <returns> informations about text  </returns>
		public virtual Text TextInformation
		{
			get
			{
				return textInformation;
			}
		}

		/// 
		/// <returns> scanned document </returns>
		public virtual BufferedImage Image
		{
			get
			{
				return image;
			}
		}



	}

}