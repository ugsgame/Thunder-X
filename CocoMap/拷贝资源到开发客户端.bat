mkdir "..\\Tech\\Resources\\Data\\Map\\MapData"
mkdir "..\\Tech\\Resources\\Data\\Map\\CustomProperty"
xcopy /E /Y /H /R ".\\CocosStudio\\assets\\Data\\Effects" "..\\Tech\\Resources\\Data\\Effects"
xcopy /E /Y /H /R ".\\CocosStudio\\assets\\Data\\Map" "..\\Tech\\Resources\\Data\\Map"
xcopy /E /Y /H /R ".\\CocosStudio\\assets\\Custom_Property" "..\\Tech\\Resources\\Data\\Map\\CustomProperty"
xcopy /E /Y /H /R ".\\CocosStudio\\assets\\publish" "..\\Tech\\Resources\\Data\\Map\\MapData"
pause