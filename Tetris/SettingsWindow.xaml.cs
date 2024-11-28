using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Tetris
{
    public partial class SettingsWindow : Window
    {
        // Property to hold the leaderboard text
        public string LeaderboardText { get; set; }

        public SettingsWindow()
        {
            // Set the DataContext to this window's instance
            this.DataContext = this;

            // Generate the leaderboard text
            GenerateLeaderboard();
        }

        // Method to generate leaderboard text
        private void GenerateLeaderboard()
        {
            // Example: List of top 10 players and their scores
            var leaderboard = new List<string>
            {
                "1. PlayerOne - 1000",
                "2. PlayerTwo - 950",
                "3. PlayerThree - 900",
                "4. PlayerFour - 850",
                "5. PlayerFive - 800",
                "6. PlayerSix - 750",
                "7. PlayerSeven - 700",
                "8. PlayerEight - 650",
                "9. PlayerNine - 600",
                "10. PlayerTen - 550"
            };

            // Join the leaderboard list into a single string
            LeaderboardText = string.Join("\n", leaderboard);

            // Alternatively, you could use a StringBuilder to build the text dynamically.
            // StringBuilder sb = new StringBuilder();
            // foreach(var player in leaderboard)
            // {
            //     sb.AppendLine(player);
            // }
            // LeaderboardText = sb.ToString();
        }
    }
}
