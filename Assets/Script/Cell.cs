using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Cell : MonoBehaviour
{

    [Header("Cell Type")]
    [SerializeField] private Enums.CalculatorButtonType buttonType;

    [Header("Cell Value")]
    [SerializeField] private string cellValue;

    [Header("Cell Color")]
    [SerializeField] private Color operatorCellColor;
    [SerializeField] private Color digitCellColor;

    [Header("Image")]
    [SerializeField] private Image cellImage;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI cellText;

    [Header("Button")]
    [SerializeField] private Button cellButton;
    
    public static event Action<string, Enums.CalculatorButtonType> OnCellPressed;

    private void OnEnable()
    {
        SetCellColor();
        SetCellText(cellValue);

        cellButton.onClick.AddListener(OnCellButtonPressed);

    }

    private void OnDisable()
    {
        cellButton.onClick.RemoveListener(OnCellButtonPressed);
    }
    

    public void SetCellText(string value)
    {
        if (cellText == null)
        {
            Debug.Log("<color=red>Cell Text is not assigned</color>");
            return;
        }

        cellText.text = value ?? string.Empty;
    }

    public void SetCellColor()
    {
        if (cellImage == null)
        {
            Debug.Log("<color=red>Cell Image is not assigned</color>");
            return;
        }

        if (buttonType == Enums.CalculatorButtonType.Operator || buttonType == Enums.CalculatorButtonType.Equal)
        {
            cellImage.color = operatorCellColor;
        }
        else
        {
            cellImage.color = digitCellColor;
        }
    }

    public void OnCellButtonPressed()
    {
        ReturnValue();
    }

    public void ReturnValue()
    {
        if (buttonType == Enums.CalculatorButtonType.None)
        {
            return;
        }

        string emittedValue = string.IsNullOrEmpty(cellValue)
            ? (cellText != null ? cellText.text : string.Empty)
            : cellValue;

        if (buttonType == Enums.CalculatorButtonType.Digit)
        {
            if (int.TryParse(emittedValue, out int parsedValue))
            {
                Debug.Log("Digit Value: " + parsedValue);
            }
            else
            {
                Debug.Log("Invalid digit value: " + emittedValue);
            }
        }
        else
        {
            Debug.Log("Operator Value: " + emittedValue);
        }

        OnCellPressed?.Invoke(emittedValue, buttonType);
    }

}
