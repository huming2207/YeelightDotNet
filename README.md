# Xiaomi Yeelight Library for .NET
# 小米/亿联智能灯泡产品极客模式API解析库 for .NET


## Description 简介

本项目是为小米Yeelight智能灯泡产品开发的.NET解析库，可以在内网中通过极客模式API寻找并控制灯泡。

本项目所有API reference来自[小米亿联官网的文档](http://www.yeelight.com/download/Yeelight_Inter-Operation_Spec.pdf)，版权归其公司所有，本人仅对此文档内所提及的内容进行开发，文档内容归小米公司所有，本项目则按照[CC-BY-NC-SA 3.0中国大陆版](http://creativecommons.net.cn)授权，未经允许严禁商用。

This is a .NET library used for finding and controlling the Yeelight light bulb products via its "geek mode" APIs in internal network.

This library is based on [Xiaomi Yeelight's official API reference document.](http://www.yeelight.com/download/Yeelight_Inter-Operation_Spec.pdf). This project is authorized and protected by [CC-BY-NC-SA 3.0 China Mainland](http://creativecommons.net.cn). Any commercial uses is **NOT allowed.**

## Tested features 已测试的功能
目前我手上只有俩单色版本的灯泡，正拿它们当小白鼠。目前所有单色产品的API测试貌似没什么问题。不过我也顺手搞了（其实是一块儿的）给彩色版用的API，只是没法测试。这里欢迎各位帮助测试相关API，若有问题，请fork一份本项目并进行修改，或是提个issue，**但务必记住要附带上相关错误信息，谢谢。**

**同时亦欢迎感兴趣的关注者对本项目捐赠彩色版产品，或资助相应资金购买彩色版产品以测试并改善此项目。所有相关捐助资金用途可供质询，保证不乱用、浪费。欢迎联系：huming2207@gmail.com。**

Currently I've only got two single-color (mono) lightbulbs, which means I can only test on them. So all single-color lightbulbs product related stuff are functional. 

I have already included all the multi-color product related API, but they are not tested and maybe not functional. I welcome someone who can assist me to do the test. Simply fork and submit a pull request or an issue with detailed information of any crashes/exceptions/other errors. 

I also welcome someone who can donate me one (or more) multi-color products if you really wish. I promise I will use it in the development as soon as I receive it. Contact me via huming2207@gmail.com

## To-Do list 还差啥……？

- [X] 小米特色SSDP支持
- [x] 异步TCP通信（暂用第三方库实现）
- [x] JSON操作
  - [x] COMMAND信息
  - [X] RESULT信息
  - [ ] NOTIFICATION信息（貌似没啥用，以后加上）
- [ ] 某些异常处理
- [ ] Mono/Xamarin.Mac移植和测试
- [ ] PCL库移植测试
- [ ] 接口文档

- [X] Xiaomi's "Fake" SSDP handling
- [x] Asynchronous TCP (Telnet) communication, currently still using 3rd party library
- [x] JSON handling
  - [x] COMMAND message
  - [X] RESULT message
  - [ ] NOTIFICATION message (a bit useless, will add later)
- [ ] Some nasty exceptions handling
- [ ] Mono/Xamarin.Mac port & test
- [ ] Portable library port & test
- [ ] Documentation


