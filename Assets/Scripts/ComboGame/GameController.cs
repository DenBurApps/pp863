using System.Collections.Generic;
using FlashElement;
using RecordSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ComboGame
{
    public class GameController : MonoBehaviour
    {
        private const int InitSequenceCount = 3;

        [SerializeField] private FlashSpawner _flashSpawner;
        [SerializeField] private TouchInputHandler _inputHandler;
        [SerializeField] private RulesScreen _rulesScreen;
        [SerializeField] private LoseScreen _loseScreen;
        [SerializeField] private PauseScreen _pauseScreen;
        [SerializeField] private Button _startGameButton, _stopButton, _rulesButton, _homeButton;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private GameType _gameType;
        [SerializeField] private GameObject _rememberPlane;

        private int _currentSequenceCount;
        private int _score;
        private List<Flash> _clickedFlashes = new List<Flash>();
        private List<Flash> _sequenceFlashes = new List<Flash>();
        private int _currentSequenceIndex;

        private void OnEnable()
        {
            _inputHandler.FlashClicked += ProcessFlashCatched;

            _rulesScreen.PlayClicked += ContinueGame;

            _pauseScreen.StartClicked += ContinueGame;

            _loseScreen.AgainClicked += ResetGame;

            _rulesButton.onClick.AddListener(OpenRules);
            _stopButton.onClick.AddListener(PauseGame);
            _homeButton.onClick.AddListener(QuitGame);
            _startGameButton.onClick.AddListener(StartNewGame);

            _flashSpawner.FullSequenceShowed += EnableStartGameButton;
        }

        private void OnDisable()
        {
            _inputHandler.FlashClicked -= ProcessFlashCatched;

            _rulesScreen.PlayClicked -= ContinueGame;

            _pauseScreen.StartClicked -= ContinueGame;

            _loseScreen.AgainClicked -= ResetGame;

            _rulesButton.onClick.RemoveListener(OpenRules);
            _stopButton.onClick.RemoveListener(PauseGame);
            _homeButton.onClick.RemoveListener(QuitGame);
            _startGameButton.onClick.RemoveListener(StartNewGame);

            _flashSpawner.FullSequenceShowed += EnableStartGameButton;
        }

        private void Start()
        {
            ResetGame();
        }

        private void ResetGame()
        {
            _score = 0;
            _currentSequenceCount = InitSequenceCount;
            ShowStartScreen();
        }

        private void ShowStartScreen()
        {
            _clickedFlashes.Clear();
            _sequenceFlashes.Clear();
            _currentSequenceIndex = 0;
            
            UpdateScoreText();
            _pauseScreen.Disable();
            _loseScreen.gameObject.SetActive(false);
            _rulesScreen.gameObject.SetActive(false);
            
            _flashSpawner.StartSequenceSpawn(_currentSequenceCount);
            _inputHandler.StopDetectingTouch();
            
            _startGameButton.interactable = false;
            _rememberPlane.SetActive(true);
            _stopButton.gameObject.SetActive(false);
            _rulesButton.gameObject.SetActive(false);
            _homeButton.gameObject.SetActive(true);
        }

        private void EnableStartGameButton()
        {
            _startGameButton.interactable = true;
        }

        private void StartNewGame()
        {
            _sequenceFlashes.AddRange(_flashSpawner.SequenceObjects);
            
            _rememberPlane.SetActive(false);
            _stopButton.gameObject.SetActive(true);
            _rulesButton.gameObject.SetActive(true);
            _flashSpawner.StopSequenceSpawn();
            _flashSpawner.StartSpawn();
            _inputHandler.StartDetectingTouch();
        }

        private void PauseGame()
        {
            _pauseScreen.Enable();
            _stopButton.gameObject.SetActive(false);
            _inputHandler.StopDetectingTouch();
            _flashSpawner.StopSpawn();
            _flashSpawner.ReturnAllObjectsToPool();
        }

        private void ContinueGame()
        {
            _pauseScreen.Disable();
            _stopButton.gameObject.SetActive(true);
            _inputHandler.StartDetectingTouch();
            _flashSpawner.StartSpawn();
        }

        private void OpenRules()
        {
            _inputHandler.StopDetectingTouch();
            _flashSpawner.StopSpawn();
            _flashSpawner.ReturnAllObjectsToPool();
            _rulesScreen.gameObject.SetActive(true);
        }

        private void ProcessFlashCatched(Flash flash)
        {
            if (_sequenceFlashes == null || _sequenceFlashes.Count == 0)
            {
                ProcessMiss();
                return;
            }

            if (_currentSequenceIndex < 0 || _currentSequenceIndex >= _sequenceFlashes.Count)
            {
                ProcessMiss();
                return;
            }

            if (flash.Type != _sequenceFlashes[_currentSequenceIndex].Type)
            {
                ProcessMiss();
                return;
            }

            _currentSequenceIndex++;
            _flashSpawner.ReturnToPool(flash);

            if (_currentSequenceIndex == _currentSequenceCount)
            {
                ProcessGameWin();
            }
        }

        private void UpdateScoreText()
        {
            _scoreText.text = "Score: " + _score.ToString();
        }

        private void QuitGame()
        {
            SceneManager.LoadScene("MainScene");
        }

        private void ProcessGameWin()
        {
            _score++;
            UpdateScoreText();
            _currentSequenceCount++;
            _inputHandler.StopDetectingTouch();
            _flashSpawner.StopSpawn();
            _flashSpawner.ReturnAllObjectsToPool();
            ShowStartScreen();
            RecordHolder.AddData(_gameType, _score);
        }

        private void ProcessMiss()
        {
            _inputHandler.StopDetectingTouch();
            _flashSpawner.StopSpawn();
            _flashSpawner.ReturnAllObjectsToPool();
            _loseScreen.Enable(_score);
            _homeButton.gameObject.SetActive(false);
            _score = 0;
        }
    }
}