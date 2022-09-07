using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, MOVESELECT, PERFORMINGMOVES, NEXTPOKEMONSELECT, WON, LOST }

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
        float damage = 0;
        // check which pokemon is faster
        if (player.currentPokemon.speed > enemy.currentPokemon.speed)
        {
            damage = player.currentPokemon.calculateDamage(enemy.currentPokemon, currentlySelectedMove);
            enemy.currentPokemon.currentHealth -= damage;
            Debug.Log("EnemyIsSlower!!");
            if (enemy.currentPokemon.currentHealth > 0)
            {
                damage = enemy.currentPokemon.calculateDamage(player.currentPokemon, enemy.currentPokemon.pokemonMoves[0]);
                player.currentPokemon.currentHealth -= damage;
                if(player.currentPokemon.currentHealth <= 0)
                {
                    state = BattleState.NEXTPOKEMONSELECT;
                    return;
                }
                
            }
            else
            {
                state = BattleState.NEXTPOKEMONSELECT;
                return;
            }            

        }
        else
        {
            damage = enemy.currentPokemon.calculateDamage(player.currentPokemon, enemy.currentPokemon.pokemonMoves[0]);
            player.currentPokemon.currentHealth -= damage;
            if (player.currentPokemon.currentHealth <= 0)
            {
                state = BattleState.NEXTPOKEMONSELECT;
                return;
            }
            else
            {
                damage = player.currentPokemon.calculateDamage(enemy.currentPokemon, currentlySelectedMove);
                enemy.currentPokemon.currentHealth -= damage;
                if(enemy.currentPokemon.currentHealth <= 0)
                {
                    state = BattleState.NEXTPOKEMONSELECT;
                    return;
                }
            }


        }

        state = BattleState.MOVESELECT;
    }
}
