using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCutscene : MonoBehaviour
{

    public Mobile stone1;
    public Mobile stone2;
    public Mobile stone3;
    public Mobile finalpos;
    public bool forcePreview = false;

    public ParticleSystem part1;
    public ParticleSystem part2;
    public CanvasRenderer superOverlay;
    public AudioSource theMusic;


    public bool cutsceneBegan = false;
    public float timePassed = 0f;
    // Start is called before the first frame update
    void Start()
    {
        superOverlay.SetAlpha(0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!cutsceneBegan && (CheckConditions() || forcePreview))
        {
            TriggerCutscene();
        }
        if (cutsceneBegan)
        {

            timePassed += Time.deltaTime;
            if (timePassed > 15f)
            {
                part1.Stop();
                
            }
            if (timePassed > 18f)
            {
                part2.Play();
            }
            if (timePassed > 20f)
            {
                superOverlay.SetAlpha(1f);
            }
        }
    }

    bool CheckConditions()
    {
        if (stone1.spells.Count > 0 && stone2.spells.Count > 0 && stone3.spells.Count > 0)
        {
            if (Ghost.Instance == finalpos)
            {
                return true;
            }
        }
        return false;
    }
    void TriggerCutscene()
    {
        GameManager.Instance.timeflowOption = TimeMode.AUTO;
        cutsceneBegan = true;

        part1.Play();
        theMusic.Play();
        
    }
}
