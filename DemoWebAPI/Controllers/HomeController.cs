
using Demo.Models;
using Demo.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // GET: api/<HomeController>
        private static ProjectModel inMemory = null;
        
      
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [Route("GetProjects")]
        [AllowAnonymous]
        public ApiResponse GetProjects()
        {
            ApiResponse response = new ApiResponse();
            inMemory = new ProjectModel();
            try
            {
                _logger.LogInformation("Attempted Get All Projects");
                inMemory = ApiHandler.GetProjects();
                if (inMemory!=null)
                {
                    response.Data = inMemory;
                    response.Message = "Success";
                    response.IsSuccess = true;
                }
                else
                {
                    response.Data = inMemory;
                    response.IsSuccess = false;
                    response.Message = "No Record Found";
                }
                _logger.LogInformation("Successfully got All Projects");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                response.Message = ex.ToString();

                response.IsSuccess = false;
            }
            return response;
        }

        // GET api/<HomeController>/5

        [HttpGet("{ProjectSubTypeId}")]
        public ApiResponse GetProjectById(int ProjectSubTypeId)
        {
            ApiResponse response = new ApiResponse();
            ProjectModel projectModel = new ProjectModel();

            try
            {
                if (inMemory == null)
                {
                    response.Data = inMemory;
                    response.IsSuccess = false;
                    response.Message = "No Record Found";

                    return response;
                }
                _logger.LogInformation("Attempted GetProjectById");
                  var model = inMemory.openProjects.Where(a => a.projectSubTypeId == ProjectSubTypeId).ToList();
                 projectModel.openProjects = model;
                projectModel.totalCount = model.Count();
                  response.Data = projectModel;
                    response.IsSuccess = true;
               
                _logger.LogInformation("Successfully got All Projects By Id");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                response.Message = ex.ToString();
                response.IsSuccess = false;
            }
            return response;
        }


        [HttpGet("{ProjectByProjectId}")]
        public ApiResponse ProjectByProjectId(int ProjectId)
        {
            ApiResponse response = new ApiResponse();
            ProjectModel projectModel = new ProjectModel();

            try
            {
                if (inMemory == null)
                {
                    response.Data = inMemory;
                    response.IsSuccess = false;
                    response.Message = "No Record Found";

                    return response;
                }
                _logger.LogInformation("Attempted GetProjectById");
                var model = inMemory.openProjects.Where(a => a.projectId == ProjectId).ToList();
                projectModel.openProjects = model;
                projectModel.totalCount = model.Count();
                response.Data = projectModel;
                response.IsSuccess = true;

                _logger.LogInformation("Successfully got All Projects By Id");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                response.Message = ex.ToString();
                response.IsSuccess = false;
            }
            return response;
        }
        [HttpPost]
        [Route("ProjectSortByDates")]
        public ApiResponse ProjectSortByDates(SortModel model)
        {
            ApiResponse response = new ApiResponse();
          
            ProjectModel projectModel = new ProjectModel();
            try
            {
                if (string.IsNullOrEmpty(model.SortOrderType))
                {
                    response.IsSuccess = false;
                    response.Message = "SortType  Required";
                    return response;
                }
                if (string.IsNullOrEmpty(model.sortOrder))
                {
                    response.IsSuccess = false;
                    response.Message = "sortOrder Required";

                    return response;
                }
                if (!string.IsNullOrEmpty(model.SortOrderType))
                {
                    if (inMemory == null)
                    {
                        response.Data = inMemory;
                        response.IsSuccess = false;
                        response.Message = "No Record Found";

                        return response;
                    }
                    switch (model.SortOrderType)
                    {
                        case "ModifyDate":
                            if (model.sortOrder.Contains("desc"))
                            {
                                _logger.LogInformation("Attempted Sort Order Descending By ModifyDate");

                                projectModel.openProjects = inMemory.openProjects.OrderByDescending(s => s.modifyDate).ToList();
                                _logger.LogInformation("Successfully got Sort Order Ascending By ModifyDate");

                            }
                            else
                            {
                                _logger.LogInformation("Attempted Sort Order Ascending By ModifyDate");
                                projectModel.openProjects = inMemory.openProjects.OrderBy(s => s.modifyDate).ToList();
                                _logger.LogInformation("Successfully got Sort Order Ascending By ModifyDate");

                            }
                            break;
                        case "DeadlineDate":
                            if (model.sortOrder.Contains("desc"))
                            {
                                _logger.LogInformation("Attempted Sort Order Ascending By DeadlineDate");

                                projectModel.openProjects = inMemory.openProjects.OrderByDescending(s => s.deadlineDate).ToList();
                                _logger.LogInformation("Successfully got Sort Order Ascending By DeadlineDate");

                            }
                            else
                            {
                                _logger.LogInformation("Attempted Sort Order Ascending By DeadlineDate");

                                projectModel.openProjects = inMemory.openProjects.OrderBy(s => s.deadlineDate).ToList();
                                _logger.LogInformation("Successfully got All Sort Order Ascending By DeadlineDate");


                            }
                            break;
                    }

                        projectModel.totalCount = projectModel.openProjects.Count();
                        response.Data = projectModel.openProjects;
                        response.IsSuccess = true;
                    
                   
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                response.Message = ex.ToString();
                response.IsSuccess = false;
            }
            return response;
        }




        [HttpPost("FavoriteProject")]
        public ApiResponse FavoriteProject(FavoriteProjectModel model)
        {
            ApiResponse response = new ApiResponse();
           

            try
            {
                if (inMemory == null)
                {
                    response.Data = inMemory;
                    response.IsSuccess = false;
                    response.Message = "Load data First, No Record ";

                    return response;
                }
                _logger.LogInformation("Attempted GetProjectById");
                var exists = inMemory.openProjects.Where(a => a.projectId == model.projectId).FirstOrDefault();
                if (exists!=null)
                {
                    foreach (var item in inMemory.openProjects)
                    {
                        if (item.projectId== exists.projectId)
                        {
                            item.IsFavorite = model.IsFavorite;
                            break;
                        }                        
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Project Id not exists";
                }                
                response.Data = inMemory;
                response.IsSuccess = true;
                _logger.LogInformation("Successfully updated FavoriteProject By Id");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                response.Message = ex.ToString();
                response.IsSuccess = false;
            }
            return response;
        }
    }
}
