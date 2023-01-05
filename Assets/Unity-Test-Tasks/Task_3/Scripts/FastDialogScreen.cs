using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FastDialogScreen : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private DialogBubble _bubbleTemplate;
    [SerializeField] private List<string> _texts;
    [SerializeField] private float _spacing;
    private List<DialogBubble> _bubblesPool;
    private List<float> _distanceList;
    private int _currentStartIndex = 0;
    private float _updateThreshold;
    private void Start()
    {
        for (int i = 0; i < 16; i++)
        {
            _texts.AddRange(_texts);
        }
        _bubblesPool = new List<DialogBubble>();
        _distanceList = new List<float>();
        _currentStartIndex = 0;
        var viewHeight = _scrollRect.viewport.rect.height;
        float heightSum = _spacing;
        int minBubblesCount = 2;
        _updateThreshold = 0;
        foreach (var bubbleHeight in _texts.Select(str => _bubbleTemplate.GetPreferredHeight(str)).OrderBy(val => val))
        {
            heightSum += bubbleHeight; 
            heightSum += _spacing;
            if (viewHeight >= heightSum)
            {
                minBubblesCount++;
            }
            if (_updateThreshold == 0)
            {
                _updateThreshold = bubbleHeight;
            }
        }

        minBubblesCount = Mathf.Min(_texts.Count, minBubblesCount);
        
        var content = _scrollRect.content;
        content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, heightSum);
        float offset = _spacing;
        for (int i = 0; i < _texts.Count; i++)
        {
            var text = _texts[i];
            
            if (i < minBubblesCount)
            {
                var item = Instantiate(_bubbleTemplate, content);
                item.SetText(text);
                item.UpdateSize();
                item.transform.localPosition = Vector3.down * offset;
                _bubblesPool.Add(item);
            }
            _distanceList.Add(offset);
            offset += _spacing + _bubbleTemplate.GetPreferredHeight(text);
        }
        
        _scrollRect.normalizedPosition = Vector2.up;
        _scrollRect.onValueChanged.AddListener(OnScrollChanged);
    }

    private void UpdateBubbles()
    {
        for (int i = 0; i < _bubblesPool.Count; i++)
        {
            if (_currentStartIndex + i < _texts.Count)
            {
                var text = _texts[_currentStartIndex + i];
                var item = _bubblesPool[i];
                item.SetText(text);
                item.UpdateSize();
                var itemTransform = item.transform;
                var pos = itemTransform.localPosition;
                pos.y = -_distanceList[_currentStartIndex + i];
                itemTransform.localPosition = pos;
            }
        }
    }

    private void UpdateIndex(float sign)
    {
        var offsetY = _scrollRect.content.localPosition.y;
        if (sign < 0)
        {
            for (int i = _currentStartIndex; i < _distanceList.Count - 1; i++)
            {
                var dist = _distanceList[i + 1];
                if (offsetY - dist <= 0)
                {
                    _currentStartIndex = i;
                    break;
                }
            }
        }
        else
        {
            if (_currentStartIndex > 0)
            {
                for (int i = _currentStartIndex; i > 0; i--)
                {
                    var dist = _distanceList[i - 1];
                    if (offsetY - dist >= 0)
                    {
                        _currentStartIndex = i - 1;
                        break;
                    }
                }
            }
        }
    }

    private void OnScrollChanged(Vector2 normalizedPos)
    {
        var offsetY = _scrollRect.content.localPosition.y; 
        var delta =_distanceList[_currentStartIndex] - Mathf.Abs(offsetY);
        if (_updateThreshold - Mathf.Abs(delta) < 0)
        {
            UpdateIndex(Mathf.Sign(delta));
            UpdateBubbles();
        }
    }
}
