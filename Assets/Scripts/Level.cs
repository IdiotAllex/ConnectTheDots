using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Level
{
    //Saves the numbers from the game data file
    public float[] level_data;

    //List of points converted to in Unity world space
    public List<Point> pointList = new List<Point>();

    //Converts point position data from the given data to Unity world space
    //and adds them into the converted point list
    public void convertToPoints()
    {
        for (int i = 0; i < level_data.Length; i = i + 2)
        { 
            Point newPoint = new Point();
            newPoint.x = level_data[i] * 0.1f;

            if (newPoint.x > 50)
            {
                newPoint.x = trans(newPoint.x, 100, 50, 50, 0);
            }
            else if (newPoint.x == 50)
            {
                newPoint.x = 0;

            }
            else if (newPoint.x < 50)
            {

                newPoint.x = trans(newPoint.x, 50, 0, 0, -50);
            }

            newPoint.y = level_data[i + 1] * 0.1f;;


            if (newPoint.y > 50)
            {
                newPoint.y = trans(newPoint.y, 100, 50, -50, 0);
            }
            else if (newPoint.y == 50)
            {
                newPoint.y = 0;
            }
            else if (newPoint.y < 50)
            {
                newPoint.y = trans(newPoint.y, 50, 0, 0, 50);
            }


            pointList.Add(newPoint);
        }
    }

    /*translates and scales points from one coordinate system to another
    float point - the x or y value of the point you want to convert
    float S2 - the maximum value of the source coordinate system
    float S1 - the maximum value of the source coordinate system
    float T2 - the maximum value of target coordinate system
    float T1 - the maximum value of target coordinate system*/
    private float trans(float point, float S2, float S1, float T2, float T1)
    {
        float traslate = (T2 * S1 - T1 * S2) / (S1 - S2);
        float scale = (T2 - T1) / (S2 - S1);
        return traslate + scale * point;
    }

}
