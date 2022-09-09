using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rorymon : MonoBehaviour
{

    public Move[] pokemonMoves = new Move[4];

    public Type rorymonType;

    public string name;
    public string type;

    public float attack;
    public float defence;
    public float speed;
    public float maxHealth;
    public float currentHealth;
    public bool hasFainted;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        hasFainted = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float calculateDamage(Rorymon DefensiveMon, Move moveUsed)
    {
        float damage = 0;
        damage = attack / DefensiveMon.defence * moveUsed.movePower;

        
        Debug.Log("Damage calculated: " + damage);

        return damage;
        

    }

}
