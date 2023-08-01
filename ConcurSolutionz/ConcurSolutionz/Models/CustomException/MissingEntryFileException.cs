namespace ConcurSolutionz.Models.CustomException
{
	public class MissingEntryFileException : Exception
	{
		public MissingEntryFileException()
		{
		}


        public MissingEntryFileException(string message) : base(message)
        {
        }


        public MissingEntryFileException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}

