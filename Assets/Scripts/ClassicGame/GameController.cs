using System;
using FlashElement;
using RecordSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ClassicGame
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private FlashSpawner _flashSpawner;
        [SerializeField] private TouchInputHandler _touchInputHandler;
        [SerializeField] private PauseScreen _pauseScreen;
        [SerializeField] private RulesScreen _rulesScreen;
        [SerializeField] private LoseScreen _loseScreen;
        [SerializeField] private Button _stopButton, _rulesButton, _homeButton;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private GameType _gameType;

        private int _score;

        private void OnEnable()
        {
            _touchInputHandler.Missed += ProcessMiss;
            _touchInputHandler.FlashClicked += ProcessFlashCatched;
            
            _rulesScreen.PlayClicked += ContinueGame;
            
            _pauseScreen.StartClicked += ContinueGame;

            _loseScreen.AgainClicked += StartNewGame;
            
            _rulesButton.onClick.AddListener(OpenRules);
            _stopButton.onClick.AddListener(PauseGame);
            _homeButton.onClick.AddListener(QuitGame);
        }

        private void OnDisable()
        {
            _touchInputHandler.Missed -= ProcessMiss;
            _touchInputHandler.FlashClicked -= ProcessFlashCatched;
            
            _rulesScreen.PlayClicked -= ContinueGame;
            
            _pauseScreen.StartClicked -= ContinueGame;

            _loseScreen.AgainClicked -= StartNewGame;
            
            _rulesButton.onClick.RemoveListener(OpenRules);
            _stopButton.onClick.RemoveListener(PauseGame);
            _homeButton.onClick.RemoveListener(QuitGame);
        }

        private void Start()
        {
            StartNewGame();
        }

        private void StartNewGame()
        {
            _score = 0;
            UpdateScoreText();
            _pauseScreen.Disable();
            _loseScreen.gameObject.SetActive(false);
            _rulesScreen.gameObject.SetActive(false);
            _homeButton.gameObject.SetActive(true);
            _flashSpawner.StartSpawn();
            _touchInputHandler.StartDetectingTouch();
        }

        private void PauseGame()
        {
            _pauseScreen.Enable();
            _stopButton.gameObject.SetActive(false);
            _touchInputHandler.StopDetectingTouch();
            _flashSpawner.StopSpawn();
            _flashSpawner.ReturnAllObjectsToPool();
        }

        private void ContinueGame()
        {
            _pauseScreen.Disable();
            _stopButton.gameObject.SetActive(true);
            _touchInputHandler.StartDetectingTouch();
            _flashSpawner.StartSpawn();
        }

        private void OpenRules()
        {
            _touchInputHandler.StopDetectingTouch();
            _flashSpawner.StopSpawn();
            _flashSpawner.ReturnAllObjectsToPool();
            _rulesScreen.gameObject.SetActive(true);
        }

        private void ProcessFlashCatched(Flash flash)
        {
            _flashSpawner.ReturnToPool(flash);
            _score++;
            UpdateScoreText();
        }

        private void ProcessMiss()
        {
            _touchInputHandler.StopDetectingTouch();
            _flashSpawner.StopSpawn();
            _flashSpawner.ReturnAllObjectsToPool();
            RecordHolder.AddData(_gameType, _score);
            _loseScreen.Enable(_score);
            _homeButton.gameObject.SetActive(false);
        }

        private void UpdateScoreText()
        {
            _scoreText.text = "Score: " + _score.ToString();
        }

        private void QuitGame()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
