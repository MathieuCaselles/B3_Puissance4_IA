using System;

namespace Serveur_Puissance_4.game {
    public class Board {
        public readonly static int sizeRow = 6;
        public readonly static int sizeColumn = 7;
        public Cell[][] Grid { get; set; } = new Cell[sizeRow][];
        public int WhoPlay { get; set; } = 1;

        public Board() {
            //initialiser les colonne
            for (int row = 0; row < sizeRow; row++) {
                this.Grid[row] = new Cell[sizeColumn];
            }
            //initialiser les Case
            for (int row = 0; row < sizeRow; row++) {
                for (int column = 0; column < sizeColumn; column++) {
                    this.Grid[row][column] = new Cell();
                }
            }
        }

        public void Play(int column) {
            int row = 0;

            while (this.Grid[row][column].StateCase != TypeCell.EMPTY) {
                row++;
            }
            this.Grid[row][column].StateCase = WhoPlay == 1 ? TypeCell.P1 : TypeCell.P2;

        }

        public int Win() {
            // recherche dans le plateau les successions identiques horizontales
            for (int row = 0; row < sizeRow; row++) {
                for (int column = 0; column < sizeColumn - 3; column++) {
                    if (this.Grid[row][column].StateCase != TypeCell.EMPTY &&
                        this.Grid[row][column].StateCase == this.Grid[row][column + 1].StateCase &&
                        this.Grid[row][column].StateCase == this.Grid[row][column + 2].StateCase &&
                        this.Grid[row][column].StateCase == this.Grid[row][column + 3].StateCase) {
                        return (int) this.Grid[row][column].StateCase;
                    }
                }
            }

            // recherche dans le plateau les successions identiques verticales
            for (int row = 0; row < sizeRow - 3; row++) {
                for (int column = 0; column < sizeColumn; column++) {
                    if (this.Grid[row][column].StateCase != TypeCell.EMPTY &&
                        this.Grid[row][column].StateCase == this.Grid[row + 1][column].StateCase &&
                        this.Grid[row][column].StateCase == this.Grid[row + 2][column].StateCase &&
                        this.Grid[row][column].StateCase == this.Grid[row + 3][column].StateCase) {
                        return (int) this.Grid[row][column].StateCase;
                    }
                }
            }

            // recherche dans le plateau les successions identiques en diagonales descendant vers la droite
            for (int row = 0; row < sizeRow - 3; row++) {
                for (int column = 0; column < sizeColumn - 3; column++) {
                    if (this.Grid[row][column].StateCase != TypeCell.EMPTY &&
                        this.Grid[row][column].StateCase == this.Grid[row + 1][column + 1].StateCase &&
                        this.Grid[row][column].StateCase == this.Grid[row + 2][column + 2].StateCase &&
                        this.Grid[row][column].StateCase == this.Grid[row + 3][column + 3].StateCase) {
                        return (int) this.Grid[row][column].StateCase;
                    }
                }
            }

            // recherche dans le plateau les successions identiques en diagonales descendant vers la gauche
            for (int row = 0; row < sizeRow - 3; row++) {
                for (int column = sizeColumn - 3; column < sizeColumn; column++) {
                    if (this.Grid[row][column].StateCase != TypeCell.EMPTY &&
                        this.Grid[row][column].StateCase == this.Grid[row + 1][column - 1].StateCase &&
                        this.Grid[row][column].StateCase == this.Grid[row + 2][column - 2].StateCase &&
                        this.Grid[row][column].StateCase == this.Grid[row + 3][column - 3].StateCase) {
                        return (int) this.Grid[row][column].StateCase;
                    }
                }
            }
            return 0;
        }

        public override String ToString() {

            String res = "";
            for (int ligne = sizeRow - 1; ligne >= 0; ligne--) {
                String lignePlateau = "";
                for (int colonne = 0; colonne < sizeColumn; colonne++) {
                    lignePlateau += this.Grid[ligne][colonne];
                }
                res += lignePlateau + "\n";
            }
            return res;
        }

        public ushort[][] getBoardForIa() {

            ushort[][] res = new ushort[sizeRow][];
            //initialiser les colonne
            for (int row = 0; row < sizeRow; row++) {
                res[row] = new ushort[sizeColumn];
            }

            for (int ligne = sizeRow - 1; ligne >= 0; ligne--) {
                for (int colonne = 0; colonne < sizeColumn; colonne++) {
                    res[ligne][colonne] = this.Grid[ligne][colonne].getStateCaseNumber();
                }
            }
            Array.Reverse(res);
            return res;
        }

        public void changePlayer() {
            WhoPlay = (WhoPlay == 1) ? 2 : 1;
        }

        public int whoDoesNotPlay() {
            return WhoPlay == 1 ? 2 : 1;
        }

        public short[] getCompleteColumns() {
            short[] result = new short[7] {-1, -1, -1, -1, -1, -1, -1 };

            // recherche dans le plateau des colonnes pleine
            for (short column = 0; column < sizeColumn; column++) {
                short counterCaseFills = -1;

                for (ushort row = 0; row < sizeRow; row++) {
                    if (this.Grid[row][column].StateCase != TypeCell.EMPTY) {
                        counterCaseFills++;
                    }
                }

                if (counterCaseFills == sizeRow - 1) {
                    result[column] = column;
                }

            }

            return result;
        }

        public Boolean thereIsEquality() {
            Boolean result = true;
            foreach (short number in getCompleteColumns()) {
                if (number == -1) {
                    result = false;
                }
            }
            return result;
        }

    }
}