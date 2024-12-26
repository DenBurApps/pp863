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
        [SerializeField] private FigureType _type;

        [SerializeField] private SpriteRenderer _spriteRenderer;

        private CircleCollider2D _circleCollider2D;
        private Transform _transform;
        private IEnumerator _disablingCoroutine;

        public event Action<Flash> Disabled;
        public FigureType Type => _type;

        private void Awake()
        {
            _transform = transform;
        }

        private void OnEnable()
        {
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

        public void SetScaleAndType()
        {
            _transform.localScale = _possibleScales[Random.Range(0, _possibleScales.Length)];
            _spriteRenderer.color = _possibleColors[Random.Range(0, _possibleColors.Length)];

            AssignFigureType(_spriteRenderer.color);
        }

        private IEnumerator DisablingCoroutine()
        {
            yield return new WaitForSeconds(_disableInterval);

            Disabled?.Invoke(this);
        }

        private void AssignFigureType(Color color)
        {
            for (int i = 0; i < _possibleColors.Length; i++)
            {
                if (color == _possibleColors[i])
                {
                    _type = (FigureType)i;
                    return;
                }
            }

            _type = FigureType.Purple;
        }
    }

    public enum FigureType
    {
        Green,
        Purple,
        Red,
        Blue
    }
}
