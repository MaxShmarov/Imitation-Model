using LittleWorld.Common;
using UnityEngine;

namespace LittleWorld.Controllers
{
    public class GameController : MonoBehaviour
    {
        public void NextStep()
        {
            EventManager.Trigger(Config.NextStep);
        }
    }
}