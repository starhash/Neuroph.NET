using System;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
namespace org.neuroph.nnet.comp
{

	  /// <summary>
	  /// Dimensions (width and height) of the Layer2D
	  /// </summary>
	[Serializable]
	public class Dimension2D
	{

			private const long serialVersionUID = -4491706467345191108L;

			private int width;
			private int height;

			/// <summary>
			/// Creates new dimensions with specified width and height
			/// </summary>
			/// <param name="width">  total number  of columns </param>
			/// <param name="height"> total number of rows </param>
			public Dimension2D(int width, int height)
			{
				this.width = width;
				this.height = height;
			}

			public virtual int Width
			{
				get
				{
					return width;
				}
				set
				{
					this.width = value;
				}
			}


			public virtual int Height
			{
				get
				{
					return height;
				}
				set
				{
					this.height = value;
				}
			}


			public override string ToString()
			{
				string dimensions = "Width = " + width + "; Height = " + height;
				return dimensions;
			}
	}
}