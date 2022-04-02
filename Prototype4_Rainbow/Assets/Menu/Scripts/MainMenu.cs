using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Text recordText;

    private void Start()
    {
        int recordScore = PlayerPrefs.GetInt("recordScore");
        recordText.text = ": " + recordScore.ToString();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(0);
    }
}
