
public static class GlobalDefine  {

	public enum UnitState
	{
		Standing=0,
		Patrolling,
		BeingPiked,
		PursuingTarget
	}

	public enum RhythmState
	{
		StealthSong=0,
		FastMovingSong
	}




	public static class PathDefines{
		public const string XML_Path="/StreamingAssets/XML/";
		public const string TROOP_PREFAB="Prefab/Units/";
		public const string UI_PREFAB="Prefab/UI/";
	}
	public static class FileName{
		public const string Fraction="Fractions.XML";
		public const string SoldierType="RhythemEvents1.XML";

	}
	public static class ObjectTag{
		public const string SpawnPointTag="SpawnPoint";
		public const string Fraction0Tag="Fraction0Unit";
		public const string Fraction1Tag="Fraction1Unit";
		public const string Fraction2Tag="Fraction2Unit";

	}
}
