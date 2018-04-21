using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Button yourButton;

    void Start()
    {
        Button btn = GetComponentInChildren<Button>();
        btn.onClick.AddListener(StartGameOnClick);
    }

    void StartGameOnClick()
    {
        SceneManager.LoadScene("main", LoadSceneMode.Single);
    }
}