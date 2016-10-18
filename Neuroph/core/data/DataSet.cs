using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using org.neuroph.core.exceptions;
using org.neuroph.util.data.sample;
using java.io;
using java.lang;
using java.util;

/// <summary>
/// Copyright 2014 Neuroph Project http://neuroph.sourceforge.net
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
namespace org.neuroph.core.data
{



	/// <summary>
	/// This class represents a collection of data rows (DataSetRow instances) used
	/// for training and testing neural network.
	/// 
	/// @author Zoran Sevarac <sevarac@gmail.com> </summary>
	/// <seealso cref= DataSetRow
	/// http://openforecast.sourceforge.net/docs/net/sourceforge/openforecast/DataSet.html </seealso>
	[Serializable]
	public class DataSet
	{

		/// <summary>
		/// The class fingerprint that is set to indicate serialization compatibility
		/// with a previous version of the class
		/// </summary>
		private const long serialVersionUID = 2L;
		/// <summary>
		/// Collection of data rows
		/// </summary>
		private List<DataSetRow> rows;

		/// <summary>
		/// Size of the input vector in data set rows
		/// </summary>
		private int inputSize = 0;

		/// <summary>
		/// Size of output vector in data set rows
		/// </summary>
		private int outputSize = 0;

		/// <summary>
		/// Column names/labels
		/// </summary>
		private string[] columnNames;

		/// <summary>
		/// Flag which indicates if this data set containes data rows for supervised training
		/// </summary>
		private bool isSupervised = false;

		/// <summary>
		/// Label for this training set
		/// </summary>
		private string label;

		/// <summary>
		/// Full file path including file name
		/// </summary>
		[NonSerialized]
		private string filePath;


		/// <summary>
		/// Creates an instance of new empty training set
		/// </summary>
		/// <param name="inputSize"> </param>
		public DataSet(int inputSize)
		{
            this.rows = new List<DataSetRow>();
			this.inputSize = inputSize;
			this.isSupervised = false;
			this.columnNames = new string[inputSize];
		}

		/// <summary>
		/// Creates an instance of new empty training set
		/// </summary>
		/// <param name="inputSize">  Length of the input vector </param>
		/// <param name="outputSize"> Length of the output vector </param>
		public DataSet(int inputSize, int outputSize)
		{
            this.rows = new List<DataSetRow>();
			this.inputSize = inputSize;
			this.outputSize = outputSize;
			this.isSupervised = true;
			this.columnNames = new string[inputSize + outputSize];
		}

		/// <summary>
		/// Adds new row row to this data set
		/// </summary>
		/// <param name="row"> data set row to add </param>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void addRow(DataSetRow row) throws org.neuroph.core.exceptions.VectorSizeMismatchException
		public virtual void addRow(DataSetRow row)
		{

			if (row == null)
			{
				throw new System.ArgumentException("Data set row cannot be null!");
			}

			// check input vector size if it is predefined
			if ((this.inputSize != 0) && (row.Input.Length != this.inputSize))
			{
				throw new VectorSizeMismatchException("Input vector size does not match data set input size!");
			}


			if ((this.outputSize != 0) && (row.DesiredOutput.Length != this.outputSize))
			{
				throw new VectorSizeMismatchException("Output vector size does not match data set output size!");
			}

			// if everything was ok add training row
			this.rows.Add(row);
		}

		/// <summary>
		/// Adds a new dataset row with specified input
		/// </summary>
		/// <param name="input"> </param>
		public virtual void addRow(double[] input)
		{
			if (input == null)
			{
				throw new System.ArgumentException("Input for dataset row cannot be null!");
			}

			if (input.Length != inputSize)
			{
				throw new NeurophException("Input size for given row is different from the data set size!");
			}

			if (isSupervised)
			{
				throw new NeurophException("Cannot add unsupervised row to supervised data set!");
			}

			this.addRow(new DataSetRow(input));
		}

		/// <summary>
		/// Adds a new dataset row with specified input and output
		/// </summary>
		/// <param name="input"> </param>
		/// <param name="output"> </param>
		public virtual void addRow(double[] input, double[] output)
		{
			this.addRow(new DataSetRow(input, output));
		}

		/// <summary>
		/// Removes training row at specified index position
		/// </summary>
		/// <param name="idx"> position of row to remove </param>
		public virtual void removeRowAt(int idx)
		{
			this.rows.RemoveAt(idx);
		}

		/// <summary>
		/// Returns Iterator for iterating training elements collection
		/// </summary>
		/// <returns> Iterator for iterating training elements collection </returns>
		public virtual IEnumerator<DataSetRow> iterator()
		{
			return this.rows.GetEnumerator();
		}

		/// <summary>
		/// Returns elements of this training set
		/// </summary>
		/// <returns> training elements </returns>
		public virtual List<DataSetRow> Rows
		{
			get
			{
				return this.rows;
			}
		}

		/// <summary>
		/// Returns training row at specified index position
		/// </summary>
		/// <param name="idx"> index position of training row to return </param>
		/// <returns> training row at specified index position </returns>
		public virtual DataSetRow getRowAt(int idx)
		{
			return this.rows[idx];
		}

		/// <summary>
		/// Removes all alements from training set
		/// </summary>
		public virtual void clear()
		{
			this.rows.Clear();
		}

		/// <summary>
		/// Returns true if training set is empty, false otherwise
		/// </summary>
		/// <returns> true if training set is empty, false otherwise </returns>
		public virtual bool Empty
		{
			get
			{
				return this.rows.Count == 0;
			}
		}

		/// <summary>
		/// Returns true if data set is supervised,  false otherwise
		/// 
		/// @return
		/// </summary>
		public virtual bool Supervised
		{
			get
			{
				return this.isSupervised;
			}
		}

		/// <summary>
		/// Returns number of training elements in this training set set
		/// </summary>
		/// <returns> number of training elements in this training set set </returns>
		public virtual int size()
		{
			return this.rows.Count;
		}

		/// <summary>
		/// Returns label for this training set
		/// </summary>
		/// <returns> label for this training set </returns>
		public virtual string Label
		{
			get
			{
				return label;
			}
			set
			{
				this.label = value;
			}
		}


		public virtual string[] ColumnNames
		{
			get
			{
				return columnNames;
			}
			set
			{
				this.columnNames = value;
			}
		}


		public virtual string getColumnName(int idx)
		{
			return columnNames[idx];
		}

		public virtual void setColumnName(int idx, string columnName)
		{
			columnNames[idx] = columnName;
		}


		/// <summary>
		/// Sets full file path for this training set
		/// </summary>
		/// <param name="filePath"> </param>
		public virtual string FilePath
		{
			set
			{
				this.filePath = value;
			}
			get
			{
				return filePath;
			}
		}


		/// <summary>
		/// Returns string representation of this data set
		/// </summary>
		/// <returns> string representation of this data set </returns>
		public override string ToString()
		{
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("Dataset Label: ").Append(label).Append(java.lang.System.lineSeparator());

			if (columnNames != null)
			{
				sb.Append("Columns: ");
				foreach (string columnName in columnNames)
				{
					sb.Append(columnName).Append(", ");
				}
				sb.Remove(sb.Length - 2, sb.Length - 1 - sb.Length - 2);
				sb.Append(java.lang.System.lineSeparator());
			}

			foreach (DataSetRow row in rows)
			{
				sb.Append(row).Append(java.lang.System.lineSeparator());
			}

			return sb.ToString();
		}

		/// <summary>
		/// Returns enire dataset in csv format
		/// 
		/// @return
		/// </summary>
		public virtual string toCSV()
		{
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

			if ((columnNames != null) && (columnNames.Length > 0))
			{
				foreach (string columnName in columnNames)
				{
					sb.Append(columnName).Append(", ");
				}
				sb.Remove(sb.Length - 2, sb.Length - 1 - sb.Length - 2);
				sb.Append(java.lang.System.lineSeparator());
			}

			// promeniti
			foreach (DataSetRow row in rows)
			{
				sb.Append(row.toCSV()); // nije dobro jer lepi input i desired output; treba bez toga mozda dodati u toCSV
				sb.Append(java.lang.System.lineSeparator());
			}

			return sb.ToString();
		}

		/// <summary>
		/// Saves this training set to the specified file
		/// </summary>
		/// <param name="filePath"> </param>
		public virtual void save(string filePath)
		{
			this.filePath = filePath;
			this.save();
		}

		/// <summary>
		/// Saves this training set to file specified in its filePath field
		/// </summary>
		public virtual void save()
		{
			ObjectOutputStream @out = null;

			try
			{
				File file = new File(this.filePath);
				@out = new ObjectOutputStream(new FileOutputStream(file));
				@out.writeObject(this);
				@out.flush();

			}
			catch (IOException ioe)
			{
				throw new NeurophException(ioe);
			}
			finally
			{
				if (@out != null)
				{
					try
					{
						@out.close();
					}
					catch (IOException)
					{
					}
				}
			}
		}

		public virtual void saveAsTxt(string filePath, string delimiter)
		{

			if (filePath == null)
			{
				throw new System.ArgumentException("File path is null!");
			}

			// default delimiter is space if other is not specified
			if ((delimiter == null) || delimiter.Equals(""))
			{
				delimiter = " ";
			}


			try
			{
					using (PrintWriter @out = new PrintWriter(new FileWriter(new File(filePath))))
					{
        
					int columnCount = inputSize + outputSize;
					if ((columnNames != null) && (columnNames.Length > 0))
					{
						for (int i = 0; i < columnNames.Length; i++)
						{
							@out.print(columnNames[i]);
							if (i < columnCount - 1)
							{
								@out.print(delimiter);
							}
						}
						@out.println();
					}
        
					foreach (DataSetRow row in this.rows)
					{
						double[] input = row.Input;
						for (int i = 0; i < input.Length; i++)
						{
							@out.print(input[i]);
							if (i < columnCount - 1)
							{
								@out.print(delimiter);
							}
						}
        
						if (row.Supervised)
						{
							double[] output = row.DesiredOutput;
							for (int j = 0; j < output.Length; j++)
							{
								@out.print(output[j]);
								if (inputSize + j < columnCount - 1)
								{
									@out.print(delimiter);
								}
							}
						}
						@out.println();
					}
        
					@out.flush();
        
					}
			}
			catch (IOException ex)
			{
				throw new NeurophException("Error saving data set file!", ex);
			}
		}

		/// <summary>
		/// Loads training set from the specified file
		/// </summary>
		/// <param name="filePath"> training set file </param>
		/// <returns> loded training set </returns>
		public static DataSet load(string filePath)
		{
			ObjectInputStream oistream = null;

			try
			{
				File file = new File(filePath);
				if (!file.exists())
				{
					throw new FileNotFoundException("Cannot find file: " + filePath);

				}

				oistream = new ObjectInputStream(new FileInputStream(filePath));
				DataSet dataSet = (DataSet) oistream.readObject();
				dataSet.FilePath = filePath;

				return dataSet;

			}
			catch (IOException ioe)
			{
				throw new NeurophException("Error reading file!", ioe);
			}
			catch (ClassNotFoundException ex)
			{
				throw new NeurophException("Class not found while trying to read DataSet object from the stream!", ex);
			}
			finally
			{
				if (oistream != null)
				{
					try
					{
						oistream.close();
					}
					catch (IOException)
					{
					}
				}
			}
		}

		/// <summary>
		/// Creates and returns data set from specified csv file
		/// </summary>
		/// <param name="filePath">        path to csv dataset file to import </param>
		/// <param name="inputsCount">     number of inputs </param>
		/// <param name="outputsCount">    number of outputs </param>
		/// <param name="delimiter">       delimiter of values </param>
		/// <param name="loadColumnNames"> true if csv file contains column names in first line, false otherwise </param>
		/// <returns> instance of dataset with values from specified file
		/// 
		/// TODO: try with resources, provide information on exact line of error if format is not good in NumberFormatException </returns>
		public static DataSet createFromFile(string filePath, int inputsCount, int outputsCount, string delimiter, bool loadColumnNames)
		{
			BufferedReader reader = null;

			if (filePath == null)
			{
				throw new System.ArgumentException("File name cannot be null!");
			}
			if (inputsCount <= 0)
			{
				throw new System.ArgumentException("Number of inputs cannot be <= 0 : " + inputsCount);
			}
			if (outputsCount < 0)
			{
				throw new System.ArgumentException("Number of outputs cannot be < 0 : " + outputsCount);
			}
			if ((delimiter == null) || delimiter.Length == 0)
			{
				throw new System.ArgumentException("Delimiter cannot be null or empty!");
			}

			try
			{
				DataSet dataSet = new DataSet(inputsCount, outputsCount);
				dataSet.FilePath = filePath;
				reader = new BufferedReader(new FileReader(new File(filePath)));

				string line = null;

				if (loadColumnNames)
				{
					// get column names from the first line
					line = reader.readLine();
					string[] colNames = line.Split(delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
					dataSet.ColumnNames = colNames;
				}

				while ((line = reader.readLine()) != null)
				{
					string[] values = line.Split(delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    double[] inputs = new double[inputsCount];
					double[] outputs = new double[outputsCount];

					if (values[0].Equals(""))
					{
						continue; // skip if line was empty
					}
					for (int i = 0; i < inputsCount; i++)
					{
						inputs[i] = Convert.ToDouble(values[i]);
					}

					for (int i = 0; i < outputsCount; i++)
					{
						outputs[i] = Convert.ToDouble(values[inputsCount + i]);
					}

					if (outputsCount > 0)
					{
						dataSet.addRow(new DataSetRow(inputs, outputs));
					}
					else
					{
						dataSet.addRow(new DataSetRow(inputs));
					}
				}

				reader.close();

				return dataSet;

			}
			catch (FileNotFoundException ex)
			{
				throw new NeurophException("Could not find data set file!", ex);
			}
			catch (IOException ex)
			{
				if (reader != null)
				{
					try
					{
						reader.close();
					}
					catch (IOException)
					{
					}
				}
				throw new NeurophException("Error reading data set file!", ex);
			}
			catch (NumberFormatException ex)
			{
				if (reader != null)
				{
					try
					{
						reader.close();
					}
					catch (IOException)
					{
					}
				}
                System.Console.WriteLine(ex.ToString());
                System.Console.Write(ex.StackTrace);
				throw new NeurophException("Bad number format in data set file!", ex);
			}

		}

		/// <summary>
		/// Creates and returns data set from specified csv file
		/// </summary>
		/// <param name="filePath">        path to csv dataset file to import </param>
		/// <param name="inputsCount">     number of inputs </param>
		/// <param name="outputsCount">    number of outputs </param>
		/// <param name="delimiter">       delimiter of values </param>
		/// <returns> instance of dataset with values from specified file </returns>
		public static DataSet createFromFile(string filePath, int inputsCount, int outputsCount, string delimiter)
		{
			return createFromFile(filePath, inputsCount, outputsCount, delimiter, false);
		}



		// http://java.about.com/od/javautil/a/uniquerandomnum.htm

		/// <summary>
		/// Returns training and test subsets in the specified percent ratio </summary>
		/// <param name="trainSetPercent"> </param>
		/// <param name="testSetPercent">
		/// @return </param>
		public virtual DataSet[] createTrainingAndTestSubsets(int trainSetPercent, int testSetPercent)
		{
			SubSampling sampling = new SubSampling(trainSetPercent, testSetPercent);
			return new List<DataSet>(sampling.sample(this)).ToArray();
		}


		public virtual List<DataSet> sample(Sampling sampling)
		{
			return sampling.sample(this);
		}


		/// <summary>
		/// Returns output vector size of training elements in this training set.
		/// </summary>
		public virtual int OutputSize
		{
			get
			{
				return this.outputSize;
			}
		}

		/// <summary>
		/// Returns input vector size of training elements in this training set This
		/// method is implementation of EngineIndexableSet interface, and it is added
		/// to provide compatibility with Encog data sets and FlatNetwork
		/// </summary>
		public virtual int InputSize
		{
			get
			{
				return this.inputSize;
			}
		}

		public virtual void shuffle()
		{
            java.util.ArrayList rowlist = new java.util.ArrayList();
            for (int i = 0; i < rows.Count; i++) {
                rowlist.add(rows[i]);
            }
            Collections.shuffle(rowlist);
		}

	}
}