import os
name = input("Look For:")
command = f"findstr /s /i {name} *.*"
os.system(command)