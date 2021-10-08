
using System;
using UnityEngine;
using IdleEngine.Session;

namespace IdleEngine
{
    public class IdleEngine : MonoBehaviour
    {

        public Session.Session session;

        private void Update()
        {
            if (!session)
            {
                return;
            }
            session.Tick(Time.deltaTime);
        }

        

        //replace with Iserializable calback reciever
        private void OnEnable()
        {
            if(!session)
            {
                return;
            }

            session.CalculateOfflineTick();
        }

        private void OnDestroy()
        {
            if (!session)
            {
                return;
            }

            session.SaveTicks();
        }
    }
}