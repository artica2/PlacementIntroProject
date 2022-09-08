using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, MOVESELECT, PERFORMINGMOVES, NEXTPOKEMONSELECT, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    [SerializeField]
    private Text playerName;
    [SerializeField]
    private Text enemyName;

    [SerializeField]
    private Slider playerHealth;
    [SerializeField]
    private Slider enemyHealth;

    [SerializeField]
    private Text move1;
    [SerializeField]
    private Text move2;
    [SerializeField]
    private Text move3;
    [SerializeField]
    private Text move4;

    [SerializeField]
    private Text actionText1;
    [SerializeField]
    private Text actionText2;

    bool TrainerPokemonFainted;

    public BattleState state;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    private bool enemyHasSelectedMove;

    Trainer player;
    Trainer enemy;

    private Move currentlySelectedMove;
    private Rorymon selectedRorymon;


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
        if (state == BattleState.NEXTPOKEMONSELECT)
        {
            NextRorymon(TrainerPokemonFainted);
        }

        // Update the UI to reflect the game state
        playerName.text = player.currentPokemon.name;
        enemyName.text = enemy.currentPokemon.name;

        playerHealth.value = player.currentPokemon.currentHealth / player.currentPokemon.maxHealth;
        enemyHealth.value = enemy.currentPokemon.currentHealth / enemy.currentPokemon.maxHealth;

        move1.text = player.currentPokemon.pokemonMoves[0].moveName;
        move2.text = player.currentPokemon.pokemonMoves[1].moveName;
        move3.text = player.currentPokemon.pokemonMoves[2].moveName;
        move4.text = player.currentPokemon.pokemonMoves[3].moveName;
    }

    void SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab);
        player = playerGO.GetComponent<Trainer>();
        player.currentPokemon = player.TrainerPokemon[0];

        foreach (Rorymon rorymon in player.TrainerPokemon)
        {
            rorymon.currentHealth = rorymon.maxHealth;
        }


        GameObject enemyGO = Instantiate(enemyPrefab);
        enemy = enemyGO.GetComponent<Trainer>();
        enemy.currentPokemon = enemy.TrainerPokemon[0];

        foreach (Rorymon rorymon in enemy.TrainerPokemon) {
            rorymon.currentHealth = rorymon.maxHealth;
        }


        enemyHasSelectedMove = false;

        state = BattleState.MOVESELECT;

        actionText1.text = "";
        actionText2.text = "";
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

            actionText1.text = "Player's " + player.currentPokemon.name + " Attacked with " + currentlySelectedMove.moveName + " dealing " + damage + " damage!";

            
            if (enemy.currentPokemon.currentHealth > 0)
            {
                damage = enemy.currentPokemon.calculateDamage(player.currentPokemon, enemy.currentPokemon.pokemonMoves[0]);
                player.currentPokemon.currentHealth -= damage;

                actionText2.text = "Enemy's " + enemy.currentPokemon.name + " Attacked with " + enemy.currentPokemon.pokemonMoves[0].moveName + " dealing " + damage + " damage!";
                if(player.currentPokemon.currentHealth <= 0)
                {
                    player.currentPokemon.hasFainted = true;
                    TrainerPokemonFainted = true;
                    state = BattleState.NEXTPOKEMONSELECT;
                    return;
                }
                
            }
            else
            {
                TrainerPokemonFainted = false;
                enemy.currentPokemon.hasFainted = true;
                state = BattleState.NEXTPOKEMONSELECT;
                return;
            }            

        }
        else
        {
            damage = enemy.currentPokemon.calculateDamage(player.currentPokemon, enemy.currentPokemon.pokemonMoves[0]);
            player.currentPokemon.currentHealth -= damage;
            actionText1.text = "Enemy's " + enemy.currentPokemon.name + " Attacked with " + enemy.currentPokemon.pokemonMoves[0].moveName + " dealing " + damage + " damage!";


            if (player.currentPokemon.currentHealth <= 0)
            {
                player.currentPokemon.hasFainted = true;
                TrainerPokemonFainted = true;
                state = BattleState.NEXTPOKEMONSELECT;
                return;
            }
            else
            {
                damage = player.currentPokemon.calculateDamage(enemy.currentPokemon, currentlySelectedMove);
                enemy.currentPokemon.currentHealth -= damage;

                actionText2.text = "Player's " + player.currentPokemon.name + " Attacked with " + currentlySelectedMove.moveName + " dealing " + damage + " damage!";
                if (enemy.currentPokemon.currentHealth <= 0)
                {
                    enemy.currentPokemon.hasFainted = true;
                    TrainerPokemonFainted = false;
                    state = BattleState.NEXTPOKEMONSELECT;
                    return;
                }
            }


        }

        state = BattleState.MOVESELECT;
    }

    void NextRorymon(bool TrainerPokemonFainted)
    {
        bool pokemonLeft = false;

        

        if (TrainerPokemonFainted) {
            foreach(Rorymon rorymon in player.TrainerPokemon) {
                if (!player.currentPokemon.hasFainted) {
                    pokemonLeft = true;
                }

            }
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                selectedRorymon = player.TrainerPokemon[0];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                selectedRorymon = player.TrainerPokemon[1];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                selectedRorymon = player.TrainerPokemon[2];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4)) {
                selectedRorymon = player.TrainerPokemon[3];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5)) {
                selectedRorymon = player.TrainerPokemon[4];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6)) {
                selectedRorymon = player.TrainerPokemon[5];
            }


            if (Input.GetKeyDown(KeyCode.Space) && !selectedRorymon.hasFainted) {
                player.currentPokemon = selectedRorymon;
            }
            else if (Input.GetKeyDown(KeyCode.Space) && selectedRorymon.hasFainted) {
                actionText1.text = "That Rorymon has fainted! Choose another!";
            }

            if (!pokemonLeft) {
                state = BattleState.LOST;
            }
        }






    }



}


