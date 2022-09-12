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

        attack.statType = StatType.attackT;
        defence.statType = StatType.defenceT;
        specialAttack.statType = StatType.specialAttackT;
        specialDefence.statType = StatType.specialDefenceT;
        speed.statType = StatType.speedT;


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

    public void lowerStat(Rorymon DefensiveMon, bool TargetsOtherMon, Move moveUsed) {
        if (moveUsed.effect == moveEffect.stat) {
            if (moveUsed.statToChange == StatType.attackT) {
                if (TargetsOtherMon) {
                    DefensiveMon.attack.stage += moveUsed.stageToChange;
                } else {
                    attack.stage += moveUsed.stageToChange;
                }
            }
            if (moveUsed.statToChange == StatType.defenceT) {
                if (TargetsOtherMon) {
                    DefensiveMon.defence.stage += moveUsed.stageToChange;
                } else {
                    defence.stage += moveUsed.stageToChange;
                }
            }
            if (moveUsed.statToChange == StatType.specialAttackT) {
                if (TargetsOtherMon) {
                    DefensiveMon.specialAttack.stage += moveUsed.stageToChange;
                } else {
                    specialAttack.stage += moveUsed.stageToChange;
                }
            }
            if (moveUsed.statToChange == StatType.specialDefenceT) {
                if (TargetsOtherMon) {
                    DefensiveMon.specialDefence.stage += moveUsed.stageToChange;
                } else {
                    specialDefence.stage += moveUsed.stageToChange;
                }
            }
            if (moveUsed.statToChange == StatType.speedT) {
                if (TargetsOtherMon) {
                    DefensiveMon.speed.stage += moveUsed.stageToChange;
                } else {
                    speed.stage += moveUsed.stageToChange;
                }
            }
        }
    }
}
