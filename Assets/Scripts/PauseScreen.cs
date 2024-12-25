using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private Button _startButton;

    public event Action StartClicked;

    private void OnEnable()
    {
        _startButton.onClick.AddListener(OnStartClicked);
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(OnStartClicked);;
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
    
    private void OnStartClicked() => StartClicked?.Invoke();
}
