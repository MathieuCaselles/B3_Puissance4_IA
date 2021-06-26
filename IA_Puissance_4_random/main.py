from Classes.IA import Ia
from tkinter import messagebox
import threading
import numpy as np
from random import randint
import random
import time
import sys

def game(ia):
    isGame = True
    env = ia
    try:
        while (isGame):
            json = ia.getServerResponse()
            if("opponent" in json["Message"]):
                waitingMessage = json["Message"]
            elif("won" in json["Message"]):
                isGame = False
            elif ("disconnected" in json["Message"]):
                isGame = False
            elif("Waiting" in json["Message"]):
                couleur = json["OpponentColor"]
            elif("Tied" in json["Message"]):
                isGame = False
            else:
                couleur = json["PlayerColor"]
                if("turn" in json["Message"]):
                    for numCol in json["CompleteColumns"]:
                        if(numCol != -1):
                            env.actions.remove(numCol)
                            
                    at = take_action(env.actions)
                    time.sleep(0.1)
                    env.sendPlayerMoove(at)
                    env.actions = [0, 1, 2, 3, 4, 5, 6]
    except ValueError:
        print("Error message")


def take_action(list):
    # Take an action
    action = list[randint(0, len(list)-1)]
    return action

def main(args):
    env = Ia(args)
    game(env)
    env.s.close()
    del env

main(sys.argv)