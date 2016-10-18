using System;
using System.Collections;

/// <summary>
/// Copyright 2010 Neuroph Project http://neuroph.sourceforge.net
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
namespace org.neuroph.util
{

    using Neuron = org.neuroph.core.Neuron;
    using InputFunction = org.neuroph.core.input.InputFunction;
    using WeightedSum = org.neuroph.core.input.WeightedSum;
    using Linear = org.neuroph.core.transfer.Linear;
    using TransferFunction = org.neuroph.core.transfer.TransferFunction;
    using core.transfer;

    /// <summary>
    /// Represents properties of a neuron.
    /// 
    /// @author Zoran Sevarac <sevarac@gmail.com>
    /// </summary>
    public class NeuronProperties : Properties
	{

		private const long serialVersionUID = 2L;

	   // public static final DEFAULT = new NeuronProperties();


		public NeuronProperties()
		{
			initKeys();
			this.setProperty("inputFunction", typeof(WeightedSum));
			this.setProperty("transferFunction", typeof(Linear));
			this.setProperty("neuronType", typeof(Neuron));
		}

		public NeuronProperties(Type neuronClass)
		{
			initKeys();
			this.setProperty("inputFunction", typeof(WeightedSum));
			this.setProperty("transferFunction", typeof(Linear));
			this.setProperty("neuronType", neuronClass);
		}

		public NeuronProperties(Type neuronClass, Type transferFunctionClass)
		{
			initKeys();
			this.setProperty("inputFunction", typeof(WeightedSum));
			this.setProperty("transferFunction", transferFunctionClass);
			this.setProperty("neuronType", neuronClass);
		}

		public NeuronProperties(Type neuronClass, Type inputFunctionClass, Type transferFunctionClass)
		{
			initKeys();
			this.setProperty("inputFunction", inputFunctionClass);
			this.setProperty("transferFunction", transferFunctionClass);
			this.setProperty("neuronType", neuronClass);
		}

		public NeuronProperties(Type neuronClass, TransferFunctionType transferFunctionType)
		{
			initKeys();
			this.setProperty("inputFunction", typeof(WeightedSum));
			this.setProperty("transferFunction", transferFunctionType);
			this.setProperty("neuronType", neuronClass);
		}

		public NeuronProperties(TransferFunctionType transferFunctionType, bool useBias)
		{
			initKeys();
	//		this.setProperty("weightsFunction", WeightedInput.class);
	//		this.setProperty("summingFunction", Sum.class);
			this.setProperty("inputFunction", typeof(WeightedSum));
			this.setProperty("transferFunction", transferFunctionType);
			this.setProperty("useBias", useBias);
			this.setProperty("neuronType", typeof(Neuron));
		}

		// uraditi override za setProperty tako da za enum type uzima odgovarajuce klase
		private void initKeys()
		{
			createKeys("inputFunction", "transferFunction", "neuronType", "useBias"); // use bias prebaciti u NeuralNetworkProperties
		}

		public virtual Type InputFunction
		{
			get
			{
				object val = this.getProperty("inputFunction");
				if (!val.Equals(""))
				{
					return (Type) val;
				}
				return null;
			}
		}

		public virtual Type TransferFunction
		{
			get
			{
                TransferFunctionType type = (TransferFunctionType) this.getProperty("transferFunction");
                switch (type) {
                    case TransferFunctionType.LINEAR:
                        return typeof(Linear);
                    case TransferFunctionType.RAMP:
                        return typeof(Ramp);
                    case TransferFunctionType.STEP:
                        return typeof(Step);
                    case TransferFunctionType.SIGMOID:
                        return typeof(Sigmoid);
                    case TransferFunctionType.TANH:
                        return typeof(Tanh);
                    case TransferFunctionType.GAUSSIAN:
                        return typeof(Gaussian);
                    case TransferFunctionType.TRAPEZOID:
                        return typeof(Trapezoid);
                    case TransferFunctionType.SGN:
                        return typeof(Sgn);
                    case TransferFunctionType.SIN:
                        return typeof(Sin);
                    case TransferFunctionType.LOG:
                        return typeof(Log);
                    default:
                        throw new ArgumentException();
                }
            }
		}

		public virtual Type NeuronType
		{
			get
			{
				return (Type) this.getProperty("neuronType");
			}
		}

		public virtual Properties TransferFunctionProperties
		{
			get
			{
				Properties tfProperties = new Properties();
				//Enumeration<?> en = this.keys(); 
				IEnumerator iterator = this.Keys.GetEnumerator();
				while (iterator.MoveNext())
				{
					string name = iterator.Current.ToString();
					if (name.Contains("transferFunction"))
					{
						tfProperties.setProperty(name, this.getProperty(name));
					}
				}
				return tfProperties;
			}
		}

		public override sealed void setProperty(string key, object value)
		{
	//                if (!this.containsKey(key))
	//                    throw new RuntimeException("Unknown property key: "+key);

			//if (value is TransferFunctionType)
			//{
			//	value = ((TransferFunctionType) value).TypeClass;
			//}
			//      if (value instanceof InputFunctionType) value = ((InputFunctionType)value).getTypeClass();

			this.setProperty(key, value);
		}
	}
}