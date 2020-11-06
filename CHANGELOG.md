# Changelog
Changelog format: [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)
Version scheme: [Semantic Versioning](https://semver.org/spec/v2.0.0.html)

## [Unreleased]
### Added
- AndASM-NMS library
  * See library changelog
  * Non-UI code moved to library
- Created new WPF XAML GUI
  * Can toggle mods
  * Option to open mods folder
  * Option to create desktop shortcut to mods folder
### Removed
- Console application

## [0.0.4] - 2020-10-26
### Added
- Github publishing action for releases
- MSBuild action to create Release.zip
- This changelog
### Changed
- Let Resharper have it's way with code formatting
- Builds outputted to root/Build folder instead of project folder
- Target .Net Framework 4.7.2
  - Release a minimal-sized framework-dependant exe
  - No Man's Sky requirements imply .Net Framework 4.8 so all users should be able to run 4.7.2 applications

## [0.0.3] - 2020-10-24
### Changed
- Color console
  - Switched to using VT100 codes
  - Refactored into separate class
### Fixed
- Issue #1
  - Package.InstalledLocation used due to InstalledPath being newer than the minimum required Windows version for No Man's Sky

## [0.0.2] - 2020-10-23
### Added
- C# code to create tombstone file directly, marking `DISABLEMODS.TXT` as deleted.
### Changed
- Complete rewrite to be a less-intrusive standalone application
### Removed
- UWPInjector and UWPDumper

## [0.0.1] - 2020-10-20
### Added
- Created an initial proof-of-concept using UWPDumper

[Unreleased]: https://github.com/AndASM/ANMSMEMSPC
[0.1.0]: https://github.com/AndASM/ANMSMEMSPC/v0.0.4...v0.1.0
[0.0.4]: https://github.com/AndASM/ANMSMEMSPC/v0.0.3...v0.0.4
[0.0.3]: https://github.com/AndASM/ANMSMEMSPC/v0.0.2...v0.0.3
[0.0.2]: https://github.com/AndASM/ANMSMEMSPC/v0.0.1...v0.0.2
[0.0.1]: https://github.com/AndASM/ANMSMEMSPC/releases/v0.0.1
