using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectChanger : MonoBehaviour
{
    [SerializeField] private ScrollRect[] _scrollsRect;

    private void OnEnable()
    {
        Debug.Log("СКРОЛЛИМСЯЙ!");

        if (_scrollsRect.Length > 0)
            foreach (var scrollRect in _scrollsRect)
                scrollRect.normalizedPosition = new Vector2(0, 1);
    }

    private IEnumerator Start()
    {
        yield return null;
        Debug.Log("СКРОЛЛИМСЯЙ!111111111111111111111");

        if (_scrollsRect.Length > 0)
            foreach (var scrollRect in _scrollsRect)
                scrollRect.normalizedPosition = new Vector2(0, 1);
    }

    /*private void OnRectTransformDimensionsChange()
    {
        if (_scrollRect != null)
            _scrollRect.normalizedPosition = new Vector2(0, 1);
    }*/
}