using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArenaChange { friendlyLightScreen, enemyLightScreen, friendlyReflect, enemyReflect, rain, sun, none }

public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    public Type moveType;
    public string moveName;
    public bool attackingMove;
    public bool changesArena;
    public bool statMove;
    public float movePower;

    public float accuracy;

    public bool isPhysical;

    public StatType statToChange;

    public ArenaChange arenaChange;

    public int stageToChange;

    public bool targetsOtherMon;



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
