using System;
using System.Collections.Generic;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
namespace org.neuroph.core.input {


    /// 
    /// <summary>
    /// @author zoran
    /// </summary>
    public class EuclideanRBF : InputFunction {

        public override double getOutput(List<Connection> inputConnections) {
            //	double output = 0d;

            double sqrSum = 0d;
            foreach (Connection connection in inputConnections) {
                Neuron neuron = connection.FromNeuron;
                Weight weight = connection.Weight;
                double diff = neuron.Output - weight.Value;
                sqrSum += diff * diff;
            }

            return 0.5 * Math.Sqrt(sqrSum) / (double)inputConnections.Count; // ovo trebaprebaciti u novu funkciju transfera sa odgovarajucim izvodom
        }

    }

}