# Changelog

## 20241221T1602

- 加入了最简单的附身功能：按右键可以从objectA附身到objectB
- 改了一下函数名（onTick→OnTick）；重构调整了OnTick和snapshot的顺序（tick完了才snapshot）。
- 实现方式：
  - 加入一个全scene的单例：Ghost，管理附身
  - Mobile里加入OnPossess和OnUnpossess函数
  - Ghost里面处理，检测右键，检测到了从GameManager里找同座标的对象（demo实现，会改的），然后改附身对象
  - 现在史莱姆不被附身后会spriteRenderer.enabled = false
  - 初始的附身对象：写Ghost的public里了

## 20241225T1638
- 修改了Tick的位置，从GameManager的Update改到了FixedUpdate里。
  - 好处是，没有两个Update函数了？
- 实现了个基本的动画的设计pattern（见demo slime）。原理：
  - Animation script单方面地监视slime里面的参数，然后更新Animator里面的数值
  - Slime的主script不能直接调用/读取Animation script和Animator里面的东西（以防万一）
- 已知bug：附身功能+时间回溯会产生矛盾（Ghost存储的附身对象不会随着时间回溯而更新）

## 20241226T0053
- 加入了一个Tilemap，设置了Physics layer
- 把史莱姆的行动逻辑改成键盘操作了；只能动3次就会累死（动画上没有显示出来）
  - TODO: 动画反映真实状态
- 史莱姆的savedata加入了fx(facing_x)和fy(facing_y)，还没有用
- 把Tick的顺序改回去了
  - TODO: Tick和Snapshot的顺序待议。首先我们第0帧（游戏刚加载进去）就应该进行一次Snapshot的，现在是这么做的；然后过1秒后先Tick++再Snapshot，这样Snapshot的是第1帧。
    - 问题是目前是反过来的，也就是说第0帧存了两次（#0和#1）。其实算bug，，，吧

## 20241226T1445
- 附身+小动画
  - 1502: 加入了附身距离
  - 后续参考文献（camera设置pixel perfect）：https://docs.unity3d.com/Packages/com.unity.2d.pixel-perfect@1.0/manual/index.html

## 20241226T1622
- 附身优化（了吗？）
- 加入传送端点
- 修复了PathPoint用SlimeData的bug，删除了用不着的字段

## 20241228T1311
- Key的移动的UI做好力
- 已知漏洞：由于所有对象的Input检测都是内部的，所以说UI的input并不会block掉玩家角色的input检测
  - 现在附身移动只用右键，UI只用左键，所以姑且还没有矛盾、、
- 如何使用：
  - 在Mobile继承类的`List<string> spells`里面，添加字符串就是新的一段字；
  - 这个字符串里面，用`~`分隔，`~`前面的部分是这个spell的识别码；
  - 比如`M114514~My name is donk`这样：`M`是识别码
    - 如果识别码是空的，就不能移动到玩家的道具栏里。反之，则可以。
    - 每个Mobile又有一个`acceptingSpellPrefixes`，表示哪些识别码的首字母（仅首字母，比如`M`但不是`M1`）是可接受的；
    - 如果是可接受的，玩家就能添加其到这个Mobile里