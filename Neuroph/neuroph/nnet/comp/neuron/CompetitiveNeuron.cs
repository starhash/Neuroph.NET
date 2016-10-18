using System.Collections.Generic;

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

namespace org.neuroph.nnet.comp.neuron
{


	using Connection = org.neuroph.core.Connection;
	using InputFunction = org.neuroph.core.input.InputFunction;
	using TransferFunction = org.neuroph.core.transfer.TransferFunction;

	/// <summary>
	/// Provides neuron behaviour specific for competitive neurons which are used in
	/// competitive layers, and networks with competitive learning.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class CompetitiveNeuron : DelayedNeuron
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization
		/// compatibility with a previous version of the class.
		/// </summary>
		private const long serialVersionUID = 1L;

		/// <summary>
		/// Flag indicates if this neuron is in competing mode
		/// </summary>
		private bool isCompeting = false;

		/// <summary>
		/// Collection of conections from neurons in other layers
		/// </summary>
		private List<Connection> connectionsFromOtherLayers;

		/// <summary>
		/// Collection of connections from neurons in the same layer as this neuron
		/// (lateral connections used for competition)
		/// </summary>
		private List<Connection> connectionsFromThisLayer;

		/// <summary>
		/// Creates an instance of CompetitiveNeuron with specified input and transfer functions </summary>
		/// <param name="inputFunction"> neuron input function </param>
		/// <param name="transferFunction"> neuron ransfer function </param>
		public CompetitiveNeuron(InputFunction inputFunction, TransferFunction transferFunction) : base(inputFunction, transferFunction)
		{
			connectionsFromOtherLayers = new List<Connection>();
			connectionsFromThisLayer = new List<Connection>();
			addInputConnection(this, 1);
		}

		public override void calculate()
		{
			if (this.isCompeting)
			{
				// get input only from neurons in this layer
				this.totalInput = this.inputFunction.getOutput(this.connectionsFromThisLayer);
			}
			else
			{
				// get input from other layers
				this.totalInput = this.inputFunction.getOutput(this.connectionsFromOtherLayers);
				this.isCompeting = true;
			}

			this.output = this.transferFunction.getOutput(this.totalInput);
			outputHistory.Insert(0, this.output);
		}

		/// <summary>
		/// Adds input connection for this competitive neuron </summary>
		/// <param name="connection"> input connection </param>
		public override void addInputConnection(Connection connection)
		{
			base.addInputConnection(connection);
			if (connection.FromNeuron.ParentLayer == this.ParentLayer)
			{
	//                    this.connectionsFromThisLayer =  Arrays.copyOf(connectionsFromThisLayer, connectionsFromThisLayer.length+1);     // grow existing connections  array to make space for new connection
	//                    this.connectionsFromThisLayer[connectionsFromThisLayer.length - 1] = connection;                    
				connectionsFromThisLayer.Add(connection);
			}
			else
			{
	//                    this.connectionsFromOtherLayers =  Arrays.copyOf(connectionsFromOtherLayers, connectionsFromOtherLayers.length+1);     // grow existing connections  array to make space for new connection
	//                    this.connectionsFromOtherLayers[connectionsFromOtherLayers.length - 1] = connection;                          
				connectionsFromOtherLayers.Add(connection);
			}
		}

		/// <summary>
		/// Returns collection of connections from other layers </summary>
		/// <returns> collection of connections from other layers </returns>
		public virtual List<Connection> ConnectionsFromOtherLayers
		{
			get
			{
				return connectionsFromOtherLayers;
			}
		}

		/// <summary>
		/// Resets the input, output and mode for this neuron
		/// </summary>
		public override void reset()
		{
			base.reset();
			this.isCompeting = false;
		}

		/// <summary>
		/// Retruns true if this neuron is in competing mode, false otherwise </summary>
		/// <returns> true if this neuron is in competing mode, false otherwise </returns>
		public virtual bool Competing
		{
			get
			{
				return isCompeting;
			}
		}

		/// <summary>
		/// Sets the flag to indicate that this neuron is in competing mode </summary>
		/// <param name="isCompeting"> value for the isCompeting flag </param>
		public virtual bool IsCompeting
		{
			set
			{
				this.isCompeting = value;
			}
		}

	}

}