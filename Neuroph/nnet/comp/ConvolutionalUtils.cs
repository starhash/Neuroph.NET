/// <summary>
/// Copyright 2013 Neuroph Project http://neuroph.sourceforge.net
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
/// 
///    http://www.apache.org/licenses/LICENSE-2.0
/// 
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// </summary>

namespace org.neuroph.nnet.comp
{

	using FeatureMapLayer = org.neuroph.nnet.comp.layer.FeatureMapLayer;
	using PoolingLayer = org.neuroph.nnet.comp.layer.PoolingLayer;
	using ConvolutionalLayer = org.neuroph.nnet.comp.layer.ConvolutionalLayer;
	using FeatureMapsLayer = org.neuroph.nnet.comp.layer.FeatureMapsLayer;
	/// <summary>
	/// Utility functions for convolutional networks
	/// 
	/// @author Boris Fulurija
	/// @author Zorn Sevarac
	/// </summary>
	public class ConvolutionalUtils
	{

		/// <summary>
		/// Creates full connectivity between feature maps in two layers
		/// </summary>
		/// <param name="fromLayer"> from feature maps layer </param>
		/// <param name="toLayer">   to feature maps layer </param>
		public static void fullConnectMapLayers(FeatureMapsLayer fromLayer, FeatureMapsLayer toLayer)
		{
			if (toLayer is ConvolutionalLayer)
			{
				for (int i = 0; i < fromLayer.NumberOfMaps; i++)
				{
					for (int j = 0; j < toLayer.NumberOfMaps; j++)
					{
						FeatureMapLayer fromMap = fromLayer.getFeatureMap(i);
						FeatureMapLayer toMap = toLayer.getFeatureMap(j);
						toLayer.connectMaps(fromMap, toMap); // da li treba svaka sa svakom ???
					}
				}
			} // ???? CHECK: da li je ovo dobro
			else if (toLayer is PoolingLayer)
			{
				for (int i = 0; i < toLayer.NumberOfMaps; i++)
				{
					FeatureMapLayer fromMap = fromLayer.getFeatureMap(i);
					FeatureMapLayer toMap = toLayer.getFeatureMap(i);
					toLayer.connectMaps(fromMap, toMap);
				}
			}
		}


		/// <summary>
		/// Creates connections between two feature maps - not used???
		/// </summary>
		/// <param name="fromLayer">           parent layer for from feature map </param>
		/// <param name="toLayer">             parent layer for to feature map </param>
		/// <param name="fromFeatureMapIndex"> index of from feature map </param>
		/// <param name="toFeatureMapIndex">   index of to feature map </param>
		public static void connectFeatureMaps(FeatureMapsLayer fromLayer, FeatureMapsLayer toLayer, int fromFeatureMapIndex, int toFeatureMapIndex)
		{
			FeatureMapLayer fromMap = fromLayer.getFeatureMap(fromFeatureMapIndex);
			FeatureMapLayer toMap = toLayer.getFeatureMap(toFeatureMapIndex);
			toLayer.connectMaps(fromMap, toMap);
		}

	}

}