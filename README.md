## DrivingTestSimulation
 Unity3D Project, subject two, simulated driving test


#### Introduction
 - Project Name: Simulated Driving 2
 - Start time: ‎October 26, 2020
 - Author: Alan21 | SummerRainCity
 - Operating platform: Mobile terminal | PC
 - Unity3D version: [Unity 2019.4.13f1c1 (64-bit)](https://unity.cn/releases/full/2019)
 - Libraries used: iTween
 - Auxiliary tools: FFmpeg
 - Demo address: [Video](https://www.bilibili.com/video/bv1hK4y1Q79G)
 - Explain: The beginner’s project may not be perfect, but I hope this project will help you a bit.


#### 更新信息：
 - 更新时间-2021-1-17
 1. 解决了方向盘不同机型转动轴心偏离

 - 更新时间-2021-2-18
 1. 加入了手刹系统

 - 更新时间-2021-6-19（待更新，工作太忙少有时间更新，先指出问题）
 1. 解决了在REC回放下偶然出现的物体回放错位BUG（每一个挂ReplayEntity.cs的脚本都将被记录状态，且被记录的物体若为子物体那么应当记录的是localPosition，若所有的父子物体都记录世界坐标，在回放时可能将出现一些怪异的现象。
 2. 关于移动端滑动时视觉出现“闪移”的问题，滑动移动视觉的代码在TPD.cs与FED.cs中，其中有代码if(results[k].gameObject.tag == "camera_panel")，标签camera_panel是UI组件的标签，你应当限制这个组件只接收首次触摸点，其中camera_panel面板应当按照比例缩小，把锚点与自己的四个实心蓝点重合即可。
 3. 在回放时，应当取消掉被回放的物体上有可能控制物体变换组件的所有脚本。
 4. 单项练习的玩家生成点与项目位置似乎没有对接好，可手动调整生成点到合适的位置（场景：Exam Mode.unity）

#### 设置项目：
 1. 倒车入库（3步骤+时间限制210s+压线检测+中途停车检测）
 3. 侧方停车（2步骤+时间限制90s+压线检测+中途停车检测）　　
 4. 半坡起步（1步骤+时间限制30s+压线检测+中途停车检测）
 5. 直角转弯（2步骤+时间限制30s+压线检测+中途停车检测）
 6. 曲线行驶（1步骤+中途停车检测）


#### 功能设定
1. **Home** *[主页]*
 - 考试模式（可回放）
 - 自由练习
 - 教程
 - 退出游戏
2. **Choose** *[单项练习]*
 - 倒车入库（可回放）
 - 侧方停车（可回放）
 - 半坡起步（可回放）
 - 直角转弯（可回放）
 - 曲线行驶（可回放）
 

#### 未完善：
 1. 某些项目没有设置边界空气墙，玩家可以绕过项目到达终点（若假设玩家遵守规则，那么这条可以忽略）
 2. “曲线行驶”没有设定压线检测

#### 基于此项目开发的《DES : Heavy Make》
 - Demo address: [DES:Heavy Make](https://www.bilibili.com/video/BV1Sq4y1r7eK)
