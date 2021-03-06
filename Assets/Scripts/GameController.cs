﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public Text resultText;
    public Text[] rankingTexts = new Text[5];

    // Use this for initialization
    void Start () {
        string playerName = PlayerPrefs.GetString("playerName");
        int score = (int)PlayerPrefs.GetFloat("scoreTime");
        SaveScoreRanking(playerName, score);
        resultText.text = score.ToString();
        Invoke("GetScoreRanking", 1.5f);
    }

	// Update is called once per frame
	void Update () {
    }

    public void ReturnToGameScene(){
        SceneManager.LoadScene("Main");
    }

    // スコアセーブ
    void SaveScoreRanking(string playerName, int score){
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("ScoreRanking"); // NCMB上のScoreRankingクラスを取得
        query.WhereEqualTo("playername", playerName); // プレイヤー名でデータを絞る
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null)
            { // データの検索が成功したら、
                if (objList.Count == 0)
                { // スコアが未登録の場合
                    NCMBObject cloudObj = new NCMBObject("ScoreRanking");
                    cloudObj["playername"] = playerName;
                    cloudObj["score"] = score;
                    cloudObj.SaveAsync(); // セーブ
                }
                else
                { // ハイスコアが登録済みの場合
                    int cloudScore = System.Convert.ToInt32(objList[0]["score"]); // クラウド上のスコアを取得
                    if (score > cloudScore)
                    { // 今プレイしたスコアの方が高かったら、
                        objList[0]["score"] = score; // それを新しいスコアとしてデータを更新
                        objList[0].SaveAsync(); // セーブ
                    }
                }
            }
        });
    }

    // ランキング取得
    void GetScoreRanking(){
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("ScoreRanking");
        query.OrderByDescending("score"); 
        query.Limit = rankingTexts.Length; 
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null)
            { //検索成功したら
                for (int i = 0; i < objList.Count; i++)
                {
                    string s = System.Convert.ToString(objList[i]["playername"]);
                    //int n = System.Convert.ToInt32(objList[i]["score"]);
                    rankingTexts[i].text = s;
                }
            }
        });
    }

    // プレーヤー名の存在確認/変更(使ってない)
    void CheckPlayerName(string playerName){
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("ScoreRanking");
        query.WhereEqualTo("playername", playerName);
        query.CountAsync((int count, NCMBException e) => { // 1つ上のコードで絞られたデータが何個あるかかぞえる 
            if (e == null)
            {
                if (count == 0)
                { // 0個なら名前は登録されていない
                    Debug.Log("登録可能です");
                }
                else
                { // 0個じゃなかったらすでに名前が登録されている
                    Debug.Log("登録できません");
                }
            }
        });
    }
    void RenamePlayerName(string previousName, string newName)
    {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("ScoreRanking");
        query.WhereEqualTo("playername", previousName); // 古い名前でデータを絞る
        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e == null)
            { //検索成功したら
                if (objList.Count > 0)
                { // 1個以上あれば
                    for (int i = 0; i < objList.Count; i++)
                    {
                        objList[i]["playername"] = newName; // 新しい名前にする
                        objList[i].SaveAsync(); // セーブ
                    }
                }
            }
        });
    }
}
