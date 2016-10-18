using System.Collections.Generic;

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


	using Layer = org.neuroph.core.Layer;
	using Neuron = org.neuroph.core.Neuron;
	using NeuronProperties = org.neuroph.util.NeuronProperties;

	/// <summary>
	/// This class represents an array of feature maps which are 2 dimensional layers
	/// (Layer2D instances) and it is base class for Convolution and Pooling layers,
	/// which are used in ConvolutionalNetwork
	/// 
	/// @author Boris Fulurija
	/// @author Zoran Sevarac </summary>
	/// <seealso cref= ConvolutionalLayer </seealso>
	/// <seealso cref= PoolingLayer </seealso>
	/// <seealso cref= org.neuroph.nnet.ConvolutionalNetwork </seealso>
	public abstract class FeatureMapsLayer : Layer
	{

	   // static final ForkJoinPool mainPool = new ForkJoinPool(Runtime.getRuntime().availableProcessors());


		private const long serialVersionUID = -6706741997689639209L;

		/// <summary>
		/// Kernel used for all 2D layers (feature maps)
		/// </summary>
	 //   protected Kernel kernel;

		/// <summary>
		/// Dimensions for all 2D layers (feature maps)
		/// </summary>
		protected internal Dimension2D mapDimensions;

		public virtual List<FeatureMapLayer> FeatureMaps
		{
			get
			{
				return featureMaps;
			}
		}

		/// <summary>
		/// Collection of feature maps
		/// </summary>
		private List<FeatureMapLayer> featureMaps;

		/// <summary>
		/// Creates a new empty feature maps layer with specified kernel
		/// </summary>
		/// <param name="kernel"> kernel to use for all feature maps </param>
		public FeatureMapsLayer() //Kernel kernel
		{
	   //     this.kernel = kernel;
			this.featureMaps = new List<FeatureMapLayer>();
		}

		/// <summary>
		/// Creates a new empty feature maps layer with specified kernel and
		/// feature map dimensions.
		/// </summary>
		/// <param name="kernel">        kernel used for all feature maps in this layer </param>
		/// <param name="mapDimensions"> mapDimensions of feature maps in this layer </param>
		public FeatureMapsLayer(Dimension2D mapDimensions) //Kernel kernel,
		{
	   //     this.kernel = kernel;
			this.mapDimensions = mapDimensions;
			this.featureMaps = new List<FeatureMapLayer>();
		}

		/// <summary>
		/// Creates new feature maps layer with specified kernel and feature maps.
		/// Also creates feature maps and neurons in feature maps;
		/// </summary>
		/// <param name="kernel">        kernel used for all feature maps in this layer </param>
		/// <param name="mapDimensions"> mapDimensions of feature maps in this layer </param>
		/// <param name="mapCount">      number of feature maps </param>
		/// <param name="neuronProp">    properties for neurons in feature maps </param>
		public FeatureMapsLayer(Dimension2D kernelDimension, Dimension2D mapDimensions, int mapCount, NeuronProperties neuronProp)
		{
		   // this.kernel = kernel;
			this.mapDimensions = mapDimensions;
			this.featureMaps = new List<FeatureMapLayer>();
			createFeatureMaps(mapCount, mapDimensions, kernelDimension, neuronProp);
		}



		public FeatureMapsLayer(Dimension2D mapDimensions, int mapCount, NeuronProperties neuronProp)
		{
			this.mapDimensions = mapDimensions;
			this.featureMaps = new List<FeatureMapLayer>();
			createFeatureMaps(mapCount, mapDimensions, neuronProp);
		}


		/// <summary>
		/// Adds a feature map (2d layer) to this feature map layer </summary>
		/// <param name="featureMap"> feature map to add </param>
		public virtual void addFeatureMap(FeatureMapLayer featureMap)
		{
			if (featureMap == null)
			{
				throw new System.ArgumentException("FeatureMap cant be null!");
			}

			featureMaps.Add(featureMap);
            neurons.AddRange((featureMap.Neurons));

		}

		/// <summary>
		/// Creates and adds specified number of feature maps to this layer
		/// </summary>
		/// <param name="mapCount">         number of feature maps to create </param>
		/// <param name="dimensions">       feature map dimensions </param>
		/// <param name="neuronProperties"> properties of neurons in feature maps </param>
		protected internal void createFeatureMaps(int mapCount, Dimension2D mapDimensions, Dimension2D kernelDimension, NeuronProperties neuronProperties)
		{
			for (int i = 0; i < mapCount; i++)
			{
				addFeatureMap(new FeatureMapLayer(mapDimensions, neuronProperties, kernelDimension));
			}
		}


		private void createFeatureMaps(int mapCount, Dimension2D mapDimensions, NeuronProperties neuronProperties)
		{
			for (int i = 0; i < mapCount; i++)
			{
				addFeatureMap(new FeatureMapLayer(mapDimensions, neuronProperties));
			}
		}


		/// <summary>
		/// Returns feature map (Layer2D) at specified index
		/// </summary>
		/// <param name="index"> index of feature map </param>
		/// <returns> feature map (Layer2D instance) at specified index </returns>
		public virtual FeatureMapLayer getFeatureMap(int index)
		{
			return featureMaps[index];
		}

		/// <summary>
		/// Returns number of feature maps in this layer
		/// </summary>
		/// <returns> number of feature maps in this layer </returns>
		public virtual int NumberOfMaps
		{
			get
			{
				return featureMaps.Count;
			}
		}

		/// <summary>
		/// Returns neuron instance at specified (x, y) position at specified feature map layer
		/// </summary>
		/// <param name="x">        neuron's x position </param>
		/// <param name="y">        neuron's y position </param>
		/// <param name="mapIndex"> feature map index </param>
		/// <returns> neuron at specified (x, y, map) position </returns>
		public virtual Neuron getNeuronAt(int x, int y, int mapIndex)
		{
			FeatureMapLayer map = featureMaps[mapIndex];
			return map.getNeuronAt(x, y);
		}

		/// <summary>
		/// Returns total number of neurons in all feature maps
		/// </summary>
		/// <returns> total number of neurons in all feature maps </returns>
		public override int NeuronsCount
		{
			get
			{
				int neuronCount = 0;
				foreach (FeatureMapLayer map in featureMaps)
				{
					neuronCount += map.NeuronsCount;
				}
				return neuronCount;
			}
		}

		/// <summary>
		/// Calculates this layer (all feature maps)
		/// </summary>
	//    @Override
	//    public void calculate() {
	//        mainPool.invokeAll(featureMaps);   // << Obican calcualte n efork join???? kako radi sinhronizacija?
	//    }

		/// <summary>
		/// Returns kernel used by all feature maps in this layer
		/// </summary>
		/// <returns> kernel used by all feature maps in this layer </returns>
	//    public Kernel getKernel() {
	//        return kernel;
	//    }

		/// <summary>
		/// Returns dimensions of feature maps in this layer
		/// </summary>
		/// <returns> dimensions of feature maps in this layer </returns>
		public virtual Dimension2D MapDimensions
		{
			get
			{
				return mapDimensions;
			}
		}


		/// <summary>
		/// Creates connections between two feature maps. It does nothing here,
		/// connectivity patterns are defined by subclasses...
		/// Maybe it should be even removed from here or made abstract......
		/// </summary>
		/// <param name="fromMap"> </param>
		/// <param name="toMap"> </param>
		public abstract void connectMaps(FeatureMapLayer fromMap, FeatureMapLayer toMap);


	}

}