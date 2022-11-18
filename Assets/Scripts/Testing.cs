using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private int width = 5;
    private int height = 5;
    //private Grid<bool> grid;
    [SerializeField] private Camera cam;
    private Pathfinding pathfinding;

    // Start is called before the first frame update
    void Start()
    {
        pathfinding = new Pathfinding(width, height);
        //cam.transform.position = new Vector3(((float)width/2)*10, ((float)height / 2)*10, -10);
        //grid = new Grid<bool>(4, 2, 10f, new Vector3(20,0), (Grid<bool> g, int x, int y) => false);  
    }

    // Update is called once per frame
    void Update()
    {   
        if(Input.GetMouseButtonDown(0)) 
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            List<PathNode> path = pathfinding.FindPath(0, 0, x, y);
            if (path != null)
            {
                for (int i=0; i<path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x,path[i].y)*10f + Vector3.one*5f, new Vector3(path[i+1].x, path[i+1].y) * 10f + Vector3.one * 5f,Color.green,5f);
                }
            }
            //grid.SetGridObject(GetMouseWorldPosition(), true);

        }

        //if (Input.GetMouseButtonDown(1))
        //{
        //    Debug.Log(grid.GetGridObject(GetMouseWorldPosition()));
        //}

    }

    // Get Mouse Position in World with Z = 0f
    public static Vector3 GetMouseWorldPosition()
    {  
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0f;
        return worldPosition;
    }
}
