using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VisualQueues : MonoBehaviour
{
    [SerializeField] private GuessBoard guessBoard;
    [SerializeField] private TextMeshProUGUI goalText;
    [SerializeField] private TextMeshProUGUI guessText;
    [SerializeField] private Transform endBtn;
    
    private float _alpha;
    
    private void Start()
    {
        guessBoard.OnGuessNotCorrect += SayGuessNotCorrect;
        guessBoard.OnEndGame += EndGame;
    }

    private void Update()
    {
        guessText.alpha = _alpha;
    }

    private void EndGame(bool win, string goal)
    {
        if (!win) 
            goalText.text = goal;

        endBtn.DOScale(1, .5f).SetEase(Ease.OutQuint);
    }

    private void SayGuessNotCorrect()
    {
        DOTween.RestartAll();
        _alpha = 1;
        DOTween.To(() => _alpha, t => _alpha = t, 0, 3);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}