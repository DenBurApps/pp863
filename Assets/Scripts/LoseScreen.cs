using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseScreen : MonoBehaviour
{
    [SerializeField] private Button _againButton;
    [SerializeField] private Button _homeButton;
    [SerializeField] private TMP_Text _score;
    
    public event Action AgainClicked;
    
    private void OnEnable()
    {
        _againButton.onClick.AddListener(OnAgainClicked);
        _homeButton.onClick.AddListener(OnHomeClicked);
    }

    private void OnDisable()
    {
        _againButton.onClick.RemoveListener(OnAgainClicked);
        _homeButton.onClick.RemoveListener(OnHomeClicked);
    }

    public void Enable(int score)
    {
        gameObject.SetActive(true);
        _score.text = score.ToString();
    }

    private void OnHomeClicked() => SceneManager.LoadScene("MainScene");
    private void OnAgainClicked() => AgainClicked?.Invoke();
}
