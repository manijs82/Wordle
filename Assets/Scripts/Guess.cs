using System.Collections.Generic;
using UnityEngine;

public class Guess : MonoBehaviour
{
    [SerializeField] private Letter[] letters;

    private Stack<string> _wordStack;
    private GuessBoard _guessBoard;

    public bool Filled => _wordStack.Count == _guessBoard.MaxLetter;

    public void Init(GuessBoard guessBoard)
    {
        _wordStack = new Stack<string>();
        _guessBoard = guessBoard;
    }

    public void Lock()
    {
        int i = 0;
        foreach (var letter in letters)
        {
            letter.SetColor(_guessBoard.GetValidation(letter.Character, i));
            i++;
        }
    }

    public void PutLetter(string letter)
    {
        if(_wordStack.Count >= _guessBoard.MaxGuess) return;
        
        _wordStack.Push(letter);
        UpdateLetters();
    }

    public void DeleteLastLetter()
    {
        if(_wordStack.Count == 0) return;
        
        _wordStack.Pop();
        UpdateLetters();
    }

    public void ChangeColorForAll(Color color)
    {
        foreach (var letter in letters) letter.SetColor(color);
    }

    public string GetWord()
    {
        string word = "";
        for (int i = 0; i < _wordStack.Count; i++) 
            word += _wordStack.ToArray()[_wordStack.Count - 1 - i];

        return word;
    }

    private void UpdateLetters()
    {
        foreach (var letter in letters) 
            letter.Empty();
        for (int i = 0; i < _wordStack.Count; i++) 
            letters[i].Fill(_wordStack.ToArray()[_wordStack.Count - 1 - i]);
    }
}