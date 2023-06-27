using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcurSolutionz.Database
{
    public static class Utilities
    {
        /// <summary>Checks if an argument is null.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="arg">The argument to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when the argument is null.</exception>
        public static void checkNull<T>(T arg){
            if (arg == null){
                throw new ArgumentNullException(Message="This " + nameof(arg) + "is null!");
            }
        }
    }
}