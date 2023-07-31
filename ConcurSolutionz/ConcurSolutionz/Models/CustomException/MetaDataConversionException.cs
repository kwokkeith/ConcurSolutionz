using System;
namespace ConcurSolutionz.Models.CustomException
{
	public class MetaDataConversionException : Exception
	{
		public MetaDataConversionException()
		{
		}
        public MetaDataConversionException(string message) : base(message)
        {
        }

        public MetaDataConversionException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}

