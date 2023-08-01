namespace ConcurSolutionz.Models.CustomException
{
	public class SynchronisationException : Exception
	{
		public SynchronisationException()
		{
		}


        public SynchronisationException(string message) : base(message)
        {
        }


        public SynchronisationException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}

