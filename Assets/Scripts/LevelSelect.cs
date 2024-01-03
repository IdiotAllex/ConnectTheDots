using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class for buttons and actions of level select screen
public class LevelSelect : MonoBehaviour
{
    //Game manager object for getting the data for point positions
    [SerializeField]
    private GameObject GameManager;

    //level selection button prefab
    [SerializeField]
    private GameObject buttonPrefab;

    //object that will contain all buttons
    [SerializeField]
    private GameObject buttonParent;

    //Game point prefab
    [SerializeField]
    private GameObject pointPrefab;

    //Canvas of level selection screen
    public GameObject canvas;

    //Class that has game point position data
    private DataManager data;

    //Game level prefab
    public GameObject levelPrefab;

    //new level created in scene when level is selected
    private GameObject newLevel;


    private void OnEnable()
    {
        data = GameManager.GetComponent<DataManager>();

        //Level slecetion screen buttons are created
        for (int i = 1; i <= data.levelCount; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonParent.transform);
            int levelNumb = i - 1;
            newButton.GetComponent<ButtonText>().buttonText.text = "level " + i;
            newButton.GetComponent<Button>().onClick.AddListener(() => loadLevel(levelNumb));
        }
    }

    /* Loads level when level button is clicked. 
     * int levelNumb - number of selected level
     */
    private void loadLevel(int levelNumb)
    {
        //Destroy old level if it was loaded
        if (newLevel != null)
        {
            Destroy(newLevel);
        }

        //turns off level select screen
            canvas.SetActive(false);

        //creates new level and game points
            int pointNumb = 0;
            Level levelToload = data.levelList.levels[levelNumb];
            newLevel = Instantiate(levelPrefab);
            foreach (Point point in levelToload.pointList)
            {
                pointNumb++;
                Vector3 position = new Vector3(point.x, point.y, 0);
                GameObject newPoint = Instantiate(pointPrefab, newLevel.transform);
                newPoint.transform.position = position;
                newPoint.GetComponent<ButtonText>().buttonText.text = pointNumb.ToString();
                newLevel.GetComponent<GamePointList>().gamePointList.Add(newPoint);
            }
    }
}