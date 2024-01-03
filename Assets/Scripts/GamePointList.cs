using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class for managing and saving joint level point data
public class GamePointList : MonoBehaviour
{
    //List of game objects that are the points in a level
    public List<GameObject> gamePointList = new List<GameObject>();

    //Saves if first point has been clicked
    public bool firstClick = true;

    //Saves the starting position of a line drawn between two points
    public Vector3 startingPosition;

    //Saves position of first point in a level
    public Vector3 firstPoint;

    //Saves if line is being drawn between points 
    public bool lineIsDrawing = false;

    //Queue for saving IEnumerators of line drawing animations
    public Queue<IEnumerator> quenue = new Queue<IEnumerator>();

    //Saves the position of the first point of a level
    public void setFirstPoint()
    {
        firstPoint = gamePointList[0].transform.position;
    }

    //Saves the position of the starting connection between two points 
    public void setPosition()
    {
        startingPosition = gamePointList[0].transform.position;
    }

    private void Update()
    {
        //Makes sure the line animations happen one at a time and in the correct order
        if (quenue.Count > 0)
        {
            if (lineIsDrawing == false)
            StartCoroutine(quenue.Dequeue());
        }
    }
}
