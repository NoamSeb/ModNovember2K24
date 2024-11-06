using BepInEx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace ModSlimeRancher
{
  [BepInPlugin("com.NoamSeb.modslimerancher", "Mod Slime Rancher", "1.0.0")]
  public class Timer : BaseUnityPlugin
  {
    private Text timerText;
    private float elapsedTime;
    private GameObject canvasObj;
    
    private void Awake()
    {
      SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void Update()
    {
      // Only update if the timer has been initialized
      if (timerText != null)
      {
        // Update the elapsed time
        elapsedTime += Time.deltaTime;

        // Format the time as MM:SS and update the text
        TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
        timerText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
      }
    }
    
    private void OnDestroy()
    {
      // Unsubscribe from scene loaded event to avoid memory leaks
      SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
      // Check if we are in the game scene
      if (scene.name == "worldGenerated")
      {
        InitializeTimerUI();
        ApplyBorderRadius();
      }
      else
      {
        // Destroy Timer Canvas if we're not in the target scene
        if (canvasObj != null)
        {
          Destroy(canvasObj);
          canvasObj = null;
        }
      }
    }

    private void InitializeTimerUI()
    {
      // Create the Canvas
      canvasObj = new GameObject("TimerCanvas");
      Canvas canvas = canvasObj.AddComponent<Canvas>();
      canvas.renderMode = RenderMode.ScreenSpaceOverlay;
      canvasObj.AddComponent<CanvasScaler>();
      canvasObj.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
      canvasObj.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
      canvasObj.AddComponent<GraphicRaycaster>();
      
      // Create the Text for the timer
      GameObject textObj = new GameObject("TimerText");
      textObj.transform.SetParent(canvasObj.transform);

      // Configure Text component
      timerText = textObj.AddComponent<Text>();
      timerText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
      timerText.fontSize = 36;
      timerText.alignment = TextAnchor.MiddleCenter;
      timerText.color = Color.white;

      // Positioning the Text (top center)
      RectTransform textRectTransform = timerText.GetComponent<RectTransform>();
      textRectTransform.anchorMin = new Vector2(0.5f, 1);
      textRectTransform.anchorMax = new Vector2(0.5f, 1);
      textRectTransform.pivot = new Vector2(0.5f, 1);
      textRectTransform.anchoredPosition = new Vector2(0, -50);

      // Create a background image for the timer text
      GameObject backgroundObj = new GameObject("TimerBackground");
      backgroundObj.transform.SetParent(textObj.transform);

      // Add Image component and set color to black with 50% opacity
      Image backgroundImg = backgroundObj.AddComponent<Image>();
      backgroundImg.color = new Color(0, 0, 0, 0.5f);

      // Set the background size to cover the timer text
      RectTransform backgroundRectTransform = backgroundObj.GetComponent<RectTransform>();
      backgroundRectTransform.anchorMin = new Vector2(0, 0);
      backgroundRectTransform.anchorMax = new Vector2(1, 1);
      backgroundRectTransform.offsetMin = new Vector2(-10, -10);
      backgroundRectTransform.offsetMax = new Vector2(10, 10);

      // Initialize the timer
      elapsedTime = 0;
    }

    private void ApplyBorderRadius()
    {
      Shader roundedShader = Shader.Find("Custom/RoundedBottomCorners");
      if (roundedShader != null)
      {
        Material roundedMaterial = new Material(roundedShader);
        roundedMaterial.SetFloat("_BottomRadius", 20);  // Adjust radius as desired

        // Apply to backgroundRectTransform's Image component
        GameObject backgroundObj = GameObject.Find("TimerBackground");
        Image backgroundImage = backgroundObj.GetComponent<Image>();
        if (backgroundImage != null)
        {
          backgroundImage.material = roundedMaterial;
        }
      }
      else
      {
        Debug.LogError("Shader not found!");
      }
    }
  }
}
