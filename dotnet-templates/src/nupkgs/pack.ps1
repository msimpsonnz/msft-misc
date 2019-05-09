#Setup variables
$pkgsCache = ".\MSimpson.ConsoleTemplate.CSharp\content\*.*"
$source = "..\MSimpson.ConsoleTemplate.CSharp\*.*"

#Clean up from previous pack
if (Test-Path $pkgsCache) 
{
    Remove-Item $pkgsCache -Recurse
}

#Get latest files from source folder
Copy-Item $source .\MSimpson.ConsoleTemplate.CSharp\content -Recurse

nuget.exe pack .\MSimpson.ConsoleTemplate.CSharp\MSimpson.ConsoleTemplate.CSharp.nuspec