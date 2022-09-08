using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{

    public static Utility instance = null;

    public bool ProbabilityGenerator(float odds) {
        float sim = Random.Range(0, 100);
        if (sim > odds) {
            return false;
        }
        else {
            return true;
        }
        
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
