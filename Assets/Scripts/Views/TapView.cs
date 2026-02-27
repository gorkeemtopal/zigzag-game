using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class TapView : MonoBehaviour
{
    [SerializeField] private GameOverUI _gameOverUI = null;

    [SerializeField] private TMP_Text _tapText = null;

    private IUserInput _userInput = null;
    private Player _player = null;

    private const string START_TEXT = "BAŞLAMAK İÇİN TIKLA";
    private const string RESTART_TEXT = "YENİDEN OYNAMAK TIKLA";

    [Inject]
    private void Construct(IUserInput userInput, Player player)
    {
        _userInput = userInput;
        _player = player;

        _userInput.OnPress += OnPress;
    }

    private void OnEnable()
    {
        _tapText.text = START_TEXT;
    }

    private void OnPress()
    {
        _tapText.gameObject.SetActive(false);
        _userInput.OnPress -= OnPress;
        _player.OnLose += OnLose;
    }

    private void OnLose()
    {
        AudioManager.Instance?.StopMusic();

        if (_tapText != null)
            _tapText.gameObject.SetActive(false);

        if (_gameOverUI != null)
            _gameOverUI.Show();
        else
            Debug.LogError("TapView: _gameOverUI bagli degil (Inspector'da GameOverPanel'i surukle).");
    }

    private void Restart()
    {
        AudioManager.Instance?.RestartMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}