import os

for filename in os.listdir("."):
    if "Armature_" in filename and filename.endswith(".anim"):
        if not os.path.exists(filename.replace("Armature_", "")):
            os.rename(filename, filename.replace("Armature_", ""))
