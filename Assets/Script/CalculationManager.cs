using UnityEngine;

public class CalculationManager : MonoBehaviour
{
    [Header("Screen")]
    [SerializeField] private CalculatorScreen calculatorScreen;

    private string currentExpression = "0";
    private bool hasEvaluatedResult;
    
    private void OnEnable()
    {
        Cell.OnCellPressed += HandleCellPressed;
        RefreshDisplay();
    }

    private void OnDisable()
    {
        Cell.OnCellPressed -= HandleCellPressed;
    }

    private void HandleCellPressed(string value, Enums.CalculatorButtonType buttonType)
    {
        if (buttonType == Enums.CalculatorButtonType.None)
        {
            return;
        }

        if (buttonType == Enums.CalculatorButtonType.Ac)
        {
            currentExpression = "0";
            hasEvaluatedResult = false;
            RefreshDisplay();
            return;
        }

        if (buttonType == Enums.CalculatorButtonType.Equal)
        {
            if (string.IsNullOrWhiteSpace(currentExpression) || EndsWithOperatorOrDecimalPoint(currentExpression))
            {
                Debug.Log("<color=yellow>Expression is incomplete.</color>");
                RefreshDisplay();
                return;
            }

            if (CalculatorMath.TryCalculate(currentExpression, out string resultText))
            {
                currentExpression = resultText;
                hasEvaluatedResult = true;
            }
            else
            {
                Debug.Log("<color=yellow>Invalid expression for calculation.</color>");
            }

            RefreshDisplay();
            return;
        }

        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        if (buttonType == Enums.CalculatorButtonType.Operator)
        {
            if (EndsWithOperator(currentExpression))
            {
                Debug.Log("<color=yellow>Consecutive operators are not allowed.</color>");
                RefreshDisplay();
                return;
            }

            if (currentExpression == "0")
            {
                Debug.Log("<color=yellow>Enter a number first.</color>");
                RefreshDisplay();
                return;
            }
        }

        if (hasEvaluatedResult)
        {
            if (buttonType == Enums.CalculatorButtonType.Digit)
            {
                currentExpression = value;
                hasEvaluatedResult = false;
                RefreshDisplay();
                return;
            }

            if (buttonType == Enums.CalculatorButtonType.DecimalPoint)
            {
                currentExpression = "0.";
                hasEvaluatedResult = false;
                RefreshDisplay();
                return;
            }

            hasEvaluatedResult = false;
        }

        if (currentExpression == "0")
        {
            if (buttonType == Enums.CalculatorButtonType.Digit)
            {
                currentExpression = value;
                RefreshDisplay();
                return;
            }

            if (buttonType == Enums.CalculatorButtonType.DecimalPoint)
            {
                currentExpression = "0.";
                RefreshDisplay();
                return;
            }
        }

        currentExpression += value;
        hasEvaluatedResult = false;
        RefreshDisplay();
    }

    private void RefreshDisplay()
    {
        if (calculatorScreen == null)
        {
            Debug.Log("<color=red>CalculatorScreen reference is missing.</color>");
            return;
        }

        calculatorScreen.UpdateScreen(GetDisplayText(currentExpression));
    }

    private static string GetDisplayText(string expression)
    {
        if (string.IsNullOrEmpty(expression))
        {
            return "0";
        }

        if (EndsWithOperator(expression) && expression.Length > 1)
        {
            return expression.Substring(0, expression.Length - 1);
        }

        return expression;
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

    private static bool EndsWithOperator(string expression)
    {
        if (string.IsNullOrEmpty(expression))
        {
            return false;
        }

        char lastCharacter = expression[expression.Length - 1];
        return IsOperator(lastCharacter);
    }

    private static bool IsOperator(char value)
    {
        return value == '+' || value == '-' || value == '−' || value == '*' || value == '/' || value == 'x' || value == 'X' || value == '×' || value == '÷';
    }
}
