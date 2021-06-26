# Puissance 4

(Projet de fin d'année de B3 réupload en public sans nos accès bdd)

# Sommaire

-   1- Présentation
-   2- Installation

# 1 Présentation:

-   Création d'un puissance 4 multijoueur avec possibilité de jouer face à une IA

## Distribution des rôles

-   Mathieu : Serveur C#
-   Rémi : Client python
-   Antoine : IA python

## Notre projet a pour finalité la création d'une IA pour jouer face à l'utilisateur.

-   IHM: en python (tkinter)
-   API: logique de jeu + stream vers IHM en C#
-   BDD : NoSQL (pcq on commence et on souhaite le pratiquer) en premier pour conserver le pseudo des joueurs et si l'avancement du projet nous l'autorise : conservation du score en vue de création d'un ranking et possiblement un match-making
-   création d'une save si l'utilisateur quitte une partie en cours
-   Défis technique : IA: python (reinforcement learning)

## Fonctionnalités :

### Minimum requis: _Difficulté : 19_

-   une architecture en services (API + IHM au minimum) | Difficulté : 4
-   l’utilisation d’au moins 3 Design Patterns de façon utile | Difficulté : 3
-   une base de données NoSQL | Difficulté : 2
-   Utilisation de MongoDB et Redis | Difficulté : 4
-   réaliser au moins un défi technique (ex : génération de code, IA, machine learning,
    BI, simulation physique...). | Difficulté : 6

### 7 écrans: _Difficulté : 3_

-   Menu principal (jouer / quitter / historique partie)
-   Affichage liste historique partie
-   Sélection IP + port
-   Pseudo
-   Interface config partie (online - local / ia dispo ou pas)
-   Ecran en attente de joueur
-   Ecran de jeu
-   Ecran de fin de partie (affichage score et retour menu principale ou rejouer)

### Le serveur contenant la logique du jeu + matchmaking rapide contient : _Difficulté : 7_

-   un système de socket avec les actions suivantes : | Difficulté : 5
    -   arrivé d’un client dans la file d’attente (réception)
    -   début d’un match (envoie)
    -   réception d’un tour (réception puis envoie)
    -   fin d’un match (envoie)
-   une vérification constante de la file d’attente et création de matchs en fonction | Difficulté : 2

### Le logiciel client contient : _Difficulté : 8_

-   un système de socket avec les actions suivantes : | Difficulté : 5

    -   entrée en file d’attente (envoie)
    -   début d’un match (réception)
    -   jouer un coup (envoie)
    -   prendre en compte le coup adverse (réception)
    -   fin d’un match (réception)

-   demander à l'ia de se connecter | Difficulté 1
-   une partie de la logique du jeu choisis | Difficulté : 2

### Déroulement d'une partie: _Difficulté : 2_

-   Jeu en tour par tour sur une grille
    -   Les 2 joueurs posent 1 jetons chacun à tour de rôle
    -   Le but étant d'aligner de manière horizontal / vertical / diagonal 4 jetons de la même couleur

### L'IA : _Difficulté : 6_

-   Création d'une IA en python. difficulté 6

# Total 45 pts

# 2 Installation

## 1- Cloner ce projet

Cloner l'intégralité du projet.
Si vous voulez juste rejoindre un ami qui lancera le serveur de son côté, vos pouvez ne pas télécharger le deossier "Serveur_Puissance_4" et skip l'étape 2 "lancez le server".

## 2 - Lancer le serveur

Il va maintenant falloir télécharger ngrok. https://ngrok.com/
Si vous voulez juste tester le projet en local en jouant avec vous-même, vous n'avez pas besoin de ngrok.

Exécutez ngrok.exe et rentrez dans le terminal qui s'ouvre la commande `ngrok.exe tcp 11000`  
Déplacez vous dans le dossier`Serveur_Puissance_4/bin/Release/net5.0/publish`

-   Sur Windows exécutez `.\Serveur_Puissance_4.exe`
-   Sur Linux exécutez `.\Serveur_Puissance_4`

## 3 - Préparez le bot

Déplacez vous dans le dossier `IA_Puissance_4_random`  
Exécutez la commande : `pip install requirements.txt`

## 4 - Lancer le client

Déplacez vous dans le dossier `Jeu_Puissance_4`  
Exécutez la commande : `pip install requirements.txt`  
Exécutez la commande : `python main.py`

Le jeu va vous demander l'ip et le port du serveur.  
Si vous voulez juste tester en local, mettez en ip 127.0.0.1 et en port 11000.
Sinon cherchez une ligne ressemblant à `Forwarding tcp://4.tcp.ngrok.io:13107 -> localhost:11000` dans le terminal ngrok.
Entrez l'ip et le port correspondant dans les input du client. Dans mon exemple l'ip est `4.tcp.ngrok.io` et le port est `13107`.

Inscrivez-vous ou connectez-vous si vous avez déjà un compte et vous êtes prêt pour jouer /

-   unranked sert à faire une partie en matchmaking rapide
-   private match sert à faire une partie contre un joueur précis en rentrant son pseudo.
-   Ranked sert à faire une partie classé. De base votre score est à 1000.
-   Profil sert à regarder vos stats.
-   Leaderbord sert à regarder le top 10.
-   Bot sert à jouer contre un bot.
