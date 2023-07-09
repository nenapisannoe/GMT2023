using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Game.Enemy;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private List<ReactiveAbility> m_ReactiveAbilities = new List<ReactiveAbility>();
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void LoadRoundFinichedScene()
    {
        SceneManager.LoadScene("Round Finish Scene");
    }

    public void LoadEndScene()
    {
        SceneManager.LoadScene("End Scene");
    }


}
