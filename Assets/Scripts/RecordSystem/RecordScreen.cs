using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RecordSystem
{
    [RequireComponent(typeof(ScreenVisabilityHandler))]
    public class RecordScreen : MonoBehaviour
    {
        [SerializeField] private RecordPlane[] _recordPlanes;
        [SerializeField] private Button _classic, _speed, _combo, _chaos;

        private ScreenVisabilityHandler _screenVisabilityHandler;

        private void Awake()
        {
            _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        }
        
        private void OnEnable()
        {
            _classic.onClick.AddListener(() => SceneManager.LoadScene("ClassicGame"));
            _speed.onClick.AddListener(() => SceneManager.LoadScene("SpeedScene"));
            _combo.onClick.AddListener(() => SceneManager.LoadScene("ComboScene"));
            _chaos.onClick.AddListener(() => SceneManager.LoadScene("ChaosScene"));
        }

        private void OnDisable()
        {
            _classic.onClick.RemoveListener(() => SceneManager.LoadScene("ClassicGame"));
            _speed.onClick.RemoveListener(() => SceneManager.LoadScene("SpeedScene"));
            _combo.onClick.RemoveListener(() => SceneManager.LoadScene("ComboScene"));
            _chaos.onClick.RemoveListener(() => SceneManager.LoadScene("ChaosScene"));
        }

        private void Start()
        {
            DisableScreen();
        }

        public void EnableScreen()
        {
            _screenVisabilityHandler.EnableScreen();

            foreach (var plane in _recordPlanes)
            {
                plane.SetPlacesData();
            }
        }

        public void DisableScreen()
        {
            _screenVisabilityHandler.DisableScreen();
        }
    }
}
