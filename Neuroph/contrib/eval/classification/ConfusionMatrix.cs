using System;
using System.Text;

namespace org.org.neuroph.contrib.eval.classification
{

	/// <summary>
	/// Confusion matrix container, holds class labels and matrix values .
	/// </summary>
	public class ConfusionMatrix
	{

		/// <summary>
		/// Class labels
		/// </summary>
		private string[] classLabels;

		/// <summary>
		/// Values of confusion matrix
		/// </summary>
		private double[,] values;

		/// <summary>
		/// Number of classes
		/// </summary>
		private int classCount;

		private int total = 0;


		/// <summary>
		/// Default setting for formating toString
		/// </summary>
		private const int STRING_DEFAULT_WIDTH = 7;

		/// <summary>
		/// Creates new confusion matrix with specified class labels and number of classes </summary>
		/// <param name="labels"> </param>
		/// <param name="classCount">  </param>
		public ConfusionMatrix(string[] classLabels)
		{
			this.classLabels = classLabels;
			this.classCount = classLabels.Length;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: this.values = new double[classCount,classCount];
			this.values = new double[classCount, classCount];
		}

		/// <summary>
		/// Returns confusion matrix values as double array </summary>
		/// <returns> confusion matrix values as double array </returns>
		public virtual double[,] Values
		{
			get
			{
				return values;
			}
		}

		/// <summary>
		/// Returns value of confusion matrix at specified position </summary>
		/// <param name="actual"> actual idx position </param>
		/// <param name="predicted"> predicted idx position </param>
		/// <returns> value of confusion matrix at specified position  </returns>
		public virtual double getValueAt(int actual, int predicted)
		{
		   return values[actual,predicted];
		}

		/// <summary>
		/// Increments matrix value at specified position </summary>
		/// <param name="actual"> class id of correct classification </param>
		/// <param name="predicted"> class id of predicted classification </param>
		public virtual void incrementElement(int actual, int predicted)
		{
			values[actual,predicted]++;
			total++;
		}

		internal virtual int ClassCount
		{
			get
			{
				return classCount;
			}
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			int maxColumnLenght = STRING_DEFAULT_WIDTH;
			foreach (string label in classLabels)
			{
				maxColumnLenght = Math.Max(maxColumnLenght, label.Length);
			}

//JAVA TO C# CONVERTER TODO TASK: The following line has a Java format specifier which cannot be directly translated to .NET:
//ORIGINAL LINE: builder.append(String.format("%1$" + maxColumnLenght + "s", ""));
			builder.Append(string.Format("%1$" + maxColumnLenght + "s", ""));
			foreach (string label in classLabels)
			{
//JAVA TO C# CONVERTER TODO TASK: The following line has a Java format specifier which cannot be directly translated to .NET:
//ORIGINAL LINE: builder.append(String.format("%1$" + maxColumnLenght + "s", label));
				builder.Append(string.Format("%1$" + maxColumnLenght + "s", label));
			}
			builder.Append("\n");

			for (int i = 0; i < values.Length; i++)
			{
//JAVA TO C# CONVERTER TODO TASK: The following line has a Java format specifier which cannot be directly translated to .NET:
//ORIGINAL LINE: builder.append(String.format("%1$" + maxColumnLenght + "s", classLabels[i]));
				builder.Append(string.Format("%1$" + maxColumnLenght + "s", classLabels[i]));
				for (int j = 0; j < values.GetLength(1); j++)
				{
//JAVA TO C# CONVERTER TODO TASK: The following line has a Java format specifier which cannot be directly translated to .NET:
//ORIGINAL LINE: builder.append(String.format("%1$" + maxColumnLenght + "s", values[i,j]));
					builder.Append(string.Format("%1$" + maxColumnLenght + "s", values[i,j]));
				}
				builder.Append("\n");

			}
			return builder.ToString();
		}

		public virtual int getTruePositive(int clsIdx)
		{
			return (int)values[clsIdx,clsIdx];
		}

		public virtual int getTrueNegative(int clsIdx)
		{
			int trueNegative = 0;

			for (int i = 0; i < classCount; i++)
			{
				if (i == clsIdx)
				{
					continue;
				}
				for (int j = 0; j < classCount; j++)
				{
					if (j == clsIdx)
					{
						continue;
					}
					trueNegative += (int)(values[i,j]);
				}
			}

			return trueNegative;
		}

		public virtual int getFalsePositive(int clsIdx)
		{
			int falsePositive = 0;

			for (int i = 0; i < classCount; i++)
			{
				if (i == clsIdx)
				{
					continue;
				}
				falsePositive += (int)(values[i,clsIdx]);
			}

			return falsePositive;
		}

		public virtual int getFalseNegative(int clsIdx)
		{
			int falseNegative = 0;

			for (int i = 0; i < classCount; i++)
			{
				if (i == clsIdx)
				{
					continue;
				}
				falseNegative += (int)(values[clsIdx,i]);
			}

			return falseNegative;
		}

		public virtual string[] ClassLabels
		{
			get
			{
				return classLabels;
			}
		}

		public virtual int Total
		{
			get
			{
				return total;
			}
		}





	}

}