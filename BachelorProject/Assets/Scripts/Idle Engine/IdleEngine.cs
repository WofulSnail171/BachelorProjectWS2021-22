
using System;
using UnityEngine;
using IdleEngine.Session;
using System.Collections;

namespace IdleEngine
{
    public class IdleEngine : MonoBehaviour
    {

        #region vars
        public Session.Session session;
        private bool IsRunning = true;
        #endregion


        private void Start()
        {
            StartCoroutine(RunIdleEngine());
        }


        //should replace with Iserializable callback reciever
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



        //run idle engine in coroutine
        IEnumerator RunIdleEngine()
        {
            while (IsRunning)
            {
                if (!session)
                {
                    IsRunning = false ;
                    yield break;
                }
                session.Tick(Time.deltaTime);

                yield return null;
            }

            yield return null;
        }
    }
}