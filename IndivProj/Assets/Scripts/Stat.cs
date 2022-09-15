using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType { attack, defence, specialAttack, specialDefence, speed, accuracy }

public class Stat
{
    public float baseStat;

    public int stage;

    public float value;

    public StatType statType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public void StartStat() {
        stage = 0;
    }

    public void UpdateStat() {
        if(stage < -6) {
            stage = -6;
        }
        if(stage > 6) {
            stage = 6;
        }

        value = baseStat * Mathf.Pow(1.5f, stage);


    }

}
