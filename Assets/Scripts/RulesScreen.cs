using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RulesScreen : MonoBehaviour
{
    [SerializeField] private Button _playButton;

    public event Action PlayClicked;

    private void OnEnable()
    {
        _playButton.onClick.AddListener(OnPlayClicked);
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(OnPlayClicked);
    }

    private void OnPlayClicked()
    {
        PlayClicked?.Invoke();
        gameObject.SetActive(false);
    }
}