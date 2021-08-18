import string
safe = string.ascii_letters+string.digits
print(safe)
poo = "J̶o̶h̶n̶n̶y̶"
print(poo)
print(poo.encode().decode("ascii", errors="ignore"))