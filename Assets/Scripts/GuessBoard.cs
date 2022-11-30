using System;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

public class GuessBoard : MonoBehaviour
{
    public event Action OnGuessNotCorrect;
    public event Action<bool> OnEndGame;

    [SerializeField] private GameRules gameRules;
    [SerializeField] private Guess guess;

    private Guess[] _guesses;
    private int _currentGuess;
    private string _goal;
    private WordList _list;
    private bool gameEnded;

    public int MaxGuess => gameRules.maxGuesses;
    public int MaxLetter => gameRules.letterCount;

    private void Awake()
    {
        InitGuesses();
        _list = JsonConvert.DeserializeObject<WordList>(LoadResourceTextFile("fiveLetterWords.json"));
        _goal = _list.wordList[Random.Range(0, _list.wordList.Length)].ToUpper();
        print(_goal);

        Key.OnKeyPress += OnKeyPressed;
    }

    public Color GetValidation(char character, int index)
    {
        if (!_goal.Contains(character)) return Color.grey;
        return _goal.IndexOf(character) == index ? Color.green : Color.yellow;
    }

    private string LoadResourceTextFile(string path)
    {
        string filePath = path.Replace(".json", "");

        TextAsset targetFile = Resources.Load<TextAsset>(filePath);

        return targetFile.text;
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

    private void EndGame()
    {
        gameEnded = true;
        if(_guesses[_currentGuess].GetWord() != _goal)
            _guesses[_currentGuess].MakeRed();
        OnEndGame?.Invoke(_guesses[_currentGuess].GetWord() == _goal);
    }

    private void LockGuess()
    {
        if (!_guesses[_currentGuess].Filled) return;
        if (!_list.wordList.Contains(_guesses[_currentGuess].GetWord().ToLower()))
        {
            OnGuessNotCorrect?.Invoke();
            return;
        }

        _guesses[_currentGuess].Lock();
        if (_guesses[_currentGuess].GetWord() == _goal)
        {
            EndGame();
            return;
        }
        _currentGuess++;
    }
}