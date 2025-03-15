using UnityEngine;

public class Equipment: MonoBehaviour
{
    public enum HandType { Left, Right }
    
    public GameObject LeftPivot;
    public GameObject RightPivot;

    public void EquipItem(GameObject item, HandType handType = HandType.Left)
    {
        GameObject currPivot = handType == HandType.Left ? LeftPivot : RightPivot;
        
        // 기존의 무기 삭제 필요
        Instantiate(item, currPivot.transform);
        item.transform.SetParent(currPivot.transform);
    }
}