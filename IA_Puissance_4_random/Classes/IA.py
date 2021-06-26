from tkinter import Tk, Button, Frame, Message, Text, simpledialog, messagebox
import socket
import json
import time
import numpy as np
from random import randint
import random

class Ia:
    def __init__(self, args):
        self.opponentName = str(args[1])
        self.ip = str(args[2])
        self.port = int(args[3])
        self.username = "tequila"
        self.matchMakingType = "nickname"

        self.actions = [0, 1, 2, 3, 4, 5, 6]

        self.s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

        try:
            self.s.connect((self.ip, self.port))
            jsonDump = json.dumps({"mode":self.matchMakingType, "userid": '', "nickname": self.username, "password": "", "nicknameSearch": self.opponentName})
            self.s.send(jsonDump.encode())
        except ValueError:
            print("connection server problem")

    def sendPlayerMoove(self, colonneNumber):
        self.s.send(str(colonneNumber).encode())

    def getServerResponse(self):
        dataFromServer = self.s.recv(1024)
        return json.loads(dataFromServer.decode())