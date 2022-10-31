using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "GamemodeSO", menuName = "ScriptableObjects/NewGamemodeConfig")]
public class GamemodeSO : ScriptableObject
{
    public bool m_isVersus;

    public bool m_areEventsOn;
    public float m_distRqdToStartSpawningEvt;


    // Tout les m_difficultyIncreaseDistanceOffset on augmente la difficulté.
    /*
     * La difficulté est traduite par une augmentation de la fréquence d'apparition des obstacles, des malus, 
     * ainsi que par l'apparition d'événement aléatoire (Disponible uniquement si m_areEventsOn est à true).
     */
    public float m_difficultyIncreaseDistanceOffset;
}