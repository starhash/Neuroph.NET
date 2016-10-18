using System;

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
namespace org.neuroph.nnet.comp {

    using Weight = org.neuroph.core.Weight;

    /// <summary>
    /// Kernel used in convolution networks. Kernel is (width x height) window
    /// sliding over 2D input space. Each window position provides (width x height)
    /// inputs for the neurons in next layer.
    /// 
    /// @author Boris Fulurija
    /// @author Zoran Sevarac
    /// </summary>
    [Serializable]
    public class Kernel {

        private const long serialVersionUID = -3948374914759253222L;

        /// <summary>
        /// Kernel width
        /// </summary>
        private int width;

        /// <summary>
        /// Kernel height
        /// </summary>
        private int height;

        private Weight[,] weights;


        /// <summary>
        /// Creates new kernel with specified width and height
        /// </summary>
        /// <param name="width"> kernel width </param>
        /// <param name="height"> kernel height </param>
        public Kernel(Dimension2D dimension) {
            this.width = dimension.Width;
            this.height = dimension.Height;
        }

        /// <summary>
        /// Creates new kernel with specified width and height
        /// </summary>
        /// <param name="width"> kernel width </param>
        /// <param name="height"> kernel height </param>
        public Kernel(int width, int height) {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Returns width of this kernel
        /// </summary>
        /// <returns> width of this kernel </returns>
        public virtual int Width {
            get {
                return width;
            }
            set {
                this.width = value;
            }
        }


        /// <summary>
        /// Returns height of this kernel
        /// </summary>
        /// <returns> height of this kernel </returns>
        public virtual int Height {
            get {
                return height;
            }
            set {
                this.height = value;
            }
        }


        /// <summary>
        /// Returns area of this kernel (width*height)
        /// </summary>
        /// <returns> area of this kernel </returns>
        public virtual int Area {
            get {
                return width * height;
            }
        }

        public virtual Weight[,] Weights {
            get {
                return weights;
            }
            set {
                this.weights = value;
            }
        }


        public virtual void initWeights(double min, double max) {
            //JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
            //ORIGINAL LINE: weights = new Weight[height][width];
            weights = new Weight[height, width];

            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    Weight weight = new Weight();
                    weight.randomize(min, max);
                    weights[i, j] = weight;
                }
            }
        }

    }

}