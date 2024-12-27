using nbbpfei_reworked.FundamentalsOptions;

namespace nbbpfei_reworked.FundamentalsPlayerData
{
    public class PlayerData
    {
        public string GetEncryptedPassword() { return filePassword; }

        public string playerName = "Namefile"; 

        protected string filePassword = "fundamentalsiscool!";
        private bool isBetaTester;

        //Options
        public bool outlineRooms = false;
        public bool debugMode = false;
        public bool freeCamera = false;
        public bool mestizoColors = false;
        public bool universeParallel = false;

        public bool disableTextures = false;
        public bool disableRoomsVariants = false;
        public bool disableMusics = false;
        public bool disableMultipleEvelators = false;

        //ReachPoints
        public bool skipLobbyCutscene = false;
        public bool skipLobbyTutorial = false;
        public bool unlockLibrary = false;
        public int booksCollected = 0;
        public bool unlockMinigamesArea = false;
        public int unlockedMinigames = 0;
        public bool aparearDeathBoxes = false;
        public bool unlockCustomPlayersMirror = false;
        public bool unlockLobbySecretArea = false;
        public bool hasFoundMuseumPass = false;
        public bool hasShimishiTicket = false;
        public bool hasCheapStoreTicket = false;
        public bool hasRandomizedItemPrizerReapirTicket = false;
        public bool hasFoundAllBaldiPlushies = false;
        public bool hasFoundDeathLocation = false;
    }
}
