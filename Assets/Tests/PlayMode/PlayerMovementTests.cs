using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerMovementTests
{
    private GameObject player;
    private PlayerMovement playerMovement;
    private CharacterController2D characterController;
    Scene activeScene;
    bool oneTimeSetup;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        LogAssert.ignoreFailingMessages = true;
        if (oneTimeSetup)
        {
            yield break;
        }

        SceneManager.LoadScene(0);
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Main menu");
        activeScene = SceneManager.GetActiveScene();
        MainMenu mainMenu = activeScene.GetRootGameObjects().FirstOrDefault(go => go.name == "Canvas").GetComponent<MainMenu>();
        MyNetworkManager networkManager = GameObject.Instantiate(mainMenu.networkManagerKCPPrefab).GetComponent<MyNetworkManager>();
        NetworkManagerConfig.Character = "Kayden";
        NetworkManagerConfig.IsClient = false;
        NetworkManagerConfig.Transport = "IP";
        Assert.IsNotNull(networkManager, "MyNetworkManager не знайдено на сцені.");
        networkManager.StartManager();
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "AndrewScene");
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded == true);
        activeScene = SceneManager.GetActiveScene();
        yield return new WaitUntil(() => FindInDontDestroyOnLoad("Kayden(Clone)") != null);
        player = FindInDontDestroyOnLoad("Kayden(Clone)");
        Assert.IsNotNull(player, "Об'єкт гравця не знайдено!");
        playerMovement = player.GetComponent<PlayerMovement>();
        Assert.IsNotNull(playerMovement, "Компонент PlayerMovement не знайдено на об'єкті гравця!");
        activeScene.GetRootGameObjects().FirstOrDefault(go => go.name == "Ground").SetActive(false);
        oneTimeSetup = true;
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
    public IEnumerator PlayerMovesRight_WhenHorizontalInputIsPositive()
    {
        playerMovement.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        playerMovement.SetInputProvider(new MockInputProvider(1));
        Vector3 initialPosition = player.transform.position;
        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(5f);
        Assert.Greater(player.transform.position.x, initialPosition.x);
    }

    [UnityTest]
    public IEnumerator PlayerMovesLeft_WhenHorizontalInputIsNegative()
    {
        playerMovement.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Vector3 initialPosition = player.transform.position;
        playerMovement.SetInputProvider(new MockInputProvider(-1));
        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(5f);
        Assert.Less(player.transform.position.x, initialPosition.x);
    }
}
