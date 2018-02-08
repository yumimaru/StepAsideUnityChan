using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//↑UIを使うので、using UnityEngine.UI;を記述

public class UnityChanController : MonoBehaviour {
	//アニメーションするためのコンポーネントを入れる
	private Animator myAnimator;
	//Unityちゃんを移動させるコンポーネントを入れる
	private Rigidbody myRigidbody;
	//前進するための力
	private float forwardForce = 800.0f;
	//左右に移動するための力
	private float turnForce = 500.0f;
	//ジャンプするための力
	private float upForce = 500.0f;
	//左右の移動できる範囲
	private float movableRange = 3.4f;
	//オブジェクトにぶつかった際、動きを減速させる係数
	private float coefficient = 0.95f;
	//ぶつかったときのゲーム終了の判定
	private bool isEnd = false;
	//ゲーム終了時に表示するテキスト
	private GameObject stateText;
	//スコアを表示するテキスト
	private GameObject scoreText;
	//得点
	//テキストではスコアを計算することができないので、スコアを管理するためのscore変数を宣言
	private int score = 0;
	//左ボタン押下の判定（追加）
	private bool isLButtonDown = false;
	//右ボタン押下の判定（追加）
	private bool isRButtonDown = false;

	// Use this for initialization
	void Start () {

		//スクリプトでアニメーションの操作をするため
		//GetComponent関数を使って
		//オブジェクトにアタッチしているAnimatorコンポーネントを取得
		this.myAnimator = GetComponent<Animator>();

		//Animatorコンポーネントにアニメーションの再生を指示
		//走るアニメーションを開始
		//Animatorクラスの「SetFloat」関数は、
		//第一引数に与えられたパラメータに、第二引数の値を代入する関数
		//第一引数のバラメータがアニメーション再生の条件
		//Speedパラメータが0.8以上の値の場合に走るアニメーション再生するため、
		//SetFloat関数ではSpeedパラメータに1を代入
		this.myAnimator.SetFloat ("Speed", 1);

		//力を加えるためにはRigidbodyコンポーネントが必要なのでStart関数内で取得
		//Rigidbodyコンポーネントを取得（
		this.myRigidbody = GetComponent<Rigidbody>();

		//シーン中のstateTextオブジェクトを取得
		//Find関数で文字を表示するためのGameResultTextを取得
		this.stateText = GameObject.Find("GameResultText");

		//シーン中のscoreTextオブジェクトを取得
		this.scoreText = GameObject.Find("ScoreText");

		}

	void Update () {
		
		//ぶつかってゲーム終了ならUnityちゃんの動きを減衰する
		if (this.isEnd) {
			this.forwardForce *= this.coefficient;
			this.turnForce *= this.coefficient;
			this.upForce *= this.coefficient;
			this.myAnimator.speed *= this.coefficient;
		}

		//Unityちゃんに前進するための力を加える
		//RigidbodyクラスのAddForce関数は、引数で指定した方向の力をRigidbodyにかける関数
		//力の加える方向を「this.transform.forward」と指定
		//forwardを指定すると、キャラを選択した時に出る青い矢印の方向に力が加わり、前進
		this.myRigidbody.AddForce (this.transform.forward * this.forwardForce);

		//Unityちゃんを矢印キーまたはボタンに応じて左右に移動させる
		//道路をはみ出してしまわないように、左右の移動できる範囲をif文で設定
		//力をかけるためにはAddForce関数を使う。
		if ((Input.GetKey (KeyCode.LeftArrow) || this.isLButtonDown) && -this.movableRange < this.transform.position.x) {
			//左の矢印が押されている間は、ユニティちゃんに左方向の力をかけ左に移動
			this.myRigidbody.AddForce (-this.turnForce, 0, 0);
		} else if ((Input.GetKey (KeyCode.RightArrow) || this.isRButtonDown) && this.transform.position.x < this.movableRange) {
			//右の矢印キーが押されている間は、ユニティちゃんに右方向の力をかけ右に移動
			this.myRigidbody.AddForce (this.turnForce, 0, 0);
		} 
		//Jumpステート（ジャンプ状態）の場合はJumpにfalseをセットする
		//Jumpパラメータをtrueにしたままではジャンプアニメーションを何度も再生し続けてしまうため
		//Animatorクラスの「GetCurrentAnimatorStateInfo(0)」で現在のアニメーションの状態を取得
		//「IsName」関数で取得したステートの名前が引数の文字列と一致しているかどうかを調べる
		if (this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName ("Jump")) {
			this.myAnimator.SetBool ("Jump", false);
		}

		//ジャンプしていない時にスペースが押されたらジャンプする
		//多段ジャンプを防ぐため、地面付近にいる時にスペースキーが押された場合のみ
		if (Input.GetKeyDown(KeyCode.Space) && this.transform.position.y < 0.5f) {
			//ジャンプアニメを再生
			//Animatorクラスの「SetBool」関数は、第一引数に与えられたパラメータに、第二引数の値を代入する関数
			//第一引数のバラメータがアニメーション再生の条件に使われている
			this.myAnimator.SetBool ("Jump", true);
			//Unityちゃんに上方向の力を加える
			//力の加える方向は、「this.transform.up」
			//upでユニティちゃんを選択した時に出る緑の矢印の方向に力が加わるので、ユニティちゃんが上方向に上がる
			this.myRigidbody.AddForce (this.transform.up * this.upForce);
		}


	}
			
	//トリガーモードで他のオブジェクトと接触した場合の処理（減速）
	//オブジェクトと衝突した時に呼ばれるOnTriggerEnter関数
	//「OnTrrigerEnter」関数は、自分のColliderが他のオブジェクトのColliderと接触した時に呼ばれる関数
	//この関数が呼ばれるためには少なくともどちらか一方のオブジェクトがTriggerモードでなくてはならない 
	//今回は、ユニティちゃんがTriggerモードのオブジェクトと衝突したときに呼び出される
	//引数には接触した相手のCollisionが渡される（何と衝突したかは引数のゲームオブジェクトが持つTagを調べることで判別）
		void OnTriggerEnter(Collider other) {

			//障害物に衝突した場合
		//動きを止めるために、isEnd変数をtrue（上部）にする
			if (other.gameObject.tag == "CarTag" || other.gameObject.tag == "TrafficConeTag") {
				this.isEnd = true;
			//stateTextにGAME OVERを表示
			//文字列をGameResultTextが持つTextコンポーネントのtextに代入
			this.stateText.GetComponent<Text>().text = "GAME OVER";

			}

			//ゴール地点に到達した場合
			if (other.gameObject.tag == "GoalTag") {
				this.isEnd = true;
			//stateTextにGAME CLEARを表示
			//文字列をGameResultTextが持つTextコンポーネントのtextに代入
			this.stateText.GetComponent<Text>().text = "CLEAR!!";

			}  
		//コインに衝突した場合
		if (other.gameObject.tag == "CoinTag") {

			// スコアを加算(追加)
			this.score += 10;

			//ScoreText獲得した点数を表示(追加)
			//コインに触れたらscore変数に点数を加算し、ScoreTextが持つTextコンポーネントのtextに得点を代入して表示
			//文字列と値を「+」演算子でつないで代入
			this.scoreText.GetComponent<Text> ().text = "Score " + this.score + "pt";

			//コインゲットのパーティクルを再生
			//GetComponent関数を使ってParticleSystemコンポーネントを取得し、Play関数を呼び出す
			//ParticleSystemクラスの「Play」関数を呼ぶとパーティクルが再生される
			GetComponent<ParticleSystem> ().Play ();

			//接触したコインのオブジェクトを破棄（Destroy関数は、引数に指定したオブジェクトを破棄する関数）
			//ここでは引数をother.gameObjectとして、接触したゲームオブジェクトを指定
			Destroy (other.gameObject);

		}

	}
	//ジャンプボタンが押された時はGetMyJumpButtonDown関数を呼び出す（追加）
	//ジャンプ状態でなければ、ジャンプアニメーションを再生すると同時に上向きの力をかける
		public void GetMyJumpButtonDown() {
			if (this.transform.position.y < 0.5f) {
				this.myAnimator.SetBool ("Jump", true);
				this.myRigidbody.AddForce (this.transform.up * this.upForce);
			}
		}

	 //左ボタンを押し続けた場合の関数GetMyLeftButtonDown（追記）
	  //ボタンが押されているかどうかはisLButtonDown変数で管理
	//isLButtonDown変数がtrueの場合にはUpdate関数でユニティちゃんに左向きの力をかける
		public void GetMyLeftButtonDown() {
			this.isLButtonDown = true;
		}
	//左ボタンを離した場合に呼び出す関数GetMyLeftButtonUp（追記）
		public void GetMyLeftButtonUp() {
			this.isLButtonDown = false;
		}

		//右ボタンを押し続けた場合の処理（追加）
		public void GetMyRightButtonDown() {
			this.isRButtonDown = true;
		}
		//右ボタンを離した場合の処理（追加）
		public void GetMyRightButtonUp() {
			this.isRButtonDown = false;

	}
}
