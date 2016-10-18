using System;
using System.Collections.Generic;

namespace org.neuroph.samples.convolution.util
{

	using Neuron = org.neuroph.core.Neuron;
	using FeatureMapsLayer = org.neuroph.nnet.comp.layer.FeatureMapsLayer;
	using FeatureMapLayer = org.neuroph.nnet.comp.layer.FeatureMapLayer;

	using Dimension2D = org.neuroph.nnet.comp.Dimension2D;

	public class LayerVisialize
	{
		private const int RATIO = 20;

		private List<List<double?>> layerMaps;
		private FeatureMapsLayer featureMapsLayer;
		private Dimension2D mapDimensions;

		public LayerVisialize(FeatureMapsLayer featureMapsLayer)
		{

			this.layerMaps = new List<>();
			this.featureMapsLayer = featureMapsLayer;
			this.mapDimensions = featureMapsLayer.MapDimensions;
			initWeights();
		}

		private void initWeights()
		{

			foreach (FeatureMapLayer featureMap in featureMapsLayer.FeatureMaps)
			{
				List<double?> map = new List<double?>();
				foreach (Neuron neuron in featureMap.Neurons)
				{
					map.Add(neuron.Output);
				}
				layerMaps.Add(map);
			}
		}

		public virtual void displayWeights()
		{
			foreach (List<double?> currentKernel in layerMaps)
			{
				displayWeight(currentKernel);
			}
		}

		private void displayWeight(List<double?> currentKernel)
		{

			JFrame frame = new JFrame("Weight Visualiser: ");
			frame.setSize(400, 400);

			JLabel label = new JLabel();
			Dimension d = new Dimension(mapDimensions.Width * RATIO, mapDimensions.Height * RATIO);
			label.Size = d;
			label.PreferredSize = d;

			frame.ContentPane.add(label, BorderLayout.CENTER);
			frame.pack();
			frame.Visible = true;

			BufferedImage image = new BufferedImage(mapDimensions.Width, mapDimensions.Height, BufferedImage.TYPE_BYTE_GRAY);

			int[] rgb = convertWeightToRGB(currentKernel);
			image.setRGB(0, 0, mapDimensions.Width, mapDimensions.Height, rgb, 0, mapDimensions.Width);
			label.Icon = new ImageIcon(image.getScaledInstance(mapDimensions.Width * RATIO, mapDimensions.Height * RATIO, Image.SCALE_SMOOTH));

		}

		private int[] convertWeightToRGB(List<double?> pixels)
		{
			normalizeWeights(pixels);
			int[] data = new int[mapDimensions.Width * mapDimensions.Height];
			int i = 0;
			foreach (double? weight in pixels)
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