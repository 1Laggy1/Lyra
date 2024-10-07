using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestingTests
{
    [UnityTest]
    public IEnumerator CharacterSelection()
    {
        GameObject go = new GameObject();

        CharacterSelection cs = go.AddComponent<CharacterSelection>();
        string expectedName = "Kayden";
        string actualName = cs.ChangeCharacter(expectedName);
        Assert.AreEqual(expectedName, actualName);
        yield return null;
    }
}
