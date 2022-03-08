using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    bool value = true;
    public GameObject Paus;
    public void Show()
    {
        if (value == true)
        {
            Paus.SetActive(true);
            Time.timeScale = 0.0f;
        }
        if(value==false)
        {
            Paus.SetActive(false);
            Time.timeScale = 1.0f;
        }
        value = !value;
    }
}
