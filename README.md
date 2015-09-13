#Unity-UI-Framework的设计与实现

@(归档)[Unity3D, UGUI]

[TOC]

###如何使用

请直接导入UnityUIFramework这个UnityPackage，然后进入名为Test的Scene即可开始体验各种特性，Enjoy！你可以通过访问[我的Github](https://github.com/MrNerverDie/Unity-UI-Framework)进行查阅和下载，也可以通过下面的附件直接下载。

![MainMenu](http://images2015.cnblogs.com/blog/447331/201509/447331-20150913161748965-843856388.png)

###基本概念：View，Context和UI的定义

UI是游戏中主要界面和它的子节点上的物体的统称，如装备列表界面中的**装备列表**和**每个装备**通常会被制作成两个Prefab，这两个Prefab被我们称作两个UI，这两个UI会对应两个UIType，在UIType里面会存储有这个UI全局唯一的名字和路径，如下：

```
public class UIType {

    public string Path { get; private set; }

    public string Name { get; private set; }

    public UIType(string path)
    {
        Path = path;
        Name = path.Substring(path.LastIndexOf('/') + 1);
    }

    public override string ToString()
    {
        return string.Format("path : {0} name : {1}", Path, Name);
    }
}
```

View代指游戏中的主要界面，例如：主界面，装备界面，装备详情界面等等。在View中包含了界面中处理数据的逻辑。

Context代指游戏中每个View的上下文，存储了这个界面的各种数据状态，每个特定的View会持有特定的Context，游戏中会通过栈的方式管理Context。

在本框架中，会把Context和View定义在同一个cs文件中，如下：

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

###View的创建和销毁

所有界面上挂上的Mono脚本和关联的Prefab统一以`XXXView`命名。

所有View的路径统一放在UIType中进行管理，每当新创建一个View的时候，都需要在UIType中新添加一个成员变量指明View的路径。

```
public static readonly UIType MainMenu = new UIType("View/MainMenuView");
public static readonly UIType OptionMenu = new UIType("View/OptionMenuView");
public static readonly UIType NextMenu = new UIType("View/NextMenuView");
public static readonly UIType HighScore = new UIType("View/HighScoreView");
```

在游戏中单独出现的View会通过UIManager中的`GetSingleUI`和`DestroySingleUI`来进行创建和销毁。

###View的跳转

每个View都相应拥有相应的Context来保存该界面的状态，View的跳转通过ContextManger管理，ContextManager中以栈的形式储存了已经经过的界面的Context。这样在返回的时候就可以得到需要的状态参数。

当需要进入下一个View的时候，调用`ContextManger.Instance.Push(nextContext)`即可，nextContext即为下一个View需要的上下文参数， 这是会调用当前View的OnPause函数，对当前View的上下文进行存储，并调用下一个View的OnEnter函数，对下一个Viwe的上下文进行初始化

当需要返回上一个界面的时候，调用`ContextManger.Instance.Push.Pop()`即可。这是会调用当前界面的OnExit函数，接着调用下一个界面的OnResume函数。

###View的动画

如果在界面上使用3D的旋转动画，就很难使用DoTween或者iTween在代码里面进行动画控制，而且为了保持战斗模块和UI模块设计的一致性。因此建议使用Animator对View的各种动画进行控制，而View的动画一般又和View的跳转逻辑联系紧密，所以建议将两者进行绑定，一个View的动画状态机如下图：

![ViewAnimator](http://images.cnblogs.com/cnblogs_com/neverdie/688179/o_ViewAnimator_resizeSmall_width=1920.png)

一个界面在没有显示的时候会处于Empty状态，当接收到OnEnter的Trigger的时候，会播放OnEnter动画，其他的状态如图所示，可以参考上图以及项目中的状态机。不同的界面可以使用相同的状态机，只是在某些状态上绑定的动画会有所不同。

这样做的另一个好处是，我们可以使用动画时间的方式在动画过程中做一些回调，这样的在界面上对回调时机进行编辑，相比使用协程或者Dotween的OnFinished函数，有更好的可编辑性。

###本地化

本地化是通过单例Localization和组件LocalizedText两个来协同实现的，不同语言的文字会存储在`Resources/Localization`中的不同JSON文件中，在单例Localization中配置后语言之后，即可读入相应的JSON文件。

每个LocalizedText所在的GameObject上都需要与Text绑定，LocalizedText会根据自己的textID对Text中的text进行本地化

###分辨率适配

UGUI中的分辨率适配是通过CanvasScaler来实现的，如下图：
![CanvasScaler](http://images.cnblogs.com/cnblogs_com/neverdie/688179/o_-Unnamed%20QQ%20Screenshot20150728200015.png)

在这里，我建议使用Scale With Width Or Height这种Scale模式，同时，由于大多数游戏是横屏游戏，通过使用高度固定，宽度随之变化的模式。这样我们就可以以一个固定的高度进行UI设计，只需要考虑UI在水平尺度上的延伸就可以了。

###提升滑动列表的性能

在UGUI中Scroller和Grid都是很好用的组件，但是由于它们在实现过程中考虑了太多对齐，排序的问题，这就导致它们在处理无限列表问题的时候遇到了极大的性能瓶颈，相关资料参考：[Performance issues on android with Scrollrect](http://forum.unity3d.com/threads/performance-issues-on-android-with-scrollrect.284448/)。在本框架中实现的自定义组件GridScroller可以在保证可编辑性的同时，提升了滑动列表的性能。

GridScroller的原理是：在滑动到某个item上的时候，会把之前的item进行回收，并且把它放到下一个位置进行再利用。在使用GridScroller的时候，你同样要使用ScrollRect和GridLayout，GridScroller会从这两个组件中读取相应的属性并且运用到UI逻辑中。

GridScroller对外界代码提供了一个Init的接口，通过这个接口，外界模块可以向GridScroller传入一个onChange回调函数，这样在GridScroller在刷新的时候，就会动态刷新相应的itemPrefab，实现用到时再加载的特性。

```
[RequireComponent(typeof(ScrollRect))]
public class GridScroller : MonoBehaviour {

    // public UI elements //
    [SerializeField]
    private Transform _itemPrefab;
    [SerializeField]
    private GridLayoutGroup _grid;

    // public fields //
    [SerializeField]
    private Movement _moveType = Movement.Horizontal;
    
    public delegate void OnChange(Transform trans, int index);
    
    public void Init(OnChange onChange, int itemCount, Vector2? normalizedPosition = null)
    {
        Clear();
        InitScroller();
        InitGrid();
        InitChildren(onChange, itemCount);
        InitTransform(normalizedPosition);
    }
```

###对UI进行修饰

由于UGUI的Image，Text等属性一般是不会设置Material的，我们可以通过写脚本继承BaseVertexEffect来对UI的Vertex进行修饰，项目中的Gradient Color和Blend Color就通过这种方式实现了颜色渐变和颜色运算的功能。通过重载`ModifyVertices`这个方法，你可以不实用Shader直接在脚本里对UI的渲染方式进行修饰。