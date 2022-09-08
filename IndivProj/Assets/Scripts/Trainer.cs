using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] pokemonPrefabs = new GameObject[6];

    public Rorymon[] TrainerPokemon = new Rorymon[6];


    public Rorymon currentPokemon;


    void Start()
    {
        
    }

    public void CreateTrainer() {
        for (int i = 0; i < 6; i++) {
            TrainerPokemon[i] = Instantiate(pokemonPrefabs[i].GetComponent<Rorymon>());
        }

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
