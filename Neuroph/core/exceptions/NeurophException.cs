using System;

namespace org.neuroph.core.exceptions
{

	/// <summary>
	/// Base exception type for Neuroph.
	/// @author jheaton
	/// </summary>
	public class NeurophException : java.lang.Exception
	{
		/// <summary>
		/// The version ID.
		/// </summary>
		private const long serialVersionUID = 0L;


		/// <summary>
		/// Default constructor.
		/// </summary>
		public NeurophException()
		{

		}

		/// <summary>
		/// Construct a message exception.
		/// </summary>
		/// <param name="msg">
		///            The exception message. </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public NeurophException(final String msg)
		public NeurophException(string msg) : base(msg)
		{
		}

		/// <summary>
		/// Construct an exception that holds another exception.
		/// </summary>
		/// <param name="t">
		///            The other exception. </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public NeurophException(final Throwable t)
		public NeurophException(Exception t) : base(t)
		{
		}

		/// <summary>
		/// Construct an exception that holds another exception.
		/// </summary>
		/// <param name="msg">
		///            A message. </param>
		/// <param name="t">
		///            The other exception. </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public NeurophException(final String msg, final Throwable t)
		public NeurophException(string msg, Exception t) : base(msg, t)
		{
		}
	}

}