using UnityEngine;

/*Read the summary!*/
/// <summary>
/// Add this script to a camera. You can shake
/// the camera with OneShot() and BeginRumble().
/// Be aware that you must use SetLocalPosition()
/// from this script to change the camera's local
/// position! Works with moving & rotating cameras.
/// PLEASE NOTE: If you want the camera to be able to
/// move (with usual transform.position +=...) then
/// you must parent the camera to a GameObject. It 
/// can be an empty GameObject if you like!
/// </summary>
[RequireComponent(typeof(Camera))]
public class ScreenShake : MonoBehaviour
{
    /// <summary>
    /// Speed of the shake, I've found that 10 is reasonable.
    /// </summary>
    public float m_speed = 10;

    /// <summary>
    /// Shake for a given duration. Ramps up into it and falls
    /// off over time. No effect if rumble is active.
    /// </summary>
    /// <param name="radius">Maximum radius of shake.</param>
    /// <param name="duration">Duration (seconds) for shake.</param>
    /// <param name="offset">Offset the shake from the current position.</param>
    /// <param name="intro">Normalised time val for maximum radius start.</param>
    /// <param name="exit">Normalised time val to begin radius falloff.</param>
    public void OneShot(float radius, float duration, Vector3 offset = new Vector3(), float intro = 0.3f, float exit = 0.7f)
    {
        /*do nothing if continuous is active*/
        if (m_continuous) return;
        /*setup one shot*/
        m_duration = duration;
        m_maxRadius = radius;
        m_currentRadius = 0;
        m_oneshot = true;
        m_offset = offset;
        m_shakeTarget = m_position;
        m_timer.Restart();
        m_exit = exit;
        m_intro = intro;
    }

    /// <summary>
    /// Begin a continuous shake.
    /// </summary>
    /// <param name="radius">Maximum radius of shake.</param>
    /// <param name="offset">Offset the shake from the current position.</param>
    public void BeginRumble(float radius, Vector3 offset = new Vector3())
    {
        m_continuous = true;
        m_maxRadius = radius;
        m_currentRadius = m_maxRadius;
        m_offset = offset;
        m_shakeTarget = m_position;
    }

    /// <summary>
    /// End a continuous shake. No effect if not rumbling and
    /// will not end a OneShot.
    /// </summary>
    public void EndRumble(float falloff = 0.0f)
    {
        /*do nothing if not rumbling*/
        if (!m_continuous) return;
        /*do nothing if already falling off*/
        if (m_falloff) return;

        if (falloff > 0)
        {
            m_falloff = true;
            m_duration = falloff;
            m_timer.Restart();
        }
        else m_continuous = false;
    }

    /// <summary>
    /// End the OneShot shake early. No effect on rumble. 
    /// </summary>
    public void EndOneShot()
    {
        m_oneshot = false;
    }

    /// <summary>
    /// End any and all shaking.
    /// </summary>
    public void EndAllShake()
    {
        m_oneshot = false;
        m_falloff = false;
        m_continuous = false;
    }

    public bool RumbleIsActive()
    {
        return m_continuous;
    }

    /// <summary>
    /// Use this if you wish to change the radius while shaking.
    /// Keep in mind that OneShot automatically adjusts radius
    /// to ease in and fall off!
    /// </summary>
    public void SetRadius(float radius)
    {
        m_maxRadius = radius;
    }

    /// <summary>
    /// Use this to change the cameras local position!
    /// </summary>
    /// <param name="position"></param>
    public void SetLocalPosition(Vector3 position)
    {
        m_position = position;
    }


    //Nothing public down here!------------------------------------------------

    /*types of shake*/
    private bool m_oneshot = false;
    private bool m_continuous = false;
    private bool m_falloff = false;
    /*position variables*/
    private Vector3 m_offset;
    private Vector3 m_position;     //position camera should be returning to, update if it's a moving camera
    private Vector3 m_shakeTarget;
    private const float NEAR = 0.1f;
    /*camera we're shaking*/
    private Camera m_camera;
    /*time variables*/
    private Timer m_timer;
    private float m_duration;
    /*radius variables*/
    private float m_currentRadius;
    private float m_maxRadius;
    /*easing in and out of OneShot*/
    private float m_intro;
    private float m_exit;

    void Awake()
    {
        m_camera = GetComponent<Camera>();
        m_position = m_camera.transform.localPosition;
        m_timer = new Timer();
    }

    // Update is called once per frame
    void Update()
    {
        bool shaking = m_continuous || m_oneshot;

        if (shaking)
        {
            /*close to target?*/
            if ((m_camera.transform.localPosition - m_shakeTarget).sqrMagnitude <= NEAR)
            {
                SetShakeTarget();
            }
        }

        if (m_oneshot || m_falloff)
        {
            /*linearly increase radius from 0-full for the beginning,
            const maximum for the middle,
            linearly decrease radius from full-0 for the end*/
            calculateRadius();
            /*if time is up, stop oneshot*/
            if (m_timer.Elapsed() >= m_duration) { m_oneshot = false; m_falloff = false; m_continuous = false; }
        }


        bool atRest = false;
        /*if not doing any shaking*/
        if (!shaking)
        {
            m_shakeTarget = m_position;
            atRest = (m_camera.transform.localPosition - m_shakeTarget).sqrMagnitude <= NEAR;
        }

        /*if either returning to default pos or shaking*/
        if (!atRest)
        {
            /*shake!*/

            Vector3 direction = (m_shakeTarget - m_camera.transform.localPosition).normalized;
            Vector3 position = m_camera.transform.localPosition + (direction * m_speed * Time.deltaTime);

            /*if the movement will put us on the other side of the target point...*/
            if ((m_camera.transform.localPosition - position).sqrMagnitude > (m_camera.transform.localPosition - m_shakeTarget).sqrMagnitude)
            {
                m_camera.transform.localPosition = m_position;
            }
            /*else if we're clear to go ahead at full speed*/
            else m_camera.transform.localPosition = position;
        }
        else /*else if stopped shaking and returning to default pos*/
        {
            /*restore to default locatione*/
            m_camera.transform.localPosition = m_position;
        }
    }

    private float percentage(float total, float current, float offset = 0)
    {
        return (current - offset) / (total - offset);
    }

    private void calculateRadius()
    {
        float curPercentage = percentage(m_duration, m_timer.Elapsed());
        if (curPercentage <= m_intro)
            m_currentRadius = m_maxRadius * percentage(m_duration - (1 - m_intro), m_timer.Elapsed());
        else if (curPercentage >= m_exit)
            m_currentRadius = m_maxRadius - m_maxRadius * percentage(m_duration, m_timer.Elapsed(), m_exit);
        else /*if (curPercentage is between first and second)*/
            m_currentRadius = m_maxRadius;
    }

    private void SetShakeTarget()
    {
        Quaternion rotation = m_camera.transform.rotation;
        Vector3 rand = Random.insideUnitCircle * m_currentRadius;

        rand += m_offset;
        rand = rotation * rand;

        m_shakeTarget = m_position + rand;
    }
}