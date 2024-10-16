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
    public class MemoController : ControllerBase
    {
        private readonly DailyDbContext db;
        private readonly IMapper mapper;
        public MemoController(DailyDbContext _db, IMapper _mapper)
        {
            db = _db;
            mapper = _mapper;
        }
        [HttpGet]
        public IActionResult StatMemo()
        {
            ApiResponse res = new ApiResponse();
            try
            {
                int Count =db.MemoInfo.Count();
                res.ResultCode = 1;
                res.Msg = "query successfully";
                res.ResultData=Count;
            }
            catch (Exception)
            {

                res.ResultCode = -99;
                res.Msg = "Server is busy,Pls Wait";
            }
            return Ok(res);
        }
        [HttpPost]
        public IActionResult AddMemo(MemoDTO memo)
        {
            ApiResponse res = new ApiResponse();
            try
            {
               MemoInfo memoInfo = mapper.Map<MemoInfo>(memo);
                db.MemoInfo.Add(memoInfo);
                int result=db.SaveChanges();
                if (result == 1)
                {
                    res.ResultCode=1;
                    res.Msg = "Memo add successfully";
                }
                else
                {
                    res.ResultCode = -1;
                    res.Msg = "Memo add failed ";
                }
            }
            catch (Exception)
            {
                res.ResultCode = -99;
                res.Msg = "Server is busy,Pls Wait";
            }
            return Ok(res);

        }
        [HttpGet]
        public IActionResult QueryMemo(string? title)
        {
            ApiResponse res = new ApiResponse();
            try
            {
                var query = from A in db.MemoInfo
                            select new MemoDTO
                            {
                                MemoId = A.MemoId,
                                Title = A.Title,
                                Content = A.Content,
                            };
                if(!string.IsNullOrEmpty(title) )
                {
                    query = query.Where(t => t.Title.Contains(title));
                }
                res.ResultCode = 1;
                res.Msg = "query successful";
                res.ResultData = query;

            }
            catch (Exception)
            {

                res.ResultCode = -99;
                res.Msg = "Server is busy,Pls Wait";
            }
            return Ok(res);
        }
        [HttpPut]
        public IActionResult EditMemo(MemoDTO memoDTO)
        {
            ApiResponse res = new ApiResponse();
            try
            {
                var dbInfo = db.MemoInfo.Find(memoDTO.MemoId);
                if(dbInfo==null)
                {
                    res.ResultCode = -2;
                    res.Msg = "Cant find Id";
                    return Ok(res);
                }
                dbInfo.Title = memoDTO.Title;
                dbInfo.Content = memoDTO.Content;

                int result = db.SaveChanges();
                if (result == 1)
                {
                    res.ResultCode = 1;
                    res.Msg = "Memo update successfully";
                }
                else
                {
                    res.ResultCode = -1;
                    res.Msg = "Memo update failed ";
                }
            }
            catch (Exception)
            {
                res.ResultCode = -99;
                res.Msg = "Server is busy,Pls Wait";
            }
            return Ok(res);

        }
        [HttpDelete]
        public IActionResult DelMemo(int id)
        {

            ApiResponse res = new ApiResponse();
            try
            {
                var dbInfo = db.MemoInfo.Find(id);
                if (dbInfo == null)
                {
                    res.ResultCode = -2;
                    res.Msg = "Cant find Id";
                    return Ok(res);
                }
                db.MemoInfo.Remove(dbInfo); 
                int result = db.SaveChanges();
                if (result == 1)
                {
                    res.ResultCode = 1;
                    res.Msg = "Memo Delete successfully";
                }
                else
                {
                    res.ResultCode = -1;
                    res.Msg = "Memo Delete failed ";
                }
            }
            catch (Exception)
            {
                res.ResultCode = -99;
                res.Msg = "Server is busy,Pls Wait";
            }
            return Ok(res);
        }


    }
}
