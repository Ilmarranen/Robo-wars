using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNodeVisuals : MonoBehaviour
{
    public PathNode node;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        node.OnMouseDown();   
    }
}
