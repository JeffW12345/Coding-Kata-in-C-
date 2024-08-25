using System.Text.RegularExpressions;

int Add(string numbers)
{
    if (numbers.Length == 0)
    {
        return 0;
    }

    string[] delimiters = new string[] { ",", "\n" };
    AdditionalDelimiterActions(ref numbers, ref delimiters);

    List<int> arrayOfNumbers = GetNumbersArray(numbers, delimiters);
    HandleNegativeNumbers(arrayOfNumbers);

    int sumOfNumbers = arrayOfNumbers.Where(number => number < 1001).Sum();
    return sumOfNumbers;
}

void HandleNegativeNumbers(List<int> arrayOfNumbers)
{
    List<int> negativeNumbers = arrayOfNumbers.Where(number => number < 0).ToList();  

    if (negativeNumbers.Count > 0)
    {
        string message = "Negatives not allowed: ";
        foreach (int number in negativeNumbers) 
        { 
            message += number + " ";
        }
        throw new Exception(message);
    }
}

List<int> GetNumbersArray(string numbers, string[] delimiters)
{
    List<string> arrayOfNumbersAsStrings = numbers.Split(delimiters, StringSplitOptions.None).OfType<string>().ToList();
    List<int> arrayOfNumbersAsIntegers = new List<int>();
    foreach (var numberAsString in arrayOfNumbersAsStrings)
    {
        arrayOfNumbersAsIntegers.Add(Int32.Parse(numberAsString));
    }
    return arrayOfNumbersAsIntegers;
}

void AdditionalDelimiterActions(ref string numbers, ref string[] delimiters)
{
    string regexPatternForDelimiterWithSquareBrackets = @"(\A//\[)(.+)(\]\\n)";
    Regex regex = new Regex(regexPatternForDelimiterWithSquareBrackets);
    if (regex.IsMatch(numbers))
    {
        var test = regex.Match(numbers);
        delimiters = new string[] { ",", "\n", regex.Match(numbers).Groups[2].Value };
        numbers = numbers.Replace(regex.Match(numbers).Groups[0].Value, "");
        return;
    }

    string regexPatternForDelimiterWithoutSquareBrackets = @"(\A//)(.+)(\\n)";
    regex = new Regex(regexPatternForDelimiterWithoutSquareBrackets);
    if (regex.IsMatch(numbers))
    {
        delimiters = new string[] { ",", "\n", regex.Match(numbers).Groups[2].Value };
        numbers = numbers.Replace(regex.Match(numbers).Groups[0].Value, "");
    }
}

Console.WriteLine(Add(""));
Console.WriteLine(Add("1"));
Console.WriteLine(Add("1,2"));
Console.WriteLine(Add("1,2,3,4")); // To test > 2 numbers in the String. Should be 10.
Console.WriteLine(Add("1\n2,3")); // Should be 6
Console.WriteLine(Add(@"//;\n1;2")); // Should be 3. Delimiter is semicolon.

try
{
    Console.WriteLine(Add("1,-3,1,-1,1")); // Negative number throws exception
}
catch (Exception ex)
{
    Console.WriteLine($"Exception thrown as expected: {ex.Message}");
}

Console.WriteLine(Add("1001,2")); // Numbers over 1000 ignored. 
Console.WriteLine(Add(@"//[|||]\n1|||2|||3")); // Should be 6. Delimiter is |||
