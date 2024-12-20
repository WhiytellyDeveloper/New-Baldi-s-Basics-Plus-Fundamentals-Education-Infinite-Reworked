using System.IO;

namespace nbbpfei_reworked.FundamentalsManagers.Loaders
{
    internal class MusicalLoader
    {
        public static void LoadFundamentalsMusical()
        {
            LoadFloorMusics("F1");
            LoadFloorMusics("F2");
            LoadFloorMusics("F3");
            LoadFloorMusics("F4");
            LoadFloorMusics("F5");
        }

        public static void LoadFloorMusics(string floorID)
        {
            var floor = FundamentalsMainLoader.GetRandomFloorByName(floorID);

            string schoolhouseMusicName = $"{floorID}Schoolhouse";
            string escapeMusicName = $"{floorID}SchoolhouseEscape";
            string pitStopMusicName = $"{floorID}PitStop";

            if (File.Exists(FilePathsManager.GetPath(FilePaths.Musics, schoolhouseMusicName + ".midi"))) floor.schoolhouseMusic = AssetsHelper.LoadMidiFile(schoolhouseMusicName, schoolhouseMusicName, FilePathsManager.GetPath(FilePaths.Musics));
            if (File.Exists(FilePathsManager.GetPath(FilePaths.Musics, escapeMusicName + ".midi"))) floor.escapeMusic = AssetsHelper.LoadMidiFile(escapeMusicName, escapeMusicName, FilePathsManager.GetPath(FilePaths.Musics));
            if (File.Exists(FilePathsManager.GetPath(FilePaths.Musics, pitStopMusicName + ".midi"))) floor.pitStopMusic = AssetsHelper.LoadMidiFile(pitStopMusicName, pitStopMusicName, FilePathsManager.GetPath(FilePaths.Musics));
        }
    }
}
