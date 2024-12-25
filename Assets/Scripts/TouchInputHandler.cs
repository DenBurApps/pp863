using System;
using System.Collections;
using System.Collections.Generic;
using FlashElement;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchInputHandler : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject _incorrectTouchObject;

    public event Action<Flash> FlashClicked;
    public event Action Missed;

    private Coroutine _touchDetectingCoroutine;

    public void StartDetectingTouch()
    {
        if (_touchDetectingCoroutine == null)
        {
            _touchDetectingCoroutine = StartCoroutine(DetectTouch());
        }
    }

    public void StopDetectingTouch()
    {
        if (_touchDetectingCoroutine != null)
        {
            StopCoroutine(_touchDetectingCoroutine);
            _touchDetectingCoroutine = null;
        }
    }

    private IEnumerator DetectTouch()
    {
        while (true)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        Debug.Log("UI element clicked, ignoring touch");
                        yield return null;
                        continue;
                    }
                    
                    Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    
                    RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity, _layerMask);

                    if (hit.collider != null)
                    {
                        Debug.Log("Hit detected on: " + hit.collider.gameObject.name);
                        if (hit.collider.TryGetComponent(out Flash obj))
                        {
                            FlashClicked?.Invoke(obj);
                        }
                    }
                    else
                    {
                        Debug.Log("Missed, no object detected");
                        Missed?.Invoke();
                        var gameObject = Instantiate(_incorrectTouchObject);
                        gameObject.transform.position = touchPosition;
                        Destroy(gameObject, 2);
                    }
                }
            }

            yield return null;
        }
    }
}