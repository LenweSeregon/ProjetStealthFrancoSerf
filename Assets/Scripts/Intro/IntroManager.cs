using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroManager : MonoBehaviour {

    public SceneHandler sceneHandler;
    public float intervalCharacterWriter = 0.1f;
    public List<string> sentencesIntroI18NID;
    public TextMeshProUGUI text;

    private IEnumerator playIntro;

	// Use this for initialization
	void Start ()
    {
        sentencesIntroI18NID = new List<string>();
        sentencesIntroI18NID.Add("introduction.sentences.sentence1");
        sentencesIntroI18NID.Add("introduction.sentences.sentence2");
        sentencesIntroI18NID.Add("introduction.sentences.sentence3");
        sentencesIntroI18NID.Add("introduction.sentences.sentence4");
        sentencesIntroI18NID.Add("introduction.sentences.sentence5");
        sentencesIntroI18NID.Add("introduction.sentences.sentence6");

        playIntro = PlayIntro();
        StartCoroutine(playIntro);
    }
	
    public void SkipClicked()
    {
        StopCoroutine(playIntro);
        sceneHandler.SwitchToSceneWithoutParameter(2);
    }

    private IEnumerator PlayIntro()
    {
        for(int i = 0; i < sentencesIntroI18NID.Count; i++)
        {
            string sentence = I18nManager.Fields[sentencesIntroI18NID[i]];
            foreach (char c in sentence)
            {
                text.text += c;
                yield return new WaitForSeconds(intervalCharacterWriter);
            }
            yield return new WaitForSeconds(2.0f);

            text.text = "";
        }

        sceneHandler.SwitchToSceneWithoutParameter(2);
    }
}
