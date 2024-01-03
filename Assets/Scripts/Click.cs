using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Class that defines the behavior of a point
public class Click : MonoBehaviour
{
    //The Gameobjects sprite rendered for changing the sprtie at runtime
    private SpriteRenderer spriteRenderer;

    //Sprite prefab used to change old Gameobject sprite
    [SerializeField]
    private Sprite sprite;

    //LineRenderer prefab used for drawing line betwen points
    [SerializeField]
    private LineRenderer linePrefab;

    //Duration of the animation of the rope moving between points
    [SerializeField]
    private float animDuration = 1f;

    //Duration of the animation of the numbers next to points fading
    [SerializeField]
    private float fadeOutDuration = 1f;

    //Text next to buttons
    private TMP_Text buttonText;

    //Points data managment class
    private GamePointList pointsData;

    //BoxCollider of the point
    private BoxCollider2D boxCollider;

 
    private void OnEnable()
    {
        pointsData = transform.parent.gameObject.GetComponent<GamePointList>();
        buttonText = GetComponent<ButtonText>().buttonText;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    //Actions on point click
    void OnMouseUpAsButton() {

        //checks if clicked point is the next one that can be connected
        if (gameObject == pointsData.gamePointList[0])
        {   
            //changes sprite and number
            changeSprite();

            //checks if clicked point is first
            if (pointsData.firstClick == true)
            {
                pointsData.setFirstPoint();
                pointsData.firstClick = false;
            }
            else
            {
                //Enqueues line animation IEnumerators to play in correct order
                pointsData.quenue.Enqueue(drawRope(pointsData.startingPosition));
            }

            //saves position of point that has just been clicked
            pointsData.setPosition();
            pointsData.gamePointList.RemoveAt(0);

            //Checks if all points have been clicked
            if (pointsData.gamePointList.Count <= 0)
            {
                pointsData.quenue.Enqueue(drawLastRope(pointsData.firstPoint));
            }
        }
    }

    //Loads level select screen after all points have been connected
    private void LoadLevelSelect()
    {
        LevelSelect levelSelect = GameObject.Find("LevelSelect").GetComponent<LevelSelect>();
        levelSelect.canvas.SetActive(true);
    }

    /* Changes point sprite and triggers point number fade out
     * turns off point hitbox so it cannot be clicked agian
     */
    private void changeSprite()
    {
        boxCollider.enabled = false;
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        StartCoroutine(fadeOut());
    }

    /* IEnumerator that draws a rope from the last point to the first point
     * Vector3 endPos - position of first point that the line will be drawn to from the last point
     */
    private IEnumerator drawLastRope(Vector3 endPos)
    {
        yield return new WaitUntil(() => pointsData.lineIsDrawing == false);
        
        pointsData.lineIsDrawing = true;

        Vector3 newPos = transform.position;
        LineRenderer newLine = Instantiate<LineRenderer>(linePrefab, transform);
        float t = 0;


        newLine.SetPosition(0, transform.position);

        for (; t < animDuration; t += Time.deltaTime)
        {
            newPos = Vector3.Lerp(transform.position, endPos, t / animDuration);
            newLine.SetPosition(1, newPos);
            yield return null;
        }
        newLine.SetPosition(1, endPos);

        pointsData.lineIsDrawing = false;
        LoadLevelSelect();
    }

    /* IEnumerator for drawing rope between two points. Except between the last and first point
     * Vector 3 startPos - starting positiong of line between two points
     */
    private IEnumerator drawRope(Vector3 startPos)
    {
        pointsData.lineIsDrawing = true;

        Vector3 endPos = transform.position;
        Vector3 newPos = startPos;
        LineRenderer newLine = Instantiate<LineRenderer>(linePrefab, transform);
        float t = 0;
        newLine.SetPosition(0, startPos);
        for (; t < animDuration; t += Time.deltaTime)
        {
            newPos = Vector3.Lerp(startPos, endPos, t / animDuration);
            newLine.SetPosition(1, newPos);
            yield return null;
        }
        newLine.SetPosition(1, endPos);

        pointsData.lineIsDrawing = false;
    }

    //IEnumerator for fade out animation on text next to point
    private IEnumerator fadeOut()
    {
        float startTime = Time.time;
        float a = buttonText.color.a;
        float r = buttonText.color.r;
        float g = buttonText.color.g;
        float b = buttonText.color.b;
        while (a > 0)
        {
            float t = (Time.time - startTime)/fadeOutDuration;
            buttonText.color = new Color(r, g, b, (a - 0.9f)/t);
            yield return null;
        }
    }
}