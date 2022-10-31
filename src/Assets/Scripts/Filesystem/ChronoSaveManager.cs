using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

[Serializable]
public struct ChronoRecordData
{
    public ChronoRecordData(int ptimer, float pdistance)
    {
        timer = ptimer;
        max_distance = pdistance;
    }

    public int timer;
    public float max_distance;
}

[Serializable]
public class ChronoRecordsMap
{
    public List<ChronoRecordData> m_records = new List<ChronoRecordData>();
}

public class ChronoSaveManager : MonoSingleton<ChronoSaveManager>
{
    private string m_saveFilePath;
    private void Awake()
    {
        base.Awake();

        instance.m_saveFilePath = Application.dataPath + "/chrono_records.json";
    }

    private void SerializeData(ChronoRecordsMap recordsMap)
    {
        string recordsToJson = JsonUtility.ToJson(recordsMap);
        File.WriteAllText(instance.m_saveFilePath, recordsToJson);
    }

    public void SaveRecord(ChronoRecordData entry)
    {
        if (!File.Exists(instance.m_saveFilePath))
        {
            ChronoRecordsMap recordsData = new ChronoRecordsMap();

            /*
             * Les records seront enregistrés sous cette forme dans le fichier .json
             {
                [
                    15: record_pour_15_secondes,
                    30: record_pour_30_secondes
                    60: record_pour_60_secondes
                ]
             } 
             */

            recordsData.m_records.Add(entry);   

            SerializeData(recordsData);
        } else
        {
            ChronoRecordsMap loadedRecords = LoadRecords();

            if (loadedRecords.m_records.Exists(record => record.timer == entry.timer))
            {
                ChronoRecordData? optionalTimerOccurence = loadedRecords.m_records.Single(record => record.timer == entry.timer);
                if (optionalTimerOccurence.HasValue)
                {
                    var timerOccurence = optionalTimerOccurence.Value;
                    if (timerOccurence.max_distance < entry.max_distance)
                        timerOccurence.max_distance = entry.max_distance;
                }
            } else
            {
                loadedRecords.m_records.Add(entry);
            }

            SerializeData(loadedRecords);
        }
    }

    public ChronoRecordsMap LoadRecords()
    {
        if (File.Exists(instance.m_saveFilePath))
        {
            string readJsonRecordsData = File.ReadAllText(instance.m_saveFilePath);

            return JsonUtility.FromJson<ChronoRecordsMap>(readJsonRecordsData);
        }

        return default(ChronoRecordsMap);
    }
}