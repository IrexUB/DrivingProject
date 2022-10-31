using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


// Cette classe doit-être équipé sur toute entité susceptible de subir des effets
// Elle repose sur un dictionnaire stockant le type d'effet ainsi que la classe gérant l'évolution de cet effet dans le temps.
public class EffectableEntity : MonoBehaviour
{
    [SerializeField] private Dictionary<ScriptableEffect, TimedEffect> m_effects = new Dictionary<ScriptableEffect, TimedEffect>();
    void Update()
    { 
        foreach (var effect in m_effects.Values.ToList())
        {
            effect.Tick(Time.deltaTime);
            if (effect.m_isFinished)
            {
                m_effects.Remove(effect.m_effect);
            }
        }
    }

    public void AddEffect(TimedEffect effect)
    {
        if (m_effects.ContainsKey(effect.m_effect))
        {
            m_effects[effect.m_effect].Activate();
        }
        else
        {
            m_effects.Add(effect.m_effect, effect);
            effect.Activate();
        }
    }
}