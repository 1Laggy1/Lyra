using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CenterTextManager : MonoBehaviour
{
    [SerializeField]
    GameObject centerTextGO;
    [SerializeField]
    TMP_Text centerText;



    [SerializeField]
    TMP_Text descriptionText;
    public void ShowText(string center, string description)
    {
        centerText.text = center;
        descriptionText.text = description;
        StartCoroutine(ShowTextCoroutine());
    }
    IEnumerator ShowTextCoroutine()
    {
        centerText.alpha = 0;
        descriptionText.alpha = 0;
        centerText.gameObject.SetActive(true);
        descriptionText.gameObject.SetActive(true);
        centerTextGO.SetActive(true);
        Tween fadein = DOTween.To(() => centerText.alpha, x => centerText.alpha = x, 1, 2);
        DOTween.To(() => descriptionText.alpha, x => descriptionText.alpha = x, 1, 3);
        yield return new WaitUntil(() => fadein.IsActive() == false);
        yield return new WaitForSeconds(3);
        DOTween.To(() => centerText.alpha, x => centerText.alpha = x, 0, 2);
        DOTween.To(() => descriptionText.alpha, x => descriptionText.alpha = x, 0, 1);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
