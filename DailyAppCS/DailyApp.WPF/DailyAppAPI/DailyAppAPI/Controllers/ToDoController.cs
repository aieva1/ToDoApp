using DailyAppAPI.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DailyAppAPI.ApiResponses;
using DailyAppAPI.DTOs;
using AutoMapper;

namespace DailyAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly DailyDbContext db;
        private readonly IMapper mapper;
        public ToDoController(DailyDbContext _db, IMapper _mapper)
        {
            db = _db;
            mapper = _mapper;
        }
        [HttpGet]
        public IActionResult StatToDo()
        {
            ApiResponse res = new ApiResponse();
            try
            {
                var list=db.ToDoInfo.ToList();
                var completedList=list.Where(t=>t.Status==1).ToList();
                StatToDoDTO statDto= new StatToDoDTO { TotalCount=list.Count,CompletedCount=completedList.Count};
                res.ResultCode = 1;
                res.Msg = "Successfully tracked the to-do items";
                res.ResultData= statDto;
            }
            catch (Exception)
            {

                res.ResultCode = -99;
                res.Msg = "Server is busy,Pls wait";
            }
            return Ok(res);
        }
        [HttpPost]
        public IActionResult AddToDo(ToDoDTO todoDTO)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                ToDoInfo todoInfo = mapper.Map<ToDoInfo>(todoDTO);
                db.ToDoInfo.Add(todoInfo);
                int result = db.SaveChanges();
                if(result==1)
                {
                    response.ResultCode = 1;
                    response.Msg = "Add ToDoItem successful";
                    response.ResultData = todoInfo;
                }
                else
                {
                    response.ResultCode = -1;
                    response.Msg = "Add ToDoItem Failed";
                }
            }
            catch (Exception)
            {

                response.ResultCode = -99;
                response.Msg = "Server is busy,Pls Wait";
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetToDoList()
        {
            ApiResponse res= new ApiResponse();
            try
            {
                var list=from A in db.ToDoInfo
                         where A.Status == 0
                         select new ToDoDTO
                         {
                             ToDoId = A.ToDoId,
                             Title= A.Title,
                             Content= A.Content,
                             Status= A.Status,
                         };
                res.ResultCode = 1;
                res.Msg = "tracked successfully";
                res.ResultData = list;

            }
            catch (Exception)
            {

                res.ResultCode = -99;
                res.Msg = "Server is busy,Pls Wait";
            }
            return Ok(res);
        }
        [HttpPut]
        public IActionResult UpdateStatus(ToDoDTO newStatusDto)
        {
            ApiResponse res = new ApiResponse();
            try
            {
                var dbInfo = db.ToDoInfo.Find(newStatusDto.ToDoId);
                if(dbInfo !=null)
                {
                    dbInfo.Status = newStatusDto.Status;
                    int result=db.SaveChanges();
                    if (result == 1)
                    {
                        res.ResultCode = 1;
                        res.Msg = (newStatusDto.Status == 0 ? "Status successfully set to ToDo." : "Status successfully set to completed."); 
                    }
                    else
                    {
                        res.ResultCode = -1;
                        res.Msg = "Update Failed";
                    }
                }
                else
                {
                    res.ResultCode = -1;
                    res.Msg = "Cant find ToDoItem";
                }
            }
            catch (Exception)
            {

                res.ResultCode = -99;
                res.Msg = "Server is busy,Pls Wait";
            }
            return Ok(res);
        }
        [HttpPut]
        public IActionResult UpdateToDo(ToDoDTO newToDo)
        {
            ApiResponse res = new ApiResponse();
            try
            {
                var dbInfo = db.ToDoInfo.Find(newToDo.ToDoId);
                if (dbInfo != null)
                {
                    dbInfo.Status = newToDo.Status;
                    dbInfo.Title = newToDo.Title;
                    dbInfo.Content = newToDo.Content;
                    int result = db.SaveChanges();
                    if (result == 1)
                    {
                        res.ResultCode = 1;
                        res.Msg = "Edit Successful";
                    }
                    else
                    {
                        res.ResultCode = -1;
                        res.Msg = "Edit Failed";
                    }
                }
                else
                {
                    res.ResultCode = -1;
                    res.Msg = "Cant find ToDoItem";
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
        public IActionResult QueryToDo(string? title,int? status)
        {
            ApiResponse res= new ApiResponse();
            try
            {
                var query = from A in db.ToDoInfo
                            select new ToDoDTO
                            {
                                ToDoId = A.ToDoId,
                                Title = A.Title,
                                Content = A.Content,
                                Status = A.Status,
                            };
                if(!string.IsNullOrEmpty(title) )
                {
                     query = query.Where(t => t.Title.Contains(title));
                }
                if (status != null)
                {
                    query = query.Where(t => t.Status == status);
                }
                res.ResultCode = 1;
                res.Msg = "query scucessfully";
                res.ResultData = query;

            }
            catch (Exception)
            {

                res.ResultCode = -99;
                res.Msg = "Server is busy,Pls Wait";
            }
            return Ok(res);
        }
        [HttpDelete]
        public IActionResult DelToDo(int id)
        {

            ApiResponse res = new ApiResponse();
            try
            {
                var dbInfo = db.ToDoInfo.Find(id);
                if (dbInfo == null)
                {
                    res.ResultCode = -2;
                    res.Msg = "Cant find Id";
                    return Ok(res);
                }
                db.ToDoInfo.Remove(dbInfo);
                int result = db.SaveChanges();
                if (result == 1)
                {
                    res.ResultCode = 1;
                    res.Msg = "ToDoItem Delete successfully";
                }
                else
                {
                    res.ResultCode = -1;
                    res.Msg = "ToDoItem Delete failed ";
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
