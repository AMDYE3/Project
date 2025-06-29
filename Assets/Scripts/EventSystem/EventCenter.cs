using System.Collections.Generic;
using System;

namespace EventSystem
{ 
    public class EventCenter
    {
        #region 事件广播主体
        //此处的代码可以不做修改直接使用

        //设置新的委托列表
        private static Dictionary<EventType, Delegate> EventTable = new Dictionary<EventType, Delegate>();

        /// <summary>
        /// 添加监听
        /// 可以理解为：添加号码
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        private static void OnListenerAdding(EventType eventType, Delegate callBack)
        {
            if (!EventTable.ContainsKey(eventType))//是否有事件编码
            {
                //如果没有就添加
                EventTable.Add(eventType, null);
            }
            Delegate d = EventTable[eventType];
            if (d != null && d.GetType() != callBack.GetType())//d不为空，且，d的类型不为获取到的类型
            {
                //报错
                throw new Exception(string.Format("尝试为事件{0}添加不同类型的委托，当前事件所对应的委托是{1}，要添加的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
                //↑请检查号码是否正确，并重新输入
            }
        }

        /// <summary>
        /// 移除监听
        /// 可以理解为：删除号码
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        private static void OnListenerRemoving(EventType eventType, Delegate callBack)
        {
            if (EventTable.ContainsKey(eventType))
            {
                Delegate d = EventTable[eventType];
                if (d == null)
                {
                    //没有对应号码，或者号码已被删除
                    throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", eventType));
                }
                else if (d.GetType() != callBack.GetType())
                {
                    //请检查号码是否正确，并重新输入
                    throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，当前委托类型为{1}，要移除的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
                }
            }
        }
        private static void OnListenerRemoved(EventType eventType)
        {
            if (EventTable[eventType] == null)//如果号码已经被删除
            {
                EventTable.Remove(eventType);
            }
        }
        #endregion

        #region 0号广播类型(可复制修改)
        public static void AddListener(EventType eventType, CallBack callBack)
        {
            OnListenerAdding(eventType, callBack);//在基站里添加
            EventTable[eventType] = (CallBack)EventTable[eventType] + callBack;//在手机里添加号码，+xx，xxx
        }

        public static void RemoveListener(EventType eventType, CallBack callBack)
        {
            // Check if the event type exists in the dictionary before removing
            if (!EventTable.ContainsKey(eventType))
                return;
                
            OnListenerRemoving(eventType, callBack);//在基站里删除
            EventTable[eventType] = (CallBack)EventTable[eventType] - callBack;
            OnListenerRemoved(eventType);//中止通讯
        }

        public static void Broadcast(EventType eventType)
        {
            Delegate d;
            if (EventTable.TryGetValue(eventType, out d))
            {
                CallBack callBack = d as CallBack;
                if (callBack != null)
                {
                    callBack();//拨号
                }
                else
                {
                    //打错电话了
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
                }
            }
        }
        #endregion

        #region 1号广播类型(可复制修改)
        public static void AddListener<T>(EventType eventType, CallBack<T> callBack)
        {
            OnListenerAdding(eventType, callBack);//在基站里添加
            EventTable[eventType] = (CallBack<T>)EventTable[eventType] + callBack;//在手机里添加号码，+xx，xxx
        }

        public static void RemoveListener<T>(EventType eventType, CallBack<T> callBack)
        {
            // Check if the event type exists in the dictionary before removing
            if (!EventTable.ContainsKey(eventType))
                return;
                
            OnListenerRemoving(eventType, callBack);//在基站里删除
            EventTable[eventType] = (CallBack<T>)EventTable[eventType] - callBack;
            OnListenerRemoved(eventType);//中止通讯
        }

        public static void Broadcast<T>(EventType eventType, T arg)
        {
            Delegate d;
            if (EventTable.TryGetValue(eventType, out d))
            {
                CallBack<T> callBack = d as CallBack<T>;
                if (callBack != null)
                {
                    callBack(arg);//拨号
                }
                else
                {
                    //打错电话了
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
                }
            }
        }
        #endregion

        #region 2号广播类型(可复制修改)
        public static void AddListener<T,X>(EventType eventType, CallBack<T,X> callBack)
        {
            OnListenerAdding(eventType, callBack);//在基站里添加
            EventTable[eventType] = (CallBack<T,X>)EventTable[eventType] + callBack;//在手机里添加号码，+xx，xxx
        }

        public static void RemoveListener<T,X>(EventType eventType, CallBack<T,X> callBack)
        {
            // Check if the event type exists in the dictionary before removing
            if (!EventTable.ContainsKey(eventType))
                return;
                
            OnListenerRemoving(eventType, callBack);//在基站里删除
            EventTable[eventType] = (CallBack<T,X>)EventTable[eventType] - callBack;
            OnListenerRemoved(eventType);//中止通讯
        }

        public static void Broadcast<T,X>(EventType eventType, T arg1,X arg2)
        {
            Delegate d;
            if (EventTable.TryGetValue(eventType, out d))
            {
                CallBack<T,X> callBack = d as CallBack<T,X>;
                if (callBack != null)
                {
                    callBack(arg1,arg2);//拨号
                }
                else
                {
                    //打错电话了
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
                }
            }
        }
        #endregion

        #region 3号广播类型(可复制修改)
        public static void AddListener<T, X,Z>(EventType eventType, CallBack<T, X,Z> callBack)
        {
            OnListenerAdding(eventType, callBack);//在基站里添加
            EventTable[eventType] = (CallBack<T, X,Z>)EventTable[eventType] + callBack;//在手机里添加号码，+xx，xxx
        }

        public static void RemoveListener<T, X,Z>(EventType eventType, CallBack<T, X,Z> callBack)
        {
            // Check if the event type exists in the dictionary before removing
            if (!EventTable.ContainsKey(eventType))
                return;
                
            OnListenerRemoving(eventType, callBack);//在基站里删除
            EventTable[eventType] = (CallBack<T, X,Z>)EventTable[eventType] - callBack;
            OnListenerRemoved(eventType);//中止通讯
        }

        public static void Broadcast<T, X,Z>(EventType eventType, T arg1, X arg2, Z arg3)
        {
            Delegate d;
            if (EventTable.TryGetValue(eventType, out d))
            {
                CallBack<T, X,Z> callBack = d as CallBack<T, X,Z>;
                if (callBack != null)
                {
                    callBack(arg1, arg2,arg3);//拨号
                }
                else
                {
                    //打错电话了
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
                }
            }
        }
        #endregion

        #region 3号广播类型(可复制修改)
        public static void AddListener<T, X, Z,A>(EventType eventType, CallBack<T, X, Z,A> callBack)
        {
            OnListenerAdding(eventType, callBack);//在基站里添加
            EventTable[eventType] = (CallBack<T, X, Z,A>)EventTable[eventType] + callBack;//在手机里添加号码，+xx，xxx
        }

        public static void RemoveListener<T, X, Z,A>(EventType eventType, CallBack<T, X, Z,A> callBack)
        {
            // Check if the event type exists in the dictionary before removing
            if (!EventTable.ContainsKey(eventType))
                return;
                
            OnListenerRemoving(eventType, callBack);//在基站里删除
            EventTable[eventType] = (CallBack<T, X, Z,A>)EventTable[eventType] - callBack;
            OnListenerRemoved(eventType);//中止通讯
        }

        public static void Broadcast<T, X, Z,A>(EventType eventType, T arg1, X arg2, Z arg3, A arg4)
        {
            Delegate d;
            if (EventTable.TryGetValue(eventType, out d))
            {
                CallBack<T, X, Z,A> callBack = d as CallBack<T, X, Z,A>;
                if (callBack != null)
                {
                    callBack(arg1, arg2, arg3,arg4);//拨号
                }
                else
                {
                    //打错电话了
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
                }
            }
        }
        #endregion
    }

}