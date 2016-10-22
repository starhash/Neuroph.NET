/// <summary>
/// Copyright 2013 Neuroph Project http://neuroph.sourceforge.net
/// 
/// Licensed under the Apache License, Version 2.0 (the "License"); you may not
/// use this file except in compliance with the License. You may obtain a copy of
/// the License at
/// 
/// http://www.apache.org/licenses/LICENSE-2.0
/// 
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
/// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
/// License for the specific language governing permissions and limitations under
/// the License.
/// </summary>
namespace org.neuroph.nnet.comp.layer
{

	using Neuron = org.neuroph.core.Neuron;
	using Weight = org.neuroph.core.Weight;
	using Max = org.neuroph.core.input.Max;
	using Ramp = org.neuroph.core.transfer.Ramp;
	using Tanh = org.neuroph.core.transfer.Tanh;
	using ConnectionFactory = org.neuroph.util.ConnectionFactory;
	using NeuronProperties = org.neuroph.util.NeuronProperties;

	/// <summary>
	/// Pooling layer is a special type of feature maps layer (FeatureMapsLayer)
	/// which is used in convolutional networks. It contains neurons with max input
	/// function and method for creating pooling layer specific conectivity patterns.
	/// The role of pooling layer is dimensionality and complexity reduction,
	/// while it keeps essential information.
	/// 
	/// @author Boris Fulurija
	/// @author Zoran Sevarac </summary>
	/// <seealso cref= FeatureMapsLayer </seealso>
	public class PoolingLayer : FeatureMapsLayer
	{

		private const long serialVersionUID = -6771501759374920878L;

		private Kernel kernel;

		/// <summary>
		/// Default neuron properties for pooling layer
		/// </summary>
		public static readonly NeuronProperties DEFAULT_NEURON_PROP = new NeuronProperties();

		static PoolingLayer()
		{
			DEFAULT_NEURON_PROP.setProperty("useBias", true);
			DEFAULT_NEURON_PROP.setProperty("transferFunction", (util.TransferFunctionType.Tanh.ToString()));
		//    DEFAULT_NEURON_PROP.setProperty("transferFunction", Ramp.class);
			DEFAULT_NEURON_PROP.setProperty("inputFunction", typeof(Max));
		}

		/// <summary>
		/// Creates pooling layer with specified kernel, appropriate map
		/// dimensions in regard to previous layer (fromLayer param) and specified
		/// number of feature maps with default neuron settings for pooling layer.
		/// Number of maps in pooling layer must be the same as number of maps in previous
		/// layer.
		/// </summary>
		/// <param name="fromLayer"> previous layer, which will be connected to this layer </param>
		/// <param name="kernel">    kernel for all feature maps </param>
		public PoolingLayer(FeatureMapsLayer fromLayer, Dimension2D kernelDim)
		{
			this.kernel = new Kernel(kernelDim);
			int numberOfMaps = fromLayer.NumberOfMaps;
			Dimension2D fromDimension = fromLayer.MapDimensions;

			int mapWidth = fromDimension.Width / kernel.Width;
			int mapHeight = fromDimension.Height / kernel.Height;
			this.mapDimensions = new Dimension2D(mapWidth, mapHeight);

			createFeatureMaps(numberOfMaps, mapDimensions, kernelDim, DEFAULT_NEURON_PROP);
		}

		/// <summary>
		/// Creates pooling layer with specified kernel, appropriate map
		/// dimensions in regard to previous layer (fromLayer param) and specified
		/// number of feature maps with given neuron properties.
		/// </summary>
		/// <param name="fromLayer">    previous layer, which will be connected to this layer </param>
		/// <param name="kernel">       kernel for all feature maps </param>
		/// <param name="numberOfMaps"> number of feature maps to create in this layer </param>
		/// <param name="neuronProp">   settings for neurons in feature maps </param>
		public PoolingLayer(FeatureMapsLayer fromLayer, Dimension2D kernelDim, int numberOfMaps, NeuronProperties neuronProp)
		{
			this.kernel = kernel;
			Dimension2D fromDimension = fromLayer.MapDimensions;

			int mapWidth = fromDimension.Width / kernel.Width;
			int mapHeight = fromDimension.Height / kernel.Height;
			this.mapDimensions = new Dimension2D(mapWidth, mapHeight);

			createFeatureMaps(numberOfMaps, mapDimensions, kernelDim, neuronProp);
		}

		/// <summary>
		/// Creates connections with shared weights between two feature maps
		/// Assumes that toMap is from Pooling layer.
		/// <p/>
		/// In this implementation, there is no overlapping between kernel positions.
		/// </summary>
		/// <param name="fromMap"> source feature map </param>
		/// <param name="toMap">   destination feature map </param>
		public override void connectMaps(FeatureMapLayer fromMap, FeatureMapLayer toMap)
		{
			int kernelWidth = kernel.Width;
			int kernelHeight = kernel.Height;
			Weight weight = new Weight(1);
			for (int x = 0; x < fromMap.Width - kernelWidth + 1; x += kernelWidth) // < da li step treba da je kernel
			{
				for (int y = 0; y < fromMap.Height - kernelHeight + 1; y += kernelHeight)
				{

					Neuron toNeuron = toMap.getNeuronAt(x / kernelWidth, y / kernelHeight);
					for (int dy = 0; dy < kernelHeight; dy++)
					{
						for (int dx = 0; dx < kernelWidth; dx++)
						{
							int fromX = x + dx;
							int fromY = y + dy;
							Neuron fromNeuron = fromMap.getNeuronAt(fromX, fromY);
							ConnectionFactory.createConnection(fromNeuron, toNeuron, weight);
						}
					}
				}
			}
		}
	}

}