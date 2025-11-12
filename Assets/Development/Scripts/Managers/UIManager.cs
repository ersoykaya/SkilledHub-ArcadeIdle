using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject _playText;
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private TextMeshProUGUI _diamondText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        InputManager.Instance.OnMouseClicked += () =>
        {
            if (_playText.activeSelf)
            {
                _playText.SetActive(false);
            }
        };

        EconomyManager.Instance.OnGoldUpdated += (newAmount) =>
        {
            _goldText.text = newAmount.ToString();
        };

        EconomyManager.Instance.OnDiamondUpdated += (newAmount) =>
        {
            _diamondText.text = newAmount.ToString();
        };
    }

    private void OnDisable()
    {
        InputManager.Instance.OnMouseClicked -= () =>
        {

        };
        
        EconomyManager.Instance.OnGoldUpdated += (newAmount) =>
        {
            
        };

        EconomyManager.Instance.OnDiamondUpdated += (newAmount) =>
        {
            
        };
    }
}
