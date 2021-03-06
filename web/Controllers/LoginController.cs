﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Model.PersonnelManage;
using BLL.PersonnelManage;
using System.Web.Security;
using Comp;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        T_tb_InPersonnel tInPersonnel = new T_tb_InPersonnel();
        //
        // GET: /Login/
        /// <summary>
        /// 登录页面显示
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 判断用户输入的信息是否正确，[HttpPost]
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="userInfo">用户的实体类</param>
        /// <param name="Code">验证码</param>
        /// <returns>返回是否执行成功的标志</returns>
        public ActionResult CheckUserInfo(string UserName, E_tb_InPersonnel userInfo, string Code)
        {
            //调用业务逻辑层（BLL）去校验用户是否正确,,,定义变量存取获取到的用户的错误信息
            string UserInfoError = "";
            E_tb_InPersonnel eInPersonnel = tInPersonnel.Login(userInfo.UserName, userInfo.PassWord);
            if (eInPersonnel != null)
            {
                Session["UserInfo"] = eInPersonnel;
                
                //添加登陆Cookies  若出入安全考虑应该进行加密处理
                HttpCookie cookie = new HttpCookie("lims.userinfo");
                cookie["passport"]= Utils.Md5(eInPersonnel.PersonnelID.ToString() + eInPersonnel.UserName.ToString() + eInPersonnel.PassWord.ToString(), 32);
                cookie["uid"]= eInPersonnel.PersonnelID.ToString();
                cookie["personnelname"] = HttpUtility.UrlEncode(eInPersonnel.PersonnelName);
                cookie["rolename"] = HttpUtility.UrlEncode(eInPersonnel.rolename);
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(cookie);
                
                FormsAuthentication.SetAuthCookie(eInPersonnel.UserName, false);
                UserInfoError = "OK";
            }
            else
            {
                UserInfoError = "用户名或密码错误";
            }
            return Content(UserInfoError);
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        [Route("login/loginout")]
        public ActionResult LoginOut()
        {
            Session["UserInfo"] = null;

            HttpCookie cookie = new HttpCookie("lims.userinfo");
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);

           return new RedirectResult("/Login/login");
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public ActionResult Close()
        {
            Session["UserInfo"] = null;
            return Content("OK");
        }

        /// <summary>
        /// 验证码的实现
        /// </summary>
        /// <returns>返回验证码</returns>
        public ActionResult CheckCode()
        {
            //首先实例化验证码的类
            KenceryValidateCode validateCode = new KenceryValidateCode();
            //生成验证码指定的长度
            //string code = validateCode.CreateValidateCode(5);
            string code = "11111";
            //将验证码赋值给Session变量
            //Session["ValidateCode"] = code;
            this.TempData["ValidateCode"] = code;
            //创建验证码的图片
            byte[] bytes = validateCode.CreateValidateGraphic(code);
            //最后将验证码返回
            return File(bytes, @"image/jpeg");
        }

    }
}
