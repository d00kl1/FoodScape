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
    public GameObject item_0Prefab;
    public GameObject item_1Prefab;
    public GameObject item_2Prefab;
    public GameObject item_3Prefab;
    public GameObject item_4Prefab;
    public GameObject item_5Prefab;
    public GameObject groundPrefab;

    private GameObject ground;

    private int itemType;

    private Actions action;

    void Awake()
    {        
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
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
        var groundOffset = new Vector3(0.0f, 0.0f, 0.0f);

        foreach (var trackedImage in eventArgs.added)
        {
            Debug.Log("detected_image_position: " + trackedImage.transform.position);
            Debug.Log("detected_image_rotation: " + trackedImage.transform.rotation);
            Debug.Log("detected_image_local_scale: " + trackedImage.transform.localScale);

            ground = Instantiate(groundPrefab, trackedImage.transform.position + groundOffset, Quaternion.identity);            
            ground.transform.rotation = trackedImage.transform.rotation;
            //ground.transform.localScale = new Vector3(0.20f, 0.10f, 0.15f);            
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            // TODO monitor trackedImage.trackingState
            if (ground)
            {
                ground.transform.position = trackedImage.transform.position + groundOffset;
                ground.transform.rotation = trackedImage.transform.rotation;
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
                        AddItem(hit.point);
                    }
                    else if ((hit.collider.tag == "item") && (action == Actions.Harvest)) 
                    {
                        Debug.Log("Deleting object");
                        Destroy(hit.collider.gameObject);                        
                    }                    
                }
            }
        }
    }



    void AddItem(Vector3 position)
    {
        var itemOffset = new Vector3(0.0f, 0.01f, 0.0f);
        
        GameObject item = null;        

        switch (itemType) {
            case 0:
                item = Instantiate(item_0Prefab, position + itemOffset, Quaternion.identity);
                break;
            case 1:
                item = Instantiate(item_1Prefab, position + itemOffset, Quaternion.identity);
                break;
            case 2:
                item = Instantiate(item_2Prefab, position + itemOffset, Quaternion.identity);
                break;
            case 3:
                item = Instantiate(item_3Prefab, position + itemOffset, Quaternion.identity);
                break;
            case 4:
                item = Instantiate(item_4Prefab, position + itemOffset, Quaternion.identity);
                break;
            case 5:
                item = Instantiate(item_5Prefab, position + itemOffset, Quaternion.identity);
                break;
        }

        //item.transform.rotation = rotation;
        //item.transform.LookAt(Camera.main.transform);        
        item.transform.SetParent(ground.transform);
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
