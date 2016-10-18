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


    using DataSetRow = org.neuroph.core.data.DataSetRow;
    using DataSet = org.neuroph.core.data.DataSet;
    using java.io;


    /// <summary>
    /// Handles training set imports
    /// 
    /// @author Zoran Sevarac
    /// @author Ivan Nedeljkovic
    /// @author Kokanovic Rados
    /// </summary>

    // TODO: importFromDatabase(sql, ...) and importFromUrl(url, ...)
    // rename to DataSetImport
    public class TrainingSetImport
	{

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static org.neuroph.core.data.DataSet importFromFile(String filePath, int inputsCount, int outputsCount, String separator) throws java.io.IOException, java.io.FileNotFoundException, NumberFormatException
	  public static DataSet importFromFile(string filePath, int inputsCount, int outputsCount, string separator)
	  {

		FileReader fileReader = null;

		try
		{
		 DataSet trainingSet = new DataSet(inputsCount, outputsCount);
		 fileReader = new FileReader(new File(filePath));
		 BufferedReader reader = new BufferedReader(fileReader);

		 string line = "";
		 // check if firs lin econtains column names and set datatset column names
		  while ((line = reader.readLine()) != null)
		  {
			double[] inputs = new double[inputsCount];
			double[] outputs = new double[outputsCount];
			string[] values = line.Split(separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

			if (values[0].Equals("")) // skip if line was empty
			{
				continue;
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
				  trainingSet.addRow(new DataSetRow(inputs, outputs));
			}
			else
			{
				  trainingSet.addRow(new DataSetRow(inputs));
			}
		  }

		  return trainingSet;

		}
		catch (FileNotFoundException ex)
		{
		   System.Console.WriteLine(ex.ToString());
		   System.Console.Write(ex.StackTrace);
		   throw ex;
		}
		catch (IOException ex)
		{
			if (fileReader != null)
			{
				fileReader.close();
			}
			System.Console.WriteLine(ex.ToString());
			System.Console.Write(ex.StackTrace);
			throw ex;
		}
		catch (java.lang.NumberFormatException ex)
		{
		   fileReader.close();
		   System.Console.WriteLine(ex.ToString());
		   System.Console.Write(ex.StackTrace);
		   throw ex;
		}
	  }

	}
}