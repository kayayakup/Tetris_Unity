using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPanel : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene("TetrisGame");
    }
}
