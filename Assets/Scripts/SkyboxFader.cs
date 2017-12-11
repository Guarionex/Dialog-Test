using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxFader : MonoBehaviour {
    
    [SerializeField]
    private Material[] mat = new Material[7];
    [SerializeField]
    private GameObject hyperspace;
    [SerializeField]
    private List<GameObject> Scene1Objects;
    [SerializeField]
    private List<GameObject> Scene2Objects;
    private ParticleSystem hyperSpaceParticles;
    private Component[] hyperSpaceChildrenParticles;
    private bool isMatChanging = false;
    private Material currentSkybox;
    private int skyBoxIndex = 0;
    private float transitionTime = 0f;
    private float currentTime = 0f;
    private bool isNextSkyboxLoaded = false;
    private Dictionary<int, List<GameObject>> sceneToSceneObject;

	// Use this for initialization
	void Start () {
        sceneToSceneObject = new Dictionary<int, List<GameObject>>();
        sceneToSceneObject.Add(0, Scene1Objects);
        sceneToSceneObject.Add(1, Scene2Objects);
        setAllSceneObjectsToInvisible();
        resetAllSkyBoxBlends();
        hyperSpaceParticles = hyperspace.GetComponent<ParticleSystem>();
        hyperSpaceChildrenParticles = hyperspace.GetComponentsInChildren(typeof(ParticleSystem));
        stopHyperSpaceParticles();
    }
	
	// Update is called once per frame
	void Update () {
		if(isMatChanging)
        {
            Swapskybox(skyBoxIndex, transitionTime);
        }
	}

    public void swapToSkyBox(int index, float time)
    {
        skyBoxIndex = index;
        transitionTime = time / 2;
        isMatChanging = true;
        isNextSkyboxLoaded = false;
        setDurationForHyperSpaceParticles(time);
        startHyperSpaceParticles();
        currentSkybox = RenderSettings.skybox;
        Swapskybox(index, transitionTime);
    }

    public void endFade()
    {
        isMatChanging = false;
        isNextSkyboxLoaded = false;
        stopHyperSpaceParticles();
        RenderSettings.skybox = mat[skyBoxIndex];
        RenderSettings.skybox.SetFloat("_Blend", 1.0f);
        fadeInSceneObjects(skyBoxIndex, 1.0f);

    }

    private void Swapskybox(int index, float time)
    {
        float blend = 0f;
        if (!isNextSkyboxLoaded)
        {
            blend = Mathf.Lerp(0.0f, 1.0f, Mathf.Sin(currentTime * Mathf.PI * 0.5f));
        }
        else
        {
            blend = Mathf.Lerp(0.0f, 1.0f, 1.0f - Mathf.Cos(currentTime * Mathf.PI * 0.5f));
        }
        if(currentTime >= 1.0f)
        {
            blend = 1.0f;
        }
        //float blend = Mathf.Lerp(0.0f, 1.0f, currentTime);
        currentTime += Time.deltaTime / time;
        RenderSettings.skybox.SetFloat("_Blend", blend);
        if(blend >= 1.0f)
        {
            if (isNextSkyboxLoaded)
            {
                isMatChanging = false;
                isNextSkyboxLoaded = false;
                setLoopForHyperSpaceParticles(false);
                fadeInSceneObjects(index, blend);
            }
            else
            {
                RenderSettings.skybox = mat[index];
                isNextSkyboxLoaded = true;
                currentTime = 0f;
            }
        }
    }

    private void stopHyperSpaceParticles()
    {
        hyperSpaceParticles.Stop();
        foreach(ParticleSystem ps in hyperSpaceChildrenParticles)
        {
            ps.Stop();
        }
        setLoopForHyperSpaceParticles(false);
    }

    private void startHyperSpaceParticles()
    {
        hyperSpaceParticles.Play();
        foreach (ParticleSystem ps in hyperSpaceChildrenParticles)
        {
            ps.Play();
        }
        setLoopForHyperSpaceParticles(true);
    }

    private void setLoopForHyperSpaceParticles(bool state)
    {
        ParticleSystem.MainModule hspm = hyperSpaceParticles.main;
        hspm.loop = false;
        foreach (ParticleSystem ps in hyperSpaceChildrenParticles)
        {
            ParticleSystem.MainModule psm = ps.main;
            psm.loop = false;
        }
    }

    private void setDurationForHyperSpaceParticles(float time)
    {
        ParticleSystem.MainModule hspm = hyperSpaceParticles.main;
        hspm.duration = time;
        foreach (ParticleSystem ps in hyperSpaceChildrenParticles)
        {
            ParticleSystem.MainModule psm = ps.main;
            psm.duration = time;
        }
    }

    private void resetAllSkyBoxBlends()
    {
        for(int i = 0; i < mat.Length; i++)
        {
            mat[i].SetFloat("_Blend", 0);
        }
    }

    private void fadeInSceneObjects(int sceneIndex, float blend)
    {
        foreach(GameObject sceneObject in sceneToSceneObject[sceneIndex])
        {
            sceneObject.SetActive(true);
            /*Component[] sceneObjectRenderer = sceneObject.GetComponentsInChildren(typeof(Renderer));
            foreach (Renderer renderer in sceneObjectRenderer)
            {
                Color newAlphaColor = renderer.material.color;
                newAlphaColor.a = blend;
                renderer.material.color = newAlphaColor;
            }*/
        }
    }

    private void setAllSceneObjectsToInvisible()
    {
        foreach(KeyValuePair<int, List<GameObject>> kvp in sceneToSceneObject)
        {
            foreach(GameObject sceneObject in kvp.Value)
            {
                sceneObject.SetActive(false);
                /*Component[] sceneObjectRenderer = sceneObject.GetComponentsInChildren(typeof(Renderer));
                foreach (Renderer renderer in sceneObjectRenderer)
                {
                    Color newAlphaColor = renderer.material.color;
                    Debug.Log("Color = " + newAlphaColor);
                    newAlphaColor.a = 0f;
                    renderer.material.color = newAlphaColor;
                }*/
            }
        }
    }
}
