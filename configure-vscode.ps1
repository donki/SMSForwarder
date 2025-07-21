$settings = @{
    "debug.onTaskErrors" = "debugAnyway"
    "security.workspace.trust.enabled" = $false
    "debug.confirmOnExit" = "never"
    "debug.showInStatusBar" = "always"
    "debug.toolBarLocation" = "docked"
    "debug.internalConsoleOptions" = "openOnSessionStart"
    "debug.openDebug" = "openOnSessionStart"
    "debug.console.closeOnEnd" = $false
    "dotnet.enableDebugging" = $true
    "debug.confirmDelete" = $false
    "debug.saveBeforeStart" = "none"
    "debug.allowBreakpointsEverywhere" = $true
    "editor.formatOnSave" = $true
    "editor.formatOnType" = $true
    "files.autoSave" = "afterDelay"
    "files.autoSaveDelay" = 1000
    "editor.acceptSuggestionOnEnter" = "on"
    "editor.quickSuggestions" = @{
        "other" = $true
        "comments" = $true
        "strings" = $true
    }
    "editor.suggestOnTriggerCharacters" = $true
    "editor.acceptSuggestionOnCommitCharacter" = $true
}

$settingsPath = "$env:APPDATA\Code\User\settings.json"

if (Test-Path $settingsPath) {
    $currentSettings = Get-Content $settingsPath | ConvertFrom-Json
    $settings.GetEnumerator() | ForEach-Object {
        Add-Member -InputObject $currentSettings -NotePropertyName $_.Key -NotePropertyValue $_.Value -Force
    }
    $currentSettings | ConvertTo-Json -Depth 10 | Set-Content $settingsPath
} else {
    New-Item -Path $settingsPath -Force
    $settings | ConvertTo-Json -Depth 10 | Set-Content $settingsPath
}

Write-Host "Configuración de VS Code actualizada exitosamente"
