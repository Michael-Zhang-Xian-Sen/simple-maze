1. ## 简易迷宫 使用指南

   迷宫中黑色格子为墙壁，不可行走。灰色格子为道路，可以行走。

   - 开始游戏：点击开始游戏按钮，自动生成迷宫，出生点位为左上方，WASD键控制方块移动。迷宫出口为右下角，控制小方块走到出口则成功逃离迷宫，游戏结束。
   - 自动寻路：自动找到一条从迷宫入口到迷宫出口的路径，并对路径标红。
   - 生成迷宫：重新生成迷宫。
   - 自定义迷宫：鼠标左键点击格子变为黑色（墙壁），再次点击变为灰色（路径）。

   

   ## 其他信息

   - 项目背景：大二数据结构课程的课程设计之一。
   - 项目地址：https://github.com/Michael-Zhang-Xian-Sen/simple-maze
   - 应用下载地址：http://140.143.141.58/download/Maze.rar

   

   ## TODO

   1. 自定义迷宫后，不能根据该迷宫进行游戏，且不能保存该迷宫。
   2. 核心玩法有问题。现在就像是关闭了WAR3、LOL等游戏关闭了战争迷雾，或CSGO等游戏开了透视，一眼就能看出来哪里是墙，那里是路。在最初墙不应该显示，当发现走不动时，才会将墙显示出来。
   3. 在2的基础上，添加一个“遗忘”的功能。玩家在游玩时碰到了一堵墙，但是由于记忆是存在时效性的，过一段时间会忘记那里存在一堵墙，所以我们可以在用户碰到这堵墙开始，或墙离开用户的视野开始计时几秒后，使这堵墙也隐藏起来。
   4. 游戏说明的功能还未添加。