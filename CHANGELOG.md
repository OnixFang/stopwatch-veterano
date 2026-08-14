# Changelog

All notable changes to this project will be documented in this file.

---

## [0.2.0] - 2026-08-14

### Added

- Title screen with main menu and keyboard navigation.
- Menu indicator arrow for navigating UI options.
- Sound effects system for menu interactions and timer events (MenuSelect, MenuAccept, TimerClick, StopwatchClick).
- Background music system with volume control (MainTheme).
- Credits screen accessible from the title menu.
- Exit game functionality.
- Player indicator arrow during tournament gameplay.
- Separated PlayerList component for improved player management.
- Dedicated RankingPanel and RankingEntry components for enhanced leaderboard display.
- Enhanced UI workflow between title screen, tournament settings, and gameplay.

### Changed

- Reorganized tournament settings into a dedicated panel accessible from the title screen.
- Improved menu navigation with keyboard input handling and dynamic element selection.
- Refactored leaderboard rendering into a dedicated RankingPanel component.
- Audio system now separates SFX and music into distinct AudioSource instances.

### Fixed

- Consistent menu focus behavior when navigating between UI screens.

---

## [0.1.0] - 2026-07-25

### Added

- Initial playable prototype.
- Tournament setup screen.
- Player registration.
- Configurable target time.
- Stopwatch gameplay.
- Automatic player turn progression.
- Live leaderboard.
- Automatic ranking based on the closest recorded time.
- Ability to return to the setup screen after a tournament.

### Changed

- Initial UI layout and tournament flow.

### Fixed

- Consistent stopwatch formatting using `TimeSpan`.