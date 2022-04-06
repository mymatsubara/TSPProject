using TMPro;
using UnityEngine;

public class NumberText : MonoBehaviour
{
    private TextMeshProUGUI _textPro;
    private TextMesh _text;

    private void Start()
    {
        _textPro = GetComponent<TextMeshProUGUI>();
        _text = GetComponent<TextMesh>();
    }

    public void UpdateText(float number)
    {
        UpdateText($"{number:0.00}");
    }

    public void UpdateText(int number)
    {
        UpdateText(number.ToString());
    }

    public void UpdateText(string text)
    {
        if (_textPro != null)
            _textPro.text = text;
        if (_text != null)
            _text.text = text;
    }
}
