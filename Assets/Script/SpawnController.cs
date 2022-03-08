using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnController : MonoBehaviour
{
    public GameObject[] Figures;
    public GameObject[] NextFigures;
    GameObject UpNextObjects = null;
    public static int FigureIndex;
    public static int NextFigureIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        NextFigureIndex = Random.Range(0, 6);
        NewFigure();    
    }

    // Update is called once per frame
    public void NewFigure()
    {
        int FigureIndex = NextFigureIndex;
        Instantiate(Figures[FigureIndex], transform.position, Quaternion.identity);
        NextFigureIndex = Random.Range(0, 6);
        if (UpNextObjects != null)
        {
            Destroy(UpNextObjects);
        }
        UpNextObjects = Instantiate(NextFigures[NextFigureIndex], new Vector3(13.5f, 18f, 0f), Quaternion.identity);
    }
}
