using System;
using System.Text;

public class Fuzzer
{
    private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private const string Digits = "0123456789";

    public static string GenerateRandomString(int length, bool includeNumbers = false)
    {
        StringBuilder result = new StringBuilder(length);
        Random random = new Random();

        string validCharacters = Characters;
        if (includeNumbers)
        {
            validCharacters += Digits;
        }

        for (int i = 0; i < length; i++)
        {
            result.Append(validCharacters[random.Next(validCharacters.Length)]);
        }

        return result.ToString();
    }

    public static int GenerateRandomInteger(int numberOfDigits)
    {
        Random random = new Random();
        int min = (int)Math.Pow(10, numberOfDigits - 1);
        int max = (int)Math.Pow(10, numberOfDigits) - 1;
        return random.Next(min, max);
    }

    public static double GenerateRandomDouble(int numberOfDigits, int decimalPlaces)
    {
        Random random = new Random();

        double minValue = Math.Pow(10, numberOfDigits - 1);
        double maxValue = Math.Pow(10, numberOfDigits) - 1;
        double wholePart = random.NextDouble() * (maxValue - minValue) + minValue;

        double fractionPart = random.NextDouble();
        double randomDouble = wholePart + fractionPart;

        randomDouble = Math.Round(randomDouble, decimalPlaces);
        return randomDouble;
    }

    public static DateTime GenerateRandomDateTime()
    {
        Random random = new Random();
        DateTime start = new DateTime(2022, 1, 1);
        int range = (DateTime.Today - start).Days;
        DateTime randomDate = start.AddDays(random.Next(range));

        int hour = random.Next(24);
        int minute = random.Next(60);
        int second = random.Next(60);
        int millisecond = random.Next(1000);
        randomDate = randomDate.Add(new TimeSpan(hour, minute, second, millisecond * 10000));

        return randomDate;

    }
}

//class Program
//{
//    static void Main()
//    {
//        string randomString = Fuzzer.GenerateRandomString(10);
//        Console.WriteLine("Random String: " + randomString);

//        int randomNumber = Fuzzer.GenerateRandomInteger(6);
//        Console.WriteLine("Random Integer: " + randomNumber);

//        double randomDouble = Fuzzer.GenerateRandomDouble(3, 2);
//        Console.WriteLine("Random Double: " + randomDouble);

//        DateTime randomDateTime = Fuzzer.GenerateRandomDateTime();
//        Console.WriteLine("Random DateTime: " + randomDateTime);
//    }
//}