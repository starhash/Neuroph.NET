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

namespace org.neuroph.samples
{

	using MultiLayerPerceptron = org.neuroph.nnet.MultiLayerPerceptron;
	using JDBCInputAdapter = org.neuroph.util.io.JDBCInputAdapter;
	using JDBCOutputAdapter = org.neuroph.util.io.JDBCOutputAdapter;

	/// <summary>
	/// This sample shows hot to read network input and write network output to database 
	/// using Neuroph JDBC adapaters.
	/// @author Zoran Sevarac <sevarac@gmail.com>
	/// </summary>
	public class JDBCSample
	{

		/// <summary>
		/// Runs this sample
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws java.io.FileNotFoundException, java.io.IOException, ClassNotFoundException, java.sql.SQLException
		public static void Main(string[] args)
		{

			// create neural network
			MultiLayerPerceptron neuralNet = new MultiLayerPerceptron(2, 3, 1);

			// Load the database driver
			Type.GetType("sun.jdbc.odbc.JdbcOdbcDriver");
			// Get a connection to the database
			string dbName = "neuroph";
			string dbUser = "root";
			string dbPass = "";
			// create a connection to database
			Connection connection = DriverManager.getConnection("jdbc:mysql://localhost:3306/" + dbName, dbUser, dbPass);

			// ise this sql to get input from database table
			string inputSql = "SELECT * FROM input_data";
			// create dinput adapter using specidfied database connection and sql query
			JDBCInputAdapter @in = new JDBCInputAdapter(connection, inputSql);
			string outputTable = "output_data"; // write output to this table
			// create output adapter using specified connection and output table
			JDBCOutputAdapter @out = new JDBCOutputAdapter(connection, outputTable);


			double[] input;
			// read input using input adapter
			while ((input = @in.readInput()) != null)
			{
				neuralNet.Input = input;
				neuralNet.calculate();
				double[] output = neuralNet.Output;
				// and write output using output aadapter
				@out.writeOutput(output);
			}

			@in.close();
			@out.close();
			connection.close();
		}
	}
}