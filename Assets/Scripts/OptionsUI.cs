using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private MusicManager _musicManager;

    [SerializeField] private Button _btnOptions;

    private TextMeshProUGUI _soundVolumeText;
    private TextMeshProUGUI _musicVolumeText;

    private void Awake()
    {
        _soundVolumeText = transform.Find("TxtSoundVolume").GetComponent<TextMeshProUGUI>();
        _musicVolumeText = transform.Find("TxtMusicVolume").GetComponent<TextMeshProUGUI>();

        _btnOptions.onClick.AddListener(() => {
            gameObject.SetActive(!gameObject.activeSelf);

            if (gameObject.activeSelf)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        });

        transform.Find("BtnIncreaseSound").GetComponent<Button>().onClick.AddListener(() => {
            _soundManager.IncreaseVolume();
            UpdateText();
        });

        transform.Find("BtnDecreaseSound").GetComponent<Button>().onClick.AddListener(() => {
            _soundManager.DecreaseVolume();
            UpdateText();
        });

        transform.Find("BtnIncreaseMusic").GetComponent<Button>().onClick.AddListener(() => {
            _musicManager.IncreaseVolume();
            UpdateText();
        });

        transform.Find("BtnDecreaseMusic").GetComponent<Button>().onClick.AddListener(() => {
            _musicManager.DecreaseVolume();
            UpdateText();
        });

        transform.Find("BtnMainMenu").GetComponent<Button>().onClick.AddListener(() => {
            Time.timeScale = 1f;
            GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        gameObject.SetActive(false);
        UpdateText();
    }

    private void UpdateText()
    {
        _soundVolumeText.SetText(Mathf.RoundToInt(_soundManager.GetVolume() * 10).ToString());
        _musicVolumeText.SetText(Mathf.RoundToInt(_musicManager.GetVolume() * 10).ToString());
    }
}
