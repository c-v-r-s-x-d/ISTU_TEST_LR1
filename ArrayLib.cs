using System.Globalization;

namespace ISTU_TEST_LR1;

public static class ArrayLib
{
    public static Result Do(string[] array)
    {
        var errors = new List<string>();

        var n = array.Length;

        if (n == 0)
        {
            errors.Add("Input array is empty");
            return new Result
            {
                Array = Array.Empty<double>(),
                MaxElement = double.NaN,
                Errors = errors
            };
        }

        if (n > 1024)
        {
            errors.Add("Input array is too large");
            return new Result
            {
                Array = Array.Empty<double>(),
                MaxElement = double.NaN,
                Errors = errors
            };
        }

        double[] parsedArray;
        try
        {
            parsedArray = array.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToArray();
            if (parsedArray.Any(x => x is double.NegativeInfinity or double.PositiveInfinity))
            {
                throw new FormatException();
            }
        }
        catch (FormatException)
        {
            errors.Add("The number is outside the range of double");
            return new Result
            {
                Array = Array.Empty<double>(),
                MaxElement = double.NaN,
                Errors = errors
            };
        }

        var maxSum = double.MinValue;
        var half = (n + 1) / 2;

        var resultArray = new double[half];
        for (var i = 0; i < half; i++)
        {
            var sum = parsedArray[i] + parsedArray[n - 1 - i];
            if (sum is double.NegativeInfinity or double.PositiveInfinity)
            {
                errors.Add("The number is outside the range of double");
                return new Result
                {
                    Array = Array.Empty<double>(),
                    MaxElement = double.NaN,
                    Errors = errors
                };
            }
            resultArray[i] = sum;
            if (sum > maxSum)
                maxSum = sum;
        }

        if (n % 2 == 1)
        {
            resultArray[half-1] *= 2;
            if (resultArray[half-1] > maxSum)
                maxSum = resultArray[half-1];
        }
        
        return new Result
        {
            Array = resultArray,
            MaxElement = maxSum,
            Errors = errors
        };
    }

    public sealed class Result
    {
        public required IReadOnlyCollection<double> Array { get; set; }
        
        public required double MaxElement { get; set; }
        
        public required IReadOnlyCollection<string> Errors { get; set; }
    }
}
