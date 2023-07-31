using System;
namespace ConcurSolutionz.Models.CustomException
{
	public class RecordConversionException : Exception
	{
		public RecordConversionException()
		{
		}
        public RecordConversionException(string message) : base(message)
        {
        }

        public RecordConversionException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}

