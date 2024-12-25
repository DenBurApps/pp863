using System;
using UnityEngine;

namespace RecordSystem
{
    [RequireComponent(typeof(ScreenVisabilityHandler))]
    public class RecordScreen : MonoBehaviour
    {
        [SerializeField] private RecordPlane[] _recordPlanes;

        private ScreenVisabilityHandler _screenVisabilityHandler;

        private void Awake()
        {
            _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
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
