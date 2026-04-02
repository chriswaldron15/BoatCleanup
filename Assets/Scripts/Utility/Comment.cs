using NaughtyAttributes;
using UnityEngine;

namespace BoatGame
{
    public class Comment : MonoBehaviour
    {
        [SerializeField, ResizableTextArea] private string comment;
    }
}
