using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CalculatorScreen : MonoBehaviour
{
    [Header("Screen Text")]
    [SerializeField] private TextMeshProUGUI screenText;

    public void UpdateScreen(string value)
    {
        if (screenText == null)
        {
            Debug.Log("<color=red>Screen Text is not assigned</color>");
            return;
        }

        screenText.text = value ?? string.Empty;
    }
}
