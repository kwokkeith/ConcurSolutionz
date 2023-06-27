
namespace ConcurSolutionz.Database
{
    public static class Utilities
    {
        /// <summary>Checks if an argument is null.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="arg">The argument to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when the argument is null.</exception>
        public static void CheckNull<T>(T arg){
            if (arg == null){
                throw new ArgumentNullException(nameof(arg));
            }
        }
    }
}