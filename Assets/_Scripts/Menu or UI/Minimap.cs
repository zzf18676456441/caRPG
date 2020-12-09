using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject carPrefab;
    public GameObject enemyPrefab;
    public GameObject arrowPrefab;

    private Enemy[] enemyBase;
    private Dictionary<int, RectTransform> enemies;
    private GameObject car;
    private RectTransform carPic;
    private Waypoint[] waypointBase;
    private Dictionary<int,Waypoint> waypoints;

    private FinishLine[] arrowBase;
    
    private float minX, minY, maxX, maxY;
    private float mapWidth, offset;
    private bool xOffset;

    private float boundary = 0.1f;

    private GameController controller;

    // Update is called once per frame
    void Awake(){
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }

    void Start(){
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        float scale = 1 - rect.rect.height/rect.rect.width;
        rect.anchorMin = new Vector2(scale/2, 0);
        rect.anchorMax = new Vector2(1-scale/2, 1);
        arrowBase = FindObjectsOfType<FinishLine>();
        enemyBase = FindObjectsOfType<Enemy>();
        car = controller.GetCar();
        waypointBase = FindObjectsOfType<Waypoint>();
        waypoints = new Dictionary<int, Waypoint>();
        if (waypointBase.Length == 0) {
            gameObject.SetActive(false);
            return;
        }
        foreach(Waypoint point in waypointBase){
            waypoints[point.index] = point;
        }
        FindWorldMinMax();
        DrawAllWaypoints();
        DrawAllArrows();
        carPic = Instantiate(carPrefab,gameObject.transform,false).GetComponent<RectTransform>();
        carPic.pivot = new Vector2(0.5f, 0.5f);
        enemies = new Dictionary<int, RectTransform>();
        for (int i = 0; i < enemyBase.Length; i++){
            enemies[i] = Instantiate(enemyPrefab,gameObject.transform, false).GetComponent<RectTransform>();
        }
    }

    void Update(){
        UpdatePosition(car, carPic);
        for (int i = 0; i < enemyBase.Length; i++){
            if(enemyBase[i] != null){
                UpdatePosition(enemyBase[i].gameObject,enemies[i]);
            } else {
                enemies[i].gameObject.SetActive(false);
            }

        }
    }

    private void UpdatePosition(GameObject obj, RectTransform image){
        Vector2 pos = ConvertToLocalScale(obj.transform.position);
        Vector2 cur = new Vector2((image.anchorMax.x + image.anchorMin.x)/2, (image.anchorMax.y + image.anchorMin.y)/2);
        Vector2 translate = pos-cur;
        image.anchorMin += translate;
        image.anchorMax += translate;
        image.rotation = obj.transform.rotation;
    }

    private void DrawAllArrows(){
        for (int i = 0; i < arrowBase.Length; i++){
            RectTransform image = Instantiate(arrowPrefab,gameObject.transform, false).GetComponent<RectTransform>();
            UpdatePosition(arrowBase[i].gameObject, image);
        }
    }

    private void DrawAllWaypoints(){
        for (int i = 0; i < waypoints.Count - 1; i++){
            DrawLine(ConvertToLocalScale(waypoints[i].transform.position),ConvertToLocalScale(waypoints[i+1].transform.position), 0.05f, Color.grey);
        }
    }

    private void FindWorldMinMax(){
        minX = waypoints[0].x-1;
        maxX = waypoints[0].x+1;
        minY = waypoints[0].y-1;
        maxY = waypoints[0].y+1;

        foreach(Waypoint point in waypoints.Values){
            if (point.x < minX) minX = point.x;
            if (point.y < minY) minY = point.y; 
            if (point.x > maxX) maxX = point.x;
            if (point.y > maxY) maxY = point.y; 
        }

        mapWidth = maxX - minX;
        if(mapWidth >= maxY - minY){
            xOffset = false;
            offset = (mapWidth - maxY + minY)/2;
        } else {
            mapWidth = maxY - minY;
            xOffset = true;
            offset = (mapWidth - maxX + minX)/2;
        }
        mapWidth = Mathf.Max(maxY - minY, maxX - minX);
    }

    private Vector2 ConvertToLocalScale(Vector2 position){
        Vector2 result = new Vector2();
        if (xOffset){
            result.x = (1 - 2 * boundary) * (position.x - minX + offset)/mapWidth + boundary;
            result.y = (1 - 2 * boundary) * (position.y - minY)/mapWidth + boundary;
        } else {
            result.x = (1 - 2 * boundary) * (position.x - minX)/mapWidth + boundary;
            result.y = (1 - 2 * boundary) * (position.y - minY + offset)/mapWidth + boundary;
        }
        
        return result;
    }

    private void DrawLine(Vector2 start, Vector2 end, float thickness, Color color){
        RectTransform linesegment = Instantiate(linePrefab, gameObject.transform, false).GetComponent<RectTransform>();
        linesegment.GetComponent<Image>().color = color;
        linesegment.anchorMin = new Vector2(start.x - thickness/2, start.y - thickness/2);
        linesegment.anchorMax = new Vector2(start.x + (end-start).magnitude + thickness/2,start.y + thickness/2);
        linesegment.pivot = new Vector2(thickness/(2*((end-start).magnitude + thickness)), 0.5f);
        float angle = Mathf.Atan2((end - start).y, (end - start).x) * Mathf.Rad2Deg;
        linesegment.localRotation = Quaternion.Euler(0, 0, angle);
    }

}
