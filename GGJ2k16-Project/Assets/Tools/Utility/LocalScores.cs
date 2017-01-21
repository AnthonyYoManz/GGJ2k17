using UnityEngine;
using System.IO;

/// <summary>
/// Class to handle loading and saving scores to a file.
/// Will generate a file for you on LoadScores() call
/// if one doesn't exist.
/// </summary>
public class LocalScores 
{
    public struct Pair
    {
        public Pair(float _score, string _name) { score = _score; name = _name; }

        public float score;
        public string name;
    }
    
    /// <summary>
    /// Loads scores from file.
    /// </summary>
    /// <param name="fpath">File path (include file extention).</param>
    public void LoadScores(string fpath = "scores.txt")
    {
        m_savedDataUpToDate = true;
        int index = 0;

        if (File.Exists(fpath))
        {
            StreamReader fsreader = new StreamReader(fpath);
            try
            {
                while (fsreader.Peek() != -1)
                {
                    float f;
                    float.TryParse(fsreader.ReadLine(), out f);
                    m_leaderBoard[index].score = f;
                    m_leaderBoard[index].name = fsreader.ReadLine();
                    ++index;         //index update for next loop    
                }
            }
            catch
            { }
            finally
            {
                fsreader.Close();
            }
            m_numScores = index;
        }
        //file doesn't exist
        else
        {
            StreamWriter fswriter = new StreamWriter(fpath);
            fswriter.Write("");
        }

    }

    /// <summary>
    /// Save the scores to file.
    /// </summary>
    public void SaveScores()
    {
        //no need to save if the cache is up to date
        if (m_savedDataUpToDate == false)
        {
            StreamWriter fswriter = new StreamWriter("scores.txt");
            for (int i = 0; i < m_numScores; ++i)
            {
                fswriter.WriteLine(m_leaderBoard[i].score);
                fswriter.WriteLine(m_leaderBoard[i].name);
            }

            fswriter.Close();
            m_savedDataUpToDate = true;
        }
    }


    /// <summary>
    /// Call to check the player score against the highest score.
    /// </summary>
    /// <param name="_score"></param>
    /// <returns></returns>
    public bool ScoreIsHighestScore(float _score)
    {
        //return true if the score is better than the worst OR there is a free slot!
        return m_numScores == 0 || _score >= m_leaderBoard[0].score;
    }


    /// <summary>
    /// Call to check whether the score would place on leaderboard.
    /// </summary>
    /// <param name="_score"></param>
    /// <returns></returns>
    public bool ScoreIsHighScore(float _score)
    {
        //return true if the score is better than the worst OR there is a free slot!
        return m_numScores < m_maxScores || _score >= m_leaderBoard[m_numScores - 1].score;
    }

    /// <summary>
    /// Attempt to add the player to the scoreboard.
    /// Returns true if they managed to place.
    /// By defualt, also saves scores to file 
    /// (override with _save parameter).
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_score"></param>
    /// <returns></returns>
    public bool AddPlayerScore(string _name, float _score, bool _save = true)
    {
        return AddPlayerScore(new Pair(_score, _name), _save);
    }

    /// <summary>
    /// Attempt to add the player to the scoreboard.
    /// Returns true if they managed to place.
    /// By defualt, also saves scores to file 
    /// (override with _save parameter).
    /// </summary>
    /// <param name="_pair"></param>
    /// <returns></returns>
    public bool AddPlayerScore(Pair _pair, bool _save = true)
    {
        //early out if they didn't place
        if (ScoreIsHighScore(_pair.score) == false) return false;

        bool placed = false;
        for (int i = 0; i < m_numScores; ++i)
        {
            if (_pair.score >= m_leaderBoard[i].score)
            {
                //add player to list here, reshuffle rest of list
                AddAndShuffle(_pair, i);
                if (m_numScores < m_maxScores) m_numScores++;
                placed = true;
                break;
            }
        }

        //if there is space and they didn't get on the list then add them
        if (!placed && m_numScores < m_maxScores)
        {
            //add player to list at m_numScores +1
            m_leaderBoard[m_numScores++] = _pair;
            placed = true;
            Debug.Log("appending score");
        }

        //if we have changed the board then it's
        m_savedDataUpToDate = m_savedDataUpToDate && !placed;

        if (placed && _save) SaveScores();
        return placed;
    }

    /// <summary>
    /// Returns the maximum number of scores 
    /// that the board can hold.
    /// </summary>
    /// <returns></returns>
    public int GetMaxScores()
    {
        return m_maxScores;
    }

    /// <summary>
    /// Returns the number of scores 
    /// currently recorded on the board.
    /// </summary>
    /// <returns></returns>
    public int GetNumberOfScores()
    {
        return m_numScores;
    }

    /// <summary>
    /// Get score at index. Index 0 is highscore.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Pair GetScore(int index)
    {
        return m_leaderBoard[index];
    }

    /// <summary>
    /// Get a reference to the internal array of Pairs.
    /// </summary>
    /// <returns></returns>
    public Pair[] AsArray()
    {
        return m_leaderBoard;
    }

    /*Private stuff and constructors...*/
    /// <param name="maxNumScores">Maximum number of scores that the scoreboard can hold.</param>
    public LocalScores(int maxNumScores)
    {
        m_maxScores = maxNumScores;
        m_leaderBoard = new Pair[m_maxScores];
    }

    ~LocalScores()
    {
        SaveScores();
    }

    private Pair[] m_leaderBoard;
    private int m_maxScores = 0;
    private int m_numScores = 0;
    private bool m_savedDataUpToDate = false;

    private void AddAndShuffle(Pair _p, int _index)
    {
        Pair temp;
        Pair prev = _p;
        for (int i = _index; i < m_maxScores; ++i)
        {
            temp = m_leaderBoard[i];
            m_leaderBoard[i] = prev;
            prev = temp;
        }
    }
}
