using System;

/// <summary>
/// Copyright 2010 Neuroph Project http://neuroph.sourceforge.net
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

namespace org.neuroph.util
{

    using Neuron = org.neuroph.core.Neuron;
    using NeurophException = org.neuroph.core.exceptions.NeurophException;
    using InputFunction = org.neuroph.core.input.InputFunction;
    using TransferFunction = org.neuroph.core.transfer.TransferFunction;
    using InputOutputNeuron = org.neuroph.nnet.comp.neuron.InputOutputNeuron;
    using ThresholdNeuron = org.neuroph.nnet.comp.neuron.ThresholdNeuron;
    using java.lang.reflect;

    /// <summary>
    /// Provides methods to create customized instances of Neuron.
    /// @author Zoran Sevarac <sevarac@gmail.com>
    /// </summary>
    public class NeuronFactory
	{

			 /// <summary>
			 /// Private System.Reflection.ConstructorInfo prevents creating an instances of this class.
			 /// </summary>
			private NeuronFactory()
			{
			}

		/// <summary>
		/// Creates and returns neuron instance according to the given specification in neuronProperties.
		/// </summary>
		/// <param name="neuronProperties">
		///            specification of neuron properties </param>
		/// <returns> returns instance of neuron with specified properties </returns>
		public static Neuron createNeuron(NeuronProperties neuronProperties)
		{

					InputFunction inputFunction = null;
					System.Type inputFunctionClass = neuronProperties.InputFunction;

					if (inputFunctionClass != null)
					{
						inputFunction = createInputFunction(inputFunctionClass);
					}

			TransferFunction transferFunction = createTransferFunction(neuronProperties.TransferFunctionProperties);

			Neuron neuron = null;
            System.Type neuronClass = neuronProperties.NeuronType;

					// use two param System.Reflection.ConstructorInfo to create neuron
						try
						{
                System.Type[] paramTypes = new System.Type[] {typeof(InputFunction), typeof(TransferFunction)};
                System.Reflection.ConstructorInfo con = neuronClass.GetConstructor(paramTypes);

							object[] paramList = new object[2];
							paramList[0] = inputFunction;
							paramList[1] = transferFunction;

							neuron = (Neuron) con.Invoke(paramList);

						}
						catch (java.lang.NoSuchMethodException)
						{
						//    throw new NeurophException("NoSuchMethod while creating Neuron!", e);
						}
						catch (java.lang.InstantiationException e)
						{
							 throw new NeurophException("InstantiationException while creating Neuron!", e);
						}
						catch (java.lang.IllegalAccessException e)
						{
							throw new NeurophException("IllegalAccessException while creating Neuron!", e);
						}
						catch (InvocationTargetException e)
						{
							throw new NeurophException("InvocationTargetException while creating Neuron!", e);
						}

						if (neuron == null)
						{
							// use System.Reflection.ConstructorInfo without params to create neuron
							try
							{
                    System.Reflection.ConstructorInfo con = neuronClass.GetConstructor(new System.Type[] { });
                                neuron = (Neuron) con.Invoke(new object[] { });
							}
							catch (java.lang.IllegalAccessException e)
							{
								Console.Error.WriteLine("InstantiationException while creating Neuron!");
								System.Console.WriteLine(e.ToString());
								Console.Write(e.StackTrace);
							}
							catch (java.lang.InstantiationException e)
							{
								Console.Error.WriteLine("InstantiationException while creating Neuron!");
								System.Console.WriteLine(e.ToString());
								Console.Write(e.StackTrace);
							}
						}

						if (neuronProperties.hasProperty("thresh"))
						{
							((ThresholdNeuron)neuron).Thresh = (double)neuronProperties.getProperty("thresh");
						}
						else if (neuronProperties.hasProperty("bias"))
						{
							((InputOutputNeuron)neuron).Bias = (double)neuronProperties.getProperty("bias");
						}

						 return neuron;

		}


			private static InputFunction createInputFunction(System.Type inputFunctionClass)
			{
				InputFunction inputFunction = null;

				try
				{
					inputFunction = (InputFunction) inputFunctionClass.GetConstructor(new System.Type[] { }).Invoke(new object[] { });
				}
				catch (java.lang.InstantiationException e)
				{
					throw new NeurophException("InstantiationException while creating InputFunction!", e);
				}
				catch (java.lang.IllegalAccessException e)
				{
					throw new NeurophException("IllegalAccessException while creating InputFunction!", e);
				}

				return inputFunction;
			}


		/// <summary>
		/// Creates and returns instance of transfer function
		/// </summary>
		/// <param name="tfProperties">
		///            transfer function properties </param>
		/// <returns> returns transfer function </returns>
		private static TransferFunction createTransferFunction(Properties tfProperties)
		{
			TransferFunction transferFunction = null;

			System.Type tfClass = (System.Type)tfProperties.getProperty("transferFunction");

						try
						{
							System.Reflection.ParameterInfo[] paramTypes = null;

							System.Reflection.ConstructorInfo[] cons = tfClass.GetConstructors();
							for (int i = 0; i < cons.Length; i++)
							{
								 paramTypes = cons[i].GetParameters();

								// use System.Reflection.ConstructorInfo with one parameter of Properties System.Type
								if ((paramTypes.Length == 1) && (paramTypes[0].ParameterType == typeof(Properties)))
								{
									System.Type[] argTypes = new System.Type[1];
									argTypes[0] = typeof(Properties);
									System.Reflection.ConstructorInfo ct = tfClass.GetConstructor(argTypes);

									object[] argList = new object[1];
									argList[0] = tfProperties;
									transferFunction = (TransferFunction) ct.Invoke(argList);
									break;
								} // use System.Reflection.ConstructorInfo without params
								else if (paramTypes.Length == 0)
								{
									transferFunction = (TransferFunction) tfClass.GetConstructor(new System.Type[] { }).Invoke(new object[] { });
									break;
								}
							}

							return transferFunction;

						}
						catch (java.lang.NoSuchMethodException e)
						{
							Console.Error.WriteLine("getConstructor() couldn't find the System.Reflection.ConstructorInfo while creating TransferFunction!");
							System.Console.WriteLine(e.ToString());
							Console.Write(e.StackTrace);
						}
						catch (java.lang.InstantiationException e)
						{
							Console.Error.WriteLine("InstantiationException while creating TransferFunction!");
							System.Console.WriteLine(e.ToString());
							Console.Write(e.StackTrace);
						}
						catch (java.lang.IllegalAccessException e)
						{
							Console.Error.WriteLine("No permission to invoke method while creating TransferFunction!");
							System.Console.WriteLine(e.ToString());
							Console.Write(e.StackTrace);
						}
						catch (InvocationTargetException e)
						{
							Console.Error.WriteLine("Method threw an: " + e.getTargetException() + " while creating TransferFunction!");
							System.Console.WriteLine(e.ToString());
							Console.Write(e.StackTrace);
						}

						return transferFunction;
		}

	}
}