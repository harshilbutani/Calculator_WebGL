using System;
using System.Collections.Generic;
using System.Globalization;

public static class CalculatorMath
{
    public static bool TryCalculate(string expression, out string resultText)
    {
        resultText = string.Empty;

        if (!TryCalculate(expression, out double result))
        {
            return false;
        }

        bool hasDecimalInput = NormalizeExpression(expression).Contains(".");
        resultText = FormatResult(result, hasDecimalInput);
        return true;
    }

    public static bool TryCalculate(string expression, out double result)
    {
        result = 0d;

        if (string.IsNullOrWhiteSpace(expression))
        {
            return false;
        }

        string normalized = NormalizeExpression(expression);

        if (string.IsNullOrWhiteSpace(normalized))
        {
            return false;
        }

        if (EndsWithOperatorOrDecimalPoint(normalized))
        {
            return false;
        }

        return TryEvaluateDMAS(normalized, out result);
    }

    private static string NormalizeExpression(string expression)
    {
        return expression
            .Replace("×", "*")
            .Replace("x", "*")
            .Replace("X", "*")
            .Replace("÷", "/")
            .Replace(" ", string.Empty);
    }

    private static bool TryEvaluateDMAS(string expression, out double result)
    {
        result = 0d;

        Stack<double> values = new Stack<double>();
        Stack<char> operators = new Stack<char>();

        for (int i = 0; i < expression.Length; i++)
        {
            char current = expression[i];

            if (char.IsDigit(current) || current == '.' || IsUnaryMinus(expression, i))
            {
                if (!TryReadNumber(expression, ref i, out double parsedValue))
                {
                    return false;
                }

                values.Push(parsedValue);
                continue;
            }

            if (!IsOperator(current))
            {
                return false;
            }

            while (operators.Count > 0 && HasHigherOrEqualPrecedence(operators.Peek(), current))
            {
                if (!TryApplyTopOperator(values, operators))
                {
                    return false;
                }
            }

            operators.Push(current);
        }

        while (operators.Count > 0)
        {
            if (!TryApplyTopOperator(values, operators))
            {
                return false;
            }
        }

        if (values.Count != 1)
        {
            return false;
        }

        result = values.Pop();
        return true;
    }

    private static bool TryReadNumber(string expression, ref int index, out double value)
    {
        value = 0d;

        int startIndex = index;

        if (expression[index] == '-')
        {
            index++;
        }

        bool hasDigit = false;
        bool hasDecimalPoint = false;

        while (index < expression.Length)
        {
            char ch = expression[index];

            if (char.IsDigit(ch))
            {
                hasDigit = true;
                index++;
                continue;
            }

            if (ch == '.')
            {
                if (hasDecimalPoint)
                {
                    return false;
                }

                hasDecimalPoint = true;
                index++;
                continue;
            }

            break;
        }

        if (!hasDigit)
        {
            return false;
        }

        string numberToken = expression.Substring(startIndex, index - startIndex);
        index--;

        return double.TryParse(numberToken, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
    }

    private static bool TryApplyTopOperator(Stack<double> values, Stack<char> operators)
    {
        if (values.Count < 2 || operators.Count == 0)
        {
            return false;
        }

        double right = values.Pop();
        double left = values.Pop();
        char op = operators.Pop();

        if (!TryApplyOperation(left, right, op, out double operationResult))
        {
            return false;
        }

        values.Push(operationResult);
        return true;
    }

    private static bool TryApplyOperation(double left, double right, char operation, out double result)
    {
        result = 0d;

        switch (operation)
        {
            case '+':
                result = left + right;
                return true;
            case '-':
                result = left - right;
                return true;
            case '*':
                result = left * right;
                return true;
            case '/':
                if (Math.Abs(right) < 0.0000000001d)
                {
                    return false;
                }

                result = left / right;
                return true;
            default:
                return false;
        }
    }

    private static bool IsUnaryMinus(string expression, int index)
    {
        if (expression[index] != '-')
        {
            return false;
        }

        if (index == 0)
        {
            return true;
        }

        char previous = expression[index - 1];
        return IsOperator(previous);
    }

    private static bool IsOperator(char token)
    {
        return token == '+' || token == '-' || token == '*' || token == '/';
    }

    private static bool EndsWithOperatorOrDecimalPoint(string expression)
    {
        if (string.IsNullOrEmpty(expression))
        {
            return true;
        }

        char lastCharacter = expression[expression.Length - 1];
        return IsOperator(lastCharacter) || lastCharacter == '.';
    }

    private static bool HasHigherOrEqualPrecedence(char existing, char incoming)
    {
        return GetPrecedence(existing) >= GetPrecedence(incoming);
    }

    private static int GetPrecedence(char operation)
    {
        if (operation == '+' || operation == '-')
        {
            return 1;
        }

        if (operation == '*' || operation == '/')
        {
            return 2;
        }

        return 0;
    }

    private static string FormatResult(double value, bool preserveDecimalPlace)
    {
        double rounded = Math.Round(value);

        if (Math.Abs(value - rounded) < 0.0000000001d)
        {
            if (preserveDecimalPlace)
            {
                return rounded.ToString("0.0", CultureInfo.InvariantCulture);
            }

            return rounded.ToString(CultureInfo.InvariantCulture);
        }

        return value.ToString("G15", CultureInfo.InvariantCulture);
    }
}