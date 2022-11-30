using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Key : MonoBehaviour
{
    public static event Action<KeyCode> OnKeyPress;
    
    public KeyCode keyCode;

    private Button btn;

    private void Update()
    {
        if (Input.GetKeyDown(keyCode))
            OnKeyPress?.Invoke(keyCode);
    }

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => OnKeyPress?.Invoke(keyCode));
    }
}