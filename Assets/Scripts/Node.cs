using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int cost = 1;

    public List<Node> neighbors = new List<Node>();
}
