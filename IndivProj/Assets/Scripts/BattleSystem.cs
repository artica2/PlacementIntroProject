using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, MOVESELECT, PERFORMINGMOVES, NEXTPOKEMONSELECT, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    // text fields for the names of the trainer's Rorymons
    [SerializeField]
    private Text playerName;
    [SerializeField]
    private Text enemyName;

    // Sliders to display the health of each pokemon
    [SerializeField]
    private Slider playerHealth;
    [SerializeField]
    private Slider enemyHealth;

    // text to display available moves
    [SerializeField]
    private Text[] moveText = new Text[4];

    // text to display the pokemon names
    [SerializeField]
    private Text[] rorymonNames = new Text[6];

    // UI text to explain what is happening
    [SerializeField]
    private Text actionText1;
    [SerializeField]
    private Text actionText2;

    // a bool to be used in the event a pokemon faints. Tells the game whether the fainted pokemon is friendly or Computer operated
    bool TrainerPokemonFainted;

    // the gamestate
    public BattleState state;

    // prefabs for the 2 trainers in the scene
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    // a bool to check if the enemy pokemon has already chosen its move for the next turn
    private bool enemyHasSelectedMove;

    // the two trainers in the scene
    Trainer player;
    Trainer enemy;

    // tracker bools for selecting moves and pokemon (and having this selection be displayed properly on the UI)
    private Move currentlySelectedMove;
    private Rorymon selectedRorymon;

    // A sample of colors
    private Color red = new Color(1, 0, 0, 1);
    private Color blue = new Color(0, 0, 1, 1);
    private Color gray = new Color(0.5f, 0.5f, 0.5f, 1);
    private Color green = new Color(0, 1, 0, 1);
    private Color black = new Color(0, 0, 0, 1);

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START; // set the state
    }

    private void Update()
    {
        if (state == BattleState.START)
        {
            SetupBattle(); // set up the battle
        }
        if (state == BattleState.MOVESELECT)
        {
            // set up the UI
            // set the move UI to be active
            for(int i = 0; i < 4; i++) {
                moveText[i].gameObject.SetActive(true);

            }
            // set the pokemon selection UI to be inactive
            for (int i = 0; i < 6; i++) {
                rorymonNames[i].gameObject.SetActive(false);

            }
            // run the move select function
            MoveSelect();
        }
        if (state == BattleState.PERFORMINGMOVES)
        {
            MoveCalcs(); // perform the moves
        }
        if (state == BattleState.NEXTPOKEMONSELECT)
        {
            // set the move UI to be inactive
            for (int i = 0; i < 4; i++) {
                moveText[i].gameObject.SetActive(false);

            }
            // set the pokemon select UI to be active
            for (int i = 0; i < 6; i++) {
                rorymonNames[i].gameObject.SetActive(true);

            }

            NextRorymon(TrainerPokemonFainted); // run the function to select the next pokemon
        }

        // Update the UI to reflect the game state
        playerName.text = player.currentPokemon.rorymonName;
        enemyName.text = enemy.currentPokemon.rorymonName;

        playerHealth.value = player.currentPokemon.currentHealth / player.currentPokemon.maxHealth;
        enemyHealth.value = enemy.currentPokemon.currentHealth / enemy.currentPokemon.maxHealth;
        // set the currently selected move to stand out as green 
        for (int i = 0; i < 4; i++) {
            if (player.currentPokemon.pokemonMoves[i]){
            moveText[i].text = player.currentPokemon.pokemonMoves[i].moveName;

            if (player.currentPokemon.pokemonMoves[i].attackingMove) {
                moveText[i].text = moveText[i].text + "          " + player.currentPokemon.pokemonMoves[i].movePower;
            }

            if (player.currentPokemon.pokemonMoves[i] == currentlySelectedMove) {
                moveText[i].color = green;
            } else {
                moveText[i].color = black;
            }
        }

        }

        // PLACEHOLDERS FOR NOW
        if( state == BattleState.WON) {
            actionText1.text = "Congratulations, you won!";
        }
        if (state == BattleState.LOST) {
            actionText1.text = "You lost :/ Better luck next time!";
        }
    }

    void SetupBattle()
    {
        // create the player
        GameObject playerGO = Instantiate(playerPrefab);
        player = playerGO.GetComponent<Trainer>();
        player.currentPokemon = player.TrainerPokemon[0];
        player.CreateTrainer();
        // set up pokemon PLACEHOLDER IS THIS NEEDED???
        foreach (Rorymon rorymon in player.TrainerPokemon)
        {
            rorymon.currentHealth = rorymon.maxHealth;
        }

        // create the enemy
        GameObject enemyGO = Instantiate(enemyPrefab);
        enemy = enemyGO.GetComponent<Trainer>();
        enemy.currentPokemon = enemy.TrainerPokemon[0];
        enemy.CreateTrainer();

        foreach (Rorymon rorymon in enemy.TrainerPokemon) {
            rorymon.currentHealth = rorymon.maxHealth;
        }
        // set up the UI
        for (int i = 0; i < 6; i++) {
            rorymonNames[i].text = player.TrainerPokemon[i].name;
            if (player.TrainerPokemon[i] == selectedRorymon) {
                rorymonNames[i].color = green;
            }
            else if (player.TrainerPokemon[i].hasFainted) {
                rorymonNames[i].color = red;
            }
            else {
                rorymonNames[i].color = black;
            } 
        }

        // Set the initial values of our variables
        enemyHasSelectedMove = false;
        state = BattleState.MOVESELECT;
        selectedRorymon = player.TrainerPokemon[0];
        currentlySelectedMove = selectedRorymon.pokemonMoves[0];
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
        // choose a move
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
        // excecute the chosen move
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            state = BattleState.PERFORMINGMOVES;
        }      

    }

    void MoveCalcs()
    {
        // tracks whose moving first
        bool playerMovesFirst = true;

        if (player.currentPokemon.speed.value > enemy.currentPokemon.speed.value) { // player is faster
            playerMovesFirst = true;
        } 
        else if (player.currentPokemon.speed.value < enemy.currentPokemon.speed.value) { // enemy is faster
            playerMovesFirst= false;
        } else { // Speed tie
            playerMovesFirst = Utility.instance.ProbabilityGenerator(50); // 50/50 chance
        }

        float damage = 0; // tracker variable for damage
        // check which pokemon is faster
        if (playerMovesFirst)
        {
            if (currentlySelectedMove.effect == moveEffect.offensive) {
                // calculate and inflict damage to enemy
                damage = player.currentPokemon.calculateDamage(enemy.currentPokemon, currentlySelectedMove);
                enemy.currentPokemon.currentHealth -= damage;
            }

            // print to UI
            actionText1.text = "Player's " + player.currentPokemon.name + " Attacked with " + currentlySelectedMove.moveName + " dealing " + damage + " damage!";
          
            if (enemy.currentPokemon.currentHealth > 0) // if enemy is still alive to have their move
            {

                // calculate and inflict damage to player pokemon
                damage = enemy.currentPokemon.calculateDamage(player.currentPokemon, enemy.currentPokemon.pokemonMoves[0]);
                player.currentPokemon.currentHealth -= damage;

                // print to UI
                actionText2.text = "Enemy's " + enemy.currentPokemon.name + " Attacked with " + enemy.currentPokemon.pokemonMoves[0].moveName + " dealing " + damage + " damage!";

                if(player.currentPokemon.currentHealth <= 0) // if the player pokemon has died
                {
                    // move to the pokemon selection state with appropriate tracker variables
                    player.currentPokemon.hasFainted = true;
                    TrainerPokemonFainted = true;
                    state = BattleState.NEXTPOKEMONSELECT;
                    return;
                }               
            }
            else // enemy died before they could attack
            {
                // move to pokemon selection with appropriate tracker variables
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
                if (currentlySelectedMove.effect == moveEffect.offensive) {
                    damage = player.currentPokemon.calculateDamage(enemy.currentPokemon, currentlySelectedMove);
                    enemy.currentPokemon.currentHealth -= damage;
                }

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
                if (!rorymon.hasFainted) {
                    pokemonLeft = true;
                }

            }
            actionText1.text = "Currently Selected Pokemon: " + selectedRorymon;

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

            if (Input.GetKeyDown(KeyCode.Return) && !selectedRorymon.hasFainted) {
                player.currentPokemon = selectedRorymon;
                state = BattleState.MOVESELECT;
            }
            else if (Input.GetKeyDown(KeyCode.Return) && selectedRorymon.hasFainted) {
                actionText2.text = "That Rorymon has fainted! Choose another!";
            }

            if (!pokemonLeft) {
                state = BattleState.LOST;
            }

            for (int i = 0; i < 6; i++) {
                rorymonNames[i].text = player.TrainerPokemon[i].name;
                if (player.TrainerPokemon[i] == selectedRorymon) {
                    rorymonNames[i].color = green;
                } else if (player.TrainerPokemon[i].hasFainted) {
                    rorymonNames[i].color = red;
                } else {
                    rorymonNames[i].color = black;
                }

            }
        }
        else if (!TrainerPokemonFainted) {
            foreach(Rorymon rorymon in enemy.TrainerPokemon) {
                if (!rorymon.hasFainted) {
                    pokemonLeft = true;
                    enemy.currentPokemon = rorymon;
                    state = BattleState.MOVESELECT;
                    break;
                }
            }
            if (!pokemonLeft) {
                state = BattleState.WON;
            }
        }
    }

}


