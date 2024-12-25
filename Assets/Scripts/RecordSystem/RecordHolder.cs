using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace RecordSystem
{
    public static class RecordHolder
    {
        private static List<RecordData> _recordDatas = new(4);
        private static string _savePath = Path.Combine(Application.persistentDataPath, "Records");

        static RecordHolder()
        {
            _recordDatas.Add(new RecordData { GameType = GameType.Classic });
            _recordDatas.Add(new RecordData { GameType = GameType.Speed });
            _recordDatas.Add(new RecordData { GameType = GameType.Combo });
            _recordDatas.Add(new RecordData { GameType = GameType.Chaos });
            LoadDatas();
        }

        public static void AddData(GameType type, int score)
        {
            foreach (var recordData in _recordDatas)
            {
                if (recordData.GameType == type)
                {
                    recordData.Scores.Add(score);
                }
            }
            SaveData();
        }

        public static List<int> GetScores(GameType gameType)
        {
            foreach (var recordData in _recordDatas)
            {
                if (recordData.GameType == gameType)
                {
                    return recordData.Scores;
                }
            }

            return null;
        }

        private static void SaveData()
        {
            var wrapper = new RecordDatasWrapper(_recordDatas);
            var json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
            File.WriteAllText(_savePath, json);
        }

        private static void LoadDatas()
        {
            if(!File.Exists(_savePath))
                return;

            var json = File.ReadAllText(_savePath);
            var data = JsonConvert.DeserializeObject<RecordDatasWrapper>(json);

            for (int i = 0; i < data.RecordDatas.Count; i++)
            {
                _recordDatas[i] = data.RecordDatas[i];
            }
        }

        [Serializable]
        private class RecordDatasWrapper
        {
            public List<RecordData> RecordDatas;

            public RecordDatasWrapper(List<RecordData> recordDatas)
            {
                RecordDatas = recordDatas;
            }
        }
    }
    
   

    [Serializable]
    public class RecordData
    {
        public List<int> Scores = new();
        public GameType GameType;
    }

    public enum GameType
    {
        Classic,
        Speed,
        Combo,
        Chaos
    }
}