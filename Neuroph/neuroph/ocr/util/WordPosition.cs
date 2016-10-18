/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace org.neuroph.ocr.util
{

	/// 
	/// <summary>
	/// @author Mihailo Stupar
	/// </summary>
	public class WordPosition
	{

		private int startPixel;
		private int endPixel;

		public WordPosition(int startPixel, int endPixel)
		{
			this.startPixel = startPixel;
			this.endPixel = endPixel;
		}

		public virtual int StartPixel
		{
			get
			{
				return startPixel;
			}
		}

		public virtual int EndPixel
		{
			get
			{
				return endPixel;
			}
		}


	}

}