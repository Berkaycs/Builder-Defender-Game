using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        transform.Find("BtnRetry").GetComponent<Button>().onClick.AddListener(() => {
            GameSceneManager.Load(GameSceneManager.Scene.GameScene);
        });

        transform.Find("BtnMainMenu").GetComponent<Button>().onClick.AddListener(() => {
            GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
        });

        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);

        transform.Find("TxtWave").GetComponent<TextMeshProUGUI>().SetText("You Survived " + EnemyWaveManager.Instance.GetWaveNumber() + " Waves!");
    }

    private void Hide()
    {
        gameObject?.SetActive(false);
    }
}
