import time
from dhooks import Webhook
from random import randrange
print("Imports loaded")
hook = Webhook('url')
print("Webhook loaded")
with open('img.png', 'rb') as f:
    img = f.read()
    print("Image loaded")
hook.modify(name='Clownie', avatar=img)
print("Profile loaded")
QList = []
with open("Quotes.txt") as file:
    for line in file:
        QList += [line.rstrip()]
QListItems = len(QList)
print(f'List loaded(Items loaded:{QListItems}):')
print(QList)
Count = 1
def Hook(QListItems):
    QListItems = QListItems-1
    QItem = randrange(QListItems)
    print(f'Choice made: {QItem}|{QList[QItem]}')
    hook.send(QList[QItem])
    print("Sent!")
def Timer(QListItems, Mins, Count):
    Hook(QListItems)
    print(f'Sleeping for {Mins}Mins or {Mins * 60}Secs')
    time.sleep(Mins * 60)
    print(f'Count:{Count}')
    Count = Count+1
    Timer(QListItems, randrange(120), Count)
Timer(QListItems, randrange(120), Count)
