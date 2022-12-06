using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

public class GuessBoard : MonoBehaviour
{
    public static event Action<char, Color> OnGuessLetter;
    
    public event Action OnGuessNotCorrect;
    public event Action<bool, string> OnEndGame;

    [SerializeField] private GameRules gameRules;
    [SerializeField] private Guess guess;

    private WordList _list;
    private Guess[] _guesses;
    private string _goal;
    private int _currentGuess;
    private Dictionary<char, int> _checkedLetters;
    private bool gameEnded;

    public int MaxGuess => gameRules.maxGuesses;
    public int MaxLetter => gameRules.letterCount;

    private void Awake()
    {
        InitGuesses();
        _checkedLetters = new Dictionary<char, int>();
        _list = new WordList(LoadResourceTextFile("WordList"));
        _goal = _list.words[Random.Range(0, _list.words.Length)];
        print(_goal);

        Key.OnKeyPress += OnKeyPressed;
    }

    private string[] LoadResourceTextFile(string path)
    {
        var file = Resources.Load<TextAsset>(path);
        var content = file.text;
        var allWords = content.Split("\n");
        for (var i = 0; i < allWords.Length; i++)
        {
            allWords[i] = allWords[i].ToUpper();
            allWords[i] = allWords[i].Substring(0, 5);
        }

        return allWords;
    }

    private void InitGuesses()
    {
        _guesses = new Guess[gameRules.maxGuesses];
        for (int i = 0; i < gameRules.maxGuesses; i++)
        {
            var g = Instantiate(guess, transform);
            g.Init(this);
            _guesses[i] = g;
        }
    }

    private void OnKeyPressed(KeyCode keyCode)
    {
        if (gameEnded) return;
        if (keyCode == KeyCode.Backspace)
        {
            _guesses[_currentGuess].DeleteLastLetter();
            return;
        }
        if (keyCode == KeyCode.Return)
        {
            if (_currentGuess == gameRules.maxGuesses - 1)
            {
                if (!_guesses[_currentGuess].Filled) return;
                EndGame();
                return;
            }

            LockGuess();
            return;
        }

        _guesses[_currentGuess].PutLetter(keyCode.ToString());
    }

    public Color GetValidation(char character, int index)
    {
        if (_checkedLetters.ContainsKey(character))
        {
            if(GetNumberOfLettersInGoal(character) == _checkedLetters[character])
            {
                OnGuessLetter?.Invoke(character, Color.grey);
                return Color.grey;
            }
        }
        
        if (!_goal.Contains(character))
        {
            OnGuessLetter?.Invoke(character, Color.grey);
            return Color.grey;
        }
        Color color = Color.grey;
        color = _goal.IndexOf(character) == index ? Color.green : Color.yellow;

        if (!_checkedLetters.ContainsKey(character))
            _checkedLetters.Add(character, 1);
        else
            _checkedLetters[character] += 1;
        
        OnGuessLetter?.Invoke(character, color);
        return color;
    }

    private int GetNumberOfLettersInGoal(char character)
    {
        int o = 0;
        foreach (var c in _goal)
        {
            if (c == character)
                o++;
        }

        return o;
    }

    private void LockGuess()
    {
        if (!_guesses[_currentGuess].Filled) return;
        if (!_list.words.Contains(_guesses[_currentGuess].GetWord()))
        {
            OnGuessNotCorrect?.Invoke();
            return;
        }

        _checkedLetters = new Dictionary<char, int>();
        _guesses[_currentGuess].Lock();
        if (_guesses[_currentGuess].GetWord() == _goal)
        {
            EndGame();
            return;
        }
        _currentGuess++;
    }

    private void EndGame()
    {
        gameEnded = true;
        if(_guesses[_currentGuess].GetWord() != _goal)
            _guesses[_currentGuess].ChangeColorForAll(Color.red);
        else
            _guesses[_currentGuess].ChangeColorForAll(Color.green);
        OnEndGame?.Invoke(_guesses[_currentGuess].GetWord() == _goal, _goal);
        Key.OnKeyPress -= OnKeyPressed;
    }
}