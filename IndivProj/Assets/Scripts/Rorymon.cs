using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rorymon : MonoBehaviour {
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

    public Move currentlySelectedMove;


    public Stat attack = new Stat();
    public Stat defence = new Stat();
    public Stat specialAttack = new Stat();
    public Stat specialDefence = new Stat();
    public Stat speed = new Stat();
    public bool hasFainted;

    // Start is called before the first frame update
    void Start() {
        currentHealth = maxHealth;
        hasFainted = false;

        attack.statType = StatType.attack;
        defence.statType = StatType.defence;
        specialAttack.statType = StatType.specialAttack;
        specialDefence.statType = StatType.specialDefence;
        speed.statType = StatType.speed;


        attack.baseStat = attackStat;
        defence.baseStat = defenceStat;
        specialAttack.baseStat = specialAttackStat;
        specialDefence.baseStat = specialDefenceStat;
        speed.baseStat = speedStat;

    }

    // Update is called once per frame
    void Update() {
        attack.UpdateStat();
        defence.UpdateStat();
        specialAttack.UpdateStat();
        specialDefence.UpdateStat();
        speed.UpdateStat();
    }

    public float calculateDamage(Rorymon DefensiveMon, Move moveUsed) {
        float damage = 0;

        if (moveUsed.isPhysical) {
            damage = attack.value / DefensiveMon.defence.value * moveUsed.movePower;
        } else {
            damage = specialAttack.value / DefensiveMon.specialDefence.value * moveUsed.movePower;
        }

        Debug.Log("Damage calculated: " + damage);

        return damage;
    }

    public void lowerStat(Rorymon DefensiveMon, Move moveUsed) {
        if (moveUsed.statMove) {
            if (moveUsed.statToChange == StatType.attack) {
                if (moveUsed.targetsOtherMon) {
                    DefensiveMon.attack.stage += moveUsed.stageToChange;
                } else {
                    attack.stage += moveUsed.stageToChange;
                }
            }
            if (moveUsed.statToChange == StatType.defence) {
                if (moveUsed.targetsOtherMon) {
                    DefensiveMon.defence.stage += moveUsed.stageToChange;
                } else {
                    defence.stage += moveUsed.stageToChange;
                }
            }
            if (moveUsed.statToChange == StatType.specialAttack) {
                if (moveUsed.targetsOtherMon) {
                    DefensiveMon.specialAttack.stage += moveUsed.stageToChange;
                } else {
                    specialAttack.stage += moveUsed.stageToChange;
                }
            }
            if (moveUsed.statToChange == StatType.specialDefence) {
                if (moveUsed.targetsOtherMon) {
                    DefensiveMon.specialDefence.stage += moveUsed.stageToChange;
                } else {
                    specialDefence.stage += moveUsed.stageToChange;
                }
            }
            if (moveUsed.statToChange == StatType.speed) {
                if (moveUsed.targetsOtherMon) {
                    DefensiveMon.speed.stage += moveUsed.stageToChange;
                } else {
                    speed.stage += moveUsed.stageToChange;
                }
            }
        }
    }

    public Move SelectMove(Rorymon enemyMon) {
        float damage = 0f;
        float highestDamage = 0f;
        Move highestDamageMove = null;
        List<Move> moves = new List<Move>();
        for (int i = 0; i < 4; i++) {
            if (pokemonMoves[i].attackingMove) {
                damage = calculateDamage(enemyMon, pokemonMoves[i]);
                if (damage > enemyMon.currentHealth) {
                    return pokemonMoves[i];
                }
                if (damage > highestDamage) {
                    highestDamage = damage;
                    highestDamageMove = pokemonMoves[i];
                }

            } else {
                moves.Add(pokemonMoves[i]);
            }
        }
        moves.Add(highestDamageMove);
        int dummy = Random.Range(0, (int)moves.Count);
        return pokemonMoves[dummy];
    }
}



