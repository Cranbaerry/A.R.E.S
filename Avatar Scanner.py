import os
name = input("Avatar Name:")
command = f"findstr /s /i {name} *.*"
os.system(command)