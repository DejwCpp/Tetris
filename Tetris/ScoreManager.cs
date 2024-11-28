using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Tetris
{
    public class ScoreEntry
    {
        public int Score { get; set; }
        public string Nickname { get; set; }
        public string OperatingSystem { get; set; }
    }

    public class ScoreManager
    {
        private readonly string _filePath;

        public ScoreManager(string filePath)
        {
            _filePath = filePath;
        }

        public void SaveScore(int score, string nickname)
        {
            var entry = new ScoreEntry
            {
                Score = score,
                Nickname = nickname,
                OperatingSystem = Environment.OSVersion.ToString()
            };

            List<ScoreEntry> scores = LoadScores();
            scores.Add(entry);

            string json = JsonSerializer.Serialize(scores, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public int GetBestScore()
        {
            List<ScoreEntry> scores = LoadScores();

            if (scores.Count == 0)
                return 0;

            return scores.Max(entry => entry.Score);
        }

        private List<ScoreEntry> LoadScores()
        {
            if (!File.Exists(_filePath))
                return new List<ScoreEntry>();

            string json = File.ReadAllText(_filePath);

            try
            {
                return JsonSerializer.Deserialize<List<ScoreEntry>>(json) ?? new List<ScoreEntry>();
            }
            catch (JsonException)
            {
                // Handle cases where the JSON file is corrupted
                return new List<ScoreEntry>();
            }
        }
    }
}
