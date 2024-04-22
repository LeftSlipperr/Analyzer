using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SyntaxAnalyzer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "C:\\Users\\Laper\\OneDrive\\Desktop\\text.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string input = reader.ReadToEnd();
                Console.WriteLine(input);
                AnalyzeArithmeticExpressions(input);
            }

            Console.ReadKey();
        }

        static void AnalyzeArithmeticExpressions(string input)
        {
            string[] expressions = input.Split(';');

            foreach (var expression in expressions)
            {
                if (!string.IsNullOrWhiteSpace(expression))
                {
                    AnalyzeArithmeticExpressionSyntax(expression.Trim());
                    Console.WriteLine();
                }
            }
        }

        static void AnalyzeArithmeticExpressionSyntax(string expression)
        {
            Console.WriteLine(expression);

            string arithmeticPattern = @"^([a-z][a-zA-Z0-9]*)\s*:=\s*(([a-z][a-zA-Z0-9]*)|[IVXLCDM]+)\s*([+\-*/]\s*(([a-z][a-zA-Z0-9]*)|[IVXLCDM]+)\s*)*$";
            if (!Regex.IsMatch(expression, arithmeticPattern))
            {
                Console.WriteLine("Ошибка в структуре арифметического выражения:");
                AnalyzeArithmeticExpressionParts(expression);
                return;
            }

            Console.WriteLine("Синтаксис корректен.");
            OutputLexemeTable(expression);
        }

        static void AnalyzeArithmeticExpressionParts(string expression)
        {
            string[] parts = expression.Split(new[] { ":=", "\\+", "\\-", "\\*", "\\/" }, StringSplitOptions.RemoveEmptyEntries);



            // Проверка первой части (идентификатор присваивания)
            string identifierPattern = @"^\s*([a-z][a-zA-Z0-9]*)\s*$";
            if (!Regex.IsMatch(parts[0], identifierPattern))
            {
                Console.WriteLine("Неправильный идентификатор присваивания.");
                return;
            }

            // Проверка второй части (значение или переменная)
            string valueOrVariablePattern = @"^\s*(([a-z][a-zA-Z0-9]*)|[IVXLCDM]+)\s*$";
            if (!Regex.IsMatch(parts[1], valueOrVariablePattern))
            {
                Console.WriteLine("Неправильное значение или переменная после присваивания.");
                return;
            }

            // Проверка третьей части (арифметическое выражение)
            string arithmeticExpressionPattern = @"^\s*([+\-*/]\s*(([a-z][a-zA-Z0-9]*)|[IVXLCDM]+)\s*)*$";
            if (!Regex.IsMatch(parts[2], arithmeticExpressionPattern))
            {
                Console.WriteLine("Неправильное арифметическое выражение после присваивания.");
                return;
            }
        }

        static void OutputLexemeTable(string expression)
        {
            Console.WriteLine("\nТаблица лексем:");
            Console.WriteLine("Символ \t\t Тип");

            string[] operators = { ":=" };
            string[] ariphmetic = { "+", "-", "*", "/" };
            string[] separators = { "(", ")" };

            MatchCollection matches = Regex.Matches(expression, @"[+\-*/()]|\b\w+\b|:=|<|>|=");

            foreach (Match match in matches)
            {
                string token = match.Value;
                string tokenType;
                string tokenValue;

                if (operators.Contains(token))
                {
                    tokenType = "Оператор присваивания";
                    tokenValue = token;
                }
                else if (ariphmetic.Contains(token))
                {
                    tokenType = "Математический оператор";
                    tokenValue = token;
                }
                else if (separators.Contains(token))
                {
                    tokenType = "Разделитель";
                    tokenValue = token;
                }
                else if (Regex.IsMatch(token, @"^[a-z][a-zA-Z0-9]*$"))
                {
                    tokenType = "Идентификатор";
                    tokenValue = token;
                }
                else if (IsRomanNumeral(token))
                {
                    tokenType = "Римское число";
                    tokenValue = token;
                }
                else
                {
                    tokenType = "Неизвестный";
                    tokenValue = token;
                }

                Console.WriteLine($"{tokenValue}\t\t{tokenType}");
            }
        }

        static bool IsRomanNumeral(string number)
        {
            return Regex.IsMatch(number, @"^(?=[MDCLXVI])M{0,4}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})$", RegexOptions.IgnoreCase);
        }
    }
}