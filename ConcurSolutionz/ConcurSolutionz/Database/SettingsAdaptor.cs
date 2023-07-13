using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcurSolutionz.Database
{
    public static class SettingsAdaptor
    {
        public static dynamic ConvertSettings( Settings settings) {
            switch (settings.SubType){
                case "Concur": 
                    return (Concur) settings;
                default:
                    throw new ArgumentException("Invalid Settings subtype detected, could not convert using SettingsSocket!");
            }
        }
    }
}