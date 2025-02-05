using UnityEngine;

public class RoomFloor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collider[] floorColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Ground"));
            GlobalVariables.selectedRoomFloor = new GameObject[floorColliders.Length];

            for (int i = 0; i < floorColliders.Length; i++)
            {
                GlobalVariables.selectedRoomFloor[i] = floorColliders[i].gameObject;
            }
        }
    }
}