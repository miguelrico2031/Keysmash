using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageAnimation : MonoBehaviour
{
    private const float PERIOD = 0.2f;
    private const float DURATION = 0.75f;
    private const float ALPHA = 0.4f;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Color _damageColor = new Color(1, 0.4f, 0.4f, 1);
    private Color _originalColor;

    private bool _flickering = false;


    public void StartAnimation()
    {
        _originalColor = _spriteRenderer.color;
        _damageColor.a = ALPHA;
        StartCoroutine(FlickerTime());
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        if(!_flickering)
        {
            _spriteRenderer.color = _originalColor;
            yield break;
        }
        _spriteRenderer.color = _damageColor;
        yield return new WaitForSeconds(PERIOD);

        if(!_flickering)
        {
            _spriteRenderer.color = _originalColor;
            yield break;
        }
        _spriteRenderer.color = _originalColor;
        yield return new WaitForSeconds(PERIOD);
        StartCoroutine(Flicker());
    }

    IEnumerator FlickerTime()
    {
        _flickering = true;
        yield return new WaitForSeconds(DURATION);
        _flickering = false;
    }
}
