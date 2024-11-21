using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

[TestFixture]
public class SteamInfoTest
{
    Scene activeScene;
    MockSteamInfo msi;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene(0);
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Main menu");
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded == true);
        activeScene = SceneManager.GetActiveScene();
        SteamInfo si = activeScene.GetRootGameObjects().FirstOrDefault(go => go.name == "Canvas").GetComponent<SteamInfo>();
        si.enabled = false;
        activeScene.GetRootGameObjects().FirstOrDefault(go => go.name == "Canvas").SetActive(false);
        msi = activeScene.GetRootGameObjects().FirstOrDefault(go => go.name == "Canvas").AddComponent<MockSteamInfo>();
        msi.FriendsInGameText = si.FriendsInGameText;
        msi.SteamImage = si.SteamImage;
        msi.UsernameText = si.UsernameText;
        activeScene.GetRootGameObjects().FirstOrDefault(go => go.name == "Canvas").SetActive(true);
        yield return new WaitForFixedUpdate();
    }

    [Test]
    public void UsernameTest()
    {
        Assert.AreEqual("Player username test", msi.UsernameText.text);
    }

    [Test]
    public void ImageTest()
    {
        Assert.AreEqual(Texture2D.blackTexture, msi.SteamImage.texture);
    }

    [Test]
    public void FriendsTest()
    {
        string friends = msi.FriendsInGameText.text.Substring(msi.FriendsInGameText.text.Length - 2);
        Assert.AreEqual("10", friends);
    }
}
