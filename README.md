# 北语信科共享场地管理系统

------------
##### 这个项目是对北京语言大学信息科学学院日常维护和管理学院场地资源而设计。我们设计了用户端和管理端两个不同的界面。
------------
## 用户端
#### 用户端的主要需求有
###### 1.借阅教室
###### 2.取消预约
###### 3.查看可借教室
###### 4.联系管理员
###### 5.公告栏
###### 6.长期预约
## 管理端
#### 管理端的主要需求有
###### 	1.借阅教室
###### 2.取消预约
###### 3.审核消息
###### 4.查看可借教室
###### 5.管理员设置
###### 6.公告栏
###### 7.长期借用
###### 8.用户管理


------------

# 数据库设计
##### 为了实现主要的功能。我们对数据库进行了设计。数据库中有这么几张表：
### Admin表
##### Admin表用来存储管理员信息，表的结构是：
|Admin|数据类型|
| ------------ | ------------ |
|ID |nchar(10)|
|密码|nchar(12)|

### BorrowLog表
##### BorrowLog表用来记录借用教室等信息，表的结构是
|BorrowLog |数据类型|
| ------------ | ------------ |
|date|date|
|starttime |time(7)|
|campus|nvarchar(50)|
|building|nchar(10)|
|room|nchar(10)nvarchar(50)|
|userID|nvarchar(50)|
|reason|nchar(10)|
|checkstate|time(7)|
|endtime|time(7)|
|IsDeleted|int|
|orderdate|int|
### Buildings表
##### Buildings表用来保存学校教学楼信息
| Buildings  |数据类型   |
| ------------ | ------------ |
|  buildingID |bigint   |
| buildingName  |varchar(50)   |
|   campus| varchar(50)  |
### Campus表
##### Campus表用来处理校区等信息

| Campus  |数据类型   |
| ------------ | ------------ |
|campusID|bigint|
|campus|varchar(50)|
### classrooms表
##### classrooms表用来记录可以使用的教室。
| classroom  |数据类型   |
| ------------ | ------------ |
|campus|nvarchar(50)|
|building|nchar(10)|
|room|nchar(10)|
|roomID|nchar(10)|
|type|nchar(10)|
### contact_Admin表
##### contact_Admin表用来保存可以联系的管理员信息
| contact_Admin  |数据类型   |
| ------------ | ------------ |
|姓名|nchar(10)|
|联系方式|nchar(11)|
|QQ|nchar(10)|
|微信|nchar(20)|
### notice表
##### notice表用来保存公告消息
| notice  |数据类型   |
| ------------ | ------------ |
|标题|nchar(30)|
|发布时间|date|
|链接|nchar(100)|

### Time表
##### Time表用来保存一天中可用的时间
| Time  |数据类型   |
| ------------ | ------------ |
|startTime|nchar(10)|
|endTime|nchar(10)|
### users表
##### users表用来保存用户信息
| users  |数据类型   |
| ------------ | ------------ |
|userID|nvarchar(50)|
|password|nchar(10)|
|contact|nvarchar(50)|
|college|nvarchar(50)|
|grade|nchar(10)|
|major|nvarchar(50)|


#项目成员
##牛昊、张骋、张璇、王诗可、樊君瑶





