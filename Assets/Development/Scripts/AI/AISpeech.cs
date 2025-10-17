using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AISpeech : MonoBehaviour
{
    public List<string> speeches = new();
    public TextMeshProUGUI _speechText;
    
    private Canvas _canvas;
    private void Start()
    {
        _canvas = GetComponent<Canvas>();
        _canvas.enabled = false;
        StartCoroutine(SpeakNow());
    }

    private void Update()
    {
        Quaternion lookRot = Quaternion.LookRotation(Camera.main.transform.position - transform.position);
        transform.rotation = lookRot;
    }

    private IEnumerator SpeakNow()
    {
        int randDuration = UnityEngine.Random.Range(1, 10);

        yield return new WaitForSeconds(randDuration);
        
        int randIndex = UnityEngine.Random.Range(0, speeches.Count);
        _speechText.text = speeches[randIndex];
        
        _canvas.enabled = true;

        yield return new WaitForSeconds(3f);
        
        _canvas.enabled = false;
        
        StartCoroutine(SpeakNow());
    }
}
