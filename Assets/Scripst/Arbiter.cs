using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Arbiter : MonoBehaviour
{
    // Players
    public bool[] activePlayers;
    public GameObject pj1;
    public GameObject pj2;
    public GameObject pj3;
    public GameObject pj4;

    public PlayerBase p1Script;
    public PlayerBase p2Script;
    public PlayerBase p3Script;
    public PlayerBase p4Script;

    // Reference Matrix
    public float[,] repMatrix = new float[4, 4]
        {
        {0.5f, 0.8f, 0.5f, 0.2f},
        {0.8f, 0.2f, 0.3f, 0.4f},
        {0.5f, 0.3f, 0.6f, 0.8f},
        {0.2f, 0.4f, 0.8f, 0.9f}
        };

    public string playerName = "Default Player Name";
    public string pKingdomName = "Default Player Kingdom Name";

    private int playerPlaying = 0;

    private string p2kName;
    private string player2Name;
    private string p3kName;
    private string player3Name;
    private string p4kName;
    private string player4Name;



    private string[] pNames = new string[] { "name1", "name2", "name3", "name4", "name5" };
    private string[] kNames = new string[] { "kingdom1", "kingdom2", "kingdom3", "kingdom4", "kingdom5" };

    // Start is called before the first frame update
    void Start()
    {
        p2kName = pNames[1];
        player2Name = kNames[1];

        p3kName = pNames[2];
        player3Name = kNames[2];

        p4kName = pNames[3];
        player4Name = kNames[3];

        activePlayers = new bool[] { true, true, true, true };
        initializeClasses(p1Script, p2Script, p3Script, p4Script);

    }

    // Update is called once per frame
    void Update()
    {
        if (activePlayers[0] == false)
        {
            //Game Over man
        }
    }

    public void initializeClasses(PlayerBase p1Script, PlayerBase p2Script, PlayerBase p3Script, PlayerBase p4Script)
    {
        // PlayerBase Arguments:
        // id: 0 - 4 (0 = Human Player) | kName: String | pName: String
        // pType: Bool (true = Player, default = false) | army: int (default = 0)
        // civilians: int (default = 0) | money: int (default = 0) | nHeroes: int (default = 0)
        // prod: int (default = 0)

        // Player 1 - Human
        //pj1 = new PlayerBase(id: 0, kName: pKingdomName, pName: playerName,
        //                    pType: true, army: 30, civilians: 300, money: 500, prod: 0);
        p1Script = pj1.GetComponent<PlayerBase>();
        p1Script.pType = true;
        p1Script.actions = 3;
        p1Script.initializeHP();

        // Player 2 - CPU
        //pj2 = new PlayerBase(id: 1, kName: p2kName, pName: player2Name, army: 10,
        //                    civilians: 600, money: 400, prod: 0);
        //pj2.initializeHP();
        p2Script = pj2.GetComponent<PlayerBase>();
        p2Script.pName = player2Name;
        p2Script.kName = p2kName;
        p2Script.initializeHP();

        // Player 3 - CPU
        //pj3 = new PlayerBase(id: 2, kName: p3kName, pName: player3Name, army: 30,
        //                     civilians: 400, money: 500, nHeroes: 1, prod: 0);
        //pj3.initializeHP();
        p3Script = pj3.GetComponent<PlayerBase>();
        p3Script.pName = player3Name;
        p3Script.kName = p3kName;
        p3Script.initializeHP();

        // Player 4 - CPU
        //pj4 = new PlayerBase(id: 3, kName: p4kName, pName: player4Name, army: 50,
        //                     civilians: 300, money: 400, nHeroes: 2, prod: 0);
        //pj4.initializeHP();
        p4Script = pj4.GetComponent<PlayerBase>();
        p4Script.pName = player4Name;
        p4Script.kName = p4kName;
        p4Script.initializeHP();

    }

    void changePlayer()
    {
        playerPlaying++;
        if (playerPlaying == 3)
        {
            playerPlaying = 0;
        }
    }

}
