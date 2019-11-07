rd /s /q Tech\pro.Android(release)\assets\Data\ActionScript
rd /s /q Tech\pro.Android(release)\assets\Data\Armatures
rd /s /q Tech\pro.Android(release)\assets\Data\Bullets
rd /s /q Tech\pro.Android(release)\assets\Data\Levels
rd /s /q Tech\pro.Android(release)\assets\Data\Map
rd /s /q Tech\pro.Android(release)\assets\Data\Particles
rd /s /q Tech\pro.Android(release)\assets\Data\Sounds
rd /s /q Tech\pro.Android(release)\assets\Data\UI

mkdir "Tech\\pro.Android(release)\\assets\\Data\\ActionScript"
mkdir "Tech\\pro.Android(release)\\assets\\Data\\Armatures"
mkdir "Tech\\pro.Android(release)\\assets\\Data\\Bullets"
mkdir "Tech\\pro.Android(release)\\assets\\Data\\Levels"
mkdir "Tech\\pro.Android(release)\\assets\\Data\\Map"
mkdir "Tech\\pro.Android(release)\\assets\\Data\\Particles"
mkdir "Tech\\pro.Android(release)\\assets\\Data\\Sounds\\Effects\\ogg"
mkdir "Tech\\pro.Android(release)\\assets\\Data\\Sounds\\Music"
mkdir "Tech\\pro.Android(release)\\assets\\Data\\UI"

xcopy /E /Y /H /R "Tech\Resources\\Data\\ActionScript" "Tech\\pro.Android(release)\\assets\\Data\\ActionScript"
xcopy /E /Y /H /R "Tech\Resources\\Data\\Armatures" "Tech\\pro.Android(release)\\assets\\Data\\Armatures"
xcopy /E /Y /H /R "Tech\Resources\\Data\\Bullets" "Tech\\pro.Android(release)\\assets\\Data\\Bullets"
xcopy /E /Y /H /R "Tech\Resources\\Data\\Levels" "Tech\\pro.Android(release)\\assets\\Data\\Levels"
xcopy /E /Y /H /R "Tech\Resources\\Data\\Map" "Tech\\pro.Android(release)\\assets\\Data\\Map"
xcopy /E /Y /H /R "Tech\Resources\\Data\\Particles" "Tech\\pro.Android(release)\\assets\\Data\\Particles"
xcopy /E /Y /H /R "Tech\Resources\\Data\\Sounds\\Effects\\ogg" "Tech\\pro.Android(release)\\assets\\Data\\Sounds\\Effects\\ogg"
xcopy /E /Y /H /R "Tech\Resources\\Data\\Sounds\\Music" "Tech\\pro.Android(release)\\assets\\Data\\Sounds\\Music"
xcopy /E /Y /H /R "Tech\Resources\\Data\\UI" "Tech\\pro.Android(release)\\assets\\Data\\UI"

copy /y Tech\Resources\Config  Tech\pro.Android(release)\assets\Config

copy /y Tech\Resources\ScriptAssemblies\Game.dll  Tech\pro.Android(release)\assets\ScriptAssemblies
copy /y Tech\Resources\ScriptAssemblies\MatrixEngine.dll  Tech\pro.Android(release)\assets\ScriptAssemblies

Python ImagePak.py
pause