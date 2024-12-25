using System;
using UnityEngine;
using UnityEngine.UI;

namespace MainScreen
{
    [RequireComponent(typeof(ScreenVisabilityHandler))]
    public class MainScreen : MonoBehaviour
    {
        [SerializeField] private Button _classic, _speed, _combo, _chaos;
        
        private ScreenVisabilityHandler _screenVisabilityHandler;
        
        private void Awake()
        {
            _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        private void Start()
        {
            EnableScreen();
        }

        public void EnableScreen()
        {
            _screenVisabilityHandler.EnableScreen();
        }

        public void DisableScreen()
        {
            _screenVisabilityHandler.DisableScreen();
        }
    }
}
