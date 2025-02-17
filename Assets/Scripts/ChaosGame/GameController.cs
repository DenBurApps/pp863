using FlashElement;
using RecordSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ChaosGame
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Color[] _possibleColors;
        [SerializeField] private FigureType[] _possibleTypes;
        
        [SerializeField] private FlashSpawner _flashSpawner;
        [SerializeField] private TouchInputHandler _inputHandler;
        [SerializeField] private RulesScreen _rulesScreen;
        [SerializeField] private LoseScreen _loseScreen;
        [SerializeField] private PauseScreen _pauseScreen;
        [SerializeField] private Button _stopButton, _rulesButton, _homeButton;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _typeText;
        [SerializeField] private GameType _gameType;
        
        private int _score;
        private FigureType _currentType;
        
        private void OnEnable()
        {
            _inputHandler.FlashClicked += ProcessFlashCatched;

            _rulesScreen.PlayClicked += ContinueGame;

            _pauseScreen.StartClicked += ContinueGame;

            _loseScreen.AgainClicked += StartNewGame;

            _rulesButton.onClick.AddListener(OpenRules);
            _stopButton.onClick.AddListener(PauseGame);
            _homeButton.onClick.AddListener(QuitGame);
        }

        private void OnDisable()
        {
            _inputHandler.FlashClicked -= ProcessFlashCatched;

            _rulesScreen.PlayClicked -= ContinueGame;

            _pauseScreen.StartClicked -= ContinueGame;

            _loseScreen.AgainClicked -= StartNewGame;

            _rulesButton.onClick.RemoveListener(OpenRules);
            _stopButton.onClick.RemoveListener(PauseGame);
            _homeButton.onClick.RemoveListener(QuitGame);
        }

        private void Start()
        {
            _typeText.enabled = false;
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
            _flashSpawner.StartSpawnMultiple();
            _inputHandler.StartDetectingTouch();
            ResetRound();
        }

        private void ResetRound()
        {
            _typeText.enabled = true;
            _currentType = _possibleTypes[Random.Range(0, _possibleTypes.Length)];
            _typeText.text = _currentType.ToString();
            _typeText.color = _possibleColors[Random.Range(0, _possibleColors.Length)];
        }
        
        private void PauseGame()
        {
            _typeText.enabled = false;
            _pauseScreen.Enable();
            _stopButton.gameObject.SetActive(false);
            _inputHandler.StopDetectingTouch();
            _flashSpawner.StopSpawnMultiple();
            _flashSpawner.ReturnAllObjectsToPool();
        }
        
        private void ContinueGame()
        {
            _typeText.enabled = true;
            _pauseScreen.Disable();
            _stopButton.gameObject.SetActive(true);
            _inputHandler.StartDetectingTouch();
            _flashSpawner.StartSpawnMultiple();
        }
        
        private void ProcessFlashCatched(Flash flash)
        {
            if (flash.Type == _currentType)
            {
                _score++;
                UpdateScoreText();
                ResetRound();
                RecordHolder.AddData(_gameType, _score);
            }
            else
            {
                ProcessMiss();
            }
            
            _flashSpawner.ReturnToPool(flash);
        }
        
        private void ProcessMiss()
        {
            _typeText.enabled = false;
            _inputHandler.StopDetectingTouch();
            _flashSpawner.StopSpawnMultiple();
            _flashSpawner.ReturnAllObjectsToPool();
            _loseScreen.Enable(_score);
            _homeButton.gameObject.SetActive(false);
        }
        
        private void OpenRules()
        {
            _typeText.enabled = false;
            _inputHandler.StopDetectingTouch();
            _flashSpawner.StopSpawnMultiple();
            _flashSpawner.ReturnAllObjectsToPool();
            _rulesScreen.gameObject.SetActive(true);
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
