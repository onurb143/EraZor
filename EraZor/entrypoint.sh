#!/bin/sh
set -e
dotnet ef database update
dotnet EraZor.dll
