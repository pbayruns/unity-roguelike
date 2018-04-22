using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Button yourButton;
    public BoxCollider2D Bounds;

    void Start()
    {
        Player.Move(Vector3.zero);

        Player.GetCamera().SetBounds(Bounds);
        Button btn = GetComponentInChildren<Button>();
        btn.onClick.AddListener(StartGameOnClick);
    }

    void StartGameOnClick()
    {
        SceneManager.LoadScene("main", LoadSceneMode.Single);
    }
}