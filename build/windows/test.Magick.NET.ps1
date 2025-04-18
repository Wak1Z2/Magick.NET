# Copyright Dirk Lemstra https://github.com/dlemstra/Magick.NET.
# Licensed under the Apache License, Version 2.0.

param (
    [string]$quantumName = $env:QuantumName,
    [string]$platformName = $env:PlatformName
)

. $PSScriptRoot\..\..\tools\windows\utils.ps1

function runTests($quantumName, $platformName, $targetFramework, $project) {
    $platform = $platformName
    $testPlatform = $platformName

    if ($platform -eq "Any CPU") {
      $platform = "AnyCPU"
      $testPlatform = "x64"
    }

    $folder = fullPath "tests\$project.Tests\bin\Test$quantumName\$platform\$targetFramework"
    & "$folder\$project.Tests.exe"

    CheckExitCode("Failed to test Magick.NET")
}

function testMagickNET($quantumName, $platformName) {
    runTests $quantumName $platformName "net472" "Magick.NET"

    if ($platformName -ne "Any CPU") {
        runTests $quantumName $platformName "net8.0" "Magick.NET"

        if ($quantumName -like "*OpenMP*") {
            return
        }

        runTests $quantumName $platformName "net8.0" "Magick.NET.AvaloniaMediaImaging"
        runTests $quantumName $platformName "net8.0" "Magick.NET.SystemDrawing"
        runTests $quantumName $platformName "net8.0" "Magick.NET.SystemWindowsMedia"
    } else {
        runTests "" $platformName "net472" "Magick.NET.Core"
        runTests "" $platformName "net8.0" "Magick.NET.Core"
    }

    if ($quantumName -like "*OpenMP*") {
        return
    }

    runTests $quantumName $platformName "net472" "Magick.NET.SystemDrawing"
    runTests $quantumName $platformName "net472" "Magick.NET.SystemWindowsMedia"
}

testMagickNET $quantumName $platformName
