using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

[TestFixture]
public class CameraMovementTests
{
    Scene activeScene;
    GameObject camera;
    

    GameObject Kayden;
    GameObject Lyra;
    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("AndrewScene");
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "AndrewScene");
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded == true);
        activeScene = SceneManager.GetActiveScene();
        Kayden = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Lyra = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Kayden.transform.position = new Vector3(1, 2, 0);
        Lyra.transform.position = new Vector3(-1, 2, 0);
        yield return new WaitForSeconds(3);
        Kayden.tag = "Player";
        Lyra.tag = "Player";
        camera = activeScene.GetRootGameObjects().FirstOrDefault(go => go.name == "Main Camera");
        yield return new WaitForFixedUpdate();
    }

    [UnityTest]
    public IEnumerator DynamicModeMidPointTest()
    {

        camera.GetComponent<CameraMovement>().curretMode = CameraMovement.CameraMode.Dynamic;
        yield return new WaitForSeconds(3);
        if (camera.transform.position.x >= -1 && camera.transform.position.x <= 1)
        {
            Assert.Pass();
        }
    }
    [UnityTest]
    public IEnumerator DynamicModeMidPointMoveTest()
    {
        camera.GetComponent<CameraMovement>().curretMode = CameraMovement.CameraMode.Dynamic;
        Kayden.transform.position = new Vector3(1, 2, 0);
        Lyra.transform.position = new Vector3(4, 2, 0);
        
        yield return new WaitForSeconds(3);
        if (camera.transform.position.x >= 1.5 && camera.transform.position.x <= 3)
        {
            Assert.Pass();
        }
    }
    [UnityTest]
    public IEnumerator DynamicModeMidPointSwapTest()
    {
        camera.GetComponent<CameraMovement>().curretMode = CameraMovement.CameraMode.Dynamic;
        Kayden.transform.position = new Vector3(4, 2, 0);
        Lyra.transform.position = new Vector3(1, 2, 0);
        
        yield return new WaitForSeconds(3);
        if (camera.transform.position.x >= 1.5 && camera.transform.position.x <= 3)
        {
            Assert.Pass();
        }
    }
}
