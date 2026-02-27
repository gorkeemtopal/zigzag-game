using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverUI : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 1f;         
        gameObject.SetActive(false);  // baþta gizli
    }

    public void Show()
    {
        Time.timeScale = 0f;          
        gameObject.SetActive(true);   // paneli aç
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false; // Editor'da çýkýþ böyle test edilir
#else
        Application.Quit(); // Build'de çalýþýr
#endif
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        AudioManager.Instance?.RestartMusic();
    }
}
