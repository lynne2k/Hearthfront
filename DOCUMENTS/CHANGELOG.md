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