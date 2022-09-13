using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arena : MonoBehaviour
{
    public static Arena instance = null;

    public bool friendlyLightScreen;
    public int friendlyLightScreenRemaining;

    public bool enemyLightScreen;
    public int enemyLightScreenRemaining;

    public bool friendlyReflect;
    public int friendlyReflectRemaining;

    public bool enemyReflect;
    public int enemyReflectRemaining;

    public bool isRaining;
    public bool isSunny;
    public int weatherTurnsRemaining;

    public bool turnHasEnded;

    public Text arenaText;


    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        arenaText.text = " ";

    }


    // Start is called before the first frame update
    void Start()
    {
        turnHasEnded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (turnHasEnded) {
            friendlyLightScreenRemaining--;
            enemyLightScreenRemaining--;
            friendlyReflectRemaining--;
            enemyReflectRemaining--;
            weatherTurnsRemaining--;

            if (friendlyLightScreenRemaining <= 0) {
                friendlyLightScreen = false;
            }
            if (enemyLightScreenRemaining <= 0) {
                enemyLightScreen = false;
            }
            if (friendlyReflectRemaining <= 0) {
                friendlyReflect = false;
            }
            if (enemyReflectRemaining <= 0) {
                enemyReflect = false;
            }
            if (weatherTurnsRemaining <= 0) {
                isRaining = false;
                isSunny = false;
            }
            turnHasEnded = false;

        }
        arenaText.text = " ";

        if (friendlyLightScreen) {
            arenaText.text += "Friendly Light Screen: " + friendlyLightScreenRemaining + "\n";
        }
        if (enemyLightScreen) {
            arenaText.text += "Enemy Light Screen: " + enemyLightScreenRemaining + "\n";
        }
        if (friendlyReflect) {
            arenaText.text += "Friendly Reflect: " + friendlyReflectRemaining + "\n";
        }
        if (enemyReflect) {
            arenaText.text += "Enemy Reflect: " + enemyReflectRemaining + "\n";
        }
        if (isSunny) {
            arenaText.text += "Sun remaining: " + weatherTurnsRemaining + "\n";
        } else if (isRaining) {
            arenaText.text += "Rain remaining: " + weatherTurnsRemaining + "\n";
        }

    }
}
