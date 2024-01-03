using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class for reading and converting data from files
public class DataManager : MonoBehaviour
{
    //Json file with point data
    public TextAsset jsonFile;

    //List of all leves read from file
    public Levels levelList = new Levels();

    //Number of total levels in game
    public float levelCount = 0;

    //Class for managing level select screen
    [SerializeField]
    private GameObject levelSelect;

    void Start()
    {
        //Read game data file
        levelList = JsonUtility.FromJson<Levels>(jsonFile.text);

        //converts points from data into Unity coordinate space
        foreach (Level level in levelList.levels)
        {
            level.convertToPoints();
            levelCount++;
        }

        //turns on level select screen
        levelSelect.GetComponent<LevelSelect>().enabled = true;
    }
}

