using AutoMapper;
using DailyAppAPI.ApiResponses;
using DailyAppAPI.DataModel;
using DailyAppAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DailyAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DailyDbContext db;
        private readonly IMapper mapper;
        public AccountController(DailyDbContext _db,IMapper _mapper) 
        {
            db = _db;
            mapper = _mapper;
        }
        [HttpPost]
        public IActionResult Register(AccountInfoDTO accountInfoDTO)
        {
            ApiResponse res = new ApiResponse();
            try
            {
                var dbAccount = db.AccountInfo.Where(t => t.UserName == accountInfoDTO.UserName).FirstOrDefault();
                if (dbAccount != null)
                {
                    res.ResultCode = -1;
                    res.Msg = "sry,This username is existed";
                    return Ok(res);

                }
                AccountInfo accountInfo = mapper.Map<AccountInfo>(accountInfoDTO);
                db.AccountInfo.Add(accountInfo);
                int result=db.SaveChanges();
                if (result == 1)
                {
                    res.ResultCode = 1;
                    res.Msg = "account registed successful";

                }
                else
                {
                    res.ResultCode = -99;
                    res.Msg = "waiting for server...";
                }
            }
            catch (Exception)
            {

                res.ResultCode = -99;
                res.Msg = "waiting for server...";
            }
            return Ok(res);
        }

        [HttpGet]
        public IActionResult Login(string username, string pwd)
        {
            ApiResponse res = new ApiResponse();
            try
            {
                var dbAccountInfo = db.AccountInfo.Where(t => t.UserName == username).FirstOrDefault();
                if (dbAccountInfo == null)
                {
                    res.ResultCode = -1;
                    res.Msg = "Username or Password is not existed";
                    return Ok(res);
                }
                if (dbAccountInfo.Pwd != pwd)
                {
                    res.ResultCode = -2;
                    res.Msg = "Wrong Password";
                    return Ok(res);
                }
                res.ResultCode = 1;
                res.Msg = "Login successful";
                res.ResultData = dbAccountInfo;
                return Ok(res);
            }
            catch (Exception)
            {

                res.ResultCode = -99;
                res.Msg = "unknown Error";

            }
            return Ok(res);
        }
    }

}
