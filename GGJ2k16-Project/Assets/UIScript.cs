using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public Text m_majorTextElement;
    public Text m_minorTextElement;
    public Image m_fadeImg;
    public Image m_winImg;
    public Image m_lossImg;

    public Color m_startColour;
    public Color m_endColour;
    public Color m_textStartColour;
    public Color m_textEndColour;
    public float m_fadeDuration;

    public string m_minorText;
    public string m_winText;
    public string m_loseText;

    private bool m_win, m_loss;

    private Timer   m_fadeTimer;
    private Color   m_curStart;
    private Color   m_curTarget;
    private Color   m_curTextStart;
    private Color   m_curTextTarget;
    enum FADE { OUT, IN, NONE}
    FADE m_fadeState;
    bool m_fading;
    // Use this for initialization
    void Awake ()
    {
        m_fadeTimer = new Timer();
        m_fading = false;
        m_win = false;
        m_loss = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_fading)
        {

            Color colour = Color.Lerp(m_curStart, m_curTarget, m_fadeTimer.Elapsed() / m_fadeDuration);
            m_fadeImg.color = colour;
            Color collll = new Color(1, 1, 1, colour.a);
            if (m_win)
            {
                m_winImg.color = collll;
            }
            else if (m_loss)
            {
                m_lossImg.color = collll;
            }
            m_minorTextElement.color = Color.Lerp(m_curTextStart, m_curTextTarget, m_fadeTimer.Elapsed() / m_fadeDuration);
            m_majorTextElement.color = Color.Lerp(m_curTextStart, m_curTextTarget, m_fadeTimer.Elapsed() / m_fadeDuration);
            Debug.Log(colour);
            if (colour == m_curTarget) { m_fading = false; }
        }
	}



    public void FadeOut()
    {
        m_curTarget = m_endColour;
        m_curStart = m_startColour;

        m_curTextTarget = m_textEndColour;
        m_curTextStart = m_textStartColour;

        if (!m_fading)
        {
            m_fadeTimer.Restart();
        }
        m_fading = true;
    }

    public void SetWinState(bool win)
    {
        if (win) m_majorTextElement.text = m_winText;
        else m_majorTextElement.text = m_loseText;
        
        m_minorTextElement.text = m_minorText;
    }
    public void FadeIn(bool _lose)
    {
        m_curStart = m_endColour;
        m_curTarget = m_startColour;

        m_curTextTarget = m_textStartColour;
        m_curTextStart = m_textEndColour;
        m_loss = _lose;
        m_win = !_lose;
        if (!m_fading)
        {
            m_fadeTimer.Restart();
        }
        m_fading = true;
    }
}
