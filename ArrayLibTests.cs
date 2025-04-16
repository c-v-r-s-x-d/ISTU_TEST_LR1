using System.Globalization;
using FluentAssertions;
using Xunit;

namespace ISTU_TEST_LR1;

public class ArrayLibTests
{
    private const int Coefficient = 52;
    
    [Fact]
    public void When_ArrayIsEmpty_Then_CorrectErrorReturned()
    {
        // arrange
        var array = Array.Empty<string>();
        
        // act
        var result = ArrayLib.Do(array);
        
        // assert
        result.Array.Should().BeEquivalentTo(Array.Empty<double>());
        result.MaxElement.Should().Be(double.NaN);
        result.Errors.Should().ContainSingle(x => string.Equals("Input array is empty", x));
    }

    [Fact]
    public void When_ArrayHasMoreThenAllowedElements_Then_CorrectErrorReturned()
    {
        // arrange
        var random = new Random();
        var array = new string[1025];

        for (var i = 0; i < array.Length; i++)
        {
            array[i] = random.NextDouble().ToString(CultureInfo.InvariantCulture);
        }
        
        // act
        var result = ArrayLib.Do(array);

        // assert
        result.Array.Should().BeEquivalentTo(Array.Empty<double>());
        result.MaxElement.Should().Be(double.NaN);
        result.Errors.Should().ContainSingle(x => string.Equals("Input array is too large", x));
    }

    [Theory]
    [InlineData("-1.7976931348623159E+308")]
    [InlineData("1.7976931348623159E+308")]
    public void When_ArrayElementsOutOfDoubleRange_Then_CorrectErrorReturned(string incorrectElement)
    {
        // arrange
        var array = new[] {"1", incorrectElement, "3"};

        // act
        var result = ArrayLib.Do(array);

        // assert
        result.Array.Should().BeEquivalentTo(Array.Empty<double>());
        result.MaxElement.Should().Be(double.NaN);
        result.Errors.Should().ContainSingle(x => string.Equals("The number is outside the range of double", x));
    }

    [Fact]
    public void When_ArrayHasEvenNumberOfElements_And_AllPairSumsEqualDoubleMin_Then_CorrectResultReturned()
    {
        // arrange
        const int testNumber = 4;
        
        var array = new string[GetArrayLength(testNumber) | 0];
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = (double.MinValue / 2).ToString(CultureInfo.InvariantCulture);
        }

        // act
        var result = ArrayLib.Do(array);

        // assert
        result.Array.Should().HaveCount((array.Length + 1) / 2);
        result.MaxElement.Should().Be(double.MinValue);
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void When_ArrayHasEvenNumberOfElements_And_AllPairSumsMoreThenMinDouble_Then_CorrectResultReturned()
    {
        // arrange
        const int testNumber = 5;

        var random = new Random();
        var array = new string[GetArrayLength(testNumber) | 0];

        for (var i = 0; i < array.Length; i++)
        {
            array[i] = random.NextDouble().ToString(CultureInfo.InvariantCulture);
        }

        // act
        var result = ArrayLib.Do(array);
        
        // assert
        result.Array.Should().HaveCount((array.Length + 1) / 2);
        result.MaxElement.Should().NotBeNaN();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void When_ArrayHasOddNumberOfElements_And_AllPairSumsMoreThenDoubleMin_Then_CorrectResultReturned()
    {
        // arrange
        const int testNumber = 6;

        var random = new Random();
        var array = new string[GetArrayLength(testNumber) | 1];

        for (var i = 0; i < array.Length; i++)
        {
            array[i] = random.NextDouble().ToString(CultureInfo.InvariantCulture);
        }

        // act
        var result = ArrayLib.Do(array);
        
        // assert
        result.Array.Should().HaveCount((array.Length + 1) / 2);
        result.MaxElement.Should().NotBeNaN();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void When_ArrayHasOddNumberOfElements_And_AllPairSumsEqualDoubleMin_Then_CorrectResultReturned()
    {
        // arrange
        const int testNumber = 7;
        var array = new string[GetArrayLength(testNumber) | 1];
        
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = (double.MinValue / 2).ToString(CultureInfo.InvariantCulture);
        }
        
        // act
        var result = ArrayLib.Do(array);
        
        // assert
        result.Array.Should().HaveCount((array.Length + 1) / 2);
        result.MaxElement.Should().Be(double.MinValue);
        result.Errors.Should().BeEmpty();
    }
    

    private static int GetArrayLength(int testNumber)
    {
        return 1 + (testNumber - 1) * Coefficient;
    }
}
