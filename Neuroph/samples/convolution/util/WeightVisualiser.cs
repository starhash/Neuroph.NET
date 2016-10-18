using System;
using System.Collections.Generic;

namespace org.neuroph.samples.convolution.util
{



	using FeatureMapLayer = org.neuroph.nnet.comp.layer.FeatureMapLayer;
	using Kernel = org.neuroph.nnet.comp.Kernel;
	using Connection = org.neuroph.core.Connection;
	using Neuron = org.neuroph.core.Neuron;
	using BiasNeuron = org.neuroph.nnet.comp.neuron.BiasNeuron;

	public class WeightVisualiser
	{

		private const int RATIO = 20;

		private List<List<double?>> featureDetector;
		private Kernel kernel;

		public WeightVisualiser(FeatureMapLayer map, Kernel kernel)
		{
			this.kernel = kernel;
			this.featureDetector = new List<>();
			initWeights(map);
		}

		private void initWeights(FeatureMapLayer map)
		{
			List<double?> weights = new List<double?>();
			Neuron neuron = map.getNeuronAt(0);
			int counter = 0;
			foreach (Connection conn in neuron.InputConnections)
			{
				if (!(conn.FromNeuron is BiasNeuron))
				{
					if (counter < kernel.Area)
					{
						weights.Add(conn.Weight.Value);
						counter++;
					}
					else
					{
						featureDetector.Add(weights);
						weights = new List<>();
						weights.Add(conn.Weight.Value);
						counter = 1;
					}
				}
			}
			featureDetector.Add(weights);

		}

		public virtual void displayWeights()
		{
			foreach (List<double?> currentKernel in featureDetector)
			{
				displayWeight(currentKernel);
			}
		}

		private void displayWeight(List<double?> currentKernel)
		{

			JFrame frame = new JFrame("Weight Visualiser: ");
			frame.setSize(400, 400);

			JLabel label = new JLabel();
			Dimension d = new Dimension(kernel.Width * RATIO, kernel.Height * RATIO);
			label.Size = d;
			label.PreferredSize = d;

			frame.ContentPane.add(label, BorderLayout.CENTER);
			frame.pack();
			frame.Visible = true;

			BufferedImage image = new BufferedImage(kernel.Width, kernel.Height, BufferedImage.TYPE_BYTE_GRAY);

			int[] rgb = convertWeightToRGB(currentKernel);
			image.setRGB(0, 0, kernel.Width, kernel.Height, rgb, 0, kernel.Width);
			label.Icon = new ImageIcon(image.getScaledInstance(kernel.Width * RATIO, kernel.Height * RATIO, Image.SCALE_SMOOTH));

		}

		private int[] convertWeightToRGB(List<double?> weights)
		{
			normalizeWeights(weights);
			int[] data = new int[kernel.Width * kernel.Height];
			int i = 0;
			foreach (double? weight in weights)
			{
				int val = (int)(weight * 255);
				data[i++] = (new Color(val, val, val)).RGB;
			}
			return data;
		}

		private void normalizeWeights(List<double?> weights)
		{
			double min = double.MaxValue;
			double max = double.Epsilon;
			foreach (double? weight in weights)
			{
				min = Math.Min(min, weight);
				max = Math.Max(max, weight);
			}

			for (int i = 0; i < weights.Count; i++)
			{
				double value = (weights[i] - min) / (max - min);
				weights[i] = value;
			}
		}
	}

}