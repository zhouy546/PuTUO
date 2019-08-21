using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;
using Unity.Jobs;

public class LogoJobSystem : MonoBehaviour
{
    [SerializeField] public bool UseJob;
    //[SerializeField]Transform testTransform;
    public float3 MousePos;
    public List<mLogo> mLogos;
    // Start is called before the first frame update
    void Start()
    {
        mLogos = FindObjectsOfType<mLogo>().ToList();
        UseJob = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            MousePos = UtilsClass.GetMouseWorldPosition();
        }

        if (UseJob) {
            //声明变量；
            NativeArray<float3> positionArray = new NativeArray<float3>(mLogos.Count, Allocator.TempJob);
            NativeArray<FlapState> flapStates = new NativeArray<FlapState>(mLogos.Count, Allocator.TempJob);

            //赋值
            for (int i = 0; i < mLogos.Count; i++)
            {

                flapStates[i] = mLogos[i].flapState;
                positionArray[i] = mLogos[i].m_transform.position;
            }

            //初始化变量
            LogoFlapParalleJob logoFlapParalleJob = new LogoFlapParalleJob
            {
                positionArray = positionArray,
                flapStates = flapStates,
                MousePos = MousePos,
            };

            //安排Job
            JobHandle jobHandle = logoFlapParalleJob.Schedule(mLogos.Count, 20);

            //结束Job
            jobHandle.Complete();

            //更新计算数据
            for (int i = 0; i < mLogos.Count; i++)
            {

                mLogos[i].flapState = flapStates[i];
                mLogos[i].transform.position = positionArray[i];
            }

            //释放内存

            flapStates.Dispose();
            positionArray.Dispose();
        }
       
    }
    [BurstCompile]
    public struct LogoFlapParalleJob : IJobParallelFor
    {

        public NativeArray<float3> positionArray;

        public NativeArray<FlapState> flapStates;

        [ReadOnly] public float3 MousePos;

        public void Execute(int index)
        {

            flapStates[index] = UpdateFlapState(MousePos, index);
       

        }

        private FlapState UpdateFlapState(float3 mousePos, int index) {

            float2 mousPos = new float2(mousePos.x, mousePos.y);
            float2 nodePos = new float2(positionArray[index].x, positionArray[index].y);
            float dis = math.distance(mousPos, nodePos);

         

            if (dis < 2)
            {
                return FlapState.Front;
            }
            else
            {
                return flapStates[index];
            }
        }
    }
}
