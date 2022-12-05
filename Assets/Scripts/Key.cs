using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Key : MonoBehaviour
{
    public static event Action<KeyCode> OnKeyPress;
    
    public KeyCode keyCode;

    private Button btn;

    private void Start()
    {
        GuessBoard.OnGuessLetter += SetColor;
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => OnKeyPress?.Invoke(keyCode));
    }

    private void SetColor(char character, Color color)
    {
        KeyCode thisKeyCode = (KeyCode) Enum.Parse(typeof(KeyCode), char.ToUpper(character).ToString());
        if (thisKeyCode == keyCode)
            GetComponent<Image>().color = color;
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyCode))
            OnKeyPress?.Invoke(keyCode);
    }

    private void OnDestroy()
    {
        GuessBoard.OnGuessLetter -= SetColor;
    }
}