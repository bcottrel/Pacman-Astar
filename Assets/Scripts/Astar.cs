using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour
{
    public GameObject player;
    public GameObject ghost;

    public Dot playerDot;
    private Dot ghostDot;
    private float radius = 0.16f;
    private int dotCount = 217;
    public List<Dot> GhostPath;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        playerDot = FindLocation(player);
        ghostDot =  FindLocation(ghost);
        Path(playerDot, ghostDot);
    }
    
    //Finds the shortest route to a object
    void Path(Dot playerDot, Dot ghostDot) 
    {
        Heap<Dot> openSet = new Heap<Dot>(dotCount);
        HashSet<Dot> closedSet = new HashSet<Dot>();
        openSet.Add(ghostDot);

        while (openSet.CurrentCapacity > 0)
        {
            Dot currentDot = openSet.RemoveFirst();
            closedSet.Add(currentDot);

            if (currentDot == playerDot) 
            {
                RetracePath(ghostDot, playerDot);
                return;
            }

            foreach (var neighbor in currentDot.neighbors)
            {
                Dot neighborDot = neighbor.GetComponent<Dot>();
                if (closedSet.Contains(neighborDot))
                    continue;


                float currentGCost = currentDot.gCost +
                    GetDistance(currentDot, neighborDot);

                if (currentGCost < neighborDot.gCost ||
                    !openSet.Contains(neighborDot))
                {
                    neighborDot.gCost = currentGCost;
                    neighborDot.hCost = GetDistance(neighborDot, playerDot);
                    neighborDot.parent = currentDot;

                    if (!openSet.Contains(neighborDot))
                        openSet.Add(neighborDot);
                }
            }
        }
    }

    //Finds distance between a object and target
    //used to find hcost and gcost
    float GetDistance(Dot start, Dot end)
    {
        Vector2 startPos = start.gameObject.transform.position;
        Vector2 endPos = end.gameObject.transform.position;

        float x = Mathf.Abs(startPos.x - endPos.x);
        float y = Mathf.Abs(startPos.y - endPos.y);
        return x + y;
    }

    //Finds closest dot to an object
    Dot FindLocation(GameObject gameobject)
    {
        Dot nearestDot = null;
        float minDistance = Mathf.Infinity;
        Vector2 location = gameObject.transform.position;

        Collider2D[] neighbors =
            Physics2D.OverlapCircleAll(location, radius, 3);

        foreach (var neighbor in neighbors)
        {
            Vector2 neighborLocation = neighbor.gameObject.transform.position;

            //Cheaper way to do .Distance();
            Vector2 distance = neighborLocation - location;
            float distSqrTarget = distance.sqrMagnitude;

            if (distSqrTarget < minDistance)
            {
                minDistance = distSqrTarget;
                nearestDot = neighbor.gameObject.GetComponent<Dot>();
            }
        }
        return nearestDot;
    }

    //Most optimized path
    void RetracePath(Dot start, Dot end)
    {
        List<Dot> path = new List<Dot>();
        Dot current = end;

        while (current != start)
        {
            path.Add(current);
            current = current.parent;
        }
        path.Reverse();
        GhostPath = path;
    }



    private void OnDrawGizmos()
    {
        if (playerDot != null )
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(player.transform.position,
                playerDot.transform.position);
            if (GhostPath != null)
            {
                foreach (var path in GhostPath)
                {
                    Gizmos.DrawLine(path.transform.position,
                        path.parent.transform.position);
                }
            }
        }
    }
}
