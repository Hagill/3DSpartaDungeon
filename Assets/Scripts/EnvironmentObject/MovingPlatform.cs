using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Vector3 _startOffset = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 _endOffset = new Vector3(0, 0, 0);
    [SerializeField] private float _moveDuration;
    [SerializeField] private float _pauseDuration;
    [SerializeField] private AnimationCurve _moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector3 _startPosition;
    private Vector3 _endPosition;

    private void Awake()
    {
        _startPosition = transform.position + _startOffset;
        _endPosition = transform.position + _endOffset;

        StartCoroutine(MovePlatform());
    }

    private IEnumerator MovePlatform()
    {
        while (true)
        {
            yield return StartCoroutine(LerpPosition(_startPosition, _endPosition, _moveDuration));
            yield return new WaitForSeconds(_pauseDuration);
            
            yield return StartCoroutine(LerpPosition(_endPosition, _startPosition, _moveDuration));
            yield return new WaitForSeconds(_pauseDuration);
        }
    }

    private IEnumerator LerpPosition(Vector3 startPos, Vector3 endPos, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            float t = timer / duration;
            float curveValue = _moveCurve.Evaluate(t);

            transform.position = Vector3.Lerp(startPos, endPos, curveValue);

            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
