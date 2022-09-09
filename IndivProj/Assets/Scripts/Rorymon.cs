using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rorymon : MonoBehaviour
{
    public Move[] pokemonMoves = new Move[4];

    public Type rorymonType;

    public string rorymonName;

    public float attackStat;
    public float defenceStat;
    public float specialAttackStat;
    public float specialDefenceStat;
    public float speedStat;
    public float maxHealth;
    public float currentHealth;


    public Stat attack = new Stat();
    public Stat defence = new Stat();
    public Stat specialAttack = new Stat();
    public Stat specialDefence = new Stat();
    public Stat speed = new Stat();
    public bool hasFainted;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        hasFainted = false;

        attack.baseStat = attackStat;
        defence.baseStat = defenceStat;
        specialAttack.baseStat = specialAttackStat;
        specialDefence.baseStat = specialDefenceStat;
        speed.baseStat = speedStat;

    }

    // Update is called once per frame
    void Update()
    {
        attack.UpdateStat();
        defence.UpdateStat();
        specialAttack.UpdateStat();
        specialDefence.UpdateStat();
        speed.UpdateStat(); 
    }

    public float calculateDamage(Rorymon DefensiveMon, Move moveUsed)
    {
        float damage = 0;

        if (moveUsed.isPhysical) {
            damage = attack.value / DefensiveMon.defence.value * moveUsed.movePower;
        } else {
            damage = specialAttack.value / DefensiveMon.specialDefence.value * moveUsed.movePower;
        }

        
        Debug.Log("Damage calculated: " + damage);

        return damage;
        

    }

}
