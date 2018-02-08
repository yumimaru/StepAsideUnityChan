using UnityEngine;
using System.Collections;

public class CoinController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//すべてのコインの回転が揃わないよう回転を開始する角度を指定
		//Random.Range関数を使って回転を開始する角度をランダムに設定
		this.transform.Rotate (0, Random.Range (0, 360), 0);
	}

	// Update is called once per frame
	void Update () {
		//Rotate関数を使ってY軸を中心にコインが回転し続ける
		this.transform.Rotate (0, 3, 0);
	}
}
