/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace org.neuroph.imgrec.filter
{

	/// <summary>
	/// Interface for image filter 
	/// @author Sanja
	/// </summary>
	public interface ImageFilter
	{
		BufferedImage processImage(BufferedImage image);
	}

}