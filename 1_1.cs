using System;
namespace Calculatorr
{
    class Program
    {
        static double memory = 0;
        static readonly int MAX_INPUT_LENGTH = 15; 
        static readonly double MAX_NUMBER_VALUE = 1e14; 
        static readonly double MIN_NUMBER_VALUE = -1e14; 
        
        static void Main(string[] args)
        {
            Console.WriteLine("Advanced Calculator");
            Console.WriteLine("Operations: +, -, *, /, %, 1/x, x^2, sqrt, M+, M-, MR");
            Console.WriteLine("Memory commands: M+ (add to memory), M- (subtract from memory), MR (recall memory)");
            Console.WriteLine($"Note: Numbers are limited to {MAX_INPUT_LENGTH} characters and range [{MIN_NUMBER_VALUE:E1}, {MAX_NUMBER_VALUE:E1}]");
            
            while (true)
            {
                try
                {
                    Console.Write("\nEnter first number (or 'MR' to use memory): ");
                    string input1 = Console.ReadLine();
                    double num1;
                    
                    if (input1.ToUpper() == "MR")
                    {
                        num1 = memory;
                        Console.WriteLine($"Using memory value: {memory}");
                    }
                    else
                    {
                        if (!ValidateInput(input1, out num1))
                        {
                            continue;
                        }
                    }
                    
                    Console.Write("Enter operation (+, -, *, /, %, 1/x, x^2, sqrt, M+, M-, MR): ");
                    string operation = Console.ReadLine().ToLower();
                    
                    double result = 0;
                    bool needSecondNumber = true;
                    
                    switch (operation)
                    {
                        case "1/x":
                            if (num1 == 0)
                            {
                                Console.WriteLine("Error: division by zero!");
                                continue;
                            }
                            result = 1.0 / num1;
                            needSecondNumber = false;
                            break;
                        case "x^2":
                        case "x2":
                            result = num1 * num1;
                            needSecondNumber = false;
                            break;
                        case "sqrt":
                            if (num1 < 0)
                            {
                                Console.WriteLine("Error: cannot calculate square root of negative number!");
                                continue;
                            }
                            result = Math.Sqrt(num1);
                            needSecondNumber = false;
                            break;
                        case "m+":
                            memory += num1;
                            if (!IsNumberInRange(memory))
                            {
                                Console.WriteLine($"Warning: Memory value {memory:E2} exceeds safe range!");
                                Console.WriteLine("Memory has been reset to 0.");
                                memory = 0;
                            }
                            else
                            {
                                Console.WriteLine($"Added {num1} to memory. Memory = {memory}");
                            }
                            needSecondNumber = false;
                            continue;
                        case "m-":
                            memory -= num1;
                            if (!IsNumberInRange(memory))
                            {
                                Console.WriteLine($"Warning: Memory value {memory:E2} exceeds safe range!");
                                Console.WriteLine("Memory has been reset to 0.");
                                memory = 0;
                            }
                            else
                            {
                                Console.WriteLine($"Subtracted {num1} from memory. Memory = {memory}");
                            }
                            needSecondNumber = false;
                            continue;
                        case "mr":
                            Console.WriteLine($"Memory recall: {memory}");
                            needSecondNumber = false;
                            continue;
                    }
                    
                    double num2 = 0;
                    if (needSecondNumber)
                    {
                        Console.Write("Enter second number (or 'MR' to use memory): ");
                        string input2 = Console.ReadLine();
                        
                        if (input2.ToUpper() == "MR")
                        {
                            num2 = memory;
                            Console.WriteLine($"Using memory value: {memory}");
                        }
                        else
                        {
                            if (!ValidateInput(input2, out num2))
                            {
                                continue;
                            }
                        }
                        
                        switch (operation)
                        {
                            case "+":
                                result = num1 + num2;
                                break;
                            case "-":
                                result = num1 - num2;
                                break;
                            case "*":
                                result = num1 * num2;
                                break;
                            case "/":
                                if (num2 == 0)
                                {
                                    Console.WriteLine("Error: division by zero!");
                                    continue;
                                }
                                result = num1 / num2;
                                break;
                            case "%":
                                if (num2 == 0)
                                {
                                    Console.WriteLine("Error: division by zero in modulo operation!");
                                    continue;
                                }
                                result = num1 % num2;
                                break;
                            default:
                                Console.WriteLine("Invalid operation!");
                                continue;
                        }
                    }
                    

                    if (!IsNumberInRange(result))
                    {
                        Console.WriteLine($"Error: Result {result:E2} exceeds safe calculation range!");
                        Console.WriteLine("Operation cancelled.");
                        continue;
                    }
                    
                    if (needSecondNumber)
                        Console.WriteLine($"Result: {num1} {operation} {num2} = {result}");
                    else
                        Console.WriteLine($"Result: {operation}({num1}) = {result}");
                    
                    Console.Write("\nDo you want to continue? (yes/no): ");
                    string answer = Console.ReadLine().ToLower();
                    if (answer == "no" || answer == "n")
                        break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Input error: {ex.Message}");
                    Console.WriteLine("Please try again.");
                }
            }
            
            Console.WriteLine("Goodbye!");
        }
        
        static bool ValidateInput(string input, out double number)
        {
            number = 0;
            

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Error: Empty input!");
                return false;
            }

            if (input.Trim().Length > MAX_INPUT_LENGTH)
            {
                Console.WriteLine($"Error: Input too long! Maximum {MAX_INPUT_LENGTH} characters allowed.");
                Console.WriteLine($"Your input: {input.Trim().Length} characters");
                return false;
            }

            if (!double.TryParse(input, out number))
            {
                Console.WriteLine("Error: Invalid number format!");
                return false;
            }

            if (!IsNumberInRange(number))
            {
                Console.WriteLine($"Error: Number {number:E2} is out of allowed range!");
                Console.WriteLine($"Allowed range: [{MIN_NUMBER_VALUE:E1}, {MAX_NUMBER_VALUE:E1}]");
                return false;
            }

            if (double.IsInfinity(number) || double.IsNaN(number))
            {
                Console.WriteLine("Error: Invalid number value (infinity or NaN)!");
                return false;
            }
            
            return true;
        }
        
        static bool IsNumberInRange(double number)
        {
            return number >= MIN_NUMBER_VALUE && number <= MAX_NUMBER_VALUE;
        }
    }
}