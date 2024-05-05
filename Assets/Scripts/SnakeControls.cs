using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeControls : MonoBehaviour
{
    private Vector2 direction = Vector2.right;
    private List<Transform> Segments;
    public Transform BodyPrefab;

    public GameObject GameOverPanel;
    bool GameOver=false;

    // Start is called before the first frame update
    void Start()
    {
        GameOverPanel.SetActive(false);
        Segments =new List<Transform> ();
        Segments.Add(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameOver)
        {
            GameOverPanel.SetActive (true);
            return;
        }

        if(Input.GetKeyDown(KeyCode.RightArrow)) { 
            direction = Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            direction = Vector2.up;
        }
    }

    private void FixedUpdate()
    {
        if (GameOver)
        {
            GameOverPanel.SetActive(true);
            return;
        }


        for (int i=Segments.Count-1 ; i>0; i--)
        {
            Segments[i].position=Segments[i-1].position;
        }
        this.transform.position=new Vector3(
            Mathf.Round(this.transform.position.x)+direction.x,
            Mathf.Round(this.transform.position.y)+direction.y,
            0);
    }

    private void Grow()
    {
        Transform segment=Instantiate(this.BodyPrefab);
        segment.position=Segments[Segments.Count-1].position;
        Segments.Add (segment);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag=="Food")
        {
            Debug.Log("Food");
            Grow();
        }

        if (other.tag=="Obstacle")
        {
            Debug.Log("Obstacle");
            GameOver=true;
            ResetState();
        }
    }

    private void ResetState()
    {
        for(int i=1; i<Segments.Count; i++) 
        {
            Destroy(Segments[i].transform.gameObject);
        }
        Segments.Clear();
        Segments.Add(this.transform);

        this.transform.position=Vector3.zero;
    }

    public void tryAgain()
    {
        GameOver=false;
        GameOverPanel.SetActive(false);
    }
}
