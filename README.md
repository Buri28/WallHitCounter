# WallHitCounter

This repository contains the WallHitCounter Beat Saber plugin extracted from the workspace.

Build (example):
```powershell
# Provide the Beat Saber installation path via environment variable or MSBuild property
$env:BEATSABER_PATH = 'D:\BSManager\BSInstances\1.39.1'
cd C:\work\plugin\WallHitCounter
dotnet build WallHitCounter.csproj -c Debug /p:BeatSaberPath="$env:BEATSABER_PATH"
```

Notes:
- Copy binary resources (e.g. `Resources/icon.png`) from the original workspace manually if needed.
- `deploy_instances.json` is intentionally not included; use a local copy if you have deployment targets.
