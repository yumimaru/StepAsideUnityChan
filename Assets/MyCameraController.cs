using UnityEngine;
using System.Collections;

public class MyCameraController : MonoBehaviour {
	//Unityちゃんのオブジェクト
	private GameObject unitychan;
	//Unityちゃんとカメラの距離
	private float difference;

	// Use this for initialization
	void Start () {
		//シーン中のユニティちゃんのオブジェクトをFind関数で取得
		this.unitychan = GameObject.Find ("unitychan");
		//Unityちゃんとカメラの位置（z座標）の差を求める
		this.difference = unitychan.transform.position.z - this.transform.position.z;

	}

	// Update is called once per frame
	void Update () {
		//ユニティちゃんとのZ軸方向の距離を一定に保ちながら、
		//Unityちゃんの位置に合わせてカメラの位置を移動
		this.transform.position = new Vector3(0, this.transform.position.y, this.unitychan.transform.position.z-difference);
	}
}