using System;

namespace org.neuroph.samples.mnist.master
{

	using ClassificationResult = org.neuroph.contrib.eval.classification.ClassificationResult;
	using org.neuroph.core;
	using DataSet = org.neuroph.core.data.DataSet;
	using MNISTDataSet = org.neuroph.samples.convolution.mnist.MNISTDataSet;

	using Utils = org.neuroph.contrib.eval.classification.Utils;

	/// <summary>
	/// Simple application which demonstrated the usage of CNN for digit recognition
	/// </summary>
	public class FuNet1 : JFrame, Runnable
	{

		private BufferedImage canvas;
		private NeuralNetwork network;


		internal DataSet testSet;
		private JLabel label;

		public FuNet1()
		{

			try
			{
				foreach (UIManager.LookAndFeelInfo info in UIManager.InstalledLookAndFeels)
				{
					if ("Nimbus".Equals(info.Name))
					{
						UIManager.LookAndFeel = info.ClassName;
						break;
					}
				}
			}
			catch (Exception)
			{
				// If Nimbus is not available, you can set the GUI to another look and feel.
			}
			try
			{
				network = network.load(new FileInputStream("/home/mithquissir/Desktop/cnn/5-50-100/30.nnet"));
				testSet = MNISTDataSet.createFromFile(MNISTDataSet.TEST_LABEL_NAME, MNISTDataSet.TEST_IMAGE_NAME, 10000);

			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
			}

			initComponents();
		}

		//this was moved from the overriden paintComponent()
		// instead it update the canvas BufferedImage and calls repaint()
		public virtual void updateCanvas()
		{
			Graphics2D g2 = canvas.createGraphics();
			g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);

			g2.Paint = Color;

			if (tool == 1)
			{

				g2.fillOval(currentX - ((int) value / 2), currentY - ((int) value / 2), (int) value, (int) value);


			}
			else if (tool == 2)
			{
				g2.Stroke = new BasicStroke((float) value, BasicStroke.CAP_ROUND, BasicStroke.JOIN_ROUND);
				g2.drawLine(oldX, oldY, currentX, currentY);
				g2.Stroke = new BasicStroke(1.0f);
			}
			repaint();


		}

		// used in both the updateCanvas and 'clear' method.
		private Color Color
		{
			get
			{
				Color c = null;
				if (color == 1)
				{
					c = Color.black;
				}
				else if (color == 2)
				{
					c = new Color(240, 240, 240);
				}
				else if (color == 3)
				{
					c = Color.white;
				}
				else if (color == 4)
				{
					c = Color.red;
				}
				else if (color == 5)
				{
					c = Color.green;
				}
				else if (color == 6)
				{
					c = Color.blue;
				}
    
				return c;
			}
		}

		// <editor-fold defaultstate="collapsed" desc="Generated Code">
		private void initComponents()
		{

			canvas = new BufferedImage(320, 320, BufferedImage.TYPE_BYTE_GRAY);

			buttonGroup1 = new ButtonGroup();
			buttonGroup2 = new ButtonGroup();
			jPanel4 = new JPanel();
			jSlider2 = new JSlider();
			jLabel1 = new JLabel();
			jPanel2 = new JPanel(new GridBagLayout());
			JLabel canvasLabel = new JLabel(new ImageIcon(canvas));
			jPanel2.add(canvasLabel, null);

			jPanel3 = new JPanel();
			jRadioButton3 = new JRadioButton();
			jRadioButton4 = new JRadioButton();
			jRadioButton5 = new JRadioButton();
			jRadioButton6 = new JRadioButton();
			jRadioButton7 = new JRadioButton();
			jRadioButton8 = new JRadioButton();
			jButton1 = new JButton();

			DefaultCloseOperation = WindowConstants.EXIT_ON_CLOSE;
			Title = "FuNet1 --- powered by Neuroph";

			jPanel4.Border = BorderFactory.createTitledBorder("Line thickness");


			jSlider2.MajorTickSpacing = 10;
			jSlider2.Maximum = 51;
			jSlider2.Minimum = 1;
			jSlider2.MinorTickSpacing = 5;
			jSlider2.PaintTicks = true;
			jSlider2.addChangeListener(new ChangeListenerAnonymousInnerClassHelper(this));

	//        jLabel1.setText("Stroke Size (Radius)");

			GroupLayout jPanel4Layout = new GroupLayout(jPanel4);
			jPanel4.Layout = jPanel4Layout;
			jPanel4Layout.HorizontalGroup = jPanel4Layout.createParallelGroup(GroupLayout.Alignment.LEADING).addGroup(jPanel4Layout.createSequentialGroup().addContainerGap().addGroup(jPanel4Layout.createParallelGroup(GroupLayout.Alignment.LEADING)).addPreferredGap(LayoutStyle.ComponentPlacement.RELATED, 51, short.MaxValue).addGroup(jPanel4Layout.createParallelGroup(GroupLayout.Alignment.TRAILING).addComponent(jLabel1).addComponent(jSlider2, GroupLayout.PREFERRED_SIZE, 150, GroupLayout.PREFERRED_SIZE)).addContainerGap());

			label = new JLabel("");
			Font labelFont = label.Font;

			label.Font = new Font(labelFont.Name, Font.PLAIN, 30);


			jPanel4Layout.VerticalGroup = jPanel4Layout.createParallelGroup(GroupLayout.Alignment.LEADING).addGroup(jPanel4Layout.createParallelGroup(GroupLayout.Alignment.TRAILING).addComponent(jSlider2, GroupLayout.PREFERRED_SIZE, GroupLayout.DEFAULT_SIZE, GroupLayout.PREFERRED_SIZE).addGroup(jPanel4Layout.createSequentialGroup().addGroup(jPanel4Layout.createParallelGroup(GroupLayout.Alignment.BASELINE).addComponent(jLabel1)).addPreferredGap(LayoutStyle.ComponentPlacement.UNRELATED)));

			jPanel2.Background = new Color(0, 0, 0);
			jPanel2.Border = BorderFactory.createBevelBorder(BevelBorder.RAISED);
			// add the listeners to the label that contains the canvas buffered image
			canvasLabel.addMouseListener(new MouseAdapterAnonymousInnerClassHelper(this));
			canvasLabel.addMouseMotionListener(new MouseMotionAdapterAnonymousInnerClassHelper(this));


			jButton1.Text = "Clear";
			jButton1.addActionListener(new ActionListenerAnonymousInnerClassHelper(this));

			GroupLayout layout = new GroupLayout(ContentPane);
			ContentPane.Layout = layout;
			layout.HorizontalGroup = layout.createParallelGroup(GroupLayout.Alignment.LEADING).addGroup(layout.createSequentialGroup().addContainerGap().addGroup(layout.createParallelGroup(GroupLayout.Alignment.LEADING).addComponent(jPanel2, GroupLayout.Alignment.TRAILING, GroupLayout.DEFAULT_SIZE, GroupLayout.DEFAULT_SIZE, short.MaxValue).addGroup(layout.createSequentialGroup().addComponent(jPanel4, GroupLayout.PREFERRED_SIZE, GroupLayout.DEFAULT_SIZE, GroupLayout.PREFERRED_SIZE).addPreferredGap(LayoutStyle.ComponentPlacement.UNRELATED).addComponent(jPanel3, GroupLayout.PREFERRED_SIZE, GroupLayout.DEFAULT_SIZE, GroupLayout.PREFERRED_SIZE).addPreferredGap(LayoutStyle.ComponentPlacement.RELATED).addGroup(layout.createParallelGroup(GroupLayout.Alignment.LEADING).addComponent(jButton1, GroupLayout.DEFAULT_SIZE, 112, short.MaxValue).addComponent(label, GroupLayout.DEFAULT_SIZE, 112, short.MaxValue)))).addContainerGap());
			layout.VerticalGroup = layout.createParallelGroup(GroupLayout.Alignment.LEADING).addGroup(layout.createSequentialGroup().addGroup(layout.createParallelGroup(GroupLayout.Alignment.LEADING, false).addGroup(layout.createSequentialGroup().addGap(4, 4, 4).addComponent(jButton1, GroupLayout.PREFERRED_SIZE, 30, GroupLayout.PREFERRED_SIZE).addComponent(label, GroupLayout.PREFERRED_SIZE, 30, GroupLayout.PREFERRED_SIZE).addPreferredGap(LayoutStyle.ComponentPlacement.RELATED)).addComponent(jPanel4, GroupLayout.DEFAULT_SIZE, GroupLayout.DEFAULT_SIZE, short.MaxValue).addComponent(jPanel3, GroupLayout.DEFAULT_SIZE, GroupLayout.DEFAULT_SIZE, short.MaxValue)).addPreferredGap(LayoutStyle.ComponentPlacement.RELATED).addComponent(jPanel2, GroupLayout.DEFAULT_SIZE, GroupLayout.DEFAULT_SIZE, short.MaxValue).addContainerGap());

			Graphics g = canvas.Graphics;
			g.Color = Color.WHITE;
			g.fillRect(0, 0, canvas.Width, canvas.Height);
			repaint();
			pack();
		} // </editor-fold>

		private class ChangeListenerAnonymousInnerClassHelper : ChangeListener
		{
			private readonly FuNet1 outerInstance;

			public ChangeListenerAnonymousInnerClassHelper(FuNet1 outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public virtual void stateChanged(ChangeEvent evt)
			{
				outerInstance.jSlider2StateChanged(evt);
			}
		}

		private class MouseAdapterAnonymousInnerClassHelper : MouseAdapter
		{
			private readonly FuNet1 outerInstance;

			public MouseAdapterAnonymousInnerClassHelper(FuNet1 outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public virtual void mousePressed(MouseEvent evt)
			{
				outerInstance.jPanel2MousePressed(evt);
			}

			public virtual void mouseReleased(MouseEvent evt)
			{
				outerInstance.jPanel2MouseReleased(evt);
			}
		}

		private class MouseMotionAdapterAnonymousInnerClassHelper : MouseMotionAdapter
		{
			private readonly FuNet1 outerInstance;

			public MouseMotionAdapterAnonymousInnerClassHelper(FuNet1 outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public virtual void mouseDragged(MouseEvent evt)
			{
				outerInstance.jPanel2MouseDragged(evt);
			}
		}

		private class ActionListenerAnonymousInnerClassHelper : ActionListener
		{
			private readonly FuNet1 outerInstance;

			public ActionListenerAnonymousInnerClassHelper(FuNet1 outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public virtual void actionPerformed(ActionEvent evt)
			{
				outerInstance.jButton1ActionPerformed(evt);
			}
		}

		// clear the canvas using the currently selected color.
		private void jButton1ActionPerformed(ActionEvent evt)
		{
	//        System.out.println("You cleared the canvas.");
			Graphics g = canvas.Graphics;
			g.Color = Color.WHITE;
			g.fillRect(0, 0, canvas.Width, canvas.Height);
			repaint();
		}


		internal int currentX, currentY, oldX, oldY;

		private void jPanel2MouseDragged(MouseEvent evt)
		{
			currentX = evt.X;
			currentY = evt.Y;
			updateCanvas();
			if (tool == 1)
			{
				oldX = currentX;
				oldY = currentY;

			}

		}

		private void jPanel2MousePressed(MouseEvent evt)
		{

			oldX = evt.X;
			oldY = evt.Y;
			if (tool == 2)
			{
				currentX = oldX;
				currentY = oldY;
			}


		}

		internal int tool = 1;

		//Slider Properties//
		internal double value = 40;

		private void jSlider2StateChanged(ChangeEvent evt)
		{
			value = jSlider2.Value;

		}

		//COLOR CODE//
		internal int color = 1;


		//mouse released//
		private void jPanel2MouseReleased(MouseEvent evt)
		{

			currentX = evt.X;
			currentY = evt.Y;


			const double SCALE = 0.1;
			BufferedImage bi = new BufferedImage(32, 32, BufferedImage.TYPE_BYTE_GRAY);

			Graphics2D grph = (Graphics2D) bi.Graphics;
			grph.scale(SCALE, SCALE);

			grph.drawImage(canvas, 0, 0, null);
			grph.dispose();

			newPix = new double[32 * 32];
			pixels = bi.getRGB(0, 0, 32, 32, pixels, 0, 32);

			for (int i = 0; i < pixels.Length; i++)
			{
				newPix[i] = 255 - (pixels[i] & 0xff);
				newPix[i] /= 255;
			}


			long start = DateTimeHelperClass.CurrentUnixTimeMillis();
			network.Input = newPix;
			network.calculate();
			Console.WriteLine("Execution time: " + (DateTimeHelperClass.CurrentUnixTimeMillis() - start) + " milliseconds");

			try
			{
				ImageIO.write(bi, "png", new File("number.png"));
			}
			catch (IOException e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
			}

			double[] networkOutput = network.Output;
			int maxNeuronIdx = Utils.maxIdx(networkOutput);

			ClassificationResult max = new ClassificationResult(maxNeuronIdx, networkOutput[maxNeuronIdx]);


			Console.WriteLine("New calculation:");
			Console.WriteLine("Class: " + max.ClassIdx);
			Console.WriteLine("Probability: " + max.NeuronOutput);

			label.Text = Convert.ToString(max.ClassIdx);


		}

		//set ui visible//
		public static void Main(string[] args)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.concurrent.ScheduledExecutorService scheduler = java.util.concurrent.Executors.newScheduledThreadPool(1);
			ScheduledExecutorService scheduler = Executors.newScheduledThreadPool(1);
			EventQueue.invokeLater(new RunnableAnonymousInnerClassHelper(scheduler));
		}

		private class RunnableAnonymousInnerClassHelper : Runnable
		{
			private ScheduledExecutorService scheduler;

			public RunnableAnonymousInnerClassHelper(ScheduledExecutorService scheduler)
			{
				this.scheduler = scheduler;
			}

			public virtual void run()
			{
				FuNet1 net = new FuNet1();

				scheduler.scheduleAtFixedRate(net, 0, 300, TimeUnit.MILLISECONDS);

				net.Visible = true;
			}

		}

		// Variables declaration - do not modify
		private ButtonGroup buttonGroup1;
		private ButtonGroup buttonGroup2;
		private JButton jButton1;
		private JButton jButton2;
		private JLabel jLabel1;
		public JPanel jPanel2;
		private JPanel jPanel3;
		private JPanel jPanel4;
		//    private JRadioButton jRadioButton10;
		private JRadioButton jRadioButton3;
		private JRadioButton jRadioButton4;
		private JRadioButton jRadioButton5;
		private JRadioButton jRadioButton6;
		private JRadioButton jRadioButton7;
		private JRadioButton jRadioButton8;
		//    private JRadioButton jRadioButton9;
		public JSlider jSlider2;

		internal int[] pixels;
		internal double[] newPix;


		public override void run()
		{

		}
	// End of variables declaration
	}
}