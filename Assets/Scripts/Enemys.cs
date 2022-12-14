using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemys : MonoBehaviour
{
    public Node[] allWaypoints;
    public NodeGrid grid;

    public float speed;
    private int _currentWP = 0;


    public Node startingNode;
    public Node goalNode;

    public List<Node> pathToFollow = new List<Node>();

    public PathFinding _pf = new PathFinding();

    public GameObject target;

    public LayerMask wallLayer;

    public float viewRadius;
    public float viewAngle;
    void Update()
    {
        if (InFOV(target))
        {
            Debug.DrawLine(transform.position, target.transform.position, Color.red);
            Vector3 dir = target.transform.position - transform.position;
            transform.position += (transform.forward * speed) * Time.deltaTime;
            transform.forward = dir;
        }
        else if (GameManager.instance.alert)
        {
            if (pathToFollow.Count == 0)
            {
                startingNode = NearNode(this.transform);
                goalNode = NearNode(target.transform);
                SetPath(_pf.AStar(startingNode, goalNode));
            }
            if (pathToFollow.Count > 0)
            {
                FollowPath();
            }
        }
        else
        {
            GameManager.instance.alert = false;
            if (InLineOfSight(allWaypoints[_currentWP].transform.position, transform.position))
            {
                Debug.DrawLine(allWaypoints[_currentWP].transform.position, transform.position, Color.blue);
                Node waypoint = allWaypoints[_currentWP];
                Vector3 dir = waypoint.transform.position - transform.position;
                transform.position += (transform.forward * speed) * Time.deltaTime;
                if (dir.magnitude <= 10f)
                {
                    _currentWP++;
                    if (_currentWP > allWaypoints.Length - 1) _currentWP = 0;
                }
                transform.forward = dir;
            }

            else
            {
                if (pathToFollow.Count == 0)
                {
                    startingNode = NearNode(this.transform);
                    goalNode = allWaypoints[0];
                    SetPath(_pf.AStar(startingNode, goalNode));
                }
                if (pathToFollow.Count > 0)
                {
                    FollowPath();
                }
                //startingNode = allWaypoints[0];
                //goalNode = NearNode(allWaypoints[1].transform);
                //SetPath(_pf.AStar(startingNode, goalNode));
            }
        }
    }

    public void SetPath(List<Node> path)
    {
        pathToFollow = path;
        Vector3 dir = startingNode.transform.position - transform.position;
        MoveToDir(dir);
    }

    void FollowPath()
    {
        Vector3 nextPos = pathToFollow[0].transform.position;
        Vector3 dir = nextPos - transform.position;
        MoveToDir(dir);

        if (dir.magnitude <= 10f) pathToFollow.RemoveAt(0);
    }

    void MoveToDir(Vector3 dir)
    {

        transform.forward = dir;
        transform.position += transform.forward * Time.deltaTime * speed;

    }

    bool InFOV(GameObject obj)
    {
        if (Vector3.Distance(transform.position, obj.transform.position) > viewRadius) return false;

        if (Vector3.Angle(transform.forward, obj.transform.position - transform.position) > (viewAngle / 2)) return false;
        GameManager.instance.alert = InLineOfSight(transform.position, obj.transform.position);

        return InLineOfSight(transform.position, obj.transform.position);

    }

    public bool InLineOfSight(Vector3 start, Vector3 end)
    {
        Vector3 dir = end - start;
        return !Physics.Raycast(start, dir, dir.magnitude, wallLayer);
    }



    public Node NearNode(Transform start)
    {
        Node bestNode = grid.allNode[0];
        foreach (var item in grid.allNode)
        {
            if (bestNode == item) continue;
            if (InLineOfSight(start.position, item.transform.position))
            {
                if (Vector3.Distance(start.position, bestNode.transform.position) > Vector3.Distance(start.position, item.transform.position))
                {
                    bestNode = item;
                }
            }

        }

        return bestNode;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 lineA = GetVectorFromAngle(viewAngle / 2 + transform.eulerAngles.y);
        Vector3 lineB = GetVectorFromAngle(-viewAngle / 2 + transform.eulerAngles.y);

        Gizmos.DrawLine(transform.position, transform.position + lineA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + lineB * viewRadius);

    }

    Vector3 GetVectorFromAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
