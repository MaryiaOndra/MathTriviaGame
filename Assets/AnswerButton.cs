using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AnswerButton : MonoBehaviour {
    private Button _button;
    private Image _buttonImage;
    private TMP_Text _text;
    private bool _isMatched;
    private Color _defaultColor;

    public event Action<bool, AnswerButton> OnAnswer;

    private void Awake() {
        _buttonImage = GetComponent<Image>();
        _text = GetComponentInChildren<TMP_Text>();
        _defaultColor = _buttonImage.color;
        _button = GetComponent<Button>();

        _button.onClick.AddListener(OnClick);
    }

    public void InitButton(string text, bool isMatched) {
        _text.text = text;
        _isMatched = isMatched;
    }

    void OnClick() {
        OnAnswer?.Invoke(_isMatched, this);
    }

    public void SetColor(Color color) {
        _buttonImage.color = color;
    }

    public void ReturnColor() {
        _buttonImage.color = _defaultColor;
    }
}
