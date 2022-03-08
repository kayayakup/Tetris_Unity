using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Tetris : MonoBehaviour
{
    public Vector3 RotationPoint;
    private float PreviousTime;
    private float FallTime = 0.3f;
    public static int Height = 25;
    public static int Width = 10;
    private static Transform[,] grid = new Transform[10, 25];
    private TextMeshProUGUI Score;
    private TextMeshProUGUI HighScore;
    private TextMeshProUGUI Level;
    public static int CleanLine = 0;
    public static int HadScore;
    public static int High_Score = 0;
    public static int Level_ = 0;
    public GUIStyle ButtonStyle;
    public AudioClip Move;
    public AudioClip Rotate;
    public AudioClip Clean;
    public AudioClip Fall;
    public AudioClip GameOver;
    public AudioSource audioSource;
    public Slider Slider;
    private float AudioVolume=1f;

    void OnGUI()
    {
        GUI.backgroundColor = Color.red;//Right Button
        if (GUI.Button(new Rect(550, 1025, 120, 120),"", ButtonStyle))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(1, 0, 0);
            }
            audioSource.PlayOneShot(Move);
        }
        GUI.backgroundColor = Color.magenta;//Left Button
        if (GUI.Button(new Rect(350, 1025, 120, 120), "", ButtonStyle))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }
            audioSource.PlayOneShot(Move);
        }
        GUI.backgroundColor = Color.blue;//Rotate Button
        if (GUI.Button(new Rect(45, 1025, 300, 300), "", ButtonStyle))
        {
            transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), 90);
            if (!ValidMove())
            {
                transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), -90);
            }
            audioSource.PlayOneShot(Move);
        }
        GUI.backgroundColor = Color.grey;//Down Button
        if (Time.time - PreviousTime > (GUI.Button(new Rect(450, 1175, 120, 120), "", ButtonStyle)? FallTime / 10 : FallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddGrid();
                CheckLine();
                FindObjectOfType<SpawnController>().NewFigure();
                this.enabled = false;
                audioSource.PlayOneShot(Fall);
                CheckEndGame();
            }
            PreviousTime = Time.time;
            audioSource.PlayOneShot(Rotate);
        }
    }

    void Start()
    {
        Score = GameObject.Find("Canvas/Score(1)").GetComponent<TextMeshProUGUI>();
        HighScore = GameObject.Find("Canvas/HighScore(1)").GetComponent<TextMeshProUGUI>();
        Level = GameObject.Find("Canvas/Level(1)").GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
        Level_ = 1;
    }

    void Update()
    {
        if(Time.time - PreviousTime > FallTime)
        {
            if (!ValidMove())
            {
                CheckLine();
                CheckEndGame();
            }
        }
        audioSource.volume = AudioVolume;
        UpdateScore();
    }

    public void SetVolume(float volume)
    {
        AudioVolume = volume;
    }

    void UpdateScore()
    {
        if (CleanLine > 0)
        {
            if (CleanLine == 1)
            {
                HadScore += 30;
                Score.text = String.Format("{0:D8}", HadScore);
            }
            else if (CleanLine == 2)
            {
                HadScore += 50;
                Score.text = String.Format("{0:D8}", HadScore);
            }
            else if (CleanLine == 3)
            {
                HadScore += 70;
                Score.text = String.Format("{0:D8}", HadScore);
            }
            else if (CleanLine == 4)
            {
                HadScore += 150;
                Score.text = String.Format("{0:D8}", HadScore);
            }
            CleanLine = 0;
        }
        if (PlayerPrefs.GetInt("HighScore") < HadScore)
        {
            High_Score = HadScore;
            PlayerPrefs.SetInt("HighScore", High_Score);
        }
        HighScore.text = String.Format("{0:D8}", PlayerPrefs.GetInt("HighScore"));
        if (HadScore >= Level_ * 5000)
        {
            Level_ += 1;
            Level.text = Level_.ToString();
        }
    }

    void CheckLine()
    {
        for(int i = Height - 1; i >=0 ; i--)
        {
            if(HasLine(i))
            {
                DeleteLine(i);
                RownDown(i);
                audioSource.PlayOneShot(Clean);
            }
        }
    }

    bool HasLine(int i)
    {
        for(int j = 0; j < Width; j++)
        {
            if (grid[j, i] == null)
            {
                return false;
            }
        }
        CleanLine++;
        return true;
    }

    void DeleteLine(int i)
    {
        for(int j = 0; j < Width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    void RownDown(int i)
    {
        for(int y = i; y < Height; y++)
        {
            for(int j = 0; j < Width; j++)
            {
                if(grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }


    void AddGrid()
    {
        foreach(Transform children in transform)
        {
            int RoundX = Mathf.RoundToInt(children.transform.position.x);
            int RoundY = Mathf.RoundToInt(children.transform.position.y);
            grid[RoundX, RoundY] = children;
        }
    }

    void CheckEndGame()
    {
        for (int j = 0; j < Width; j++)
        {
            if (grid[j,Height-2] != null)
            {
                SceneManager.LoadScene("Scenes/GameOver");
            }
        }
    }

    bool ValidMove()
    {
        foreach(Transform children in transform)
        {
            int RoundX = Mathf.RoundToInt(children.transform.position.x);
            int RoundY = Mathf.RoundToInt(children.transform.position.y);
            if (RoundX < 0 || RoundX >= Width || RoundY < 0 || RoundY >= Height)
            {
                return false;
            }
            if (grid[RoundX, RoundY] != null)
            {
                return false;
            }
        }
        return true;
    }
    

}