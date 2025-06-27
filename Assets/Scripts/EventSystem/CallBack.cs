namespace EventSystem
{
    //delegate = 委托
    //这里你可以理解为拨号的类型转接
    public delegate void CallBack();
    //T是变量类型（比如int）arg是变量名称
    public delegate void CallBack<T>(T arg);
    public delegate void CallBack<T, X>(T arg1, X arg2);
    public delegate void CallBack<T, X, Y>(T arg1, X arg2, Y arg3);
    public delegate void CallBack<T, X, Y, Z>(T arg1, X arg2, Y arg3, Z arg4);
}
