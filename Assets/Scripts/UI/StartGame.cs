using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void LoadGameLevel()
    {
        SceneManager.LoadScene("GameScene");
    }
} 
