from Classes.Client import Client
from Classes.Menu import Menu
from tkinter import messagebox, Tk, simpledialog
import socket
import time
import queue
import threading

def main():
    fenetre = Tk()
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    menu = Menu(fenetre, s)
    fenetre.mainloop()
main()