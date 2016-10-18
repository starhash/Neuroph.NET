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

namespace org.neuroph.nnet.comp.layer {

    using Layer = org.neuroph.core.Layer;
    using Neuron = org.neuroph.core.Neuron;
    using NeuronFactory = org.neuroph.util.NeuronFactory;
    using NeuronProperties = org.neuroph.util.NeuronProperties;

    /// <summary>
    /// FeatureMapLayer Layer provides 2D layout of the neurons in layer. All the neurons
    /// are actually stored in one dimensional array in superclass.
    /// This type of layer is used as feature map for convolutional networks
    /// 
    /// @author Boris Fulurija
    /// @author Zoran Sevarac
    /// </summary>
    public class FeatureMapLayer : Layer //implements Callable<Void>
    {

        private const long serialVersionUID = 2498669699995172395L;

        /// <summary>
        /// Dimensions of this layer (width and height)
        /// </summary>
        private Dimension2D dimensions;

        /// <summary>
        /// Kernel of this feature map
        /// </summary>
        private Kernel kernel;



        /// <summary>
        /// Creates an empty 2D layer with specified dimensions
        /// </summary>
        /// <param name="dimensions"> layer dimensions (width and weight) </param>
        public FeatureMapLayer(Dimension2D dimensions, NeuronProperties neuronProperties) {
            this.dimensions = dimensions;

            for (int i = 0; i < dimensions.Height * dimensions.Width; i++) {
                Neuron neuron = NeuronFactory.createNeuron(neuronProperties);
                addNeuron(neuron);
            }
        }


        /// <summary>
        /// Creates an empty 2D layer with specified dimensions and kernel
        /// </summary>
        /// <param name="dimensions"> layer dimensions (width and weight) </param>
        public FeatureMapLayer(Dimension2D dimensions, Dimension2D kernelDimension) {
            this.dimensions = dimensions;
            this.kernel = new Kernel(kernelDimension);
        }

        /// <summary>
        /// Creates 2D layer with specified dimensions, filled with neurons with
        /// specified properties
        /// </summary>
        /// <param name="dimensions">       layer dimensions </param>
        /// <param name="neuronProperties"> neuron properties </param>
        public FeatureMapLayer(Dimension2D dimensions, NeuronProperties neuronProperties, Dimension2D kernelDimension) : this(dimensions, kernelDimension) {

            for (int i = 0; i < dimensions.Height * dimensions.Width; i++) {
                Neuron neuron = NeuronFactory.createNeuron(neuronProperties);
                addNeuron(neuron);
            }
        }

        /// <summary>
        /// Returns width of this layer
        /// </summary>
        /// <returns> width of this layer </returns>
        public virtual int Width {
            get {
                return dimensions.Width;
            }
        }

        /// <summary>
        /// Returns height of this layer
        /// </summary>
        /// <returns> height of this layer </returns>
        public virtual int Height {
            get {
                return dimensions.Height;
            }
        }


        /// <summary>
        /// Returns dimensions of this layer
        /// </summary>
        /// <returns> dimensions of this layer </returns>
        public virtual Dimension2D Dimensions {
            get {
                return dimensions;
            }
        }


        /// <summary>
        /// Returns neuron at specified position in this layer
        /// </summary>
        /// <param name="x"> neuron's x position </param>
        /// <param name="y"> neuron's y position </param>
        /// <returns> neuron at specified position in this layer </returns>
        public virtual Neuron getNeuronAt(int x, int y) {
            return getNeuronAt(x + y * (dimensions.Width));
        }

        public virtual Kernel Kernel {
            get {
                return kernel;
            }
        }


        //    @Override
        //    public Void call() throws Exception {
        //        calculate();
        //        return null;
        //    }





    }

}