using TMPro;
using UnityEngine;

public class DialogBubble : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private RectTransform _rectTransform;
    
    public void SetText(string text)
    {
        _text.text = text;
    }

    public void UpdateSize()
    {
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, GetPreferredHeight(_text.text));
    }

    public float GetPreferredHeight(string text)
    {
        return _text.GetPreferredValues(text, _rectTransform.rect.width, 0).y;
    }
}
