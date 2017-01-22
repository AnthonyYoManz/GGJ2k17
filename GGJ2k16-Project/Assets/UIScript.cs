using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public Text m_majorTextElement;
    public Text m_minorTextElement;
    public Image m_fadeImg;

    public Color m_startColour;
    public Color m_endColour;
    public Color m_textStartColour;
    public Color m_textEndColour;
    public float m_fadeDuration;

    public string m_minorText;
    public string m_winText;
    public string m_loseText;

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
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_fading)
        { 
            Color colour = Color.Lerp(m_curStart, m_curTarget, m_fadeTimer.Elapsed() / m_fadeDuration);
            m_fadeImg.color = colour;
            m_minorTextElement.color = Color.Lerp(m_curTextStart, m_curTextTarget, m_fadeTimer.Elapsed() / m_fadeDuration);
            m_majorTextElement.color = Color.Lerp(m_curTextStart, m_curTextTarget, m_fadeTimer.Elapsed() / m_fadeDuration);
            if (colour == m_curTarget) { m_fading = false; }
        }
	}

    void ShowGameOver(bool win)
    {
        if (win)
        {
            m_majorTextElement.text = m_winText;
        }
        else
        {
            m_majorTextElement.text = m_loseText;
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
    public void FadeIn()
    {
        m_curTarget = m_startColour;
        m_curStart = m_endColour;

        m_curTextTarget = m_textStartColour;
        m_curTextStart = m_textEndColour;

        if (!m_fading)
        {
            m_fadeTimer.Restart();
        }
        m_fading = true;
    }
}
