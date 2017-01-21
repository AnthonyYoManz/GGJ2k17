using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Templated pool class. Unless setting up with fromArray() You'll have to at 
/// least provide an initialise function so that the Pool knows how to 
/// create a new T object & can do any inital setup.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Pool<T>
{
    public delegate object  InitFunct();
    public delegate void    BasicFunt(T obj);

    protected object[]      m_pool;
    protected bool[]        m_enabled;
    protected int           m_length;
    protected InitFunct     f_initalise;
    protected BasicFunt     f_enabled;
    protected BasicFunt     f_dissabled;
    protected int           m_numEnabled;
    public Pool(InitFunct initFunct = null, BasicFunt awakeFnct = null, BasicFunt sleepFnct = null)
    {
        f_initalise = initFunct;
        f_enabled = awakeFnct;
        f_dissabled = sleepFnct;
    }

    /// <summary>
    /// Returns a reference to a free object and
    /// enables it within the pool.
    /// </summary>
    /// <param name="reference"></param>
    /// <returns></returns>
    public bool getFree(out T reference)
    {
        for (int i = 0; i < m_length; ++i)
        {
            if (!m_enabled[i])
            {
                ++m_numEnabled;
                reference = (T)m_pool[i];
                m_enabled[i] = true;
                if (f_enabled != null) f_enabled(reference);
                return true;
            }
        }
        reference = default(T);
        return false;
    }

    /// <summary>
    /// Dissabled an object if it can be found within the pool.
    /// </summary>
    /// <param name="obj"></param>
    public void dissable(object obj)
    {
        for (int i = 0; i < m_length; ++i)
        {
            if (obj == m_pool[i])
            {
                if (m_enabled[i])
                {
                    --m_numEnabled;
                    if (f_dissabled != null) f_dissabled((T)obj);
                    m_enabled[i] = false;
                }
            }
        }
    }

    public void fromArray(T[] array, bool enabled = false)
    {
        Debug.Assert(f_initalise != null, "You must provide an initalisation function!");

        m_length = array.Length;
        m_pool = new object[m_length];
        m_enabled = new bool[m_length];
        m_numEnabled = enabled ? m_length : 0;
        for (int i = 0; i < m_length; ++i)
        {
            m_enabled[i] = enabled;
            m_pool[i] = array[i];
            if (enabled && f_enabled != null) f_enabled((T)m_pool[i]);
            else if (!enabled && f_dissabled != null) f_dissabled((T)m_pool[i]);
        }
    }

    public object[] asArray()
    {
        return m_pool;
    }

    public void resize(int length, bool enabled = false)
    {
        Debug.Assert(f_initalise!=null, "You must provide an initalisation function!");

        m_length = length;
        m_pool = new object[length];
        m_enabled = new bool[length];
        m_numEnabled = enabled ? length : 0;
        for (int i = 0; i < length; ++i)
        {
            m_enabled[i] = enabled;
            m_pool[i] = f_initalise();
            if (enabled && f_enabled != null) f_enabled((T)m_pool[i]);
            else if (!enabled && f_dissabled != null) f_dissabled((T)m_pool[i]);
        }
    }

    public void setInit(InitFunct initFnct) { f_initalise = initFnct;  }
    public void setEnabled(BasicFunt awakeFnct) { f_enabled = awakeFnct; }
    public void setDissabled(BasicFunt sleepFnct) { f_dissabled = sleepFnct; }
}
