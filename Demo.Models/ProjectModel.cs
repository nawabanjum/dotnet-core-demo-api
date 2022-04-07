using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Models
{
    public class ProjectJsonModel
    {
        public int projectId { get; set; }
        public DateTime modifyDate { get; set; }
        public DateTime deadlineDate { get; set; }
        public int openCall { get; set; }
        public string title { get; set; }
        public string projectDescription { get; set; }
        public bool readMore { get; set; }
        public string episode { get; set; }
        public int serial { get; set; }
        public bool isPaid { get; set; }
        public string castingOther { get; set; }
        public bool isOpen { get; set; }
        public bool isCastingNotice { get; set; }
        public int rowNumber { get; set; }
        public string projectSubTypeName { get; set; }
        public int projectSubTypeId { get; set; }
        public string unionName { get; set; }
        public int rolesCount { get; set; }
        public int filteredRoleCount { get; set; }
        public int unionId { get; set; }
        public int agentType { get; set; }
        public bool isUrgent { get; set; }
        public bool IsFavorite { get; set; }

    }


    public class ProjectModel
    {
        public ProjectModel()
        {
            openProjects = new List<ProjectJsonModel>();
        }
        public int totalCount { get; set; }

        public List<ProjectJsonModel> openProjects { get; set; }
    }



}
