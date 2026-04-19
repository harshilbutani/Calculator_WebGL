using UnityEngine;

public class Input : MonoBehaviour
{
    private void Update()
    {
        HandleDigitKeys();
        HandleOperatorKeys();
        HandleDecimalKeys();
        HandleEqualKeys();
        HandleClearKeys();
    }

    private static void HandleDigitKeys()
    {
        if (global::UnityEngine.Input.GetKeyDown(KeyCode.Alpha0) || global::UnityEngine.Input.GetKeyDown(KeyCode.Keypad0)) EmitDigit("0");
        if (global::UnityEngine.Input.GetKeyDown(KeyCode.Alpha1) || global::UnityEngine.Input.GetKeyDown(KeyCode.Keypad1)) EmitDigit("1");
        if (global::UnityEngine.Input.GetKeyDown(KeyCode.Alpha2) || global::UnityEngine.Input.GetKeyDown(KeyCode.Keypad2)) EmitDigit("2");
        if (global::UnityEngine.Input.GetKeyDown(KeyCode.Alpha3) || global::UnityEngine.Input.GetKeyDown(KeyCode.Keypad3)) EmitDigit("3");
        if (global::UnityEngine.Input.GetKeyDown(KeyCode.Alpha4) || global::UnityEngine.Input.GetKeyDown(KeyCode.Keypad4)) EmitDigit("4");
        if (global::UnityEngine.Input.GetKeyDown(KeyCode.Alpha5) || global::UnityEngine.Input.GetKeyDown(KeyCode.Keypad5)) EmitDigit("5");
        if (global::UnityEngine.Input.GetKeyDown(KeyCode.Alpha6) || global::UnityEngine.Input.GetKeyDown(KeyCode.Keypad6)) EmitDigit("6");
        if (global::UnityEngine.Input.GetKeyDown(KeyCode.Alpha7) || global::UnityEngine.Input.GetKeyDown(KeyCode.Keypad7)) EmitDigit("7");
        if (global::UnityEngine.Input.GetKeyDown(KeyCode.Alpha8) || global::UnityEngine.Input.GetKeyDown(KeyCode.Keypad8)) EmitDigit("8");
        if (global::UnityEngine.Input.GetKeyDown(KeyCode.Alpha9) || global::UnityEngine.Input.GetKeyDown(KeyCode.Keypad9)) EmitDigit("9");
    }

    private static void HandleOperatorKeys()
    {
        if (global::UnityEngine.Input.GetKeyDown(KeyCode.Minus) || global::UnityEngine.Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            EmitOperator("-");
        }

        if (global::UnityEngine.Input.GetKeyDown(KeyCode.Slash) || global::UnityEngine.Input.GetKeyDown(KeyCode.KeypadDivide))
        {
            EmitOperator("/");
        }

        if (global::UnityEngine.Input.GetKeyDown(KeyCode.Asterisk) || global::UnityEngine.Input.GetKeyDown(KeyCode.KeypadMultiply))
        {
            EmitOperator("*");
        }

        if (IsPlusPressed())
        {
            EmitOperator("+");
        }
    }

    private static void HandleDecimalKeys()
    {
        if (global::UnityEngine.Input.GetKeyDown(KeyCode.Period) || global::UnityEngine.Input.GetKeyDown(KeyCode.KeypadPeriod))
        {
            Cell.EmitCellPressed(".", Enums.CalculatorButtonType.DecimalPoint);
        }
    }

    private static void HandleEqualKeys()
    {
        if (global::UnityEngine.Input.GetKeyDown(KeyCode.Return)
            || global::UnityEngine.Input.GetKeyDown(KeyCode.KeypadEnter)
            || global::UnityEngine.Input.GetKeyDown(KeyCode.Equals))
        {
            Cell.EmitCellPressed("=", Enums.CalculatorButtonType.Equal);
        }
    }

    private static void HandleClearKeys()
    {
        if (global::UnityEngine.Input.GetKeyDown(KeyCode.Backspace)
            || global::UnityEngine.Input.GetKeyDown(KeyCode.Escape)
            || global::UnityEngine.Input.GetKeyDown(KeyCode.Delete)
            || global::UnityEngine.Input.GetKeyDown(KeyCode.C))
        {
            Cell.EmitCellPressed("AC", Enums.CalculatorButtonType.Ac);
        }
    }

    private static bool IsPlusPressed()
    {
        bool keypadPlus = global::UnityEngine.Input.GetKeyDown(KeyCode.KeypadPlus);
        bool mainPlus = global::UnityEngine.Input.GetKeyDown(KeyCode.Plus)
            || (global::UnityEngine.Input.GetKeyDown(KeyCode.Equals)
                && (global::UnityEngine.Input.GetKey(KeyCode.LeftShift)
                    || global::UnityEngine.Input.GetKey(KeyCode.RightShift)));

        return keypadPlus || mainPlus;
    }

    private static void EmitDigit(string digit)
    {
        Cell.EmitCellPressed(digit, Enums.CalculatorButtonType.Digit);
    }

    private static void EmitOperator(string op)
    {
        Cell.EmitCellPressed(op, Enums.CalculatorButtonType.Operator);
    }
}
