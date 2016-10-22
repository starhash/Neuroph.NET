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

namespace org.neuroph.nnet.comp.layer
{

	using Linear = org.neuroph.core.transfer.Linear;
	using InputNeuron = org.neuroph.nnet.comp.neuron.InputNeuron;
	using NeuronProperties = org.neuroph.util.NeuronProperties;

	/// <summary>
	/// Input layer for convolutional networks
	/// @author Boris Fulurija
	/// @author Zoran Sevarac
	/// </summary>
	public class InputMapsLayer : FeatureMapsLayer
	{

		private const long serialVersionUID = -4982081431101626706L;

		/// <summary>
		/// Default neuron properties for InputMapsLayer is InputNeuron with Linear transfer function 
		/// </summary>
		public static readonly NeuronProperties DEFAULT_NEURON_PROP = new NeuronProperties();

		static InputMapsLayer()
		{
			DEFAULT_NEURON_PROP.setProperty("neuronType", typeof(InputNeuron));
			DEFAULT_NEURON_PROP.setProperty("transferFunction", util.TransferFunctionType.Linear.ToString());
		}


		/// <summary>
		/// Create InputMapsLayer with specified number of maps with specified dimensions </summary>
		/// <param name="mapDimension"> dimensions of a single feature map </param>
		/// <param name="mapCount">  number of feature maps </param>
		public InputMapsLayer(Dimension2D mapDimensions, int mapCount) : base(mapDimensions, mapCount, InputMapsLayer.DEFAULT_NEURON_PROP)
		{
		}

		public override void connectMaps(FeatureMapLayer fromMap, FeatureMapLayer toMap)
		{
		   // does nothing
		}


	}
}