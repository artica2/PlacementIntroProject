using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public enum moveEffect { offensive, stat }

public class Move : MonoBehaviour
{
    // Start is called before the first frame update

    public Type moveType;
    public string moveName;
    public bool attackingMove;
    public bool statMove;
    public float movePower;

    public bool isPhysical;

    public StatType statToChange;

    public int stageToChange;

    public bool targetsOtherMon;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float CalculateMultiplier(Rorymon rorymon) {

        float multiplier = 1f;

        foreach (Type T in moveType.effectiveAgainst) {
            if (T == rorymon.rorymonType) {
                multiplier *= 2f;
            }
        }

        foreach (Type T in moveType.resistedBy) {
            if (T == rorymon.rorymonType) {
                multiplier *= 0.5f;
            }
        }

        foreach (Type T in moveType.doesntAffect) {
            if (T == rorymon.rorymonType) {
                multiplier *= 0.0f;
            }
        }

        return 0;
    }
}
