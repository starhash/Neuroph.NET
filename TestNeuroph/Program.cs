using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using org.neuroph.core;
using org.neuroph.nnet;
using org.neuroph.nnet.learning;
using org.neuroph.core.data;
using System.Reflection;
using org.neuroph.core.learning.error;
using System.IO;
using org.neuroph.core.events;
using org.neuroph.nnet.comp.neuron;

namespace TestNeuroph {

    class MLPListener : org.neuroph.core.events.NeuralNetworkEventListener {
        public void handleNeuralNetworkEvent(NeuralNetworkEvent @event) {
            if(@event.EventType == NeuralNetworkEvent.NeuralNetworkEventType.CALCULATED) {
                List<Layer> list = ((NeuralNetwork)@event.getSource()).Layers;
                double error = ((MomentumBackpropagation)((NeuralNetwork)@event.getSource()).LearningRule).TotalNetworkError;
                if (error < 0.002) {
                    string sgtr = "TErr = " + error + ", ";
                    for (int i = 0; i < list.Count; i++) {
                        string neuronweights = list[i].Neurons.Where((x) => !(x is BiasNeuron)).Aggregate("", (x, y) => x + "," + Math.Round(y.Output, 3)).Trim(',', ' ');
                        sgtr += "Layer " + i + " = [" + neuronweights + "], ";
                    }
                    Console.Write("\r" + sgtr.Trim(',', ' '));
                }
            }
        }
    }

    class Program {
        static void Main(string[] args) {
            DataSet ds = DataSet.createFromFile(@"C:\Users\harsh\Documents\Visual Studio 2015\Projects\AISmartReader\AISmartReaderNN\NNProject\testfinal-norm.csv", 4, 1, ",");
            DataSet[] tnt = ds.createTrainingAndTestSubsets(75, 25);
            LoadDataSetAndTrain(tnt[0]);
            LoadAndTest(tnt[1]);
            Console.ReadKey();
        }

        public static void LoadDataSetAndTrain(DataSet tnt) {
            MultiLayerPerceptron mlp = new MultiLayerPerceptron(4, 3, 1);
            MomentumBackpropagation mombp = new MomentumBackpropagation();
            mlp.addListener(new MLPListener());
            mombp.MaxError = 0.004;
            mombp.LearningRate = 0.2;
            mombp.Momentum = 0.7;
            mlp.LearningRule = mombp;
            mlp.learn(tnt);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Saving neural network:");
            mlp.Save(@"C:\Users\harsh\Documents\Visual Studio 2015\Projects\AISmartReader\AISmartReaderNN\NNProject\smartnn.nnet");
        }
        public static void LoadAndTest(DataSet tnt) {
            Console.WriteLine("Loading saved neural network : ");
            MultiLayerPerceptron mlp = (MultiLayerPerceptron)NeuralNetwork.Load(@"C:\Users\harsh\Documents\Visual Studio 2015\Projects\AISmartReader\AISmartReaderNN\NNProject\smartnn.nnet");

            TextWriter tw = new StreamWriter(@"C:\Users\harsh\Documents\Visual Studio 2015\Projects\AISmartReader\AISmartReaderNN\NNProject\testfinal-norm-output.csv");

            MeanSquaredError mse = new MeanSquaredError();
            foreach (DataSetRow r in tnt) {
                mlp.Input = r.Input;
                mlp.calculate();
                mse.calculatePatternError(mlp.Output, r.Input);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.BackgroundColor = ConsoleColor.Black;
                string inputs = r.Input.Aggregate("", (x, y) => x + ", " + Math.Round(y, 3)).Trim(',', ' ');
                string outputs = mlp.Output.Aggregate("", (x, y) => x + ", " + Math.Round(y, 3)).Trim(',', ' ');
                Console.Write("\rInput = " + inputs);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Output = " + outputs);
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Total Error : " + mse.TotalError);
                tw.WriteLine(inputs + ", " + outputs);
            }
            tw.Close();
        }
    }
}
