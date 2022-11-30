using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VisualQueues : MonoBehaviour
{
    [SerializeField] private GuessBoard guessBoard;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Transform endBtn;
    
    private float _alpha;
    
    private void Start()
    {
        guessBoard.OnGuessNotCorrect += SayGuessNotCorrect;
        guessBoard.OnEndGame += EndGame;
    }

    private void Update()
    {
        textMesh.alpha = _alpha;
    }

    private void EndGame(bool win)
    {
        endBtn.DOScale(1, .5f).SetEase(Ease.OutQuint);
        if (win)
        {
            
        }
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