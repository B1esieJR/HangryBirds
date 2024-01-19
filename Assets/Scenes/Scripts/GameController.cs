using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum GameMode
{
    idle,
    playing,
    levelEnd
}
public class GameController : MonoBehaviour
{
    static private GameController S;
    public Text uiLevel;
    public Text uiShots;
    public Text uiButton;
   [SerializeField] private Vector3 castlePos;
    [SerializeField] private GameObject[] castles;
    [Header("Динамические поля")]
    [SerializeField] int level;
    [SerializeField] int levelMax;
    [SerializeField] int shotsTaken;
    [SerializeField] GameObject castle;
    [SerializeField] GameMode mode = GameMode.idle;
    [SerializeField] string showing = "Show Slingshot";

    private void Start()
    {
        S = this;
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }
    void StartLevel()
    {
        if (castle != null)
        {
            Destroy(castle);
        }
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (var temp in gos)
        {
            Destroy(temp);
        }
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;
        SwitchView("Show Both");
        ProjectileLine.S.Clear();
        Goal.goalMet = false;
        UpdateGUI();
        mode = GameMode.playing;
    }

    void UpdateGUI()
    {
        uiLevel.text = "Level: " + (level + 1) + "of " + levelMax;
        uiShots.text = "Shots Taken: " + shotsTaken;

    }
    void Update()
    {
        UpdateGUI();
        if (mode == GameMode.playing && Goal.goalMet)
        {
            mode = GameMode.levelEnd;
            SwitchView("Show Both");
            Invoke("NextLevel", 2f);
        }
    }
    void NextLevel()
    {
        level++;
        if (level == levelMax)
            level = 0;
        StartLevel();
    }
    public void SwitchView(string eView = " ")
    {
        if (eView == "")
            eView = uiButton.text;
        showing = eView;
        switch (showing)
        {
            case "Show Slingshot":
                cameraFollow.POI = null;
                uiButton.text = "Show Castle";
                break;
            case "Show Castle":
                cameraFollow.POI = S.castle;
                uiButton.text="Show Both";
                break;
            case "Show Both":
                uiButton.text = "Show Slingshot";
                break;
        }
        
    }
    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}
