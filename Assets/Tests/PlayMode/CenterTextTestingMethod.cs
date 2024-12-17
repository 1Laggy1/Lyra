using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Mirror;

public class CenterTextTestingMethod
{
    Scene activeScene;



    [UnitySetUp]
    public IEnumerator Setup()
    {
        LogAssert.ignoreFailingMessages = true;

        // Перевіряємо наявність старого NetworkManager і зупиняємо сервер/клієнт
        if (NetworkManager.singleton != null)
        {

            if (NetworkManager.singleton.isNetworkActive)
            {
                if (NetworkServer.active)
                    NetworkManager.singleton.StopServer();

                if (NetworkClient.isConnected)
                    NetworkManager.singleton.StopClient();

                if (NetworkClient.active && NetworkServer.active)
                    NetworkManager.singleton.StopHost();
            }
            NetworkManager.singleton.transport.Shutdown();
            // Знищуємо старий NetworkManager
            GameObject.Destroy(NetworkManager.singleton.gameObject);
        }

        // Чекаємо кадр, щоб Unity знищив об'єкти
        yield return null;

        // Завантаження сцени Main menu
        SceneManager.LoadScene(0);
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Main menu");
        yield return new WaitForSecondsRealtime(1);

        // Зберігаємо активну сцену
        activeScene = SceneManager.GetActiveScene();

        // Ініціалізація NetworkManager
        NetworkManagerConfig.Character = "Kayden";
        MainMenu mainMenu = activeScene.GetRootGameObjects().FirstOrDefault(go => go.name == "Canvas").GetComponent<MainMenu>();
        Assert.IsNotNull(mainMenu, "MainMenu не знайдено на сцені.");

        MyNetworkManager networkManager = GameObject.Instantiate(mainMenu.networkManagerKCPPrefab).GetComponent<MyNetworkManager>();
        Assert.IsNotNull(networkManager, "MyNetworkManager не знайдено!");

        NetworkManagerConfig.IsClient = false;
        NetworkManagerConfig.Transport = "IP";
        NetworkManagerConfig.IsTesting = false;

        NetworkManagerConfig.CurrentSceneLoading = "Level0";
        networkManager.StartManager();

        // Чекаємо на завантаження Level0
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Level0");
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);

        activeScene = SceneManager.GetActiveScene();

        // Чекаємо на спавн персонажа
        yield return new WaitUntil(() => FindInDontDestroyOnLoad("Kayden(Clone)") != null);
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
    public IEnumerator CenterTextTesting_Method()
    {
        GameObject CenterTextManager = FindGameObjectInScene("CenterTextManager");

        yield return new WaitForSecondsRealtime(5);
        CenterTextManager.GetComponent<CenterTextManager>().ShowText("Test", "Testing description");
        Assert.IsNotNull(CenterTextManager, "CenterTextManager не знайдено!");
        Assert.AreEqual("Testing description", CenterTextManager.GetComponent<CenterTextManager>().descriptionText.text);
        Assert.AreEqual("Test", CenterTextManager.GetComponent<CenterTextManager>().centerText.text);
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
