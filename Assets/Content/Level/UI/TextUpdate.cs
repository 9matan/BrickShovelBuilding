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

  void Update()
  {
    funds_value.text = BSBDirector.playerResources.funds.ToString();
    materials_value.text = BSBDirector.playerResources.materials.ToString();
    funds_value_profit.text = "+" + BSBDirector.playerResources.income.funds.ToString();
    materials_value_profit.text = "+" + BSBDirector.playerResources.income.materials.ToString();
    workers_value.text = "/" + BSBDirector.playerResources.workersCapacity.ToString();
    workers_free.text = BSBDirector.playerResources.workers.ToString();
  }
}
