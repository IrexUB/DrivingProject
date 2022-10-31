using System.Collections;
using UnityEngine;

// Cette classe gère l'évolution de l'effet appliqué sur une EffectableEntity
public abstract class TimedEffect
{
    [SerializeField] protected float m_activeTime;

    // Cet variable permet de gérer le fait de pouvoir "stacker" les effets
    // Quand un certain effet ce stack, il devient plus "puissant"
    protected uint m_effectStacks;
    public ScriptableEffect m_effect { get; }

    protected readonly GameObject m_obj;
    public bool m_isFinished;

    public TimedEffect(ScriptableEffect buff, GameObject obj)
    {
        m_effect = buff;
        m_obj = obj;
    }

    public void Tick(float deltaTime)
    {
        m_activeTime -= deltaTime;
        if (m_activeTime < 0)
        {
            End();
            m_isFinished = true;
        }
    }

    public void Activate()
    {
        if (m_effect.m_isEffectStacked || m_activeTime <= 0)
        {
            ApplyEffect();
            m_effectStacks++;
        }

        if (m_effect.m_isDurationStacked || m_activeTime <= 0)
        {
            m_activeTime += m_effect.m_duration;
        }

        if (m_effect.m_isDurationRefreshable || m_activeTime <= 0)
        {
            m_activeTime = m_effect.m_duration;
        }
    }

    protected abstract void ApplyEffect();
    public abstract void End();
}