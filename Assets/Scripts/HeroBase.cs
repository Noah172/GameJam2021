using UnityEngine;
using System.Collections.Generic;

public class HeroBase : MonoBehaviour
{

    //================================================================================
    // Public Variables ==============================================================
    //================================================================================

    public PlayerBase pj2;
    public PlayerBase pj3;
    public PlayerBase pj4;

    public Dictionary<string, string[]> heroNamesAndSabotaje = new Dictionary<string, string[]>();
    public string[] heroNames = new string[]
    {
        "The Ayubacca", "The Suanfonzon", "Eskaroto", "Jax Sparrow", "Leonijazz", "Rapoleon Robaparte",
        "Screen Lantern", "The Tariquiles", "The Tesledador", "Tony Spark", "Undertaleker"
    };

    //================================================================================
    // Private Variables =============================================================
    //================================================================================


    //================================================================================
    // Start is called before the first frame update =================================
    //================================================================================
    void Start()
    {
        pj2 = GameObject.Find("Player2CPU1").GetComponent<PlayerBase>();
        pj3 = GameObject.Find("Player3CPU2").GetComponent<PlayerBase>();
        pj4 = GameObject.Find("Player4CPU3").GetComponent<PlayerBase>();
        // Llamar funcion para rellenar el diccionario.
    }

    //================================================================================
    // Update is called once per frame ===============================================
    //================================================================================
    void Update()
    {

    }

    //================================================================================
    // Coroutines ====================================================================
    //================================================================================


    //================================================================================    
    // Functions =====================================================================
    //================================================================================

    // Función para rellenar el diccionario.

}