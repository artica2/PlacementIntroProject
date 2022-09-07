using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, MOVESELECT, PERFORMINGMOVES, WON, LOST }

public class BattleSystem : MonoBehaviour
{

    public BattleState state;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    private bool enemyHasSelectedMove;

    Trainer player;
    Trainer enemy;

    private Move currentlySelectedMove;


    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        SetupBattle();
    }

    private void Update()
    {
        if (state == BattleState.START)
        {
            SetupBattle();
        }
        if (state == BattleState.MOVESELECT)
        {
            MoveSelect();
        }
        if (state == BattleState.PERFORMINGMOVES)
        {
            MoveCalcs();
        }
    }

    void SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab);
        player = playerGO.GetComponent<Trainer>();

        GameObject enemyGO = Instantiate(enemyPrefab);
        enemy = enemyGO.GetComponent<Trainer>();

        enemyHasSelectedMove = false;

        state = BattleState.MOVESELECT;


    }


    void MoveSelect()
    {
        // enemy AI
        if (!enemyHasSelectedMove)
        {
            enemy.enemyMoveSelect();
            enemyHasSelectedMove = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentlySelectedMove = player.currentPokemon.pokemonMoves[0];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentlySelectedMove = player.currentPokemon.pokemonMoves[1];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentlySelectedMove = player.currentPokemon.pokemonMoves[2];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentlySelectedMove = player.currentPokemon.pokemonMoves[3];
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            state = BattleState.PERFORMINGMOVES;
        }

    }

    void MoveCalcs()
    {
        // check which pokemon is faster
        if (player.currentPokemon.speed > enemy.currentPokemon.speed)
        {
            player.currentPokemon.calculateDamage(enemy.currentPokemon, currentlySelectedMove);

            Debug.Log("EnemyIsSlower!!");

        }
        else
        {
            Debug.Log("EnemyIsFaster");
            enemy.currentPokemon.calculateDamage(player.currentPokemon, enemy.currentPokemon.pokemonMoves[0]);
        }

        state = BattleState.MOVESELECT;
        

    }





}
