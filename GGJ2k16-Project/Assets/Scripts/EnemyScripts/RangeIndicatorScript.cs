using UnityEngine;
using System.Collections;

public class RangeIndicatorScript : MonoBehaviour {

    public float m_alpha = 0.7f;
    public float m_fadeTime = 0.2f;
    public float m_scaleFactor = 1.0f;
    private float m_startAlpha;
    private float m_fadeProgress;
    private bool m_fading;
    private float m_range;
    public SpriteRenderer[] m_sprRenderers = new SpriteRenderer[2];
    public AggroScript m_aggro;

    // Use this for initialization
    void Awake () {
        m_startAlpha = m_alpha;
        m_fading = false;
        m_range = 5.0f;
        if(m_aggro)
        {
            m_range = m_aggro.m_searchRadius;
        }
        UpdateSprAlphas();
        float m_scale = m_range * m_scaleFactor;
        transform.localScale = new Vector3(m_scale, m_scale, m_scale);
	}
	
    void OnEnable()
    {
        m_fading = false;
        m_alpha = m_startAlpha;
        m_fadeProgress = 0.0f;
    }

	// Update is called once per frame
	void Update () {
        if(m_aggro.IsAggroed())
        {
            Fade();
        }
        if(m_fading)
        {
            m_fadeProgress += Time.deltaTime;
            m_alpha = Mathf.Lerp(m_startAlpha, 0, m_fadeProgress / m_fadeTime);
        }
        UpdateSprAlphas();
        float m_scale = m_range * m_scaleFactor;
        transform.localScale = new Vector3(m_scale, m_scale, m_scale);
    }

    private void UpdateSprAlphas()
    {
        foreach (SpriteRenderer spr in m_sprRenderers)
        {
            Color col = spr.color;
            col.a = m_alpha;
            spr.color = col;
        }
    }

    public void Fade()
    {
        if (!m_fading)
        {
            m_fading = true;
            m_startAlpha = m_alpha;
            m_fadeProgress = 0.0f;
        }
    }
}
