using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    // Start is called before the first frame update

    public Rorymon[] TrainerPokemon = new Rorymon[6];
    public Rorymon currentPokemon;


    void Start()
    {
        currentPokemon = TrainerPokemon[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void enemyMoveSelect()
    {
        Debug.Log("Selected Enemy Move");
    }
}
