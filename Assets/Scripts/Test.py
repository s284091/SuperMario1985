file=open("../Scenes/Livello1.unity","r")
lines=file.readlines()
file.close()

for i in range(len(lines)):
	if(lines[i].__contains__("m_LocalRotation") and not(lines[i].__contains__("x: -0, y: -0, z: -0, w: 1"))):
		print(lines[i])