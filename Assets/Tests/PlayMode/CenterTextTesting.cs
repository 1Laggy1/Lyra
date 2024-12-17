using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Mirror;

public class CenterTextTesting
{
    Scene activeScene;
    bool oneTimeSetup;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        LogAssert.ignoreFailingMessages = true;

        if (oneTimeSetup)
            yield break;

        // Очищення NetworkManager
        CleanupNetworkManager();

        // Завантаження сцени Main menu
        SceneManager.LoadScene(0);
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Main menu");

        activeScene = SceneManager.GetActiveScene();

        // Ініціалізація NetworkManager
        MainMenu mainMenu = activeScene.GetRootGameObjects().FirstOrDefault(go => go.name == "Canvas").GetComponent<MainMenu>();
        Assert.IsNotNull(mainMenu, "MainMenu не знайдено на сцені.");

        var networkManager = GameObject.Instantiate(mainMenu.networkManagerKCPPrefab).GetComponent<MyNetworkManager>();
        Assert.IsNotNull(networkManager, "MyNetworkManager не знайдено!");
        networkManager.StartManager();

        // Чекаємо на завантаження Level0
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Level0");

        activeScene = SceneManager.GetActiveScene();

        // Чекаємо на спавн персонажа
        yield return new WaitUntil(() => FindInDontDestroyOnLoad("Kayden(Clone)") != null);
    }

    // ------------------ НАВАНТАЖУВАЛЬНЕ ТЕСТУВАННЯ ------------------
    [UnityTest]
    public IEnumerator StressTest_CenterText()
    {
        GameObject centerTextManager = FindGameObjectInScene("CenterTextManager");
        Assert.IsNotNull(centerTextManager, "CenterTextManager не знайдено!");

        var manager = centerTextManager.GetComponent<CenterTextManager>();

        for (int i = 0; i < 50; i++) // Виконання тесту 50 разів
        {
            string randomTitle = Random.Range(1000, 9999).ToString();
            string randomDesc = Random.Range(10000, 99999).ToString();

            manager.ShowText(randomTitle, randomDesc);

            yield return null; // Чекаємо один кадр

            Assert.AreEqual(randomDesc, manager.descriptionText.text, $"Невірний текст опису на ітерації {i}");
            Assert.AreEqual(randomTitle, manager.centerText.text, $"Невірний заголовок на ітерації {i}");
        }
    }

    // ------------------ ІНТЕГРАЦІЙНЕ ТЕСТУВАННЯ ------------------
    [UnityTest]
    public IEnumerator IntegrationTest_CenterTextAndSceneStart()
    {
        GameObject centerTextManager = FindGameObjectInScene("CenterTextManager");
        GameObject startText = FindGameObjectInScene("SceneNameOnStart");

        Assert.IsNotNull(centerTextManager, "CenterTextManager не знайдено!");
        Assert.IsNotNull(startText, "SceneNameOnStart не знайдено!");

        var manager = centerTextManager.GetComponent<CenterTextManager>();
        var sceneStart = startText.GetComponent<SceneNameOnStart>();

        // Виконуємо інтеграційний тест
        manager.ShowText(sceneStart.name, sceneStart.description);

        yield return null;

        Assert.AreEqual(sceneStart.description, manager.descriptionText.text, "Невірний опис сцени.");
        Assert.AreEqual(sceneStart.name, manager.centerText.text, "Невірна назва сцени.");
    }

    // ------------------ ВИКЛЮЧЕННЯ ТА ПОМИЛКИ ------------------
    [UnityTest]
    public IEnumerator ExceptionHandling_CenterText()
    {
        GameObject centerTextManager = FindGameObjectInScene("CenterTextManager");

        Assert.IsNotNull(centerTextManager, "CenterTextManager не знайдено!");
        var manager = centerTextManager.GetComponent<CenterTextManager>();

        LogAssert.Expect(LogType.Error, "ShowText received a null value!");

        // Тест на помилку
        manager.ShowText(null, null);
        yield return null;

        // Переконаймося, що текст не оновився при null
        Assert.AreEqual("", manager.descriptionText.text);
        Assert.AreEqual("", manager.centerText.text);
    }

    // ------------------ ДОПОМІЖНІ МЕТОДИ ------------------
    private void CleanupNetworkManager()
    {
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
            GameObject.Destroy(NetworkManager.singleton.gameObject);
        }
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

    private GameObject FindInDontDestroyOnLoad(string name)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        return allObjects.FirstOrDefault(obj => obj.name == name);
    }
}
