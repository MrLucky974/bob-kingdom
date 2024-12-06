using LuckiusDev.Utils;
using UnityEngine;

/// <summary>
/// ScoreTracker is a singleton class responsible for tracking and managing the player's score across game sessions.
/// It supports saving the best score persistently using Unity's PlayerPrefs system.
/// </summary>
public class ScoreTracker : PersistentSingleton<ScoreTracker>
{
    /// <summary>
    /// Key used to store and retrieve the best score from PlayerPrefs.
    /// </summary>
    public const string BEST_SCORE_PLAYER_PREF = "BestScore";

    /// <summary>
    /// Default value indicating a cleared or uninitialized score.
    /// </summary>
    public const int CLEAR_SCORE = -1;

    /// <summary>
    /// The current score value. Defaults to <see cref="CLEAR_SCORE"/> when cleared or uninitialized.
    /// </summary>
    private int m_score = CLEAR_SCORE;

    /// <summary>
    /// Gets the current score.
    /// </summary>
    public int Score => m_score;

    /// <summary>
    /// Sets the current score to the specified value.
    /// </summary>
    /// <param name="score">The new score value.</param>
    public void SetScore(int score)
    {
        m_score = score;
    }

    /// <summary>
    /// Clears the current score and optionally saves it as the best score if it exceeds the previously saved best score.
    /// </summary>
    /// <param name="saveAsBest">If true, the current score will be saved as the best score if it is higher than the previously saved best score.</param>
    public void ClearScore(bool saveAsBest = false)
    {
        if (saveAsBest)
        {
            int savedBestScore = PlayerPrefs.GetInt(BEST_SCORE_PLAYER_PREF, CLEAR_SCORE);
            if (savedBestScore == CLEAR_SCORE || savedBestScore < m_score)
            {
                PlayerPrefs.SetInt(BEST_SCORE_PLAYER_PREF, m_score);
                PlayerPrefs.Save();
            }
        }

        m_score = CLEAR_SCORE;
    }

    /// <summary>
    /// Clears all score-related data, including the best score stored in PlayerPrefs, and resets the current score.
    /// </summary>
    public void ClearAll()
    {
        if (PlayerPrefs.HasKey(BEST_SCORE_PLAYER_PREF))
        {
            PlayerPrefs.DeleteKey(BEST_SCORE_PLAYER_PREF);
        }

        ClearScore();
    }
}
