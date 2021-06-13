using UnityEngine;
using System.Collections.Generic;

public class PlayerBase : MonoBehaviour
{
    public GameObject arbiter;
    public bool choosing = false;
    public bool playing = true;
    public bool pType; // true = player - false = npc
    public int actions = 0; // Number of actions given by the arbiter to each player.
    public int army;
    public int baseHp = 1000;
    public int civilians;
    public int currentHp;
    public int id;
    public int maxHp;
    public int money;
    public int nHeroes = 0;
    public int prod; // Produccion del reino
    public float[,] repMax;
    public float surrenderChance = 0.0f;
    public string kName; // Nombre del reino
    public string pName; //Nombre del jugador

    private bool[] activePj;
    private int numOpions = 9; // Options for the CPU player.
    private int[] attackOptions;
    private float ownRep;


    // Class constructor
    public PlayerBase(int id, string kName, string pName, bool pType = false,
                      int army = 0, int civilians = 0, int money = 0, int nHeroes = 0,
                      int prod = 0)
    {
        this.pType = pType;
        this.army = army;
        this.civilians = civilians;
        this.id = id;
        this.money = money;
        this.nHeroes = nHeroes;
        this.prod = prod;
        this.kName = kName;
        this.pName = pName;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // No sé si debería ir aqui
        ownRep = repMax[this.id, this.id];
        if (pType == false)
        {
            while (actions != 0)
            {
                int randint = Random.Range(0, numOpions);

                switch (randint)
                {
                    case 0:
                        recruitSoldiers(repMax[this.id, this.id], Random.Range(1, 3) * 10); // 1 Recruit Soldiers.
                        break;
                    case 1:
                        increasePopulation(this.ownRep, Random.Range(0, 6) * 10); // 2 Increase population.
                        break;
                    case 2:
                        increaseRep(this.id, Random.Range(0, 5), Random.Range(0, 4)); // 3 Increase Reputation.
                        break;
                    case 3:
                        collectResourses((int)(this.civilians * 0.1f)); // 4 Collect Resources.
                        break;
                    case 4:
                        int attackedPlayer = -1;
                        // Check if player can be attacked.
                        while (attackedPlayer == -1)
                        {
                            attackedPlayer = attackOptions[Random.Range(0, attackOptions.Length)];
                            // Player is not playing
                            if (activePj[attackedPlayer] == false)
                            {
                                attackedPlayer = -1;
                            }
                        }
                        float repAttac = repMax[this.id, attackedPlayer];
                        sabotage(attackedPlayer, Random.Range(0, 1), repAttac); // 5 Sabotage country.
                        break;
                    case 5:
                        int atkTarget = -1;
                        // Check if player can be attacked.
                        while (atkTarget == -1)
                        {
                            atkTarget = attackOptions[Random.Range(0, attackOptions.Length)];
                            // Player is not playing
                            if (activePj[atkTarget] == false)
                            {
                                atkTarget = -1;
                            }
                        }
                        attack(atkTarget, repMax[this.id, atkTarget]); // 6 Attack country.
                        break;
                    case 6:
                        recruitHero(); // 7 Recruit a hero - CPU Only.
                        break;
                    case 7:
                        endTurn(); // 8 End Turn.
                        break;
                    case 8:
                        int surrenderPlayer = -1;
                        // Check if player can be attacked.
                        while (surrenderPlayer == -1)
                        {
                            surrenderPlayer = attackOptions[Random.Range(0, attackOptions.Length)];
                            // Player is not playing
                            if (activePj[surrenderPlayer] == false)
                            {
                                attackedPlayer = -1;
                            }
                        }
                        demandSurrender(surrenderPlayer); // 9 Demand Surrender.
                        break;
                    default:
                        playerGameOver();
                        break;
                }

            }
        }
        //////
    }

    /// <summary>
    /// Method to increase Military Strengh, reduces population (and production by proxy), money 
    /// and reputation with other countries.
    /// </summary>
    /// <param name="repK">Internal reputation, affects purchace prices</param>
    /// <param name="numSoldiers">Number of soldiers to recruit</param>
    void recruitSoldiers(float repK, int numSoldiers) // 1
    {
        // Check if player has the money to complete action
        int moneyReduction = (int)(this.money - (numSoldiers * 100 * (1 - repK)));
        // Check if player has the civilians to complete action
        int reducedCivs = this.civilians - numSoldiers * 10;
        //
        if (moneyReduction > 0 && reducedCivs > 0)
        {
            // - Decrease Civilians
            this.civilians -= reducedCivs;
            // - Alter Production Rate
            this.prod = this.civilians / 2;
            // - Reduce Resources
            this.money -= moneyReduction;
            // - Lower relations with other countries
            affectRelations(repMax, this.id, -0.2f);
            // - Reduce number of actions
            reduceActions();
        }
        // No money or civilians
        else
        {
            Debug.Log("Action cannot be completed");
        }
    }

    /// <summary>
    /// Action to increase population, increases production rates and reduces money.
    /// </summary>
    /// <param name="repK"></param>
    /// <param name="numCivil"></param>
    void increasePopulation(float repK, int numCivil) // 2
    {
        // Check if player has the money to complete action
        int moneyReduction = (int)(this.money - (numCivil * 50 * (1 - repK)));
        if (moneyReduction > 0)
        {
            // Increase number of civilians
            this.civilians += numCivil;
            // Affect Production rates
            this.prod = civilians / 2;
            // Reduce money
            this.money -= moneyReduction;
            // Reduce number of actions
            reduceActions();
        }
        // No money
        else
        {
            Debug.Log("Action cannot be completed");
        }
    }

    /// <summary>
    /// Action to increase Reputation with other countries (can target itself), reduces money.
    /// </summary>
    /// <param name="ownId">"Sender" id to match in the Reputation Matrix.</param>
    /// <param name="targetId">"Target" id to match in the Reputation Matrix.</param>
    /// <param name="lvl">Increase tier, affects money.</param>
    void increaseRep(int ownId, int targetId, int lvl) // 3
    {
        // Check if can buy
        int moneyReduction = this.money - 100 * lvl;
        if (moneyReduction > 0)
        {
            float effect;
            switch (lvl)
            {
                case (1):
                    effect = 0.1f;
                    break;
                case (2):
                    effect = 0.15f;
                    break;
                case (3):
                    effect = 0.20f;
                    break;
                default:
                    effect = 0.0f;
                    break;
            }
            // Increases reputation with target country (can select oneself)
            affectRelations(repMax, ownId, effect, targetId);
            // Reduce money
            this.money -= moneyReduction;
            // Reduce actions
            reduceActions();
        }
        // No money
        else
        {
            Debug.Log("Action cannot be completed");
        }
    }

    /// <summary>
    /// Sends a number of civilians to collect resources. Decreases civilians, increases resources.
    /// </summary>
    /// <param name="civilSent">Number of civilians send to gather resources.</param>
    void collectResourses(int civilSent) // 4
    {
        // Check if can send civilians
        int reducedCiv = this.civilians - civilSent;

        if (reducedCiv > 0)
        {
            // Number of civilans lost in the mission.
            int civsLost = (int)(civilSent * Random.Range(0.0f, 1.0f));
            if ((civilSent - civsLost) != 0) // Gives money
            {
                // Adds money according to returning civilans.
                this.money += (civilSent - civsLost) * 5;
            }
            else
            {
                Debug.Log("All civilans were lost during the gathering.");
                affectRelations(repMax, this.id, -0.2f, this.id);
            }
            // Reduces civilians according to losses.
            this.civilians -= civsLost;
            // Reduce actions
            reduceActions();
        }
        // Not enough civilians
        else
        {
            Debug.Log("Action cannot be completed");
        }

    }

    /// <summary>
    /// Sabotages a target country. Sabotage usually means the attacker gets something from the defender.
    /// It's a probability event: 
    ///     -> On success the attacker gets resources from the attacked without damage to the relations.
    ///     -> On failure the attacker gets nothing and worsens the relations between the countries.
    /// The chance of failure or success depends on the relationship value between the countries.
    /// If the countries have relationship of over 90%, the sabotage option will not be available.
    /// </summary>
    /// <param name="idTarget">Country Id to sabotage</param>
    /// <param name="type">Type of sabotage: 0 - Money | 1 - Civilians | 2 - Heroes (Human Player Only)</param>
    /// <param name="repK">Reputation between the countries.</param>
    void sabotage(int idTarget, int type, float repK) // 5
    {
        int threshold = (int)(repK * 100);
        if (threshold >= 90)
        {
            Debug.Log("Action cannot be executed.");
        }
        else
        {
            PlayerBase targetPlayer = selectPlayerTarget(idTarget);
            int result = Random.Range(0, 101);
            if (result < threshold)
            {
                switch (type)
                {
                    case (0):
                        int availableMoney = targetPlayer.money;
                        int robbedMoney = (int)Random.Range(availableMoney * 0.1f, availableMoney * 0.25f);
                        targetPlayer.reduceMoney(robbedMoney);
                        this.money += robbedMoney;
                        break;
                    case (1):
                        int availableCivs = targetPlayer.civilians;
                        int robbedCivs = (int)Random.Range(availableCivs * 0.1f, availableCivs * 0.25f);
                        targetPlayer.reduceCivilians(robbedCivs);
                        this.civilians += robbedCivs;
                        break;
                    case (2):
                        Debug.Log("Heroes not implemented yet :(");
                        break;
                }
            }
            else
            {
                affectRelations(repMax, this.id, -0.2f, idTarget);
            }
            reduceActions();
        }
    }

    /// <summary>
    /// Attack another country. The attack is determined by the military forces and heroes in each country.
    /// It's a probability event: 
    ///     -> Each country rolls nD10 dices where n = (army / 10 + nHeroes).
    ///     -> The rolls are stored in lists and are ordered in ascendent fashion.
    ///     -> Compare with the shortest list as reference.
    ///     -> If Attacker(A)[i] > Defender(D)[i] -> Defender losses 10 armies.
    ///     -> if A[i] < D[i] -> Attacker losses 10 armies.
    ///     -> if A[i] == D[i] -> BOTH countries loss 10 armies.
    ///     -> if len(A) > len(D) -> the rest of the results (multiplied by 10) will be deducted 
    ///                              to the base Hp of the country.
    ///     -> if D has no armies, all attack will go to baseHp multiplied by 20.
    /// If attacker has a reputation of over 90% with defender, the attack won't commence.
    /// </summary>
    /// <param name="idTarget"></param>
    void attack(int idTarget, float repK) // 6
    {
        if (((int)repK * 100) >= 90)
        {
            Debug.Log("Action cannot be executed");
        }
        else
        {
            PlayerBase targetPlayer = selectPlayerTarget(idTarget);
            int dieA = (this.army / 10) + this.nHeroes;
            int dieD = (targetPlayer.army / 10) + targetPlayer.nHeroes;
            List<int> throwA = new List<int>();
            int i;

            for (i = 0; i < dieA; i++)
            {
                throwA.Add(Random.Range(0, 11));
            }
            throwA.Sort();

            switch (dieD)
            {
                case (0):
                    for (i = 0; i < dieA; i++)
                    {
                        targetPlayer.baseHp -= throwA[i] * 20;
                    }
                    targetPlayer.calculatecurrentHP();
                    targetPlayer.calculateSurrender();
                    break;

                default:
                    List<int> throwD = new List<int>();
                    for (i = 0; i < dieD; i++)
                    {
                        throwD.Add(Random.Range(0, 11));
                    }
                    throwD.Sort();
                    int limiter = dieA < dieD ? dieA : dieD;
                    int remainder = dieA - dieD;
                    for (i = 0; i < limiter; i++)
                    {
                        // Attacker losses squad.
                        if (throwA[i] < throwD[i])
                        {
                            this.reduceArmy();
                        }
                        // Defender losses squad.
                        else if (throwA[i] > throwD[i])
                        {
                            targetPlayer.reduceArmy();
                        }
                        // Both loss a squad.
                        else
                        {
                            this.reduceArmy();
                            targetPlayer.reduceArmy();
                        }
                    }
                    // If attacker has more dice than defencer
                    if (remainder > 0)
                    {
                        while (i < dieA)
                        {
                            targetPlayer.baseHp -= throwA[i] * 10;
                            i++;
                        }
                    }
                    calculatecurrentHP();
                    calculateSurrender();
                    targetPlayer.calculatecurrentHP();
                    targetPlayer.calculateSurrender();
                    break;
            }
            reduceActions();
        }
    }

    /// <summary>
    /// Uses resources to recruid a hero. Only CPU player.
    /// </summary>
    void recruitHero() // 7
    {
        //Check if money is enough.
        if (this.money >= 500)
        {
            if (nHeroes < 4)
            {
                this.nHeroes++;
                this.money -= 500;
                affectRelations(repMax, this.id, 0.25f, this.id);
                reduceActions();
            }

            // Asignar heroe al país que lo "compra".

        }
    }

    /// <summary>
    /// Ends the turn before using all actions available.
    /// </summary>
    void endTurn() // 8
    {
        // finaliza el turno
        actions = 0;
        choosing = false;
    }

    /// <summary>
    /// Demands surrender from a country.
    /// </summary>
    /// <param name="targetPlayerID">Target country.</param>
    void demandSurrender(int targetPlayerID) // 9
    {
        PlayerBase targetPlayer = selectPlayerTarget(targetPlayerID);
        int surrenderTarget = (int)(targetPlayer.surrenderChance * 100);
        if (Random.Range(0, 101) < surrenderTarget)
        {
            targetPlayer.playerGameOver();
        }
        reduceActions();
    }


    /// <summary>
    /// Signals to the arbiter that the player is out of the game.
    /// </summary>
    void playerGameOver() // Default
    {
        // valiste vrg papuh
        // envia senial al jugador de que perdio
        GameObject.Find("Arbiter").GetComponent<Arbiter>().activePlayers[this.id] = false;
    }

    /// <summary>
    /// Checks if money is negative (Check if this function is necesary).
    /// </summary>
    void checkMoney()
    {
        if (this.money < 0)
        {
            this.money = 0;
        }
    }

    /// <summary>
    /// Initialization for each player. Called only by the arbiter at the beginning of each game.
    /// </summary>
    public void initializeHP()
    {
        this.maxHp = baseHp + nHeroes * 250 + army * 50 + civilians;
        calculatecurrentHP();
        this.repMax = GameObject.Find("Arbiter").GetComponent<Arbiter>().repMatrix; //Reputation Matrix
        this.activePj = GameObject.Find("Arbiter").GetComponent<Arbiter>().activePlayers;
        this.prod = civilians / 2;
        Debug.Log($"Player {this.id + 1}:\n Player Name: { this.pName}\n Player Kingdom Name: { this.kName}\n Player HP: {this.currentHp}");
        switch (this.id)
        {
            case (0):
                this.attackOptions = new int[] { 1, 2, 3 };
                break;
            case (1):
                this.attackOptions = new int[] { 0, 2, 3 };
                break;
            case (2):
                this.attackOptions = new int[] { 0, 1, 3 };
                break;
            case (3):
                this.attackOptions = new int[] { 0, 1, 2 };
                break;
        }
    }

    /// <summary>
    /// Support function to control action reduction.
    /// </summary>
    void reduceActions()
    {
        this.actions--;
    }

    /// <summary>
    /// Function that controls relations between countries.
    /// </summary>
    /// <param name="matrix">Reputation matrix, belongs to the arbiter.</param>
    /// <param name="ownId">Id of the "Sender" country.</param>
    /// <param name="effect">Effect on the "Target" country (can be + or -).</param>
    /// <param name="targetId">Id of the "Target" country.</param>
    void affectRelations(float[,] matrix, int ownId, float effect, int targetId = -1)
    {
        // TargetId = -1 means the action affects all the other countries.
        if (targetId == -1)
        {
            int i;
            for (i = 0; i < 4; i++)
            {
                if (i != ownId)
                {
                    matrix[ownId, i] += effect;

                    // Limiter 0.0f <= rep <= 1.0f
                    if (matrix[ownId, i] < 0.0f)
                        matrix[ownId, i] = 0.0f;
                    else if (matrix[ownId, i] > 1.0f)
                        matrix[ownId, i] = 1.0f;
                }
            }
        }
        // TargetId != -1 means the action affects only one country.
        else
        {
            matrix[ownId, targetId] += effect;

            // Limiter 0.0f <= rep <= 1.0f
            if (matrix[ownId, targetId] < 0.0f)
                matrix[ownId, targetId] = 0.0f;
            else if (matrix[ownId, targetId] > 1.0f)
                matrix[ownId, targetId] = 1.0f;
        }
    }

    /// <summary>
    /// Selects a player inside the game. Used for calculus.
    /// </summary>
    /// <param name="idTarget">Id of the player to link.</param>
    /// <returns>Script of the player.</returns>
    private PlayerBase selectPlayerTarget(int idTarget)
    {// Select Target GameObject.
        switch (idTarget)
        {
            case (0):
                return GameObject.Find("Arbiter").GetComponent<Arbiter>().p1Script;
            case (1):
                return GameObject.Find("Arbiter").GetComponent<Arbiter>().p2Script;
            case (2):
                return GameObject.Find("Arbiter").GetComponent<Arbiter>().p3Script;
            default:
                return GameObject.Find("Arbiter").GetComponent<Arbiter>().p4Script;
        }
    }

    /// <summary>
    /// Reduces money by a certain ammount.
    /// </summary>
    /// <param name="value">Ammount of money to reduce.</param>
    public void reduceMoney(int value)
    {
        this.money -= value;
        checkMoney();
    }

    /// <summary>
    /// Reduces Civilians by a value.
    /// </summary>
    /// <param name="value">Value to reduce civilians.</param>
    public void reduceCivilians(int value)
    {
        this.civilians -= value;
        if (this.civilians < 0)
            this.civilians = 0;
    }

    /// <summary>
    /// Reduces army as result of battle.
    /// </summary>
    public void reduceArmy()
    {
        this.army -= 10;
        if (army < 0)
            army = 0;
    }

    void calculatecurrentHP()
    {
        this.currentHp = baseHp + nHeroes * 250 + army * 50 + civilians;
    }

    void calculateSurrender()
    {
        if (this.id == 0)
            this.surrenderChance = 0.0f;
        this.surrenderChance = (this.baseHp / this.maxHp)
                                + ((this.army * 10) / (this.army * 10 + this.civilians))
                                + this.ownRep;
    }

}
