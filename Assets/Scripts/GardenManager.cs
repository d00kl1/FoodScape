using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

public enum Actions 
{
    Plant,
    Harvest,    
}


[RequireComponent(typeof(ARTrackedImageManager))]
public class GardenManager : MonoBehaviour
{
    private MeshCollider meshCollider;
    private MeshRenderer meshRenderer;
    private BoxCollider  boxCollider;
    
    public Material itemMaterial;
    private Material item0Material;
    private Material item1Material;
    private Material item2Material;
    private Material item3Material;
    private Material item4Material;
    private Material item5Material;

    public Material groundMaterial;

    private GameObject groundObject;

    private int itemType;

    private Actions action;

    void Awake()
    {        
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();        
        groundObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        groundObject.tag = "ground";
        
        groundObject.GetComponent<Renderer>().material = groundMaterial;

        item0Material = Instantiate(itemMaterial);
        item0Material.color = Color.green;

        item1Material = Instantiate(itemMaterial);
        item1Material.color = Color.yellow;

        item2Material = Instantiate(itemMaterial);
        item2Material.color = Color.red;

        item3Material = Instantiate(itemMaterial);
        item3Material.color = Color.magenta;

        item4Material = Instantiate(itemMaterial);
        item4Material.color = Color.cyan;

        item5Material = Instantiate(itemMaterial);
        item5Material.color = Color.blue;       

    }    

    ARTrackedImageManager m_TrackedImageManager;    

    void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }
    
    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        var worldCubeOffset = new Vector3(0.0f, 0.0f, 0.0f);

        foreach (var trackedImage in eventArgs.added)
        {
            Debug.Log("detected_image_position: " + trackedImage.transform.position);
            Debug.Log("detected_image_rotation: " + trackedImage.transform.rotation);
            Debug.Log("detected_image_local_scale: " + trackedImage.transform.localScale);

            groundObject.transform.position = trackedImage.transform.position + worldCubeOffset;
            groundObject.transform.rotation = trackedImage.transform.rotation;
            groundObject.transform.localScale = new Vector3(0.20f, 0.10f, 0.15f);            
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            // TODO monitor trackedImage.trackingState
            if (groundObject)
            {
                groundObject.transform.position = trackedImage.transform.position + worldCubeOffset;
                groundObject.transform.rotation = trackedImage.transform.rotation;
            }
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("Detected Touch");
                
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out RaycastHit hit)) {
                    if (hit.collider.tag == "ground") {
                        Debug.Log("Touched ground");
                        AddCube(hit.point);                        
                    }
                    else if (hit.collider.tag == "item")
                    {
                        if (action == Actions.Harvest)
                        {
                            Debug.Log("Deleting object");
                            Destroy(hit.collider.gameObject);
                        }
                    }
                }
            }
        }
    }



    void AddCube(Vector3 position)
    {
        var cubeOffset = new Vector3(0.0f, 0.01f, 0.0f);

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.tag = "item";
        cube.transform.position = position + cubeOffset;
        //cube.transform.rotation = rotation;
        //cube.transform.LookAt(Camera.main.transform);
        cube.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        switch (itemType) {
            case 0:
                cube.GetComponent<Renderer>().material = item0Material;
                break;
            case 1:
                cube.GetComponent<Renderer>().material = item1Material;
                break;
            case 2:
                cube.GetComponent<Renderer>().material = item2Material;
                break;
            case 3:
                cube.GetComponent<Renderer>().material = item3Material;
                break;
            case 4:
                cube.GetComponent<Renderer>().material = item4Material;
                break;
            case 5:
                cube.GetComponent<Renderer>().material = item5Material;
                break;
        }

        cube.transform.SetParent(groundObject.transform);
    }

    public void SetItemType(int itemType)
    {
        switch (itemType)
        {
            case 0:        
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                action = Actions.Plant;
                break;
            case 6:
                action = Actions.Harvest;
                break;
        }

        this.itemType = itemType;
    }
}
