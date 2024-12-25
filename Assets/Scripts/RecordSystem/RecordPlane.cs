using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RecordSystem
{
    public class RecordPlane : MonoBehaviour
    {
        [SerializeField] private GameType _gameType;
        [SerializeField] private TMP_Text[] _placesTexts;
        [SerializeField] private GameObject _noDataPlane;
        [SerializeField] private Button _playButton;
        [SerializeField] private string _sceneName;

        private void OnEnable()
        {
            _playButton.onClick.AddListener(OpenGame);
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(OpenGame);
        }

        public void SetPlacesData()
        {
            var records = RecordHolder.GetScores(_gameType);

            if (records.Count <= 0)
            {
                _noDataPlane.SetActive(true);
                return;
            }

            _noDataPlane.SetActive(false);
            var bestScores = records.OrderByDescending(x => x).Take(3).ToList();
            UpdateTextArray(bestScores);

        }
        
        private void UpdateTextArray(List<int> values)
        {
            for (int i = 0; i < _placesTexts.Length; i++)
            {
                _placesTexts[i].text = i < values.Count ? values[i].ToString() : "-";
            }
        }
        
        private void OpenGame()
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}
