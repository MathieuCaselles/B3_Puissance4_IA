from PIL import Image, ImageTk
from tkinter import Tk, messagebox, Label, Button, RAISED, simpledialog, Entry
from Classes.Client import Client
import threading, socket, json, time
import subprocess

class Menu:
    def __init__(self, fenetre, s):

        self.fenetre = fenetre
        self.s = s
        self.createIpPort()

    # Création Menu
    def createMenu(self):
        # Changement titre et taille fenêtre
        self.fenetre.title("Connect 4")
        self.fenetre.geometry("550x750")
        # Gestion du Logo
        self.logo = Image.open('images/connect4.png')
        self.logo = ImageTk.PhotoImage(self.logo)
        self.logo_label = Label(image=self.logo)
        self.logo_label.image = self.logo
        # Création des widgets
        self.unrankedButton = Button(self.fenetre, text="Unranked", command=self.unranked, font=("Calibri", 23), relief=RAISED, width = 12)
        self.rankedButton = Button(self.fenetre, text="Ranked", command=self.ranked, font=("Calibri", 23), relief=RAISED, width = 12)
        self.privateMatchButton = Button(self.fenetre, text="Private Match", command=self.privateMatch, font=("Calibri", 23), relief=RAISED, width = 12)
        self.profileButton = Button(self.fenetre, text="Profile", command=self.createProfileMenu, font=("Calibri", 23), relief=RAISED, width = 12)
        self.leaderboardButton = Button(self.fenetre, text="Leaderboard", command=self.createLeaderboardMenu, font=("Calibri", 23), relief=RAISED, width = 12)
        self.botButton = Button(self.fenetre, text="Bot", command=self.bot, font=("Calibri", 23), relief=RAISED, width = 12)
        self.quitButton = Button(self.fenetre, text="Quit", command=self.fenetre.destroy, font=("Calibri", 23), relief=RAISED, width = 12)
        # Initialisation des boutons dans une liste pour faciliter la destruction des widgets
        self.widgetsMenu = [self.logo_label, self.unrankedButton, self.rankedButton, self.privateMatchButton, self.profileButton, self.leaderboardButton, self.quitButton, self.botButton]
        # Positionnement des widgets
        self.logo_label.grid(row=0, padx=10, pady=15)
        self.unrankedButton.grid(row=1, columnspan=5, pady=5)
        self.rankedButton.grid(row=2, columnspan=5, pady=5)
        self.privateMatchButton.grid(row=3, columnspan=5, pady=5)
        self.profileButton.grid(row=4, columnspan=5, pady=5)
        self.leaderboardButton.grid(row=5, columnspan=5, pady=5)
        self.botButton.grid(row=6, columnspan=5, pady=5)
        self.quitButton.grid(row=7, columnspan=5, pady=5)

    # Création Unranked Menu
    def unranked(self):
        self.destroyMenu()
        self.gui = Client(self.ip, self.port, self.username, self.fenetre, self.s, "quick")
        self.threadGame = threading.Thread(target=self.game, args=(self.gui,))
        self.threadGame.start()

    # Création Ranked Match Menu
    def ranked(self):
        self.destroyMenu()
        self.gui = Client(self.ip, self.port, self.username, self.fenetre, self.s, "ranked", "", self.userId)
        self.threadGame = threading.Thread(target=self.game, args=(self.gui,))
        self.threadGame.start()

    # Création Private Match Menu
    def privateMatch(self):
        # Destruction du Menu
        self.destroyMenu()
        # Demande à l'utilisateur le nom de l'adversaire qu'il recherche
        OpponentName = self.getOpponentName()
        # Création du match privé et démarrage thread et logique jeu
        self.gui = Client(self.ip, self.port, self.username, self.fenetre, self.s, "nickname", OpponentName)
        self.thread = threading.Thread(target=self.game, args=(self.gui,))
        self.thread.start()
        
    # match contre un bot
    def bot(self):
        self.destroyMenu()
        self.gui = Client(self.ip, self.port, self.username, self.fenetre, self.s, "nickname", "tequila")
        self.threadBot = threading.Thread(target=self.createBot)
        time.sleep(0.5)
        self.threadGame = threading.Thread(target=self.game, args=(self.gui,))
        self.threadBot.start()
        self.threadGame.start()
        


    def createBot(self):
        subprocess.call(["python", './../IA_Puissance_4_random/main.py', str(self.username), str(self.ip), str(self.port)])

    def getPlaceAndScore(self):
        # Création du l'objet à envoyer
        jsonDump = json.dumps({"mode":"userInformations", "userid": self.userId, "nickname": self.username, "password": "", "nicknameSearch": "", "nbMax": "10"})
        # Connection et envoi des données vers le serveur
        try:
            self.s.connect((self.ip, self.port))
            self.s.send(jsonDump.encode())
        except ValueError:
            print("connection server problem")
        # Réception des données
        dataFromServer = self.s.recv(1024)
        # Fermeture socket + initialisation nouveau socket
        self.s.close()
        self.s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        return dataFromServer.decode()

    # Création Profile Menu
    def createProfileMenu(self):
        # Détruire le menu
        self.destroyMenu()
        # Changement taille de la fenêtre et titre
        self.fenetre.geometry("550x680")
        self.fenetre.title("Connect 4 - Profile")
        # Récupération place et score
        dataFromServer = json.loads(self.getPlaceAndScore())
        scorePlace = dataFromServer["Message"]
        scorePlace = scorePlace.split(" ")
        self.place = scorePlace[0]
        self.score = scorePlace[1]
        # Récupération de l'image logo
        self.logoProfile = Image.open('images/profile.png')
        self.logoProfile = ImageTk.PhotoImage(self.logoProfile)
        self.logoProfile_label = Label(image=self.logoProfile)
        self.logoProfile_label.image = self.logoProfile
        # Création des widgets
        self.usernameLabel = Label(self.fenetre, text="Username :", font=("Calibri", 34), justify="center")
        self.usernameProfile = Label(self.fenetre, text=self.username, font=("Calibri", 34), bd=10, justify="center")
        self.rankingLabel = Label(self.fenetre, text="Ranking :", font=("Calibri", 34))
        self.ranking = Label(self.fenetre, text=self.placeSyntax(self.place), font=("Calibri", 34), bd=10, justify="center")
        self.scoreLabel = Label(self.fenetre, text="Score :", font=("Calibri", 34))
        self.score = Label(self.fenetre, text=(self.score, "points"), font=("Calibri", 34), bd=10, justify="center")
        self.backButton = Button(self.fenetre, text="Back", command=self.backToMenuFromProfile, font=("Calibri", 12), relief=RAISED, width=10)
        # Placement des widgets
        self.backButton.grid(row=0, column = 0, columnspan=5, pady=5, sticky="ne")
        self.logoProfile_label.grid(row=1, columnspan=5, padx=200, pady=15)
        self.usernameLabel.grid(row=2, columnspan=5, pady=5)
        self.usernameProfile.grid(row=3, columnspan=5, pady=5)
        self.rankingLabel.grid(row=4, columnspan=5, pady=5)
        self.ranking.grid(row=5, columnspan=5, pady=5)
        self.scoreLabel.grid(row=6, columnspan=5, pady=5)
        self.score.grid(row=7, columnspan=5, pady=5)
        # Stockage des widgets du profil pour faciliter
        self.widgetsProfile = [self.logoProfile_label, self.usernameLabel, self.usernameProfile, self.rankingLabel, self.ranking, self.scoreLabel, self.score, self.backButton]

    def createLeaderboardMenu(self):
            dataFromServer = json.loads(self.getLeaderboardInfo())
            self.destroyMenu()
            self.widgetsLeaderboard = []
            self.fenetre.geometry("620x680")
            leaderboard = dataFromServer["Leaderboard"]
            leaderboardTitle = Label(self.fenetre, text="Leaderboard", font=("Calibri", 36))
            leaderboardTitle.grid(row=0, column=1)
            br = Label(self.fenetre, text="", font=("Calibri", 36))
            br.grid(row=1, column=1)
            for index, player in enumerate(leaderboard):
                placeLeaderboard = Label(self.fenetre, text=self.placeSyntax(index+1), font=("Calibri", 18))
                usernameLeaderboard = Label(self.fenetre, text=player["Element"], font=("Calibri", 18))
                scoreLeaderboard = Label(self.fenetre, text=player["Score"], font=("Calibri", 18))
                playerInfo = [placeLeaderboard, usernameLeaderboard, scoreLeaderboard]
                self.widgetsLeaderboard.append(playerInfo)

            for index, listWidget in enumerate(self.widgetsLeaderboard):
                for index2, widget in enumerate(listWidget):
                    widget.grid(row=index+3, column=index2, padx=75, pady=10)

            self.widgetsLeaderboard[0].append(leaderboardTitle)
            self.widgetsLeaderboard[0].append(br)
            self.backButtonLeaderboard = Button(self.fenetre, text="Back", command=self.backToMenuFromLeaderboard, font=("Calibri", 12), relief=RAISED, width=10)
            self.backButtonLeaderboard.grid(row=0, column = 2)
            self.widgetsLeaderboard[0].append(self.backButtonLeaderboard)

    def createRegisterLoginMenu(self):
            self.fenetre.geometry("420x150")
            self.fenetre.title("Login / Register")
            userNameLabel = Label(self.fenetre, text="Username : ", font=("Calibri", 18))
            userNameEntry = Entry(self.fenetre, width=40)
            passwordLabel = Label(self.fenetre, text="Password : ", font=("Calibri", 18))
            passwordEntry = Entry(self.fenetre, width=40, show="*")
            self.errorLabel = Label(self.fenetre, text="", font=("Calibri", 10))
            registerButton = Button(self.fenetre, text="Register", command=lambda: self.sendRegister(userNameEntry.get(), passwordEntry.get()))
            loginButton = Button(self.fenetre, text="Login", command=lambda: self.sendLogin(userNameEntry.get(), passwordEntry.get()))

            userNameLabel.grid(row=2, column=1)
            userNameEntry.grid(row=2, column=2)
            passwordLabel.grid(row=3, column=1)
            passwordEntry.grid(row=3, column=2)
            registerButton.grid(row=4, column=1)
            loginButton.grid(row=4, column=2)
            self.errorLabel.grid(row=5, column=2)
            self.registerLoginWidgets = [self.errorLabel, userNameLabel, userNameEntry, passwordLabel, passwordEntry, registerButton, loginButton]

    def createIpPort(self):
        self.fenetre.geometry("420x150")
        self.fenetre.title("Ip/Port")
        ipLabel = Label(self.fenetre, text="Ip :", font=("Calibri", 18), width=10)
        ipEntry = Entry(self.fenetre, width=25)
        portLabel = Label(self.fenetre, text="Port :", font=("Calibri", 18), width=10)
        portEntry = Entry(self.fenetre, width=25)
        self.errorIpPortLabel = Label(self.fenetre, text="", font=("Calibri", 10))
        ipPortButton = Button(self.fenetre, text="Connection", command=lambda: self.getIpPort(ipEntry.get(), portEntry.get()))
        ipLabel.grid(row=2, column=0)
        ipEntry.grid(row=2, column=1)
        portLabel.grid(row=3, column=0)
        portEntry.grid(row=3, column=1)
        ipPortButton.grid(row=4, column=1)
        self.errorIpPortLabel.grid(row=5, column=1)
        self.ipPortWidgets = [self.errorIpPortLabel, ipLabel, ipEntry, portLabel, portEntry, ipPortButton]


    def getLeaderboardInfo(self):
        jsonDump = json.dumps({"mode":"leaderboard", "userid": self.userId, "nickname": self.username, "password": "", "nicknameSearch": "", "nbMax": "10"})
        try:
            self.s.connect((self.ip, self.port))
            self.s.send(jsonDump.encode())
        except ValueError:
            print("connection server problem")
        dataFromServer = self.s.recv(4096)
        self.s.close()
        self.s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        return dataFromServer

    def getIpPort(self, ip, port):
        if((type(ip) == str and type(int(port)) == int) or (ip == "" or port == "")):
            self.ip = ip
            self.port = int(port)
            self.destroyIpPortMenu()
            self.createRegisterLoginMenu()
        else:
            self.errorIpPortLabel.config(text="Ip or Port not valid", fg="red")
            
    def destroyIpPortMenu(self):
        for widget in self.ipPortWidgets:
                    widget.grid_remove()

    def sendRegister(self, usernameEntry, passwordEntry):
        self.username = usernameEntry
        jsonDump = json.dumps({"mode":"register", "userid": "", "nickname": usernameEntry, "password": passwordEntry, "nicknameSearch": ""})
        try:
            self.s.connect((self.ip, self.port))
            self.s.send(jsonDump.encode())
        except ValueError:
            print("problème connexion serveur")
        dataFromServer = self.s.recv(1024)
        jsonMessage = json.loads(dataFromServer.decode())['Message']
        if("This user already exists" in jsonMessage):
            self.errorLabel.config(text="This user already exists", fg="red")
            self.s.close()
            self.s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        else:
            self.userId = jsonMessage
            self.s.close()
            self.s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            self.destroyRegisterLoginMenu()
            self.createMenu()

    def sendLogin(self, usernameEntry, passwordEntry):
        self.username = usernameEntry
        jsonDump = json.dumps({"mode":"login", "userid": "", "nickname": usernameEntry, "password": passwordEntry, "nicknameSearch": ""})
        try:
            self.s.connect((self.ip, self.port))
            self.s.send(jsonDump.encode())
        except ValueError:
            print("problème connexion serveur")
        dataFromServer = self.s.recv(1024)
        jsonMessage = json.loads(dataFromServer.decode())['Message']
        if("No User Found" in jsonMessage):
            self.errorLabel.config(text="No User Found with this nickname or password", fg="red")
            self.s.close()
            self.s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        else:
            self.userId = jsonMessage
            self.s.close()
            self.s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            self.destroyRegisterLoginMenu()
            self.createMenu()


    def placeSyntax(self, place):
        place = str(place)
        if(place.endswith('1') and place != "11"):
            place += "st"
        elif(place.endswith('2') and place != "12"):
            place += "nd"
        elif(place.endswith('3') and place != "13"):
            place += "rd"
        else:
            place += "th"
        return place

    def backToMenuFromProfile(self):
        for widget in self.widgetsProfile:
            widget.grid_remove()
        self.createMenu()

    def backToMenuFromLeaderboard(self):
        for index, listWidget in enumerate(self.widgetsLeaderboard):
            for widget in listWidget:
                widget.grid_remove()
        self.createMenu()

    def destroyMenu(self):
        for widget in self.widgetsMenu:
            widget.grid_remove()

    def destroyRegisterLoginMenu(self):
        for widget in self.registerLoginWidgets:
            widget.grid_remove()

    def getOpponentName(self):
        try:
            OpponentName = simpledialog.askstring("Input", "What is your opponnent nickname ?",
                                parent=self.fenetre)
            if(OpponentName == "" or OpponentName.lower() == self.username.lower()):
                return getOpponentName(self.fenetre)
            else:
                return OpponentName
        except ValueError:
            print("Not a valid answer")
            return getOpponentName()

    def game(self, gui):
        isGame = True
        try:
            while(isGame):
                json = gui.getServerResponse()
                if("opponent" in json["Message"]):
                    waitingMessage = json["Message"]
                    gui.fenetre.title("Connect 4 - " + waitingMessage)
                    gui.disableGame()
                elif("won" in json["Message"]):
                    winnerMessage = json["Message"]
                    finalPlateau = json["Board"]
                    gui.updateGame(finalPlateau)
                    messagebox.showinfo("Connect 4", winnerMessage + "\nGame ended !")
                    gui.disableGame()
                    gui.destroyGame()
                    self.createMenu()
                    self.fenetre.title("Connect 4")
                    isGame = False
                elif ("disconnected" in json["Message"]):
                    errorMessage = json["Message"]
                    finalPlateau = json["Board"]
                    gui.updateGame(finalPlateau)
                    messagebox.showinfo("Connect 4", errorMessage + "\nGame ended !")
                    gui.disableGame()
                    gui.destroyGame()
                    self.createMenu()
                    self.fenetre.title("Connect 4")
                    isGame = False
                elif("Waiting" in json["Message"]):
                    jsonInfoPlateau = json["Board"]
                    waitingMessage = json["Message"]
                    opponnentColor = json["PlayerColor"]
                    gui.updateGame(jsonInfoPlateau)
                    gui.fenetre.title("Connect 4 - " + opponnentColor + " - " + json["Message"])
                    gui.disableGame()
                elif("Tied" in json["Message"]):
                    tieMessage = json["Message"]
                    finalPlateau = json["Board"]
                    gui.updateGame(finalPlateau)
                    messagebox.showinfo("Connect 4", tieMessage + "\nGame ended !")
                    gui.disableGame()
                    gui.destroyGame()
                    self.createMenu()
                    self.fenetre.title("Connect 4")
                    isGame = False
                elif("level" in json["Message"]):
                    errorMessage = json["Message"]
                    messagebox.showinfo("Connect 4", errorMessage)
                    gui.disableGame()
                    gui.destroyGame()
                    self.createMenu()
                    self.fenetre.title("Connect 4")
                    isGame = False
                else:
                    jsonInfoPlateau = json["Board"]
                    playerColor = json["PlayerColor"]
                    gui.fenetre.title("Connect 4 - " + playerColor + " - " + json["Message"])
                    gui.updateGame(jsonInfoPlateau)
                    if("turn" in json["Message"]):
                        gui.enableGame()
                        gui.updateGame(jsonInfoPlateau)
                        for i in json["CompleteColumns"]:
                            if(i != -1):
                                gui.buttons[i].config(state="disabled")
        except ValueError:
            print("Error message in game")
        gui.s.close()
        self.s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)