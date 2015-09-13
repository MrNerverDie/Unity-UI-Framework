#Unity-UI-Framework的设计与实现

###如何使用

请直接导入UnityUIFramework这个UnityPackage，然后进入名为Test的Scene即可开始体验各种特性，Enjoy！

###View的创建和销毁

所有界面上挂上的Mono脚本和关联的Prefab统一以`XXXView`命名。

所有View的路径统一放在UIType中进行管理，每当新创建一个View的时候，都需要在UIType中新添加一个成员变量指明View的路径。

在游戏中只会出现一个的View会通过UIManager中的GetSingleUI和DestroySingleUI来进行创建和销毁。

###View的跳转

每个View都相应拥有相应的Context来保存该界面的状态，View的跳转通过ContextManger管理，ContextManager中以栈的形式储存了已经经过的界面的Context。这样在返回的时候就可以得到需要的状态参数。

当需要进入下一个View的时候，调用`ContextManger.Instance.Push(nextContext)`即可，nextContext即为下一个View需要的上下文参数， 这是会调用当前View的OnPause函数，对当前View的上下文进行存储，并调用下一个View的OnEnter函数，对下一个Viwe的上下文进行初始化

当需要返回上一个界面的时候，调用`ContextManger.Instance.Push.Pop()`即可。这是会调用当前界面的OnExit函数，接着调用下一个界面的OnResume函数。

一个普通的View脚本如下，包含了相应的Context和View的实现：
```
    public class OptionMenuContext :BaseContext
    {
        public OptionMenuContext() : base(UIType.OptionMenu){}
    }

    public class OptionMenuView : AnimateView
    {
        public override void OnEnter(BaseContext context)

        public override void OnExit(BaseContext context)

        public override void OnPause(BaseContext context)

        public override void OnResume(BaseContext context)
    }
```

###View的动画

如果在界面上使用3D的旋转动画，就很难使用DoTween或者iTween在代码里面进行动画控制，而且为了保持游戏内和游戏外设计的一致性。因此建议使用Animator对View的各种动画进行控制，而View的动画一般又和View的跳转联系紧密，所以建议将两者进行绑定，一个一般的动画状态机如下图：

![ViewAnimator](http://images.cnblogs.com/cnblogs_com/neverdie/688179/o_ViewAnimator_resizeSmall_width=1920.png)

一个界面在没有显示的时候会处于Empty状态，当接收到OnEnter的Trigger的时候，会播放OnEnter动画，其他的状态如图所示，可以参考上图以及项目中的状态机。不同的界面可以使用相同的状态机，只是在某些状态上绑定的动画会有所不同。

###本地化

本地化是通过单例Localization和组件LocalizedText两个来协同实现的，不同语言的文字会存储在`Resources/Localization`中的不同JSON文件中，在单例Localization中配置后语言之后，即可读入相应的JSON文件。

每个LocalizedText所在的GameObject上都需要与Text绑定，LocalizedText会根据自己的textID对Text中的text进行本地化

###分辨率适配

UGUI中的分辨率适配是通过CanvasScaler来实现的，如下图：
![CanvasScaler](http://images.cnblogs.com/cnblogs_com/neverdie/688179/o_-Unnamed%20QQ%20Screenshot20150728200015.png)

在这里，我建议使用Scale With Width Or Height这种Scale模式，同时，由于我们是横屏游戏，我建议使用高度固定，宽度随之变化的模式，即为如图所示的设置方式。

###自定义组件

自定义组件GridScroller可以在保证可编辑性的同时，提升了滑动列表的性能，可以参考GridScroller的相应源代码。

由于UGUI的Image，Text等属性一般是不会设置Material的，我们可以通过写脚本继承BaseVertexEffect来对UI的Vertex进行修饰，项目中的Gradient Color和Blend Color就通过这种方式实现了颜色渐变和颜色运算的功能。
