using System.Collections;
using UnityEngine;
using DG.Tweening;

public class SphereController : MonoBehaviour
{
    public Transform[] moveTransforms;
    public float moveSpeed;
    public Color beginColor;
    public Color endColor;

    private Renderer _renderer;
    private Sequence _firstSequence;
    private float _speed = 0;

    private Tween _floatTween;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _floatTween = DOTween.To(() => _speed, x => _speed = x, 10, 5f).OnUpdate(() =>
        {
            Debug.Log("Speed = " + _speed);
        });
        
        FloatTween();
    }

    private void FloatTween()
    {
        _floatTween.Play();
        _floatTween.Pause();
        _floatTween.Restart();
        _floatTween.Kill();
    }
}
