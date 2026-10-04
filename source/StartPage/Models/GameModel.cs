using LandingPage.Converters;
using Playnite.SDK;
using Playnite.SDK.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace LandingPage.Models
{
    public class GameModel : ObservableObject
    {
        private readonly static GameSource DefaultGameSource = new GameSource("Playnite");

        public GameModel(Game game)
        {
            Game = game;
            OpenCommand = new RelayCommand(() =>
            {
                try
                {
                    if (Game is Game g && LandingPageExtension.Instance?.PlayniteApi?.Database?.Games?.Get(g.Id) is Game)
                    {
                        LandingPageExtension.Instance.PlayniteApi.MainView.SwitchToLibraryView();
                        LandingPageExtension.Instance.PlayniteApi.MainView.SelectGame(g.Id);
                    }
                }
                catch (Exception ex)
                {
                    LandingPageExtension.logger.Error(ex, $"Error when switching to library view to select game: {Game?.Name} (ID: {Game?.Id})");
                }
            });
            StartCommand = new RelayCommand(() =>
            {
                if (Game is Game g && LandingPageExtension.Instance?.PlayniteApi?.Database?.Games?.Get(g.Id) is Game)
                {
                    Application.Current.Dispatcher.BeginInvoke(
                        System.Windows.Threading.DispatcherPriority.Background,
                        (Action<Guid>)LandingPageExtension.Instance.PlayniteApi.StartGame,
                        g.Id);
                }
            });
        }

        private Game game = null;
        public Game Game
        { 
            get => game;
            set
            {
                if (value != game) 
                {
                    SetValue(ref game, value);
                    _logoUriChecked = false;
                    _logoUri = null;
                    _trailerUriChecked = false;
                    _trailerUri = null;
                    OnPropertyChanged(nameof(CoverImagePath));
                    OnPropertyChanged(nameof(LogoUri));
                    OnPropertyChanged(nameof(SortingName));
                    OnPropertyChanged(nameof(TrailerUri));
                }
            }
        }

        public string CoverImagePath => Game?.CoverImage != null ? LandingPageExtension.Instance?.PlayniteApi?.Database?.GetFullFilePath(Game.CoverImage) : null;

        private Uri _logoUri;
        private bool _logoUriChecked = false;
        public Uri LogoUri
        {
            get
            {
                if (!_logoUriChecked)
                {
                    _logoUriChecked = true;
                    if (game != null && LandingPageExtension.Instance?.PlayniteApi?.Paths?.ConfigurationPath is string configPath)
                    {
                        var logoPath = Path.Combine(configPath, "ExtraMetadata", "games", game.Id.ToString(), "Logo.png");
                        if (File.Exists(logoPath) && Uri.TryCreate(logoPath, UriKind.RelativeOrAbsolute, out var uri))
                        {
                            _logoUri = uri;
                        }
                    }
                }
                return _logoUri;
            }
        }

        private Uri _trailerUri;
        private bool _trailerUriChecked = false;
        public Uri TrailerUri 
        { 
            get 
            {
                if (!_trailerUriChecked)
                {
                    _trailerUriChecked = true;
                    if (game != null && LandingPageExtension.Instance?.PlayniteApi?.Paths?.ConfigurationPath is string configPath)
                    {
                        var dir = Path.Combine(configPath, "ExtraMetadata", "games", game.Id.ToString());
                        var trailerPath = Path.Combine(dir, "VideoTrailer.mp4");
                        if (File.Exists(trailerPath) && Uri.TryCreate(trailerPath, UriKind.RelativeOrAbsolute, out var uri))
                        {
                            _trailerUri = uri;
                        }
                        else
                        {
                            var microTrailerPath = Path.Combine(dir, "VideoMicrotrailer.mp4");
                            if (File.Exists(microTrailerPath) && Uri.TryCreate(microTrailerPath, UriKind.RelativeOrAbsolute, out var microUri))
                            {
                                _trailerUri = microUri;
                            }
                        }
                    }
                }
                return _trailerUri;
            } 
        }

        public string SortingName => string.IsNullOrEmpty(Game?.SortingName) ? Game?.Name : Game?.SortingName;
        public GameSource Source => Game?.Source ?? DefaultGameSource;

        public string CategoryGroup
        {
            get
            {
                var firstId = Game?.CategoryIds?.FirstOrDefault();
                if (firstId == null || firstId == Guid.Empty)
                    return "[None]";
                return LandingPageExtension.Instance?.PlayniteApi?.Database?.Categories?.Get(firstId.Value)?.Name ?? "[None]";
            }
        }

        public ICommand OpenCommand { get; private set; }

        public ICommand StartCommand { get; private set; }
    }
}
