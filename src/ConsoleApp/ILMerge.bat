ECHO parameter=%1
CD %1
COPY AddOSGrid.exe temp.exe
..\..\packages\ilmerge.2.13.0307\ILMerge.exe /out:AddOSGrid.exe temp.exe DotNetCoords.dll
DEL temp.exe