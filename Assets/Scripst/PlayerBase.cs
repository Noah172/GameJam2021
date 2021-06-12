using UnityEngine;

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
    public string kName; // Nombre del reino
    public string pName; //Nombre del jugador


    public float[,] repMax = GameObject.Find("Arbiter").GetComponent<float[,]>();
    private int numOpions = 8; // Options for the CPU player.
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
        ownRep = repMax[this.id, this.id];
        if (pType == false)
        {
            while (actions != 0)
            {
                int randint = Random.Range(0, numOpions);

                switch (randint)
                {
                    case 0:
                        recruitSoldiers(repMax[this.id, this.id], 10);
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    case 7:
                        break;
                    default:
                        break;
                }

            }
        }
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
        int moneyReduction = (int)(this.money - 100 * repK);
        if (moneyReduction > 0)
        {
            // - Decrease Civilians
            this.civilians -= numSoldiers * 10;
            // - Alter Production Rate
            this.prod = this.civilians / 2;
            // - Reduce Resources
            this.money -= moneyReduction;
            // - Lower Relations with other countries
            affectRelations(repMax, this.id, -0.2f);
            // - Reduce number of actions
            reduceActions();
        }
        // No money
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
        int moneyReduction = (int)(this.money - 50 * repK);
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
        int moneyReduction = this.money - 100 * lvl;
        //affectRelations()
        // Increases reputation with target country (can select oneself)

        // disminuye recurso con base en el lvl seleccionado (lvl actua como multiplicador)
    }

    void collectResourses(int civilSent) // 4
    {
        // aumenta money (inmediato)
        // reduccion gradual de la poblacion enviada a la recolectar
        // reduccion de repK (propia)
    }

    void endTurn() // 5
    {
        // finaliza el turno
        actions = 0;
        choosing = false;
    }

    void sabotage(int idTarget, int type, float repK) // 6
    {
        // aumenta el tipo de recuso seleccionado
        // disminye el recurso seleccionado del objetivo
        // disminuye reputacion con el otro pais en caso de fallo
        // la probabilidad de exito depende de la reputacion
    }

    void playerGameOver()
    {
        // valiste vrg papuh
        // envia senial al jugador de que perdio
    }

    void attack(int idTarget) // 7
    {
        // le informa al arbitro de la intencion del ataque
        // envia this.id y idTarget

        //check health
    }

    void recruitHero() // 8
    {
        // aqui se reclutan heroes (+1)
        // disminuye money (++)
        // aumenta la rep propia
    }

    void checkMoney()
    {
        if (this.money < 0)
        {
            this.money = 0;
        }
    }

    public void initializeHP()
    {
        this.maxHp = baseHp + nHeroes * 250 + army * 50 + civilians;
        this.currentHp = this.maxHp;
        this.prod = civilians / 2;
        Debug.Log($"Player {this.id + 1}:\n Player Name: { this.pName}\n Player Kingdom Name: { this.kName}\n Player HP: {this.currentHp}");
    }

    void reduceActions()
    {
        this.actions--;
    }

    void affectRelations(float[,] matrix, int ownId, float effect, int targetId = -1)
    {
        if (targetId == -1)
        {
            int i;
            for (i = 0; i < 4; i++)
            {
                if (i != ownId)
                {
                    matrix[ownId, i] += effect;
                }
            }
        }
        else
        {
            matrix[ownId, targetId] += effect;
        }
    }

}
