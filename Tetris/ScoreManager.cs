using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Tetris
{
    class ScoreManager
    {
        private readonly string _filePath;

        public ScoreManager(string filePath)
        {
            _filePath = filePath;
        }

        public void SaveScoreWithHash(int score)
        {
            string data = score.ToString();
            string hash = ComputeHash(data);

            File.AppendAllText(_filePath, $"{data}|{hash}" + Environment.NewLine);
        }

        public int GetBestScoreWithHash()
        {
            if (!File.Exists(_filePath)) return 0;

            var scores = File.ReadAllLines(_filePath)
                .Select(line =>
                {
                    var parts = line.Split('|');
                    if (parts.Length == 2 && VerifyHash(parts[0], parts[1]))
                    {
                        return int.Parse(parts[0]);
                    }
                    return -1; // invalid score
                })
                .Where(score => score != -1)
                .ToList();

            return scores.Count > 0 ? scores.Max() : 0;
        }

        private string ComputeHash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }
        }

        private bool VerifyHash(string data, string hash)
        {
            return ComputeHash(data) == hash;
        }
    }
}
