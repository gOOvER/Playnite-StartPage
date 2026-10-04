using LandingPage.Models;
using LandingPage.Models.SuccessStory;
using Playnite.SDK;
using Playnite.SDK.Data;
using SqlNado;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace LandingPage
{
    internal class PlayniteAchievementsProvider : IAchievementsProvider, IDisposable
    {
        private string _libraryPath;
        private readonly IPlayniteAPI _playniteAPI;
        private FileSystemWatcher _fileWatcher;
        private System.Timers.Timer _timer;

        public PlayniteAchievementsProvider(IPlayniteAPI playniteAPI)
        {
            _playniteAPI = playniteAPI;
            _libraryPath = FindAchievementCachePath(playniteAPI.Paths.ExtensionsDataPath)
                ?? Path.Combine(
                    playniteAPI.Paths.ExtensionsDataPath,
                    "e6aad2c9-6e06-4d8d-ac55-ac3b252b5f7b",
                    "achievement_cache.db");

            var directory = Path.GetDirectoryName(_libraryPath);
            if (!string.IsNullOrEmpty(directory) && Directory.Exists(directory))
            {
                try
                {
                    _fileWatcher = new FileSystemWatcher(directory, "*achievement_cache*");
                    _fileWatcher.Changed += _fileWatcher_Changed;
                    _fileWatcher.Created += _fileWatcher_Changed;
                    _fileWatcher.EnableRaisingEvents = true;
                }
                catch (Exception ex)
                {
                    LandingPageExtension.logger.Warn(ex, "Failed to initialize FileSystemWatcher in PlayniteAchievementsProvider");
                }
            }
            _timer = new System.Timers.Timer(TimeSpan.FromSeconds(4).TotalMilliseconds) { AutoReset = false };
            _timer.Elapsed += _timer_Elapsed;
        }

        private async void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var timer = (System.Timers.Timer)sender;

            timer.Stop();
            try
            {
                await OnAchievementsUpdates?.Invoke(default);
            }
            catch (Exception ex)
            {
                LandingPageExtension.logger.Error(ex, 
                    "Error while invoking handlers for OnAchievementsUpdates in PlayniteAchievementsProvider");
            }
        }

        private void _fileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            _timer.Stop();
            _timer.Start();
        }

        public event AchievementsUpdatedCallback OnAchievementsUpdates;

        class AchievementDto : INotifyPropertyChanged, IAchievement
        {
            public string DisplayName { get; set; }
            public string Description { get; set; }
            public string UnlockedIconPath { get; set; }
            public string LockedIconPath { get; set; }
            public string PlayniteGameId { get; set; }
            public DateTime UnlockTimeUtc { get; set; }

            public string Name => DisplayName;

            public bool IsUnlocked => true;

            public IGameAchievementsInfo GameInfo { get; set; }

            public Uri Icon { get; set; }

            public DateTime? UnlockedOn => UnlockTimeUtc;

            public event PropertyChangedEventHandler PropertyChanged { add { } remove { } }
        }

        const string GAMES_WITH_LATEST_ACHIEVEMENT =
@"
SELECT 
       MAX(ua.UnlockTimeUtc) as LastUnlock,
       g.PlayniteGameId AS GameId,
       ugp.AchievementsUnlocked, ugp.TotalAchievements
FROM UserAchievements AS ua
JOIN AchievementDefinitions ad
    ON ad.Id = ua.AchievementDefinitionId 
JOIN Games g 
    ON g.Id = ad.GameId 
JOIN UserGameProgress ugp 
    ON ad.GameId = ugp.GameId 
WHERE Unlocked = 1
GROUP BY ua.UserGameProgressId 
ORDER BY LastUnlock DESC
LIMIT @limit OFFSET @offset
";
        private const string LATEST_ACHIEVEMENTS =
@"SELECT
    ad.DisplayName, ad.Description, ad.UnlockedIconPath, ad.LockedIconPath,
    ua.UnlockTimeUtc,
    g.PlayniteGameId
FROM UserAchievements ua
JOIN AchievementDefinitions ad
    ON ad.Id = ua.AchievementDefinitionId 
JOIN Games g 
    ON g.Id = ad.GameId 
WHERE ua.Unlocked = 1
ORDER BY ua.UnlockTimeUtc  DESC
LIMIT @limit OFFSET @offset
";

        private const string LATEST_ACHIEVEMENTS_FOR_GAME_ID =
@"
SELECT
    ad.DisplayName, ad.Description, ad.UnlockedIconPath, ad.LockedIconPath,
    ua.UnlockTimeUtc,
    g.PlayniteGameId
FROM UserAchievements ua
JOIN AchievementDefinitions ad
    ON ad.Id = ua.AchievementDefinitionId 
JOIN Games g 
    ON g.Id = ad.GameId 
WHERE ua.Unlocked = 1 AND g.PlayniteGameId = @gameId
ORDER BY ua.UnlockTimeUtc  DESC
LIMIT @limit OFFSET @offset
";

        public async Task<List<IAchievement>> GetLatestAchievements(
            int maxAchievemnets,
            int maxAchievementsPerGame,
            CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            List<IAchievement> results = new List<IAchievement>();
            var iconBasePath = Path.Combine(
                _playniteAPI.Paths.ExtensionsDataPath,
                "e6aad2c9-6e06-4d8d-ac55-ac3b252b5f7b");
            if (!File.Exists(_libraryPath))
            {
                return results;
            }

            using (var db = new SQLiteDatabase(_libraryPath, SQLiteOpenOptions.SQLITE_OPEN_READONLY))
            {
                try
                {
                    var latestGames = db.Load<GameInfo>(
                        GAMES_WITH_LATEST_ACHIEVEMENT,
                        maxAchievemnets, 0);

                    foreach (var game in latestGames)
                    {
                        if (results.Count >= maxAchievemnets)
                        {
                            break;
                        }
                        var gameModel = _playniteAPI.Database.Games.Get(game.GameId);
                        if (gameModel == null)
                        {
                            continue;
                        }
                        game.GameName = gameModel.Name;
                        var achievements = db.Load<AchievementDto>(
                            LATEST_ACHIEVEMENTS_FOR_GAME_ID,
                            game.GameId,
                            Math.Min(maxAchievementsPerGame, maxAchievemnets - results.Count), // Limit
                            0); // Offset
                        foreach (var achievement in achievements)
                        {
                            achievement.GameInfo = game;
                            if (achievement.UnlockedIconPath is string path && !string.IsNullOrEmpty(path))
                            {
                                if (path.StartsWith("https://") || path.StartsWith("http://"))
                                {
                                    achievement.Icon = new Uri(path);
                                }
                                else
                                {
                                    achievement.Icon = new Uri(Path.Combine(iconBasePath, path));
                                }
                            }

                            results.Add(achievement);
                        }

                    }
                }
                catch (Exception ex)
                {
                    LandingPageExtension.logger.Error(ex, "Failed to load achievements from PlayniteAchievements database");
                }
            }
            return results;
        }

        public void Dispose()
        {
            if (_fileWatcher != null)
            {
                _fileWatcher.Changed -= _fileWatcher_Changed;
                _fileWatcher.Dispose();
                _fileWatcher = null;
            }
            _timer?.Dispose();
        }

        internal static string FindAchievementCachePath(string extensionsDataPath)
        {
            if (string.IsNullOrWhiteSpace(extensionsDataPath) || !Directory.Exists(extensionsDataPath))
            {
                return null;
            }

            try
            {
                var defaultPath = Path.Combine(extensionsDataPath, "e6aad2c9-6e06-4d8d-ac55-ac3b252b5f7b", "achievement_cache.db");
                if (File.Exists(defaultPath))
                {
                    return defaultPath;
                }

                var candidates = Directory.EnumerateFiles(extensionsDataPath, "achievement_cache.db", SearchOption.AllDirectories)
                    .ToList();

                if (candidates.Count == 0)
                {
                    return null;
                }

                return candidates.OrderByDescending(File.GetLastWriteTimeUtc).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
    }

    internal class GameInfo : INotifyPropertyChanged, IGameAchievementsInfo
    {
        public DateTime Latest { get; set; }
        public Guid GameId { get; set; }
        public int AchievementsUnlocked { get; set; }
        public int TotalAchievements { get; set; }

        public int UnlockedAchievements => AchievementsUnlocked;

        public double Progress => TotalAchievements > 0 ? (double)AchievementsUnlocked / TotalAchievements : 0;

        public string GameName { get; set; }

        public DateTime? LastUnlock { get; set; }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { }
            remove { }
        }
    }
}
