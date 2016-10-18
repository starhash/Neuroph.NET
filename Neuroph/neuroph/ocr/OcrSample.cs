using System;

/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
namespace org.neuroph.ocr
{

	using org.neuroph.core;
	using Image = org.neuroph.imgrec.image.Image;
	using ImageFactory = org.neuroph.imgrec.image.ImageFactory;

	/// 
	/// <summary>
	/// @author zoran
	/// </summary>
	public class OcrSample
	{

		public static void Main(string[] args)
		{
			NeuralNetwork nnet = NeuralNetwork.load("C:\\Users\\zoran\\Desktop\\nn.nnet");
			OcrPlugin ocrPlugin = (OcrPlugin) nnet.getPlugin(typeof(OcrPlugin));

			// load letter images
			Image charImage = ImageFactory.getImage("C:\\Users\\zoran\\Desktop\\Letters\\A.png");
			char? ch = ocrPlugin.recognizeCharacter(charImage);
			Console.WriteLine(ch);
		}

	}

}