using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class GameTestScript
{
    private float testTime = 0f;
    
    [Test]
    public void GameTestScriptSimplePasses()
    {
    }

    [UnityTest]
    public IEnumerator GameTestScriptWithEnumeratorPasses()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        yield return WaitForSceneLoad();

        var gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Assert.IsNotNull(gameManager, "gameManager == null");
        
        var roadManager = GameObject.Find("RoadManager").GetComponent<RoadManager>();
        Assert.IsNotNull(roadManager, "roadManager == null");
        
        var gasSpawner = GameObject.Find("GasSpawner").GetComponent<GasSpawner>();
        Assert.IsNotNull(gasSpawner, "gasSpawner == null");

        var startBtn = GameObject.Find("BtnStart").GetComponent<Button>();
        Assert.IsNotNull(startBtn, "startBtn == null");
        
        startBtn.onClick.Invoke();
        yield return null;

        var leftBtn = GameObject.Find("BtnLeft").GetComponent<Button>();
        Assert.IsNotNull(leftBtn, "leftBtn == null");
        
        var rightBtn = GameObject.Find("BtnRight").GetComponent<Button>();
        Assert.IsNotNull(rightBtn, "rightBtn == null");

        var vehicle = GameObject.Find("Vehicle");
        Assert.IsNotNull(vehicle, "vehicle == null");
        
        var inputController = GameObject.Find("Buttons").GetComponent<InputController>();
        Assert.IsNotNull(inputController, "inputController == null");
        
        yield return null;

        while (gameManager.gameState == GameState.InGame)
        {
            testTime += Time.deltaTime;
            yield return new WaitUntil(() => gasSpawner.gas != null);
            var gasXPos = gasSpawner.gas?.transform.position.x;

            if (vehicle.transform.position.x < gasXPos)
            {
                MoveButtonDown(rightBtn.gameObject);
                MoveButtonUp(leftBtn.gameObject);
            }
            else if (vehicle.transform.position.x > gasXPos)
            {
                MoveButtonDown(leftBtn.gameObject);                
                MoveButtonUp(rightBtn.gameObject);
            }
            else
            {
                MoveButtonUp(leftBtn.gameObject);
                MoveButtonUp(rightBtn.gameObject);
            }
    
            yield return null;
        }

        Debug.Log(testTime);
        
        yield return null;
    }

    private IEnumerator WaitForSceneLoad()
    {
        while (SceneManager.GetActiveScene().buildIndex > 0)
        {
            yield return null;
        }
    }

    private void MoveButtonDown(GameObject moveButton)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(moveButton, pointerEventData, ExecuteEvents.pointerDownHandler);
    }
    
    private void MoveButtonUp(GameObject moveButton)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(moveButton, pointerEventData, ExecuteEvents.pointerUpHandler);
    }
}
