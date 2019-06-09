function ShowNo()                        //隐藏两个层 
{
    //document.getElementById("doing").style.display = "none";
    document.getElementById("reservation").style.display = "none";
}

function getRange()                      //得到屏幕的大小 
{
    var top = document.body.scrollTop;
    var left = document.body.scrollLeft;
    var height = document.body.clientHeight;
    var width = document.body.clientWidth;

    if (top == 0 && left == 0 && height == 0 && width == 0) {
        top = document.documentElement.scrollTop;
        left = document.documentElement.scrollLeft;
        height = document.documentElement.clientHeight;
        width = document.documentElement.clientWidth;
    }
    return { top: top, left: left, height: height, width: width };
}
function checkItems() {
    if (checkCampus()) {
        if (checkBuilding()) {
            if (checkDate()) {
                if (checkTime()) {
                    return true;
                }
                else {
                    return false;
                }

            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    }
    else {
        return false;
    }
    
}
function checkCampus()
{
    var campus = document.getElementById("DropDownListCampus");
    var cs = campus.options[campus.selectedIndex].text;
    if (cs == "--请选择--")
    {
        alert("请选择校区");
        return 0;
    }
    return 1;
    

}
function checkBuilding()
{
    var building = document.getElementById("DropDownListBuilding");
    var bd = building.options[building.selectedIndex].text;
    if (bd == "--请选择--") {
        alert("请选择教学楼");
        return 0;
    }
    return 1;

}
//function checkRoom()
//{
//    var room = document.getElementById("DropDownListRoom");
//    var rm = room.options[room.selectedIndex].text;
//    if (cs == "--请选择--") {
//        alert("请选择教室");
//        return 0;
//    }
//    return 1;

//}
function checkDate() {
    var d = new Date();
    d = new Date(d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate());
    var d2 = new Date(document.getElementById("date").value);
    if (d2 == "Invalid Date") {
        alert('请选择日期');
        return 0;
    }
    else {
        var n = d.getTime() - d2.getTime();
        if (n > 0) {
            alert("小于当前日期");
            document.getElementById("date").value = d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate();
            return 0;
        }
    }
    return 1;
}
function checkTime() {
    var st = document.getElementById("DropDownListStart");
    var s = st.options[st.selectedIndex].text;
    var et = document.getElementById("DropDownListEnd");
    var e = et.options[et.selectedIndex].text;
    var stime = getHour(s);
    var etime = getHour(e);
    if (stime >= etime) {
        alert("结束时间应该大于开始时间");
        document.getElementById("DropDownListEnd").selectedIndex = 0;
        document.getElementById("DropDownListStart").selectedIndex = 0;
        return 0;
    }
    return 1;
}

function getHour(time) {
    var s = time.match(/^(([01]?\d)|(2[0-4])):[0-5]?\d$/);
    return parseInt(s);
}

function checkInput() {
   
   
    var userid = document.getElementById("TextUserID").value;
   
    var usercontact = document.getElementById("TextContact").value;
   
    var userreason = document.getElementById("TextReason").value;
   
    if (usercontact == ''||userid==''||userreason=='')
    {
        alert("请把信息填写完整!");
        return false;
    }
    else {
        tanchuang("60px", "已申请，等待管理员审核")
        ShowNo();
        return true;
    }

}

function tanchuang(pWidth, content) {
    $("#msg").remove();
    var html = '<div id="msg" style="position:fixed;top:50%;width:100%;height:30px;line-height:30px;margin-top:-15px;text-align:center"><p style="background:#37d05b;opacity:0.8;width:' + pWidth + 'px;color:#fff;text-align:center;padding:10px 10px;margin:0 auto;font-size:20px;border-radius:4px;">' + content + '</p></div>'
    $("body").append(html);
    var t = setTimeout(next, 2000);
    function next() {
        $("#msg").remove();

    }
function roomChange()
    {
        alert('执行了');
        //var roomFlag = document.getElementById("roomCheck").checked;
        //if (roomFlag)
        //{
        //    aler('checked');
        //}
    }


}
