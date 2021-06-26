using System;

namespace Serveur_Puissance_4.game {
	public class Cell {
		public TypeCell StateCase { get; set; }

		public Cell() {
			StateCase = TypeCell.EMPTY;
		}

		public override String ToString() {
			if (StateCase == TypeCell.EMPTY) {
				return " ";
			} else if (StateCase == TypeCell.P1) {
				return "1";
			} else {
				return "2";
			}
		}

		public ushort getStateCaseNumber() {
			if (StateCase == TypeCell.EMPTY) {
				return 0;
			} else if (StateCase == TypeCell.P1) {
				return 1;
			} else {
				return 2;
			}
		}
	}
}