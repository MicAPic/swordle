using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LemmaSharp.Classes;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using WordDictionary = NetSpell.SpellChecker.Dictionary.WordDictionary;

public class BattleManager : MonoBehaviour
{
    [Header("Animation & Sound")] 
    [SerializeField]
    private Animator playerAnimator;
    public Animator enemyAnimator;
    
    [Header("Wordle")]
    [SerializeField]
    private string answer;
    public const int Tries = 6;
    [SerializeField] 
    private List<string> guesses = new List<string>();
    private List<string> _guessesColour = new List<string>();
    public int _currentGuessIndex = 0;
    private readonly WordDictionary _enDictionary = new WordDictionary();
    private readonly Lemmatizer _lemmatizer = 
        new Lemmatizer(File.OpenRead("Assets/Plugins/full7z-multext-en.lem"));

    [Header("UI")] 
    [SerializeField] 
    private TMP_InputField inputField;
    private TMP_Text inputPlaceholderText;
    [SerializeField] 
    private GameObject heartContainer;
    [SerializeField] 
    private GameObject heartSpritePrefab;
    [SerializeField] 
    private GameObject scrollArrowUp;
    [SerializeField] 
    private GameObject scrollArrowDown;
    private bool _canScroll = true;

    void Awake()
    {
        // generate answer
        var words = File.ReadAllLines("Assets/corpus.txt");
        answer = words[Random.Range(0, words.Length)];
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // initialize input field
        FocusOnInput();
        Destroy(inputField.transform.Find("Text Area/Caret").gameObject);
        
        // initialize en-US dictionary
        _enDictionary.DictionaryFile = "Assets/Plugins/en-US.dic"; 
        _enDictionary.Initialize();
        
        // initialize the life bar
        for (int i = 0; i < Tries; i++)
        {
            Instantiate(heartSpritePrefab, heartContainer.transform);
        } 
        
        inputPlaceholderText = inputField.placeholder.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_canScroll)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ScrollThroughGuesses(-1);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ScrollThroughGuesses(1);
        }
    }

    public void SubmitGuess(string guess)
    {
        inputField.ActivateInputField(); // retain focus

        guess = _lemmatizer.Lemmatize(guess).ToLower();
        Debug.Log(guess);
        if (guess.Length == answer.Length && _enDictionary.Contains(guess) && !guesses.Contains(guess))
        {
            var colouredGuess = WordleComparison(guess.ToCharArray());
            inputField.text = "";
            inputPlaceholderText.text = colouredGuess;
            if (guess == answer)
            {
                LockInput();
                playerAnimator.SetTrigger("Attacks");
                return;
            }
            
            guesses.Add(guess);
            _guessesColour.Add(colouredGuess);
            playerAnimator.SetTrigger("Damaged");
            Destroy(heartContainer.transform.GetChild(0).gameObject);
            
            if (guesses.Count == Tries)
            {
                LockInput();
                playerAnimator.SetBool("IsDead", true);
                return;
            }
            
            scrollArrowDown.gameObject.SetActive(true);
        }
    }

    public void ActivateInput(string _="")
    {
        inputPlaceholderText.text = "";
        
        if (guesses.Count != 0) // this is to prevent bugs
        {
            scrollArrowUp.gameObject.SetActive(true);
            scrollArrowDown.gameObject.SetActive(false);
        }
        _currentGuessIndex = guesses.Count;
    }

    public void FocusOnInput(string _="")
    {
        inputField.ActivateInputField();
    }
    
    public void ScrollThroughGuesses(int scrollModifier)
    {

        string activeGuess = "";
        try
        {
            activeGuess = _guessesColour[_currentGuessIndex + scrollModifier];
        }
        catch (ArgumentOutOfRangeException)
        {
            if (scrollModifier > 0)
            {
                // switch to input
                scrollArrowDown.gameObject.SetActive(false);
                inputField.text = "";
                ActivateInput();
            }
            return;
        }

        inputField.text = "";
        _currentGuessIndex += scrollModifier;
        inputPlaceholderText.text = activeGuess;
        
        {
            scrollArrowUp.gameObject.SetActive(true);
            scrollArrowDown.gameObject.SetActive(true);
        }
        
        if (_currentGuessIndex == 0)
        {
            scrollArrowUp.gameObject.SetActive(false);
        }
    }
    
    private string WordleComparison(char[] guess)
    {
        // based on https://rosettacode.org/wiki/Wordle_comparison
        var result = guess.Select(c => c.ToString()).ToArray(); // default colour
        var answerTempArray = answer.ToCharArray();

        for (var i = 0; i < guess.Length; i++)
        {
            if (guess[i] == answerTempArray[i])
            {
                answerTempArray[i] = '\v';
                result[i] = "<color=#52b25f>" + guess[i] + "</color>"; // green
            }
        }
        
        for (var i = 0; i < guess.Length; i++)
        {
            var occurenceIndex = Array.IndexOf(answerTempArray, guess[i]);
            if (occurenceIndex >= 0)
            {
                answerTempArray[occurenceIndex] = '\v';
                result[i] = "<color=#ccb944>" + guess[i] + "</color>"; // yellow
            }
        }
        
        return string.Concat(result);
    }

    private void LockInput()
    {
        _canScroll = false;
        inputField.interactable = false;
    }
}
