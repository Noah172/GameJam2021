using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public bool choosing = false;
    public bool pType; // true = player - false = npc
    public int army;
    public int baseHp = 1000;
    public int civilians;
    public int currentHp;
    public int id;
    public int money;
    public int nHeroes = 0;
    public int prod; // produccion del reino
    public string kName; // nombre del reino

    
    
    

    // Class constructor
    public PlayerBase(bool pType, int army, int civilians, int id, int money, int nHeroes, int prod, string kName)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void recruitSoldiers(float repK)
    {
        // - disminuir civiles
        // - disminuir recursos (multiplicador)
        // - disminuir relaciones con otros paises
    }

    void increasePopulation(float repK)
    {
        // aumentar civiles
        // disminuye money
        // aumenta prod
    }

    void increaseRep(int id, int lvl)
    {
        // aumenta rep del pais seleccionado (se puede seleccionar a si mismo)
        // disminuye recurso con base en el lvl seleccionado (lvl actua como multiplicador)
    }

    void collectResourses(int civilSent)
    {
        // aumenta money (inmediato)
        // reduccion gradual de la poblacion enviada a la recolectar
        // reduccion de repK (propia)
    }

    void endTurn()
    {
        // finaliza el turno
    }

    void sabotage(int idTarget, int type, float repK)
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

    void attack(int idTarget)
    {
        // le informa al arbitro de la intencion del ataque
        // envia this.id y idTarget
    }

    void recruitHero()
    {
        // aqui se reclutan heroes (+1)
        // disminuye money (++)
        // aumenta la rep propia
    }
}
