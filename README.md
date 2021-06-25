## DrivingTestSimulation
 Unity3D Project, subject two, simulated driving test
 
#### Introduction
 - Project Name: Simulated Driving 2
 - Start time: ‎October 26, 2020
 - Author: Alan21 | SummerRainCity
 - Operating platform: Mobile terminal | PC
 - Unity3D version: [Unity 2019.4.13f1c1 (64-bit)](https://unity.cn/releases/full/2019)
 - Libraries used: iTween
 - External tools: FFmpeg
 - Demo address: [Video](https://www.bilibili.com/video/bv1hK4y1Q79G)
 - Explain: The beginner’s project may not be perfect, but I hope this project will help you a bit.


#### 更新信息：
 - 更新时间-2021-1-17
 1. 解决了方向盘不同机型转动轴心偏离

 - 更新时间-2021-2-18
 1. 加入了手刹系统

 - 更新时间-2021-6-19（待同步）
 1. 解决了PC端在REC回放下偶然出现的物体回放错位BUG（ReplayEntity.cs将简称为Re.cs，每一个挂Re.cs的脚本都将被记录状态，假设B是A的子物体，当两个物体都将被记录变换组建时，子物体B应当只记录localPosition，若A如果没有父物体则记录世界坐标Position。
 2. 降低REC记录帧数为30，原120（ReplayManager.cs）


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
