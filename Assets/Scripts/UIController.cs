using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreLabel;
    [SerializeField] private GameObject popup;
    [SerializeField] private Slider speedSlide;
    [SerializeField] private TextMeshProUGUI nameInput;

    private void Awake()
    {
        Messenger<int>.AddListener(Event.ScoreChanged, OnScoreChange);
    }

    private void OnDestroy()
    {
        Messenger<int>.RemoveListener(Event.ScoreChanged, OnScoreChange);
    }

    // Start is called before the first frame update
    void Start()
    {
        popup.SetActive(false);
        speedSlide.value = PlayerPrefs.GetFloat("speed", 1);
        nameInput.text = PlayerPrefs.GetString("name");        
    }

    public void OnOpenSettings()
    {
        popup.SetActive(true);
    }

    public void OnCloseSettings()
    {
        popup.SetActive(false);
    }

    public void OnSubmitName(string name)
    {
        Debug.Log("New name: " + name);
        PlayerPrefs.SetString("name", name);        
    }

    public void OnScoreChange(int score)
    {
        scoreLabel.text = score.ToString();
    }

    public void OnSpeedChange(float speed)
    {
        Debug.Log("Speed: " + speed);
        PlayerPrefs.SetFloat("speed", speed);
        try
        {
            Messenger<float>.Broadcast(Event.SpeedMultiplierChanged, speed);
        } catch(Exception _)
        {

        }
    }

    public void OnMouseDown()
    {
        Debug.Log("Mouse down");
    }
}
