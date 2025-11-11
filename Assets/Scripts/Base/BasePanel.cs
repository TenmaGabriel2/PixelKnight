using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

    public abstract class BasePanel : MonoBehaviour
    {
        //整体控制淡入淡出画布组件
        private CanvasGroup canvasGroup;
        private float alphaSpeed = 5;

        private bool isShow = false;


        /// <summary>
        /// 淡出成功时执行的委托
        /// </summary>
        private UnityAction hideCallBack;
        protected virtual void Awake()
        {
            
            //得到画布组件 若没有则添加一个
            canvasGroup = this.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
               canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
            }

        protected virtual void Start()
        {
            Init();
        }


        /// <summary>
        /// 用于初始化按钮事件监听
        /// </summary>
        public abstract void Init();
        /// <summary>
        /// 显示面板
        /// </summary>
        public virtual void ShowMe()
        {
            isShow = true;
            canvasGroup.alpha = 0;//起始值
        }
        /// <summary>
        /// 隐藏面板
        /// </summary>
        public virtual void HideMe(UnityAction callBack)
        {
            isShow = false;
            canvasGroup.alpha = 1;//起始值
            hideCallBack = callBack;
        }

        void Update()
        {
            //淡入
            if (isShow && canvasGroup.alpha != 1)
            {
                canvasGroup.alpha += Time.deltaTime * alphaSpeed;
                if (canvasGroup.alpha >= 1)
                    canvasGroup.alpha = 1;
            }
            //淡出
            else if (!isShow)
            {
                canvasGroup.alpha -= Time.deltaTime * alphaSpeed;
                if (canvasGroup.alpha <= 0)
                {
                    canvasGroup.alpha = 0;
                    //淡出后的其它操作(比如销毁面板)-
                    hideCallBack?.Invoke(); //不为空才执行

                }
            }
        }

    }
