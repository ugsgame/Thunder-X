mkdir "..\\Res\\Data\\Map\\MapData"
mkdir "..\\Res\\Data\\Map\\CustomProperty"
xcopy /E /Y /H /R ".\\CocosStudio\\assets\\Data\\Effects" "..\\Res\\Data\\Effects"
xcopy /E /Y /H /R ".\\CocosStudio\\assets\\Data\\Map" "..\\Res\\Data\\Map"
xcopy /E /Y /H /R ".\\CocosStudio\\assets\\Custom_Property" "..\\Res\\Data\\Map\\CustomProperty"
xcopy /E /Y /H /R ".\\CocosStudio\\assets\\publish" "..\\Res\\Data\\Map\\MapData"
pause