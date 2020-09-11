using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemFactory : MonoBehaviour
{
	public GameObject item_0;
	public GameObject item_1;
	public GameObject item_2;
	public GameObject item_3;
	public GameObject item_4;
	public GameObject item_5;

	void Start()
	{
		Instantiate(item_0, new Vector3(0, 0, 0), Quaternion.identity);
		Instantiate(item_1, new Vector3(0, 2, 0), Quaternion.identity);
		Instantiate(item_2, new Vector3(0, 4, 0), Quaternion.identity);
		Instantiate(item_3, new Vector3(0, 6, 0), Quaternion.identity);
		Instantiate(item_4, new Vector3(0, 8, 0), Quaternion.identity);
		Instantiate(item_5, new Vector3(0, 10, 0), Quaternion.identity);

		Debug.Log("Here");
	}
}