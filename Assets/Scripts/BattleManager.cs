using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text console;
    
    // Start is called before the first frame update
    void Start()
    {
        console.text = "ALLOW v LOLLY: " + WordleComparison("ALLOW".ToCharArray(), "LOLLY".ToCharArray()) + "\n" +
                       "BULLY v LOLLY: " + WordleComparison("BULLY".ToCharArray(), "LOLLY".ToCharArray()) + "\n" +
                       "ROBIN v ALERT: " + WordleComparison("ROBIN".ToCharArray(), "ALERT".ToCharArray()) + "\n" +
                       "ROBIN v SONIC: " + WordleComparison("ROBIN".ToCharArray(), "SONIC".ToCharArray()) + "\n" +
                       "ROBIN v ROBIN: " + WordleComparison("ROBIN".ToCharArray(), "ROBIN".ToCharArray()) + "\n";
        
        Destroy(GameObject.Find("Caret"));
    }

    // // Update is called once per frame
    // void Update()
    // {
    //     
    // }

    private string WordleComparison(char[] answer, char[] guess)
    {
        // based on https://rosettacode.org/wiki/Wordle_comparison
        var result = guess.Select(c => c.ToString()).ToArray(); // default colour

        for (var i = 0; i < guess.Length; i++)
        {
            if (guess[i] == answer[i])
            {
                answer[i] = '\v';
                result[i] = "<color=#52b25f>" + guess[i] + "</color>"; // green
            }
        }
        
        for (var i = 0; i < guess.Length; i++)
        {
            var occurenceIndex = Array.IndexOf(answer, guess[i]);
            if (occurenceIndex >= 0)
            {
                answer[occurenceIndex] = '\v';
                result[i] = "<color=#ccb944>" + guess[i] + "</color>"; // yellow
            }
        }
        
        return string.Concat(result);
    }
}
