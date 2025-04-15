namespace ISTU_TEST_LR1;

public static class ArrayLib
{
    public static Result Do(double[]? array)
    {
        var errors = new List<string>();

        if (array is null)
        {
            errors.Add("Input array is null.");
            return new Result
            {
                Array = Array.Empty<double>(),
                MaxElement = double.NaN,
                Errors = errors
            };
        }

        var n = array.Length;

        if (n == 0)
        {
            errors.Add("Input array is empty.");
            return new Result
            {
                Array = Array.Empty<double>(),
                MaxElement = double.NaN,
                Errors = errors
            };
        }

        var resultArray = new double[n];
        array.CopyTo(resultArray, 0);

        var maxSum = double.MinValue;
        var half = n / 2;

        for (var i = 0; i < half; i++)
        {
            var sum = array[i] + array[n - 1 - i];
            if (sum > maxSum)
                maxSum = sum;
        }

        if (n % 2 == 1)
        {
            var middleIndex = n / 2;
            resultArray[middleIndex] = array[middleIndex] * 2;
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
