using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using BSB;

public class TextUpdate : MonoBehaviour
{
  public Text funds_value;
  public Text materials_value;
  public Text funds_value_profit;
  public Text materials_value_profit;
  public Text workers_value;
  public Text workers_free;
  Text buy_worker_text;
  public Button buy_worker;
  Text buy_materials_text;
  public Button buy_materials;
  public Text date;

  void Start()
  {
    buy_worker.onClick.AddListener( BSBDirector.workerManager.HireWorker );
    buy_worker_text = buy_worker.GetComponentInChildren<Text>();
    buy_materials.onClick.AddListener( BSBDirector.materialManager.BuyMaterials );
    buy_materials_text = buy_materials.GetComponentInChildren<Text>();
  }
  void Update()
  {
    date.text = BSBDirector.timeManager.day.ToString()+"/"+BSBDirector.timeManager.month.ToString() +"/"+BSBDirector.timeManager.year.ToString() ; 
    funds_value.text = BSBDirector.playerResources.funds.ToString();
    materials_value.text = BSBDirector.playerResources.materials.ToString();
    funds_value_profit.text = "+" + BSBDirector.playerResources.income.funds.ToString();
    materials_value_profit.text = "+" + BSBDirector.playerResources.income.materials.ToString();
    workers_value.text = "/" + BSBDirector.playerResources.workersCapacity.ToString();
    workers_free.text = BSBDirector.playerResources.workers.ToString();
    buy_worker.interactable  =BSBDirector.workerManager.TryHireWorker();
    buy_worker_text.text = "Buy worker "+ BSBDirector.workerManager.workerPrice.funds.ToString();
    buy_materials_text.text = "Buy materials ( "+ BSBDirector.materialManager.purchasingQuantity.ToString()+"/"+ BSBDirector.materialManager.materialPrice.funds.ToString()+" )";
    buy_materials.interactable  = BSBDirector.materialManager.TryBuyMaterials();
  }
}
