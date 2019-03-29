using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;



public class CreateLevers : MonoBehaviour
{
    //private bool isModeInfinite = true; /// load from separate script later

    public BoolManager boolManager;

    public GameObject ball;
    private Rigidbody ballrb;
    private float initialBallPosition;

    public GameObject leverPrefab;
    private GameObject[] leverArray = new GameObject[5]; // Set size inspector to avoid array out-of-bounds exception
    
    private Vector3 leverPos = new Vector3(-2, 0, 0);
    private Quaternion leverRot = Quaternion.Euler(0, 0, 0);

    private int numberOfLevers;
    private bool isColumn1 = true;

    public TextMeshProUGUI infiniteScoreText;
    private string infiniteScoreString;
    public TextMeshProUGUI topInfiniteScoreText;
    private string topInfiniteScore;


    void Start()
    {
        //PlayerPrefs.SetString("topInfiniteScore", "0");

        // Set score UI
        topInfiniteScore = PlayerPrefs.GetString("topInfiniteScore", "0");
        topInfiniteScoreText.SetText("Top score: " + topInfiniteScore);

        //Set conditional based on boolean manager
        if(boolManager.isModeInfinite == true) { numberOfLevers = 5; }
        else { numberOfLevers = 5; } /// set to gamemode specific number later from separate script.

        // Get initial position of the ball
        initialBallPosition = ball.transform.position.y;
        
        // Create level
        PlaceInitialLevers();
        
        // Add small force at game start to prevent physics bug when stopping on level lever
        ballrb = ball.GetComponent<Rigidbody>();
        ballrb.AddForce(transform.right * 2, ForceMode.Impulse);

        // Start tracking ininite game mode score
        StartCoroutine(SetInfiniteScoreText());
        // Start checking for out of bounds
        StartCoroutine(CheckBounds());
        // Start checking conditionals for lever state changes
        StartCoroutine(CheckNextLever());
    }



    // Initialize levers in scene
    private void PlaceInitialLevers()
    {
        for (int i = 0; i < numberOfLevers; i++)
        {
            GameObject newLever = Instantiate(leverPrefab) as GameObject;
            newLever.transform.position = leverPos;
            newLever.transform.rotation = leverRot;
            leverArray[i] = newLever;
            if (isColumn1 == true) { leverPos.x += 4; }
            else { leverPos.x -= 4; }
            isColumn1 = !isColumn1;
            leverPos.y -= 4;
            leverRot = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));
        }
    }
    
    private void ReloadScene()
    {
        //Handle score recording
        if(int.Parse(infiniteScoreString) > int.Parse(topInfiniteScore))
        {
            PlayerPrefs.SetString("topInfiniteScore", infiniteScoreString);
        }

        // Restart level by reloading scene on 'i' press
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    void Update()
    {
        // If 'i' is pressed, reload scene
        if (Input.GetKeyDown(KeyCode.I))
        {
            ReloadScene();
        }    
    }

    IEnumerator SetInfiniteScoreText()
    {
        //Handle score variable and display
        infiniteScoreString = (-ball.transform.position.y + initialBallPosition).ToString("0");
        infiniteScoreText.SetText("Score: " + infiniteScoreString);
        if (int.Parse(infiniteScoreString) > int.Parse(topInfiniteScore))
        {
            topInfiniteScoreText.SetText("Top score: " + infiniteScoreString);
        }

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(SetInfiniteScoreText());
    }

    // Create new data for lever positioning
    IEnumerator CheckNextLever()
    {
        if (leverArray[0].transform.position.y > ball.transform.position.y + 10)
        {
            Destroy(leverArray[0]);
            for (int i = 0; i < leverArray.Length - 1; i++)
            {
                leverArray[i] = leverArray[i + 1];
            }
            // Create new lever (abstract to method)
            leverRot = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));
            GameObject newLever = Instantiate(leverPrefab) as GameObject;
            newLever.transform.position = leverPos;
            newLever.transform.rotation = leverRot;
            leverArray[leverArray.Length - 1] = newLever;
            if (isColumn1 == true) { leverPos.x += 4; }
            else { leverPos.x -= 4; }
            isColumn1 = !isColumn1;
            leverPos.y -= 4;
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(CheckNextLever());
    }

    IEnumerator CheckBounds()
    {

        if (ball.transform.position.x > 5 || ball.transform.position.x < -5)
        {
            ReloadScene();
        }
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(CheckBounds());
    }
}
