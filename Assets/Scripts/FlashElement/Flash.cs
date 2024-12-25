using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FlashElement
{
    [RequireComponent(typeof(Collider2D))]
    public class Flash : MonoBehaviour
    {
        [SerializeField] private int _disableInterval;
        [SerializeField] private Vector2[] _possibleScales;
        [SerializeField] private Color[] _possibleColors;

        [SerializeField] private SpriteRenderer _spriteRenderer;

        private CircleCollider2D _circleCollider2D;
        private Transform _transform;
        private IEnumerator _disablingCoroutine;

        public event Action<Flash> Disabled;

        private void Awake()
        {
            _transform = transform;
        }

        private void OnEnable()
        {
            _transform.localScale = _possibleScales[Random.Range(0, _possibleScales.Length)];
            _spriteRenderer.color = _possibleColors[Random.Range(0, _possibleColors.Length)];

            _disablingCoroutine = DisablingCoroutine();
            StartCoroutine(_disablingCoroutine);
        }

        private void OnDisable()
        {
            if (_disablingCoroutine != null)
            {
                StopCoroutine(_disablingCoroutine);
            }
        }

        private IEnumerator DisablingCoroutine()
        {
            yield return new WaitForSeconds(_disableInterval);

            Disabled?.Invoke(this);
        }
    }
}