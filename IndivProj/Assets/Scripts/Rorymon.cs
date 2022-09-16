using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rorymon : MonoBehaviour {
    public Move[] pokemonMovesPrefabs = new Move[4];

    public Move[] pokemonMoves = new Move[4];

    public Type rorymonType;

    public bool isFriendlyMon;

    public string rorymonName;

    public Sprite FrontalSprite;
    public Sprite BackwardSprite;

    public float attackStat;
    public float defenceStat;
    public float specialAttackStat;
    public float specialDefenceStat;
    public float speedStat;
    public float maxHealth;
    public float currentHealth;

    public Move currentlySelectedMove;

    public float level;

    public Stat attack = new Stat();
    public Stat defence = new Stat();
    public Stat specialAttack = new Stat();
    public Stat specialDefence = new Stat();
    public Stat speed = new Stat();
    public Stat accuracy = new Stat();
    public bool hasFainted;

    // Start is called before the first frame update
    void Start() {
        currentHealth = maxHealth;
        hasFainted = false;

        for (int i = 0; i < 4; i++) {
            if (pokemonMovesPrefabs[i]) {
                pokemonMoves[i] = Instantiate(pokemonMovesPrefabs[i].GetComponent<Move>());
            }
        }

        attack.statType = StatType.attack;
        defence.statType = StatType.defence;
        specialAttack.statType = StatType.specialAttack;
        specialDefence.statType = StatType.specialDefence;
        speed.statType = StatType.speed;
        accuracy.statType = StatType.accuracy;


        maxHealth = Mathf.Floor((0.01f * (2f * maxHealth) * level) + level + 10f);
        currentHealth = maxHealth;
        attack.baseStat = calculateInitialStat(attackStat);
        defence.baseStat = calculateInitialStat(defenceStat);
        specialAttack.baseStat = calculateInitialStat(specialAttackStat);
        specialDefence.baseStat = calculateInitialStat(specialDefenceStat);
        speed.baseStat = calculateInitialStat(speedStat);
        accuracy.baseStat = 100;

    }

    // Update is called once per frame
    void Update() {
        attack.UpdateStat();
        defence.UpdateStat();
        specialAttack.UpdateStat();
        specialDefence.UpdateStat();
        speed.UpdateStat();
        accuracy.UpdateStat();
    }

    public float calculateInitialStat(float stat) {
        float newStat = Mathf.Floor((0.01f * (2f * stat) * level) + 5f);
        return newStat;
    }

    public float calculateDamage(Rorymon DefensiveMon, Move moveUsed, bool crit) {
        float damage = 0;

        if (moveUsed.isPhysical) {
            damage = (((((2f * level) * 0.2f) + 2f) * moveUsed.movePower * (attack.value / DefensiveMon.defence.value)) / 50f) + 2f;
            Debug.Log("Attack Value: " + attack.value + " DefenceValue: " + DefensiveMon.defence.value);
            Debug.Log("Damage1: " + damage);

        } else {
            damage = (((((2 * level) * 0.2f) + 2) * moveUsed.movePower * (specialAttack.value / DefensiveMon.specialDefence.value)) / 50) + 2;
        }     

        if (crit) {
            if (moveUsed.isPhysical) {
                damage += (((((2f * level) * 0.2f) + 2f) * moveUsed.movePower * (attack.baseStat / DefensiveMon.defence.baseStat)) / 50f) + 2f;
            } else {
            damage = (((((2 * level) * 0.2f) + 2) * moveUsed.movePower * (specialAttack.baseStat / DefensiveMon.specialDefence.baseStat)) / 50) + 2;
        }
            
    }

        damage *= CalculateMultiplier(DefensiveMon, moveUsed);

        float damageRange = Random.Range(0f, 15f) + 85f;

        damage *= (damageRange / 100);

        return Mathf.Floor(damage);
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
            if (moveUsed.statToChange == StatType.accuracy) {
                if (moveUsed.targetsOtherMon) {
                    DefensiveMon.accuracy.stage += moveUsed.stageToChange;
                } else {
                    accuracy.stage += moveUsed.stageToChange;
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
            if (pokemonMoves[i]) {
                if (pokemonMoves[i].attackingMove && pokemonMoves[i].PP > 0) {
                    damage = calculateDamage(enemyMon, pokemonMoves[i], false);
                    if (damage > enemyMon.currentHealth) {
                        return pokemonMoves[i];
                    }
                    if (damage > highestDamage) {
                        highestDamage = damage;
                        highestDamageMove = pokemonMoves[i];
                    }

                } else if (pokemonMoves[i].PP > 0) {
                    moves.Add(pokemonMoves[i]);
                }
            }
        }
        if (highestDamageMove) {
            moves.Add(highestDamageMove);
        }
        int dummy = Random.Range(0, (int)moves.Count);
        return pokemonMoves[dummy];
    }

    public float CalculateMultiplier(Rorymon defensiveMon, Move moveUsed) {

        float multiplier = 1f;

        foreach (Type T in moveUsed.moveType.effectiveAgainst) {
            if (T == defensiveMon.rorymonType) {
                multiplier *= 2f;
            }
        }

        foreach (Type T in moveUsed.moveType.resistedBy) {
            if (T == defensiveMon.rorymonType) {
                multiplier *= 0.5f;
            }
        }

        foreach (Type T in moveUsed.moveType.doesntAffect) {
            if (T == defensiveMon.rorymonType) {
                multiplier *= 0.0f;
            }
        }

        if (defensiveMon.isFriendlyMon) {
            if (Arena.instance.friendlyLightScreen && !moveUsed.isPhysical) {
                multiplier *= 0.5f;
            }
            if (Arena.instance.friendlyReflect && moveUsed.isPhysical) {
                multiplier *= 0.5f;
            }
        } else if (!defensiveMon.isFriendlyMon) {
            if (Arena.instance.enemyLightScreen && !moveUsed.isPhysical) {
                multiplier *= 0.5f;
            }
            if (Arena.instance.enemyReflect && moveUsed.isPhysical) {
                multiplier *= 0.5f;
            }
        }

        if (Arena.instance.isSunny) {
            if (moveUsed.moveType.name == "Fire") {
                multiplier *= 1.5f;
            }
            if (moveUsed.moveType.name == "Water") {
                multiplier *= 0.5f;
            }
        }
        if (Arena.instance.isSunny) {
            if (moveUsed.moveType.name == "Water") {
                multiplier *= 1.5f;
            }
            if (moveUsed.moveType.name == "Fire") {
                multiplier *= 0.5f;
            }
        }

        if (rorymonType == moveUsed.moveType) {
            multiplier *= 1.5f;
        }

        return multiplier;
    }


}
