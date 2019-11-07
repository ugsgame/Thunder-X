import subprocess
import os
import os.path

inputPath = "Tech\\Resources\\Data"
outputPath = "Tech\\pro.Android(release)\\assets\\Data"
#inputPath = "Data_"
#outputPath = "Out"
filter = [
		"",
		"",
		]

for parent,dirnames,filenames in os.walk(inputPath):
	for	dirname in dirnames:                      
		print "parent is:" + parent
		print	"dirname is" + dirname

	for filename in filenames:                      
		print "parent is:" + parent
		print "filename is:" + filename
		print "the full name of the file is:" + os.path.join(parent,filename)
		inputFullPath = os.path.join(parent,filename)
		outputDir = parent.replace(inputPath,outputPath)
		isExists=os.path.exists(outputDir)
		
		if not isExists:
			os.makedirs(outputDir);
				
		outputFullPath ="--output " + outputDir+"/"+filename				
		cmd = "pngquant.exe --force --verbose --speed=1 256 "+ outputFullPath +" "+ inputFullPath
		os.system(cmd)

raw_input("Press enter key to close this window");