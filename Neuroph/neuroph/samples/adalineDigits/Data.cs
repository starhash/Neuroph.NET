namespace org.neuroph.samples.adalineDigits
{


	using DataSetRow = org.neuroph.core.data.DataSetRow;

	public class Data
	{

		public const int CHAR_WIDTH = 5;
		public const int CHAR_HEIGHT = 7;

		public static string[][] DIGITS = new string[][] {new string[] {" OOO ", "O   O", "O   O", "O   O", "O   O", "O   O", " OOO "}, new string[] {"  O  ", " OO  ", "O O  ", "  O  ", "  O  ", "  O  ", "  O  "}, new string[] {" OOO ", "O   O", "    O", "   O ", "  O  ", " O   ", "OOOOO"}, new string[] {" OOO ", "O   O", "    O", " OOO ", "    O", "O   O", " OOO "}, new string[] {"   O ", "  OO ", " O O ", "O  O ", "OOOOO", "   O ", "   O "}, new string[] {"OOOOO", "O    ", "O    ", "OOOO ", "    O", "O   O", " OOO "}, new string[] {" OOO ", "O   O", "O    ", "OOOO ", "O   O", "O   O", " OOO "}, new string[] {"OOOOO", "    O", "    O", "   O ", "  O  ", " O   ", "O    "}, new string[] {" OOO ", "O   O", "O   O", " OOO ", "O   O", "O   O", " OOO "}, new string[] {" OOO ", "O   O", "O   O", " OOOO", "    O", "O   O", " OOO "}};

		public static DataSetRow convertImageIntoData(string[] image)
		{

			DataSetRow dataSetRow = new DataSetRow(Data.CHAR_HEIGHT * Data.CHAR_WIDTH);

			double[] array = new double[Data.CHAR_WIDTH * Data.CHAR_HEIGHT];

			for (int row = 0; row < Data.CHAR_HEIGHT; row++)
			{
				for (int column = 0; column < Data.CHAR_WIDTH; column++)
				{
					int index = (row * Data.CHAR_WIDTH) + column;
					char ch = image[row][column];
					array[index] = (ch == 'O' ? 1 : -1);
				}
			}
			dataSetRow.Input = array;
			return dataSetRow;
		}

		public static string[] convertDataIntoImage(double[] data)
		{

			string[] image = new string[data.Length / Data.CHAR_WIDTH];
			string row = "";

			for (int i = 0; i < data.Length; i++)
			{
				if (data[i] == 1)
				{
					row += "O";
				}
				else
				{
					row += " ";
				}
				if (row.Length % 5 == 0 && row.Length != 0)
				{
					image[i / 5] = row;
					row = "";
				}
			}
			return image;
		}
	}

}