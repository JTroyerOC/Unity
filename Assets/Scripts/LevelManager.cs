using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public float TimeoutBeforeFirstLevel = 3f;
    private PlayerController _player;
    
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        _player = FindObjectOfType<PlayerController>();
        DontDestroyOnLoad(_player.gameObject);

        StartCoroutine(LoadNextOnStart());
    }
    
    public static void LoadNextLevel()
    {
        SceneManager.LoadScene(GetCurrentLevel() + 1);
    }

    private static int GetCurrentLevel()
    {
        return SceneManager.GetActiveScene().buildIndex;
    } 

    private IEnumerator LoadNextOnStart()
    {
        if (GetCurrentLevel() > 0) yield break;
        
        yield return new WaitForSeconds(TimeoutBeforeFirstLevel);
        LoadNextLevel();
    }
}
