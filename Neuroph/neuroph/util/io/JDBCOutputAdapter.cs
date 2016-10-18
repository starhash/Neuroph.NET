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

namespace org.neuroph.util.io
{


	/// <summary>
	/// Implementation of OutputAdapter interface for writing neural network outputs to database. </summary>
	/// <seealso cref= OutputAdapter
	/// @author Zoran Sevarac <sevarac@gmail.com> </seealso>
	public class JDBCOutputAdapter : OutputAdapter
	{
		internal java.sql.Connection connection;
		internal string tableName;

		/// <summary>
		/// Creates new JDBCOutputAdapter with specifed database connection and table </summary>
		/// <param name="connection"> database connection </param>
		/// <param name="tableName">  table to put data into </param>
		public JDBCOutputAdapter(java.sql.Connection connection, string tableName)
		{
			this.connection = connection;
			this.tableName = tableName;
		}

		/// <summary>
		/// Writes specified output to table in database </summary>
		/// <param name="output">  </param>
		public virtual void writeOutput(double[] output)
		{
			try
			{
				string sql = "INSERT " + tableName + " VALUES(";
				for (int i = 0; i < output.Length; i++)
				{
					sql += "?";
					if (i < (output.Length - 1)) // add coma if not last
					{
						sql = ", ";
					}
				}
				sql += ")";

                //            for (int i = 0; i < output.length; i++) {
                //                sql += output[i];
                //                if (i < (output.length - 1)) {
                //                    sql = ", ";
                //                }
                //            }


                //            Statement stmt = connection.createStatement();

                java.sql.PreparedStatement stmt = connection.prepareStatement(sql);
				for (int i = 0; i < output.Length; i++)
				{
					stmt.setDouble(i, output[i]);
				}

				stmt.executeUpdate(sql);
				stmt.close();

			}
			catch (java.sql.SQLException ex)
			{
                //JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
                throw new NeurophInputException("Error executing query at JDBCOutputAdapter", ex);
			}

		}

		public virtual void close()
		{
		}
	}

}