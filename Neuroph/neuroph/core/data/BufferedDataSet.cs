using java.io;
using org.slf4j;
using System;
using System.Collections.Generic;
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
namespace org.neuroph.core.data {


    /// <summary>
    /// This class can be used for large training sets, which are partialy read from
    /// file during the training. It loads bufferSize rows from file into DataSet,
    /// and when it iterates all of them, it takes next bufferSize rows. It can be
    /// used everywhere where DataSet class is used since it extends it. The rows
    /// should be iterated with iterator() interface.
    /// 
    /// @author Zoran Sevarac
    /// </summary>
    public class BufferedDataSet : DataSet, IEnumerator<DataSetRow> {

        /// <summary>
        /// Buffer size determines how many data rows will be loaded from file at once
        /// </summary>
        private int bufferSize = 1000;

        /// <summary>
        /// File with data set rows
        /// </summary>
        private File file;

        /// <summary>
        /// Number of lines in the file
        /// </summary>
        private long fileLinesNumber;

        /// <summary>
        /// Current line number that we're reading
        /// </summary>
        private long currentFileLineNumber;

        /// <summary>
        /// Number of rows loaded
        /// </summary>
        private int rowsLoaded;

        /// <summary>
        /// Delimiter character for values in line
        /// </summary>
        private string delimiter;

        /// <summary>
        /// File reader used to read from file
        /// </summary>
        internal FileReader fileReader = null;

        /// <summary>
        /// Buffered reader used to read from file
        /// </summary>
        internal BufferedReader bufferedReader;

        /// <summary>
        /// A reference to the buffered rows (a List collection in superclass)
        /// </summary>
        internal List<DataSetRow> bufferedRows;

        /// <summary>
        /// Iterator for buffered rowa
        /// </summary>
        internal IEnumerator<DataSetRow> bufferIterator;

        public DataSetRow Current {
            get {
                return bufferIterator.Current;
            }
        }

        object IEnumerator.Current {
            get {
                return bufferIterator.Current;
            }
        }

        public BufferedDataSet(File file, int inputSize, string delimiter) : base(inputSize) {
        }

        /// <summary>
        /// Creates new buffered data set with specified file, input and output size.
        /// Data set file is assumed to be txt value with data set rows in a single line,
        /// with input and output vector values delimited by delimiter.
        /// </summary>
        /// <param name="file"> datas et file </param>
        /// <param name="inputSize"> size of input vector </param>
        /// <param name="outputSize"> size of outut vector </param>
        /// <param name="delimiter"> delimiter for vector values </param>
        /// <exception cref="FileNotFoundException">  </exception>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public BufferedDataSet(java.io.File file, int inputSize, int outputSize, String delimiter) throws java.io.FileNotFoundException
        public BufferedDataSet(File file, int inputSize, int outputSize, string delimiter) : base(inputSize, outputSize) {

            this.delimiter = delimiter;
            this.file = file;
            this.fileReader = new FileReader(file);
            this.bufferedReader = new BufferedReader(fileReader);
            fileLinesNumber = countFileLines();

            // load first chunk of data into buffer
            loadNextBuffer();
        }

        /// <summary>
        /// Counts and returns number of lines in a file </summary>
        /// <returns> number of lines in a file </returns>
        /// <exception cref="FileNotFoundException">  </exception>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: private long countFileLines() throws java.io.FileNotFoundException
        private long countFileLines() {
            LineNumberReader lnr = new LineNumberReader(new FileReader(file));
            try {
                //  lnr.skip(Long.MAX_VALUE);
                while (lnr.skip(long.MaxValue) > 0) {
                };
            } catch (IOException ex) {
            }
            return lnr.getLineNumber() + 1;
        }

        /// <summary>
        /// Returns iterator for buffered data set </summary>
        /// <returns>  </returns>
        public override IEnumerator<DataSetRow> iterator() {
            return this;
        }

        /// <summary>
        /// Returns true if there are more rows, false otherwise </summary>
        /// <returns> true if there are more rows, false otherwise </returns>
        public bool HasNext() {
            if (currentFileLineNumber < fileLinesNumber) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns next data set row. Note that if there are no more buffered rows, 
        /// this mthod will load next bufferSize rows into buffer. </summary>
        /// <returns> next data set row </returns>
        public DataSetRow Next() {
            // if we have reached the end of buffered data
            //JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
            if (!bufferIterator.MoveNext()) {
                this.loadNextBuffer(); // load next chunk from file into buffer
            }

            currentFileLineNumber++; // increase line counter
                                     // JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
            return bufferIterator.Current; // and return next data row
        }

        public void Remove() // what should be done here?
        {
            throw new System.NotSupportedException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
        }

        /// <summary>
        /// Loads next bufferSize rows from file into buffer
        /// </summary>
        private void loadNextBuffer() {
            try {
                string line = "";
                this.clear(); // data set buffer

                rowsLoaded = 0;
                while (rowsLoaded < bufferSize) {

                    line = bufferedReader.readLine();
                    if (line == null) {
                        break;
                    }

                    rowsLoaded++;
                    double[] inputs = new double[InputSize];
                    double[] outputs = new double[OutputSize];
                    string[] values = line.Split(delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    if (values[0].Equals("")) {
                        continue; // skip if line was empty
                    }
                    for (int i = 0; i < InputSize; i++) {
                        inputs[i] = Convert.ToDouble(values[i]);
                    }

                    for (int i = 0; i < OutputSize; i++) {
                        outputs[i] = Convert.ToDouble(values[InputSize + i]);
                    }

                    if (OutputSize > 0) {
                        this.addRow(new DataSetRow(inputs, outputs));
                    } else {
                        this.addRow(new DataSetRow(inputs));
                    }
                }

                bufferedRows = this.Rows;
                bufferIterator = bufferedRows.GetEnumerator();

            } catch (FileNotFoundException ex) {
                System.Console.WriteLine(ex.ToString());
                System.Console.Write(ex.StackTrace);
            } catch (IOException ex) {
                if (fileReader != null) {
                    try {
                        fileReader.close();
                    } catch (IOException) {
                    }
                }
                System.Console.WriteLine(ex.ToString());
                System.Console.Write(ex.StackTrace);
            } catch (java.lang.NumberFormatException ex) {
                if (fileReader != null) {
                    try {
                        fileReader.close();
                    } catch (IOException) {
                    }
                }
                System.Console.WriteLine(ex.ToString());
                System.Console.Write(ex.StackTrace);
                throw ex;
            }
        }

        public IEnumerator<DataSetRow> GetEnumerator() {
            return bufferIterator;
        }

        public void Dispose() {
            bufferIterator.Dispose();
        }

        public bool MoveNext() {
            return bufferIterator.MoveNext();
        }

        public void Reset() {
            bufferIterator.Reset();
        }
    }
}