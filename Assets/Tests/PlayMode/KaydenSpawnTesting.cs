using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class KaydenSpawnTesting
{
    Scene activeScene;



    [UnitySetUp]
    public IEnumerator Setup()
    {
        LogAssert.ignoreFailingMessages = true;
        SceneManager.LoadScene(0);
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Main menu");
        activeScene = SceneManager.GetActiveScene();
    }

    private GameObject FindInDontDestroyOnLoad(string name)
    {
        // Отримуємо всі об'єкти з null-сцени (об'єкти в DontDestroyOnLoad)
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == name)
            {
                return obj; // Знайдений об'єкт
            }
        }

        return null; // Якщо об'єкт не знайдений
    }

    [UnityTest]
    public IEnumerator KaydenSpawn()
    {
        NetworkManagerConfig.Character = "Kayden";
        MainMenu mainMenu = activeScene.GetRootGameObjects().FirstOrDefault(go => go.name == "Canvas").GetComponent<MainMenu>();
        MyNetworkManager networkManager = GameObject.Instantiate(mainMenu.networkManagerKCPPrefab).GetComponent<MyNetworkManager>();
        NetworkManagerConfig.IsClient = false;
        NetworkManagerConfig.Transport = "IP";
        NetworkManagerConfig.IsTesting = false;
        Assert.IsNotNull(networkManager, "MyNetworkManager не знайдено на сцені.");
        NetworkManagerConfig.CurrentSceneLoading = "Level0";
        networkManager.StartManager();

        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Level0");
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded == true);
        activeScene = SceneManager.GetActiveScene();

        yield return new WaitUntil(() => FindInDontDestroyOnLoad("Kayden(Clone)") != null);
        GameObject player = FindInDontDestroyOnLoad("Kayden(Clone)");
        GameObject playerSpawn = FindGameObjectInScene("KaydenSpawnPoint");

        yield return new WaitForSecondsRealtime(5);

        Assert.IsNotNull(player, "Об'єкт гравця не знайдено!");
        Assert.IsNotNull(playerSpawn, "KaydenSpawnPoint не знайдено!");
        Assert.AreEqual(player.transform.position, playerSpawn.transform.position);
    }

    private GameObject FindGameObjectInScene(string name)
    {
        foreach (GameObject obj in activeScene.GetRootGameObjects())
        {
            GameObject found = FindInChildren(obj, name);
            if (found != null)
                return found;
        }
        return null;
    }

    private GameObject FindInChildren(GameObject parent, string name)
    {
        if (parent.name == name)
            return parent;

        foreach (Transform child in parent.transform)
        {
            GameObject found = FindInChildren(child.gameObject, name);
            if (found != null)
                return found;
        }
        return null;
    }

}
