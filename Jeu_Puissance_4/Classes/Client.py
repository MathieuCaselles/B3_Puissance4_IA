from tkinter import Tk, Button, Frame, Message, Text, simpledialog, Label, PhotoImage, FLAT, messagebox
import socket
import json
import time
from PIL import Image, ImageTk

class Client:
    def __init__(self, ip, port, username, fenetre, s, matchMakingType, opponentUsername = None, userId = ""):

        self.ip = ip
        self.port = port
        self.fenetre = fenetre
        self.s = s
        self.fenetre.title("Puissance 4")
        self.username = username
        self.matchMakingType = matchMakingType
        self.opponentUsername = opponentUsername
        self.userId = userId
        self.redRound = PhotoImage(file='./images/rouge.png')
        self.yellowRound = PhotoImage(file='./images/jaune.png')
        emptyCell = PhotoImage(file='./images/vide.png')
        self.fenetre.protocol("WM_DELETE_WINDOW", self.on_closing)

        try:
            self.s.connect((ip, port))
            print("connected")
            jsonDump = json.dumps({"mode":self.matchMakingType, "userid": self.userId, "nickname": self.username, "password": "", "nicknameSearch": self.opponentUsername})
            self.s.send(jsonDump.encode())
            print("send data")
        except ValueError:
            print("connection server problem")
        
        self.a0 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.a1 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.a2 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.a3 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.a4 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.a5 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)

        self.b0 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.b1 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.b2 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.b3 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.b4 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.b5 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)

        self.c0 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.c1 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.c2 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.c3 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.c4 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.c5 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)

        self.d0 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.d1 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.d2 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.d3 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.d4 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.d5 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)

        self.e0 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.e1 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.e2 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.e3 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.e4 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.e5 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)

        self.f0 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.f1 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.f2 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.f3 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.f4 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.f5 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)

        self.g0 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.g1 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.g2 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.g3 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.g4 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)
        self.g5 = Label(self.fenetre, image=emptyCell, relief=FLAT, borderwidth=0)

        self.button6 = Button(text="7", width = 13, height=6, command=lambda: self.sendPlayerMoove(6))
        self.button5 = Button(text="6", width = 13, height=6, command=lambda: self.sendPlayerMoove(5))
        self.button4 = Button(text="5", width = 13, height=6, command=lambda: self.sendPlayerMoove(4))
        self.button3 = Button(text="4", width = 13, height=6, command=lambda: self.sendPlayerMoove(3))
        self.button2 = Button(text="3", width = 13, height=6, command=lambda: self.sendPlayerMoove(2))
        self.button1 = Button(text="2", width = 13, height=6, command=lambda: self.sendPlayerMoove(1))
        self.button0 = Button(text="1", width = 13, height=6, command=lambda: self.sendPlayerMoove(0))

        self.board = [[self.a5, self.b5, self.c5, self.d5, self.e5, self.f5, self.g5], 
                        [self.a4, self.b4, self.c4, self.d4, self.e4, self.f4, self.g4], 
                        [self.a3, self.b3, self.c3, self.d3, self.e3, self.f3, self.g3], 
                        [self.a2, self.b2, self.c2, self.d2, self.e2, self.f2, self.g2],
                        [self.a1, self.b1, self.c1, self.d1, self.e1, self.f1, self.g1],
                        [self.a0, self.b0, self.c0, self.d0, self.e0, self.f0, self.g0]]

        self.buttons = [self.button0, self.button1, self.button2, self.button3, self.button4, self.button5, self.button6]
        self.createBoard() 
        
        
        
    def sendPlayerMoove(self, colonneNumber):
        self.s.send(str(colonneNumber).encode())
    
    def forceQuit(self):
        self.fenetre.destroy()
    
    def createBoard(self):
        self.fenetre.geometry("700x670")
        self.a0.grid(row = 6, column = 0)
        self.a1.grid(row = 5, column = 0)
        self.a2.grid(row = 4, column = 0)
        self.a3.grid(row = 3, column = 0)
        self.a4.grid(row = 2, column = 0)
        self.a5.grid(row = 1, column = 0)

        self.b0.grid(row = 6, column = 1)
        self.b1.grid(row = 5, column = 1)
        self.b2.grid(row = 4, column = 1)
        self.b3.grid(row = 3, column = 1)
        self.b4.grid(row = 2, column = 1)
        self.b5.grid(row = 1, column = 1)

        self.c0.grid(row = 6, column = 2)
        self.c1.grid(row = 5, column = 2)
        self.c2.grid(row = 4, column = 2)
        self.c3.grid(row = 3, column = 2)
        self.c4.grid(row = 2, column = 2)
        self.c5.grid(row = 1, column = 2)
        
        self.d0.grid(row = 6, column = 3)
        self.d1.grid(row = 5, column = 3)
        self.d2.grid(row = 4, column = 3)
        self.d3.grid(row = 3, column = 3)
        self.d4.grid(row = 2, column = 3)
        self.d5.grid(row = 1, column = 3)

        self.e0.grid(row = 6, column = 4)
        self.e1.grid(row = 5, column = 4)
        self.e2.grid(row = 4, column = 4)
        self.e3.grid(row = 3, column = 4)
        self.e4.grid(row = 2, column = 4)
        self.e5.grid(row = 1, column = 4)

        self.f0.grid(row = 6, column = 5)
        self.f1.grid(row = 5, column = 5)
        self.f2.grid(row = 4, column = 5)
        self.f3.grid(row = 3, column = 5)
        self.f4.grid(row = 2, column = 5)
        self.f5.grid(row = 1, column = 5)

        self.g0.grid(row = 6, column = 6)
        self.g1.grid(row = 5, column = 6)
        self.g2.grid(row = 4, column = 6)
        self.g3.grid(row = 3, column = 6)
        self.g4.grid(row = 2, column = 6)
        self.g5.grid(row = 1, column = 6)

        
        self.button6.grid(row = 7, column = 6)
        self.button5.grid(row = 7, column = 5)
        self.button4.grid(row = 7, column = 4)
        self.button3.grid(row = 7, column = 3)
        self.button2.grid(row = 7, column = 2)
        self.button1.grid(row = 7, column = 1)
        self.button0.grid(row = 7, column = 0)

    def updateGame(self, boardGame):
        global redRound, yellowRound
        newBoard = boardGame.split('\n')
        for i in range(len(newBoard)):
            for j in range(len(newBoard[i])):
                if(newBoard[i][j] == "1"):
                    self.board[i][j].configure(image=self.redRound)
                elif(newBoard[i][j] == "2"):
                    self.board[i][j].configure(image=self.yellowRound)


    def getServerResponse(self):
        dataFromServer = self.s.recv(1024)
        return json.loads(dataFromServer.decode())

    def disableGame(self):
        for i in self.buttons:
            i.config(state="disabled")

    def enableGame(self):
        for i in self.buttons:
            i.config(state="active")

    def destroyGame(self):
        for button in self.buttons:
            button.grid_remove()
        for row in self.board:
            for cell in row:
                cell.grid_remove()
    
    def on_closing(self):
            if messagebox.askokcancel("Quit", "Do you want to quit?"):
                self.s.close()
                self.fenetre.destroy()