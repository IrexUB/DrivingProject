using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "ScriptableObjects/NewPlayerConfig")]
public class PlayerConfigSO : ScriptableObject
{
    public uint m_playerId;
    public string m_playerAxisName;
    public KeyCode m_playerAccelerationKey;
    public KeyCode m_playerCameraSwitchKey;
}
