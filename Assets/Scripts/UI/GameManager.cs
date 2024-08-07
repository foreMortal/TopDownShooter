using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private RecordScriptableObject recordObject;
    [SerializeField] private GameObject endGameMenu, record;
    [SerializeField] private Text points, inGameText;

    private void Awake()
    {
        Time.timeScale = 1f;
        recordObject.pointsText = inGameText;
        recordObject.SetCurrentPoints(0f);
    }

    private void Start()
    {
        playerManager.Health.DiedEvent += GameEnded;
    }

    public void GameEnded()
    {
        Time.timeScale = 0f;
        endGameMenu.SetActive(true);
        points.text = "Points: " + recordObject.currentPoints.ToString();

        if (recordObject.currentPoints > recordObject.Record)
        {
            PlayerPrefs.SetFloat(ShowRecord.RecordPath, recordObject.currentPoints);
            record.SetActive(true);
            recordObject.Record = recordObject.currentPoints;
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        int i = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(i);
    }

    public void BackToLobby()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
