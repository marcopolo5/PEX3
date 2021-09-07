using System;

namespace Domain.Exceptions
{
	public class SlowDatabaseConnectionException : Exception
	{
		public SlowDatabaseConnectionException()
		{
		}

		public SlowDatabaseConnectionException(string message) : base(message)
		{
		}

		public SlowDatabaseConnectionException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}