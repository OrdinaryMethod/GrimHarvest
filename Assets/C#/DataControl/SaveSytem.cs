using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    //public static void SaveGameMaster(GameMaster GM)
    //{
    //    BinaryFormatter formatter = new BinaryFormatter();
    //    string path = Application.persistentDataPath + "/GameMaster.info";
    //    FileStream stream = new FileStream(path, FileMode.Create);

    //    GameMasterData data = new GameMasterData(GM);

    //    formatter.Serialize(stream, data);
    //    stream.Close();
    //}
    //public static GameMasterData LoadGameMaster()
    //{
    //    string path = Application.persistentDataPath + "/GameMaster.info";
    //    if (File.Exists(path))
    //    {
    //        BinaryFormatter formatter = new BinaryFormatter();
    //        FileStream stream = new FileStream(path, FileMode.Open);

    //        GameMasterData data = formatter.Deserialize(stream) as GameMasterData;

    //        stream.Close();
    //        //File.Delete(path);
    //        return data;
    //    }
    //    else
    //    {
    //        Debug.LogError("File not found in " + path);
    //        return null;
    //    }
    //}
}
