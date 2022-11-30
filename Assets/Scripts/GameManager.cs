using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Node startingNode = null;
    public Node goalNode = null;

    private PathFinding _pf;

    public Enemys en;

    public bool alert;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        _pf = new PathFinding();
        en = new Enemys();
    }

    public void ChangeObjectColor(GameObject go, Color color)
    {
        go.GetComponent<Renderer>().material.color = color;
    }
}
