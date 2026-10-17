# StartPageNG for Playnite

[![GitHub release (latest by date)](https://img.shields.io/github/v/release/gOOvER/Playnite-StartPage?style=flat-square)](https://github.com/gOOvER/Playnite-StartPage/releases)
[![Website](https://img.shields.io/badge/playnite.goover.dev-Showcase%20%26%20Downloads-ea8024?style=flat-square&logo=googlechrome&logoColor=white)](https://playnite.goover.dev)
[![Playnite 10+](https://img.shields.io/badge/Playnite-10%2B-blue?style=flat-square)](https://playnite.link)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat-square)](LICENSE)
[![Ko-fi](https://img.shields.io/badge/Ko--fi-Support-F16061?style=flat-square&logo=ko-fi&logoColor=white)](https://ko-fi.com/goover)
[![Crowdin](https://badges.crowdin.net/startpage-for-playnite/localized.svg)](https://crowdin.com/project/startpage-for-playnite)

> **StartPageNG** is a highly customizable landing and home screen extension for the [Playnite](https://playnite.link/ "Playnite - video game library manager") video game library manager. It provides flexible grid layouts, movable and resizable panels, custom game shelves, playtime summaries, achievement showcases, and rich integration with third-party extensions.
>
> Originally created as [StartPage](https://github.com/felixkmh/StartPage-for-Playnite) by felixkmh, now modernized, secured, and actively maintained as **StartPageNG** by [gOOvER](https://github.com/gOOvER).

🌐 **Official Showcase & Direct Downloads**: [https://playnite.goover.dev/](https://playnite.goover.dev/)  
[GitHub Repository](https://github.com/gOOvER/Playnite-StartPage) | [Issue Tracker](https://github.com/gOOvER/Playnite-StartPage/issues)

---

| Default Mode | Edit Mode |
|:---:|:---:|
| ![Default Mode](https://user-images.githubusercontent.com/24227002/166326510-6121a702-7abd-482b-b727-2ef2786fdaf4.png) | ![Edit Mode](https://user-images.githubusercontent.com/24227002/166326546-ed639a7d-6141-415f-8316-d98bc71118e5.png) |

---

## 🌟 Key Improvements in StartPageNG 1.0.0

- **Crash-Resistant Achievement Loading**: Fixed critical crashes (`XamlParseException` / missing icon stream) when loading achievements with null icon paths.
- **Shelves Category Grouping**: Added category-based grouping support (`GroupingField.Category`) for shelves.
- **Dynamic PlayniteAchievements Discovery**: Automatically discovers `achievement_cache.db` across custom user paths.
- **Theme-Adaptive Context Theming**: Context menus automatically adapt to light and dark theme text colors.
- **Automatic Legacy Migration**: Automatically discovers and migrates existing `DefaultLayout.json` and settings from the legacy `felixkmh_StartPage_Plugin` directory.
- **Reset Layout Command**: Quick context menu action to restore default grid layout at any time.
- **Stutter-Free UI Performance**: Eliminated synchronous disk hits and manual GC calls, switching to lazy metadata caching for game logos and trailers.
- **Modern SDK Project**: Built with modern .NET SDK targeting `.NET Framework 4.6.2` and updated to `PlayniteSDK 6.18.0`.

---

## ✨ Features

- **Open on Startup**: Automatically display StartPage whenever Playnite launches.
- **Playnite Notifications & Progress**: Native display of Playnite's global progress bar and notifications directly inside StartPage.
- **Fully Customizable Layout**: Split and merge panels in a flexible grid in Edit Mode (click background or use context menu). Swap views effortlessly via drag-and-drop.
- **Dynamic Backgrounds**:
  - Automatically use background artwork from random games, last played games, fixed images, or custom directories.
  - Configurable blur radius, noise texture overlay, and smooth transition animations.
- **Top Sidebar Positioning**: Positions the StartPage navigation icon at the top of the sidebar.

---

## 🧩 Built-in Views

### Game Shelves
Customizable shelves with custom sorting, grouping (by source, category, platform), and filter presets.

| Shelves Overview | Shelf Properties |
|:---:|:---:|
| ![Shelves](https://user-images.githubusercontent.com/24227002/166330058-2fb741be-d0ad-4878-858e-a3092a25eae0.png) | ![Shelve Properties](https://user-images.githubusercontent.com/24227002/166330144-1d99c2c6-0e26-4721-b989-60db832eabef.png) |

### Clock & Date
Displays current date and time using your Windows system format.

![Clock](https://user-images.githubusercontent.com/24227002/166329790-286656da-09f4-4106-8780-f29c98560c6e.png)

### Weekly Activity
Playtime summary over the last 7 days. Integrates seamlessly with [GameActivityNG](https://github.com/gOOvER/playnite-gameactivity-ng).

![Weekly Activity](https://user-images.githubusercontent.com/24227002/166330561-607c278e-e18a-4037-bf54-919366cbb61e.png)

### Most Played
Displays most played games across custom timeframes. Integrates with [GameActivityNG](https://github.com/gOOvER/playnite-gameactivity-ng).

![Most Played](https://user-images.githubusercontent.com/24227002/166330863-d50389dc-2a05-4183-95c8-e09b4fdcb770.png)

### Recent Achievements
Displays your latest unlocked game achievements. Compatible with [SuccessStory](https://github.com/Lacro59/playnite-successstory-plugin).

![Recent Achievements](https://user-images.githubusercontent.com/24227002/166331100-90e1e535-74b1-4618-9cce-03dac4b26fdd.png)

---

## 🔗 External Views Support

Other extensions can register custom widgets and views with StartPageNG:

| Extension | View Type |
|---|---|
| **[QuickSearchNG](https://github.com/gOOvER/Playnite-QuickSearch)** | Search bar widget |
| **[ScreenshotsVisualizer](https://github.com/Lacro59/playnite-screenshotsvisualizer-plugin)** | Screenshot carousel |
| **[HowLongToBeat](https://github.com/Lacro59/playnite-howlongtobeat-plugin)** | Completion charts & statistics |
| **[PlayState](https://github.com/darklinkpower/PlayniteExtensionsCollection/tree/master/source/Generic/PlayState)** | Active game pause manager |
| **[Play Next](https://github.com/SparrowBrain/Playnite.PlayNext)** | Recommendation list |

---

## 📥 Installation

1. Download the latest `goover_StartPageNG_Plugin_*.pext` package from [Releases](https://github.com/gOOvER/Playnite-StartPage/releases) or [playnite.goover.dev](https://playnite.goover.dev/).
2. In Playnite, drag and drop the `.pext` file into the main window (or use **Main Menu > Add-ons > Install from file...**).
3. Restart Playnite when prompted.

---

## ☕ Support & Donate

If you enjoy StartPageNG and want to support its ongoing development, feel free to support on Ko-fi:

[![Ko-fi](https://img.shields.io/badge/Ko--fi-Support%20gOOvER-F16061?style=for-the-badge&logo=ko-fi&logoColor=white)](https://ko-fi.com/goover)

Direct link: [https://ko-fi.com/goover](https://ko-fi.com/goover)

---

## 📄 License

This project is licensed under the **MIT License**. See [LICENSE](LICENSE) for details.
