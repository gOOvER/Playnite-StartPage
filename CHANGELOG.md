# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [1.0.0] - 2026-10-04

Initial release of **StartPageNG** (Fork of `felixkmh/StartPage-for-Playnite`).

### Added
- **Category Grouping for Shelves**: Added Category grouping option (`GroupingField.Category` and `GameModel.CategoryGroup`) ([#229](https://github.com/felixkmh/StartPage-for-Playnite/issues/229), [#236](https://github.com/felixkmh/StartPage-for-Playnite/pull/236)).
- **Dynamic PlayniteAchievements Discovery**: Automatic search for `achievement_cache.db` across the extension data directory via `FindAchievementCachePath`, adding full compatibility with local saves and custom install paths ([#237](https://github.com/felixkmh/StartPage-for-Playnite/pull/237)).
- **Theme-Adaptive ContextMenu Theming**: Context menus in `GridNodeView.xaml` and `StartPageView.xaml` now use `Foreground="{DynamicResource TextBrush}"` to ensure full readability across all custom light and dark themes ([#236](https://github.com/felixkmh/StartPage-for-Playnite/pull/236)).
- **Legacy Layout Migration**: Fallback support in `LandingPageExtension.cs` to migrate existing `DefaultLayout.json` from the legacy `felixkmh_StartPage_Plugin` directory.
- **Reset Layout Command**: Added "Reset layout to default" (`LOC_SPG_ResetLayout`) to the StartPage context menu to easily restore the default layout.
- **Sidebar Top Positioning**: Enabled by default and overhauled sidebar icon reordering to reliably move the StartPage icon to the very top (index 0) of the sidebar across startup.

### Changed
- **Modern SDK Project Format**: Migrated `StartPage`, `StartPage.SDK`, and `PlayniteCommon` projects from legacy MSBuild XML to modern SDK-style project format (`Microsoft.NET.Sdk` with `<UseWpf>true</UseWpf>`), enabling clean, fast builds with `dotnet build`.
- **Updated Dependencies**: Modernized `PlayniteSDK` to `6.18.0` with runtime asset exclusion.
- **Pinned Dependencies**: Pinned `Newtonsoft.Json 10.0.3` with runtime asset exclusion and suppressed `NU1903` to match Playnite 10 Desktop runtime AppDomain and avoid runtime `FileLoadException`.
- **Native Font Glyphs**: Replaced missing `StarRating.ttf` with Playnite's native `FontIcoFont` star glyphs.

### Fixed
- **Fatal Crash on StartPage Load (Issue #239)**: Added `TargetNullValue="{StaticResource DefaultAchievementUri}"` to `BitmapImage` in `RecentAchievementsView.xaml` to prevent `XamlParseException` / `InvalidOperationException: The 'UriSource' or 'StreamSource' property must be set` when an achievement has a null icon.
- **Shuffle Button Ineffective (Issue #231)**: Fixed `UpdateGamesAsync` and `UpdateGames` in `ShelveViewModel` to synchronize visual collection order via item moves when shuffling or sorting games.
- **Blank StartPage on Empty/Collapsed Layout**: Fixed StartPage remaining blank if `GridLayout` had 0 views or children; added `node.ViewCount == 0` detection, fallback layout discovery in plugin directory, and removed destructive `ViewProperties = null` clearing during view resolution.
- **Config & Shelves Not Saving (Issue #230, #242)**: Auto-save plugin settings when adding, removing, extending, or moving shelves in `ShelvesViewModel`, and when leaving the StartPage view (`StartPageViewModel.Closed()`).
- **Weekly Activity Title Typo (Issue #211)**: Replaced incorrect `LOC_SPG_DailyPlaytime` resource key with `LOC_SPG_WeeklyPlaytime` in `GameActivityView.xaml`.
- **ScoreComparer Typo**: Fixed `ScoreComparer.Compare(object x, object y)` comparing `(int?)x` twice instead of `(int?)x, (int?)y`.
- **GridNode Enumerate Recursion**: Fixed `Enumerate()` yielding `child` instead of `node`, preventing layout tree traversal bugs.
- **DayChanged Date Off-by-One**: Fixed `DayChanged` receiving yesterday's date instead of the new date.
- **PlayniteAchievements Provider Safety**: Guarded `FileSystemWatcher` creation, expanded watcher filter to `*achievement_cache*`, and handled database opening errors safely.
- **SuccessStory Watcher Leaks**: Added null checks and deduplicated `PropertyChanged` subscriptions in `SuccessStoryViewModel`.
- **Duplicate Key Crash in Shelves**: Switched dictionary insertions to safe indexers in `LandingPageExtension.cs`.
- **GameActivity Thread Safety**: Protected `ActivityLock.EnterReadLock()` with `try ... finally { ExitReadLock(); }` in `MostPlayedViewModel`.
- **GameActivity Duplicate Key Crash**: Grouped by `GameSource` directly in `GameActivityViewModel.PlaytimePerSource`.
- **Process Start Security**: Explicitly enabled `UseShellExecute = true` when launching external URLs in `GridNodeViewModel`.

### Performance
- **Lazy Cover & Trailer Metadata**: Eliminated synchronous file checks on the UI thread in `GameModel` by implementing lazy caching for `LogoUri` and `TrailerUri`.
- **Removed GC Stalls**: Eliminated all manual `GC.Collect()` calls across view models and views to prevent UI frame drops and CPU spikes.
- **Media Decoding Cleanup**: Stopped `MediaElement` playback and decoding on popup close in `GameDetailsPopup`.

### Removed
- Removed broken and unused `StarRating.ttf` file reference from project.

---

## [Legacy Versions] (felixkmh/StartPage-for-Playnite)

> All releases below are legacy versions from the original upstream repository `felixkmh/StartPage-for-Playnite`.

### [Legacy 2.14.1]
- Version bump to 2.14.1.

### [Legacy 2.14.0]
- Updated addon manifest.

### [Legacy 2.13.1]
- Version bump to 2.13.1.

### [Legacy 2.13.0]
- Merged bugfixes ([#233](https://github.com/felixkmh/StartPage-for-Playnite/pull/233)).

### [Legacy 2.12.6]
- Version bump to 2.12.6.

### [Legacy 2.12.5]
- Translations updates.

### [Legacy 2.12.1]
- General performance and UI improvements.

### [Legacy 2.12.0]
- Support for instance-specific game shelf settings.
- Scrollbar toggle options for shelves.

### [Legacy 2.11.0]
- Filter preset improvements.
- Support for customizable cover widths.

### [Legacy 2.10.0]
- Added options to ignore installed games.
- Multi-instance widget support.

### [Legacy 2.1.0]
- Most played of all time uses Playnite playtime instead of GameActivity.

### [Legacy 2.0.0]
- Major rewrite of grid layout engine.
- Movable and resizable panels with drag-and-drop.

### [Legacy 1.0.0]
- Initial release of StartPage for Playnite.
