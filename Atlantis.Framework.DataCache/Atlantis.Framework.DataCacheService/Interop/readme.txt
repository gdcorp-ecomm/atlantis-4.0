When you have to generate a new interop, you should use TLBIMP from a visual studio command window.
Here is a sample of the command and parameters you will want to use:

tlbimp gddatacache.exe /out:Interop.gdDataCacheLib.dll /namespace:gdDataCacheLib /productversion:1.13.4.29

Match the productversion to the version of the gdDatatCache.exe (right-click properties to see it)
