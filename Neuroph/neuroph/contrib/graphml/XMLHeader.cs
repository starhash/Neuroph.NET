namespace org.neuroph.contrib.graphml
{

	/// <summary>
	/// XML header, specifying the XML version and character encoding standard. 
	/// 
	/// Here XML version is set to 1.0 and encoding to UTF-8. 
	/// 
	/// @author fernando carrillo (fernando@carrillo.at)
	/// 
	/// </summary>
	public class XMLHeader
	{
		public static string Header
		{
			get
			{
				return getHeader("1.0", "UTF-8");
			}
		}

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public static String getHeader(final String version, final String encoding)
		public static string getHeader(string version, string encoding)
		{
			string @out = "<?xml";
			@out += " " + new XMLAttribute("version", version);
			@out += " " + new XMLAttribute("encoding", encoding);
			@out += " ?>";
			return @out;
		}
	}

}