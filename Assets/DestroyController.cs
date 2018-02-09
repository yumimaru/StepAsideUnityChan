using System.Collections;
using UnityEngine;

public class DestroyController : MonoBehaviour {

	//Unityちゃんのオブジェクト※
	private GameObject unitychan;
	//アイテムが消えるUnityちゃんとの距離
	private float difference = -3.0f;

	// Use this for initialization
	void Start () {
		//Unityちゃんのオブジェクトを取得※
		this.unitychan = GameObject.Find("unitychan");

	}
	
	// Update is called once per frame
	void Update () {

		//離れたら削除
		//-3.0fに-すると＋になっちゃうので、+ difference
		if(gameObject.transform.position.z < unitychan.transform.position.z + difference)
			
			//下記エラー
			//直線上の距離なので、斜めになったときに長さが違ってしまう。Z座標で計算したほうが良い
		//if (Vector3.Distance (unitychan.transform.position, gameObject.transform.position) <= difference)
		{
		Destroy (gameObject);
		}		
		
	}
}
