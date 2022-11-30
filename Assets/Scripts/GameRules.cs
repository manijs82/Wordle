using UnityEngine;

[CreateAssetMenu(fileName = "game rules", menuName = "Worlde/GameRules")]
public class GameRules : ScriptableObject
{
    public int maxGuesses = 5;
    public int letterCount = 5;
    
}
